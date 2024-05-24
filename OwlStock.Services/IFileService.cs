using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;

namespace OwlStock.Services
{
    public interface IFileService
    { 
        bool CreatePhotoFile(PhotoBase photo);
        Task CreateIFormFile(IFormFile file, string webRootPath);
    }
}
