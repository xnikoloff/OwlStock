using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs.Articles;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class BlogService : IBlogService
    {
        private const int _visibleContentByPage = 4;
        private const int _visibleTopContent = 4;

        private readonly PhotonicDbContext _context;
        private readonly ILogger<BlogService> _logger;

        public BlogService(PhotonicDbContext context, ILogger<BlogService> logger)
        {
            _context = context ?? new();
            _logger = logger;
        }

        /// <summary>
        /// Creates new article
        /// </summary>
        /// <param name="dto">The DTO for creating an article</param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> Create(CreateArticleDTO dto)
        {
            if (dto == null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, ${nameof(dto)} was null");
                return false;

            }

            if (dto.Article == null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(dto.Article)} was null");
                return false;
            }

            if (dto.Article.Content == null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(dto.Article.Content)} was null");
                return false;
            }

            if (dto.Image == null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(dto.Image)} was null");
                return false;
            }

            if (dto.WebRootPath == null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(dto.WebRootPath)} was null");
                return false;
            }

            CreateArticleCategoryDTO categoryDTO = await CreateArticleCategory(new()
            {
                CreatedById = dto.Article.CreatedById,
                Name = dto.NewCategoryName,
                
            });

            if (categoryDTO.IsSuccessful)
            {
                if (categoryDTO.Id != Guid.Empty)
                {
                    dto.Article.ArticleCategoryId = categoryDTO.Id;
                }

                else
                {
                    dto.Article.ArticleCategoryId = dto.SelectedCategoryId;
                }
            }

            else
            {
                return false;
            }

            try
            {
                dto!.Article.ImageName = dto?.Image?.FileName;
                dto!.Article.CreatedOn = DateTime.Now;

                await _context.AddAsync(dto!.Article);
                int result = await _context.SaveChangesAsync();

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        /// <summary>
        /// Deletes an article by setting the IsVisible property to false
        /// </summary>
        /// <param name="id">Id of the article</param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> Delete(Guid id)
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Delete)}, {nameof(_context.Articles)} is null");
                return false;
            }

            try
            {
                Article? article = await _context.Articles.FindAsync(id);

                if(article == null)
                {
                    _logger.LogError($"Article with Id {id} was not found, {DateTime.UtcNow}, {nameof(Delete)}");
                    return false;
                }

                article.IsVisible = false;
                article.DeletedOn = DateTime.Now;
                
                await _context.SaveChangesAsync();
                return true;
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        /// <summary>
        /// Recovers deleted article by setting the IsVisible property back to true
        /// </summary>
        /// <param name="id">If of the article</param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> Recover(Guid id)
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Recover)}, {nameof(_context.Articles)} is null");
                return false;
            }
            try
            {
                Article? article = await _context.Articles.FindAsync(id);
                if (article == null)
                {
                    _logger.LogError($"Article with Id {id} was not found, {DateTime.UtcNow}, {nameof(Recover)}");
                    return false;
                }
                article.IsVisible = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        /// <summary>
        /// Gets and article by its id
        /// </summary>
        /// <param name="id">Id of the article</param>
        /// <returns>Returs the found article</returns>
        public async Task<Article> GetById(Guid id)
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetById)}, {nameof(_context.Articles)} was null");
                return new();
            }

            try
            {
                return await _context.Articles
                .Include(dc => dc.ArticleCategories)
                .ThenInclude(dcc => dcc!.Articles)
                .Where(dc => dc.Id == id)
                .FirstOrDefaultAsync() ?? new();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        /// <summary>
        /// Gets all articles
        /// </summary>
        /// <returns>All articles with an AllArticlesDTO</returns>
        public async Task<AllArticlesDTO> GetAll()
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAll)}, {nameof(_context.Articles)} was null");
                return new();
            }

            if (_context.ArticleCategories is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAll)}, {nameof(_context.ArticleCategories)} was null");
                return new();
            }

            try
            {
                List<Article>? articles = await _context.Articles
                .Include(dc => dc.CreatedBy)
                .Include(dc => dc.ArticleCategories)
                .ToListAsync();

                List<ArticleCategory>? articleCategories = await _context.ArticleCategories.ToListAsync();

                return new AllArticlesDTO()
                {
                    Articles = articles,
                    ArticleCategories = articleCategories
                };
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }            
        }


        /// <summary>
        /// Gets all articles that belong to a given category
        /// </summary>
        /// <param name="id">Id of the category</param>
        /// <returns>All articles for the given category with an AllArticlesDTO</returns>
        public async Task<AllArticlesDTO> GetAllByCategory(Guid id)
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByCategory)}, {nameof(_context.Articles)} was null");
                return new();
            }

            if (_context.ArticleCategories is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByCategory)}, {nameof(_context.ArticleCategories)} was null");
                return new();
            }

            if (id == Guid.Empty)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByCategory)}, {nameof(id)} was null or empty");
                return new();
            }

            try
            {
                List<Article>? articles = await _context.Articles
                   .Include(dc => dc.CreatedBy)
                   .Include(dc => dc.ArticleCategories)
                   .Where(dc => dc.ArticleCategories!.Id == id)
                   .ToListAsync();

                List<ArticleCategory>? articleCategories = await _context.ArticleCategories.ToListAsync();

                return new AllArticlesDTO()
                {
                    Articles = articles,
                    ArticleCategories = articleCategories,
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        /// <summary>
        /// Gets all articles with IsVisible marked as false
        /// </summary>
        /// <returns>All articles with IsVisible marked as false with an AllArticlesDTO</returns>

        public async Task<AllArticlesDTO> GetAllDeleted()
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByCategory)}, {nameof(_context.Articles)} was null");
                return new();
            }

            if (_context.ArticleCategories is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByCategory)}, {nameof(_context.ArticleCategories)} was null");
                return new();
            }

            try
            {
                List<Article>? articles = await _context.Articles
                   .Include(dc => dc.CreatedBy)
                   .Include(dc => dc.ArticleCategories)
                   .Where(dc => dc.IsVisible == false)
                   .ToListAsync();

                List<ArticleCategory>? articleCategories = await _context.ArticleCategories.ToListAsync();

                return new AllArticlesDTO()
                {
                    Articles = articles,
                    ArticleCategories = articleCategories
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        /// <summary>
        /// Paginates the articles
        /// </summary>
        /// <param name="pageNumber">The current page number that is requested</param>
        /// <returns>All articles that need to be displyed on the current page plus total number of pages</returns>
        public async Task<AllArticlesDTO> GetAllByPage(int pageNumber)
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByPage)}, {nameof(_context.Articles)} was null");
                return new();
            }

            if (_context.ArticleCategories is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByPage)}, {nameof(_context.ArticleCategories)} was null");
                return new();
            }

            if (pageNumber <= 0) 
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByPage)}, {nameof(pageNumber)} was 0 or less that 0");
                return new();
            }

            try
            {
                List<Article> articles = await _context.Articles
                .Where(dc => dc.IsVisible)
                .Include(dc => dc.CreatedBy)
                .Skip((pageNumber * _visibleContentByPage) - _visibleContentByPage)
                .Take(_visibleContentByPage)
                .ToListAsync();

                List<ArticleCategory>? articleCategories = await _context.ArticleCategories.ToListAsync();

                int pageCount = await CalculatePagesNumber();

                if(pageCount == -1)
                {
                    _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByPage)}, {nameof(CalculatePagesNumber)} returned -1");
                    return new();
                }

                return new AllArticlesDTO()
                {
                    Articles = articles,
                    ArticleCategories = articleCategories,
                    PagesCount = pageCount
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }


        /// <summary>
        /// Gets the last four articles. If there are less than four articles, gets all
        /// </summary>
        /// <returns>List of the articles</returns>
        public async Task<IEnumerable<Article>> GetTopContent()
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllByPage)}, {nameof(_context.Articles)} was null");
                return new List<Article>();
            }

            try
            {
                int count = await _context.Articles.CountAsync();

                //if count is lower than 4 take all
                if(count < 4)
                {
                    return await _context.Articles
                    .Where(dc => dc.ShowInTopPosition && dc.IsVisible)
                    .Include(dc => dc.ArticleCategories)
                    .OrderBy(dc => dc.Id)
                    .ToListAsync();
                }

                //if count is bigger than 4, take only 4
                return await _context.Articles
                    .Where(dc => dc.ShowInTopPosition && dc.IsVisible)
                    .Include(dc => dc.ArticleCategories)
                    .OrderBy(dc => dc.Id)
                    .Take(_visibleTopContent)
                    .ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new List<Article>();
            }
        }

        /// <summary>
        /// Creates new category
        /// </summary>
        /// <param name="category">Object of ArticleCategory, containing the category name and the user that created the category</param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> CreateCategory(ArticleCategory category)
        {
            if (_context.ArticleCategories is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllArticleCategories)}, {nameof(_context.ArticleCategories)} was null");
                return false;
            }

            try
            {
                await _context.ArticleCategories.AddAsync(category);
                await _context.SaveChangesAsync();

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        /// <summary>
        /// Gets all articles alongside their categories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ArticleCategory>> GetAllArticleCategories()
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllArticleCategories)}, {nameof(_context.Articles)} was null");
                return new List<ArticleCategory>();
            }

            if (_context.ArticleCategories is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetAllArticleCategories)}, {nameof(_context.ArticleCategories)} was null");
                return new List<ArticleCategory>();
            }

            try
            {
                return await _context.ArticleCategories.ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new List<ArticleCategory>();
            }
        }

        /// <summary>
        /// Creates new ArticleCategory if the name of the category is not null
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>If created, Id of the created DynamucContentCategory, else empty GUID</returns>
        private async Task<CreateArticleCategoryDTO> CreateArticleCategory(CreateArticleCategoryDTO dto)
        {
            if (_context.ArticleCategories == null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(CreateArticleCategory)}, {nameof(_context.ArticleCategories)} was null");
                dto.IsSuccessful = false;
                return dto;
            }

            try
            {
                if (!dto.Name.IsNullOrEmpty())
                {
                    ArticleCategory category = new()
                    {
                        CreatedById = dto.CreatedById,
                        CreatedOn = DateTime.Now,
                        Name = dto.Name
                    };

                    await _context.ArticleCategories.AddAsync(category);
                    await _context.SaveChangesAsync();

                    dto.Id = category.Id;
                    dto.IsSuccessful = true;

                    return dto;
                }
                else
                {
                    dto.Id = Guid.Empty;
                    dto.IsSuccessful = true;
                    return dto;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                dto.IsSuccessful = false;
                return dto;
            }
        }

        /// <summary>
        /// Calculates the total page of numbers based on the visible content and the total number of articles
        /// </summary>
        /// <returns>Number of the pages as integer</returns>
        private async Task<int> CalculatePagesNumber()
        {
            if (_context.Articles is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(CalculatePagesNumber)}, {nameof(_context.Articles)} was null");
                return -1;
            }

            try
            {
                double total = await _context.Articles.CountAsync();

                if (total == 0)
                {
                    return 0;
                }

                int result = (int)Math.Ceiling(total / _visibleContentByPage);

                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return -1;
            }
        }
    }
}
