using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs.Place;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class PlaceService : IPlaceService
    {
        private readonly PhotonicDbContext _context;
        private readonly ILogger<PlaceService> _logger;

        public PlaceService(PhotonicDbContext context, ILogger<PlaceService> logger) 
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gets all photoshoot locations
        /// </summary>
        /// <returns>IEnumerable of Place containing data for all photoshoot locations</returns>
        public async Task<IEnumerable<Place>> All()
        {
            try
            {
                return await _context.Places
                        .Include(p => p.PhotoBase)
                        .Include(p => p.City)
                        .ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return Enumerable.Empty<Place>();
            }
        }

        /// <summary>
        /// Gets all popular photoshoot locations
        /// </summary>
        /// <returns>IEnumerable of Place containing data for all the popular photoshoot locations</returns>
        public async Task<IEnumerable<Place>> AllPopular()
        {

            try
            {
                return await _context.Places
                .Where(p => p.IsPopular)
                .ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return Enumerable.Empty<Place>();
            }
        }

        /// <summary>
        /// Gets all popular photoshoot locations for a region
        /// </summary>
        /// <param name="regionId">Id of the region</param>
        /// <returns>IEnumerable of Place containing data for all the popular photoshoot locations for a region</returns>
        public async Task<IEnumerable<Place>> GetPopularPlacesByRegion(int regionId)
        {
            if(regionId == 0)
            {
                _logger.LogError("{var} is 0 in {Method}, {Class}, {DateTime}", nameof(regionId), nameof(GetPopularPlacesByRegion), nameof(PlaceService), DateTime.Now);
                return Enumerable.Empty<Place>();
            }

            try
            {
                return await _context.Places
                        .Include(p => p.PhotoBase)
                        .Where(p => p.City!.RegionId == regionId)
                        .ToListAsync();
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return Enumerable.Empty<Place>();
            }
        }

        /// <summary>
        /// Gets a photoshoot location by Id
        /// </summary>
        /// <param name="id">Id of the photoshoto location</param>
        /// <returns>Returns a DTO with data of the requested location</returns>
        public async Task<PlaceByIdDTO?> PlaceById(Guid id)
        {

            List<PhotoShootPhoto>? photos = new();

            try
            {
                Place? place = await _context.Places
                .Include(p => p.PhotoBase)
                .Include(p => p.PhotoShoots)
                    .ThenInclude(ps => ps.PhotoShootPhotos)
                .Include(p => p.City)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

                if (place != null && place?.PhotoBase == null)
                {
                    place!.PhotoBase = new();
                }

                //collect all photoshoot photos for this place
                foreach(PhotoShoot photoshoot in place!.PhotoShoots)
                {
                    foreach(PhotoShootPhoto photo in photoshoot.PhotoShootPhotos)
                    {
                        photos.Add(photo);
                    }
                }
                
                PlaceByIdDTO dto = new()
                {
                    Id = place?.Id ?? Guid.Empty,
                    Name = place?.Name,
                    Description = place?.Description,
                    GoogleMapsURL = place?.GoogleMapsURL,
                    IsPopular = place!.IsPopular,
                    PhotoFileName = place?.PhotoBase?.FileName,
                    Photos = photos,
                    PhotoBase = place?.PhotoBase ?? new()
                };

                return dto;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }
        
        /// <summary>
        /// Creates a new Place
        /// </summary>
        /// <param name="dto">DTO with the required data to create a new Place</param>
        /// <returns>True if successful, else false</returns>
        public async Task<Guid> Create(CreatePlaceDTO dto)
        {
            if(_context.Places is null)
            {
                _logger.LogError("{context} is null in {Method}, {Class}, {DateTime}", nameof(_context.Places), nameof(Create), nameof(PlaceService), DateTime.Now);
                return Guid.Empty;
            }

            try
            {
                Place place = new()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    GoogleMapsURL = dto.GoogleMapsURL,
                    IsPopular = dto.IsPopular,
                    CityId = dto.CityId,
                    CreatedById = dto.CreatedById,
                    CreatedOn = DateTime.Now,
                    PhotoBaseId = dto.PhotoBaseId == Guid.Empty ? null : dto.PhotoBaseId,
                };

                await _context.Places.AddAsync(place);
                await _context.SaveChangesAsync();

                return place.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Updates a photoshoot location
        /// </summary>
        /// <param name="place">Place containing the required data</param>
        /// <returns>Id of the updated Place</returns>
        public async Task<Guid> Update(Place place)
        {
            if (_context.Places is null)
            {
                _logger.LogError("{context} is null in {Method}, {Class}, {DateTime}", nameof(_context.Places), nameof(Update), nameof(PlaceService), DateTime.Now);
                return Guid.Empty;
            }

            try
            {
                Place? existingPlace = await _context.Places.FindAsync(place?.Id);

                if (existingPlace != null)
                {
                    existingPlace.Name = place?.Name;
                    existingPlace.Description = place?.Description;
                    existingPlace.GoogleMapsURL = place?.GoogleMapsURL;
                    existingPlace.IsPopular = place!.IsPopular;
                    await _context.SaveChangesAsync();
                }

                return existingPlace?.Id ?? Guid.Empty;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Updates the Id of a photo related to a location
        /// </summary>
        /// <param name="placeId">Id of the Place</param>
        /// <param name="photoId">Id of the photo</param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> UpdatePhotoId(Guid placeId, Guid photoId)
        {
            try
            {
                Place? existingPlace = await _context.Places.FindAsync(placeId);

                if (existingPlace != null)
                {
                    existingPlace.PhotoBaseId = photoId;
                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }
    }
}
