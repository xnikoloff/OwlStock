using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services
{
    public interface IFileService
    {
        Task<int> Create(IFormFile? file, Photo photo, string? webRootPath, PhotoSize size, string fileName);
        Task<List<string>> GetFilesPathsForPhotoShoot(int photoShootId);
        void Create(List<IFormFile> file, string? webRootPath, PhotoSize size);
    }
}
