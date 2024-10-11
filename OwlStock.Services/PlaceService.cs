using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly OwlStockDbContext _context;

        public PlaceService(OwlStockDbContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Place>> All()
        {
            if(_context.Places is null)
            {
                throw new NullReferenceException($"{nameof(_context.Places)} is null");
            }

            return await _context.Places
                .Include(p => p.City)
                .ToListAsync();
        }

        public async Task<IEnumerable<Place>> AllPopular()
        {
            if(_context.Places is null)
            {
                throw new NullReferenceException($"{nameof(_context.Places)} is null");
            }

            return await _context.Places
                .Where(p => p.IsPopular)
                .ToListAsync();
        }

        public async Task<IEnumerable<Place>> GetPopularPlacesByRegion(int regionId)
        {
            if(regionId == 0)
            {
                throw new ArgumentNullException($"{nameof(regionId)} is {regionId}");
            }

            if(_context.Places is null)
            {
                throw new NullReferenceException($"{nameof(_context.Places)} is null");
            }

            return await _context.Places
                .Include(p => p.PhotoBase)
                .Where(p => p.City.RegionId == regionId)
                .ToListAsync();
        }

        public async Task<Place?> PlaceById(Guid id)
        {
            if (_context.Places is null)
            {
                throw new NullReferenceException($"{nameof(_context.Places)} is null");
            }

            Place? place = await _context.Places
                .Include(p => p.PhotoBase)
                .Include(p => p.City)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if(place?.PhotoBase == null)
            {
                place!.PhotoBase = new();
            }

            return place; //?? throw new NullReferenceException($"{nameof(place)} with Id {id} cannot be found");
        }
        
        public async Task<Place?> Create(Place place)
        {
            if(_context.Places is null)
            {
                throw new NullReferenceException($"{nameof(_context.Places)} is null");
            }

            await _context.AddAsync(place);
            await _context.SaveChangesAsync();

            return await PlaceById(place.Id);

        }

        public async Task<Place?> Update(Place place)
        {
            if (_context.Places is null)
            {
                throw new NullReferenceException($"{nameof(_context.Places)} is null");
            }

            Place? existingPlace = await PlaceById(place.Id);

            if (existingPlace != null)
            {
                existingPlace.Name = place.Name;
                existingPlace.Description = place.Description;
                existingPlace.GoogleMapsURL = place.GoogleMapsURL;
                existingPlace.IsPopular = place.IsPopular;
                await _context.SaveChangesAsync();
            }

            return await PlaceById(place.Id);
        }

        public async Task<Place?> UpdatePhotoId(Guid placeId, Guid photoId)
        {
            if (_context.Places is null)
            {
                throw new NullReferenceException($"{nameof(_context.Places)} is null");
            }

            Place? existingPlace = await PlaceById(placeId);

            if (existingPlace != null)
            {
                existingPlace.PhotoBaseId = photoId;
                await _context.SaveChangesAsync();
            }

            return await PlaceById(placeId);

        }
    }
}
