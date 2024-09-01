using OwlStock.Domain.Entities;

namespace OwlStock.Services.Interfaces
{
    public interface IPlaceService
    {
        Task<IEnumerable<Place>> All();
        Task<Place?> PlaceById(Guid id);
        Task<Place?> Create(Place place);
        Task<Place?> Update(Place place);
    }
}
