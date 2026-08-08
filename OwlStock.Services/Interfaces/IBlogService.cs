using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs.Articles;

namespace OwlStock.Services.Interfaces
{
    public interface IBlogService
    {
        Task<Article> GetById(Guid id);
        Task<AllArticlesDTO> GetAll();
        Task<AllArticlesDTO> GetAllByCategory(Guid id);
        Task<AllArticlesDTO> GetAllByPage(int pageNumber);
        Task<AllArticlesDTO> GetAllDeleted();
        Task<IEnumerable<Article>> GetTopContent();
        Task<IEnumerable<ArticleCategory>> GetAllArticleCategories();
        Task<bool> Create(CreateArticleDTO dto);
        Task<bool> Delete(Guid id);
        Task<bool> Recover(Guid id);
        Task<bool> CreateCategory(ArticleCategory category);
    }
}
