using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;

namespace OwlStock.Services.DTOs.Articles
{
    public class CreateArticleDTO
    {
        public Article? Article { get; set; }
        public IFormFile? Image { get; set; }
        public string? WebRootPath { get; set; }
        public Guid SelectedCategoryId { get; set; }
        public List<ArticleCategory>? ArticleCategories { get; set; }

        public string? NewCategoryName { get; set; }
    }
}
