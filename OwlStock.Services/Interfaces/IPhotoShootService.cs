using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs.PhotoShoot;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoShootService
    {
        Task<IEnumerable<PhotoShoot>> GetAll();

        /// <summary>
        /// Sets provided dates as reserved 
        /// </summary>
        /// <param name="reservedDates">Dates that need to be set as reserved</param>
        /// <returns>True if succesfull or false if not successful</returns>
        Task<bool> SetReservedDate(DateTime dates);

        Task<Guid> Add(CreateRegularPhotoShootDTO dto);
        Task<Guid> AddSmallProductPhotoShoot(CreateSmallProductPhotoshootDTO dto);
        Task<bool> Update(UpdatePhotoShootDTO dto);
        Task<PhotoShoot> PhotoShootById(Guid id);
        Task<PhotoShootByIdDTO> PhotoShootById(Guid id, string userId);
        Task<List<MyPhotoShootsDTO>> PhotoShootsByUser(string userId);
        Task<IEnumerable<DateTime>> GetReservedDates();

        /// <summary>
        /// Changes the status of a photoshoot and sends email if email is provided
        /// </summary>
        /// <param name="id">Id of the photoshoot</param>
        /// <param name="status">New status of the photoshoot</param>
        /// <param name="email">Email of the user</param>
        /// <returns></returns>
        Task<ChangePhotoshootStatusDTO> ChangeStatus(Guid id, PhotoshootStatus status);

        /// <summary>
        /// Returns name of the person that the photoshoot belongs to
        /// </summary>
        /// <param name="id">Id of the photoshoor</param>
        /// <returns>Name of the person as string</returns>
        Task<string> GetPersonName(Guid id);
    }
}