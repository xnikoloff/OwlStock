using OwlStock.Services.DTOs.Articles;

namespace OwlStock.Services.Facades.Interfaces
{
    public interface IBlogServiceFacade
    {
        Task<bool> Create(CreateArticleDTO dto);
    }
}
