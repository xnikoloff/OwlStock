using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoByIdDTO> GetById(Guid? id);
        Task<Guid> Create(PhotoBase? photo);
        Task<PhotoBase> Delete(PhotoBase photo);
        Task<Guid> ChangeDownloadPermissions(Guid id);
    }
}
