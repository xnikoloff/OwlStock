using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;

namespace OwlStock.Services.Interfaces
{
    public interface IDynamicContentService
    {
        Task<DynamicContent> GetById(Guid id);
        Task<IEnumerable<DynamicContent>> GetAll();
        Task<DynamicContent> Create(CreateDynamicContentDTO dto);
        Task Delete(Guid id);
    }
}
