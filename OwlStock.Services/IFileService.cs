using Microsoft.AspNetCore.Http;

namespace OwlStock.Services
{
    public interface IFileService
    {
        void Create(byte[] files, string? webRootPath, string filePath);
        Task<List<string>> GetFilesNamesForPhotoShoot(Guid photoShootId);
        Task<int> CreatePhotoShootFiles(List<IFormFile> files, Guid photoShootId, string webRootPath);
        byte[] ConvertFormFileToByteArray(IFormFile file);
    }
}
