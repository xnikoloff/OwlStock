using OwlStock.Domain.Entities;

namespace OwlStock.Services.DTOs
{
    public class HomeDTO
    {
        public GalleryPhoto? GalleryPhoto { get; set; }
        public IEnumerable<DynamicContent>? DynamicContents { get; set; }
    }
}
