using Microsoft.AspNetCore.Http;
using OwlStock.Domain;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<List<AllPhotosDTO>> All();
        Task<List<AllPhotosDTO>> All(string? userId);
        Task<List<AllPhotosDTO>> AllByCategory(Category category);
        Task<PhotoByIdDTO> GetById(int? id);
        Task<int> Create(CreatePhotoDTO? createPhotoDto);
    }
}
