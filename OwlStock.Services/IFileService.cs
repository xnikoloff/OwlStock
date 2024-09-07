using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;

namespace OwlStock.Services
{
    public interface IFileService
    { 
        bool CreatePhotoFile(PhotoBase photo);
        Task<bool> CreatePlacePhotoFile(CreatePlacePhotoFileDTO dto);
        Task CreateIFormFile(IFormFile file, string webRootPath);
    }
}
