using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services
{
    public interface IFileService
    {
        void Create(List<IFormFile> file, string? webRootPath, PhotoSize? size);
        Task<List<string>> GetFilesNamesForPhotoShoot(Guid photoShootId);
        Task<int> CreatePhotoShootFiles(List<IFormFile> files, Guid photoShootId, string webRootPath);
    }
}
