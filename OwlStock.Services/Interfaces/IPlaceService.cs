using OwlStock.Domain.Entities;

namespace OwlStock.Services.Interfaces
{
    public interface IPlaceService
    {
        Task<IEnumerable<Place>> All();
        Task<IEnumerable<Place>> AllPopular();
        Task<IEnumerable<Place>> GetPopularPlacesByRegion(int regionId);
        Task<Place?> PlaceById(Guid id);
        Task<Place?> Create(Place place);
        Task<Place?> Update(Place place);
        Task<Place?> UpdatePhotoId(Guid placeId, Guid photoId);
    }
}
