using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoShootPhoto>> AllByPhotoshoot(Guid? photoshootId);
        Task<PhotoByIdDTO> GetById(Guid? id);
        Task<PhotoBase> GetPhotoBaseById(Guid? id);
        Task<PhotoBase> Create(PhotoBase? photo);
        Task<PhotoBase> Delete(PhotoBase photo);
        Task<Guid> ChangeDownloadPermissions(Guid id);
    }
}
