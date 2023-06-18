using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
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
        Task AddFiles(Guid id, List<IFormFile> files, string? webRootPath, PhotoSize? size);
    }
}