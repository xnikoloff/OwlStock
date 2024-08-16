using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Common.HelperClasses;
using OwlStock.Services.DTOs.PhotoShoot;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoShootService
    {
        Task<IEnumerable<PhotoShoot>> GetAll();
        Task<PhotoShoot> Add(CreatePhotoShootDTO dto);
        Task<PhotoShoot> Update(ManagePhotoshootDTO dto);
        Task<PhotoShoot> PhotoShootById(Guid id);
        Task<PhotoShoot?> PhotoShootById(Guid id, string userId);
        Task<List<MyPhotoShootsDTO>> MyPhotoShoots(string userId);
        Task<Dictionary<DateOnly, IEnumerable<TimeSlot>>> GetPhotoShootsCalendar();
        Task<PhotoShoot> ChangeStatus(Guid id, PhotoshootStatus status);
        
        /// <summary>
        /// Returns name of the person that the photoshoot belongs to
        /// </summary>
        /// <param name="id">Id of the photoshoor</param>
        /// <returns>Name of the person as string</returns>
        Task<string> GetPersonName(Guid id);
    }
}