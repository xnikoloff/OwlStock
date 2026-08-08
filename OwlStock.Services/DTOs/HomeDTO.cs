using OwlStock.Domain.Entities;

namespace OwlStock.Services.DTOs
{
    public class HomeDTO
    {
        public GalleryPhoto? GalleryPhoto { get; set; }
        public IEnumerable<Article>? DynamicContents { get; set; }
    }
}
