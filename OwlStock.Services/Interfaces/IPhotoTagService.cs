using OwlStock.Domain.Entities;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoTagService
    {
        Task<int> Add(string tags, Guid photoId);
        Task<List<Guid>> GetPhotoIdListByTag(string tagText);
    }
}
