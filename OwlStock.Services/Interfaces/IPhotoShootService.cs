using OwlStock.Domain.Entities;
using OwlStock.Services.Common.HelperClasses;
using OwlStock.Services.DTOs.PhotoShoot;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoShootService
    {
        Task<int> Add(CreatePhotoShootDTO dto);
        Task<PhotoShoot> PhotoShootById(Guid id);
        Task<List<MyPhotoShootsDTO>> MyPhotoShoots(string userId);
        Task<Dictionary<DateOnly, IEnumerable<TimeSlot>>> GetPhotoShootsCalendar();
    }
}