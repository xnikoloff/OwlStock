using OwlStock.Domain.Entities;

namespace OwlStock.Services.DTOs.Articles
{
    public class AllArticlesDTO
    {
        public List<Article>? Articles { get; set; }
        public List<ArticleCategory>? ArticleCategories { get; set; }
        public int PagesCount { get; set; }
    }
}
