namespace OwlStock.Services.DTOs.Articles
{
    internal class CreateArticleCategoryDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? CreatedById { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
