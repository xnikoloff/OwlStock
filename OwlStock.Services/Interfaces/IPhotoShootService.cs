using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs.PhotoShoot;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoShootService
    {
        Task<int> Add(CreatePhotoShootDTO dto);
        Task<List<PhotoShoot>> AllPhotoShoots();
        Task<PhotoShootByIdDTO> PhotoShootById(Guid id);
        Task<List<MyPhotoShootsDTO>> MyPhotoShoots(string userId);
        Task<List<PhotoShoot>> ShowAvailableSlots();
    }
}