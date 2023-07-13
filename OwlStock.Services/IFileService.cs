using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;

namespace OwlStock.Services
{
    public interface IFileService
    { 
        string CreatePhotoFile(PhotoBase photo, string webRootPath);
    }
}
