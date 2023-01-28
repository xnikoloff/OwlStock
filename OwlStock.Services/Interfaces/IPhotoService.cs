using OwlStock.Domain;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<List<Photo>> All();
        Task<Photo> GetById(int? id);
        Task<int> Create(Photo? photo);
    }
}
