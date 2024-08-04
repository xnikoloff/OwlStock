using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.Interfaces
{
    public interface IGalleryService
    {
        Task<List<GalleryPhoto>> All();
        Task<List<PhotoShootPhoto>> AllByPhotoshootType(PhotoShootType photoShootType);
        Task<List<GalleryPhoto>> AllByCategory(Category category);
        Task<List<GalleryPhoto>> AllByTags(string tag);
    }
}
