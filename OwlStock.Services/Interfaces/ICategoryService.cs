using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<int> Create(IEnumerable<Category> categories, Guid photoId);
    }
}
