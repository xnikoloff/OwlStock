using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services
{
    public interface IFileService
    {
        void Create(List<IFormFile> file, string? webRootPath, PhotoSize? size);
        Task<List<string>> GetFilesNamesForPhotoShoot(int photoShootId);
        Task<int> CreatePhotoShootFiles(List<IFormFile> files, int photoShootId, string webRootPath);
    }
}
