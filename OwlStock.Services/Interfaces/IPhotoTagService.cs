using OwlStock.Domain.Entities;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoTagService
    {
        Task<int> Add(string tags, int photoId);
        Task<List<int>> GetPhotoIdListByTag(string tagText);
    }
}
