using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs.PhotoShoot;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoShootService
    {
        Task<int> Reserve(CreatePhotoShootDTO photoShoot);
        Task<List<PhotoShoot>> AllReservations();
        Task<PhotoShoot> ReservationById(int id);
        Task<List<MyPhotoShootsDTO>> MyPhotoShoots(string userId);
        Task<List<PhotoShoot>> ShowAvailableSlots();
    }
}