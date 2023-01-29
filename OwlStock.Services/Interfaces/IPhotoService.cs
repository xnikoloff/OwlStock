using Microsoft.AspNetCore.Http;
using OwlStock.Domain;
using OwlStock.Services.DTOs;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<List<AllPhotosDTO>> All();
        Task<Photo> GetById(int? id);
        Task<int> Create(CreatePhotoDTO? createPhotoDto);
    }
}
