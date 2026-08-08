using Microsoft.Extensions.Logging;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly PhotonicDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(PhotonicDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates new entry in the mapping table PhotosCategories
        /// </summary>
        /// <param name="categories">List of the categories a photo belongs to</param>
        /// <param name="photoId">Id of the photo</param>
        /// <returns>True if creation was successful, else returns false</returns>
        public async Task<bool> Create(IEnumerable<Category> categories, Guid photoId)
        {
            IEnumerable<PhotoCategory> photoCategories = BuildPhotoCateoriesList(categories, photoId);

            if(_context.PhotosCategories is null)
            {
                _logger.LogError($"{nameof(_context.PhotosCategories)} is null at {DateTime.UtcNow} in {nameof(CategoryService)}, {nameof(Create)}");
                return false;
            }

            try
            {
                await _context.PhotosCategories.AddRangeAsync(photoCategories);
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
        /// Builds IEnumerable of PhotoCategory
        /// </summary>
        /// <param name="categories">List of categories a photo belongs to</param>
        /// <param name="photoId">The photo</param>
        /// <returns>IEnumerable of PhotoCategory</returns>
        private static IEnumerable<PhotoCategory> BuildPhotoCateoriesList(IEnumerable<Category> categories, Guid photoId)
        {
            List<PhotoCategory> photoCategories = new();

            foreach (Category category in categories)
            {
                PhotoCategory photoCategory = new()
                {
                    GalleryPhotoId = photoId,
                    Category = category

                };

                photoCategories.Add(photoCategory);
            }

            return photoCategories;
        }
    }
}
