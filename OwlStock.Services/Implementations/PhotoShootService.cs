using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using SixLabors.ImageSharp;

namespace OwlStock.Services.Implementations
{
    public class PhotoShootService : IPhotoShootService
    {
        private readonly PhotonicDbContext _context;
        private readonly ILogger<PhotoShootService> _logger;

        public PhotoShootService(PhotonicDbContext context, ILogger<PhotoShootService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Sets a manually reserved date by the admin
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> SetReservedDate (DateTime date)
        {
            try
            {
                PhotoShoot photoshoot = new()
                {
                    CreatedOn = DateTime.Now,
                    ReservationDate = date,
                    PhotoShootTypeDescription = "Reserved by admin",
                    Status = PhotoshootStatus.Service
                };

                await _context.PhotoShoots.AddAsync(photoshoot);
                int result = await _context.SaveChangesAsync();

                if(result == 0)
                {
                    return false;
                }

                return true;
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }


        /// <summary>
        /// Gets all photoshoots
        /// </summary>
        /// <returns>List of PhotoShoot</returns>
        public async Task<IEnumerable<PhotoShoot>> GetAll()
        {
            try
            {
                return await _context.PhotoShoots
                .Include(ph => ph.IdentityUser)
                .OrderByDescending(ph => ph.Id)
                .ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new List<PhotoShoot>();
            }
        }

        /// <summary>
        /// Gets a photoshoot by Id
        /// </summary>
        /// <param name="id">Id of the photoshoot</param>
        /// <returns>A PhotoShoot object</returns>
        public async Task<PhotoShoot> PhotoShootById(Guid id)
        {

            if (id == Guid.Empty)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(PhotoShootById)}, {nameof(id)} is empty");
                return new();
            }

            try
            {
                PhotoShoot? dto = await _context.PhotoShoots
                .Include(phs => phs.PhotoShootPhotos)
                .Include(phs => phs.Place)
                    .ThenInclude(p => p.City)
                    .ThenInclude(c => c.Municipality)
                    .ThenInclude(m => m.Region)
                .Where(phs => phs.Id == id)
                .FirstOrDefaultAsync();

                return dto;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        /// <summary>
        /// Gets a photoshoot by Id for a concrete user
        /// </summary>
        /// <param name="id">Id of the photoshoot</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>A PhotoShootByIdDTO with the required data</returns>
        public async Task<PhotoShootByIdDTO?> PhotoShootById(Guid id, string userId)
        {

            if (id == Guid.Empty)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(PhotoShootById)}, {nameof(id)} is empty");
                return new();
            }

            if (userId.IsNullOrEmpty())
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(PhotoShootById)}, {nameof(userId)} is null or empty");
                return new();
            }

            try
            {
                PhotoShoot? photoshoot = await _context.PhotoShoots
                .Include(phs => phs.PhotoShootPhotos)
                .Include(phs => phs.Place)
                    .ThenInclude(p => p.City)
                    .ThenInclude(c => c.Region)
                .Where(phs => phs.Id == id && phs.IdentityUserId!.Equals(userId))
                .FirstOrDefaultAsync();

                if (photoshoot == null)
                {
                    //return empty guid if photoshoot is not found
                    return new PhotoShootByIdDTO()
                    {
                        Id = Guid.Empty
                    };
                }

                PhotoShootByIdDTO dto = new()
                {
                    Id = photoshoot.Id,
                    PhotoshootNumber = photoshoot.PhotoshootNumber,
                    NumberOfParticipants = photoshoot.NumberOfParticipants,
                    PersonFullName = photoshoot.PersonFullName,
                    PersonPhone = photoshoot.PersonPhone,
                    Status = photoshoot.Status,
                    ReservationDate = photoshoot.ReservationDate,
                    PhotoShootType = photoshoot.PhotoShootType,
                    PhotoShootTypeDescription = photoshoot?.PhotoShootTypeDescription,
                    CreatedOn = photoshoot!.CreatedOn,
                    IsPopularPlaceSelected = photoshoot?.PlaceId != null,
                    Place = photoshoot?.Place?.Name,
                    Settlement = photoshoot?.Place?.City?.Name,
                    Region = photoshoot?.Place?.City?.Region?.Name,
                    PhotoDeliveryAddress = photoshoot?.PhotoDeliveryAddress,
                    PhotoDeliveryMethod = photoshoot?.PhotoDeliveryMethod,
                    UIC = photoshoot!.UIC,
                    Price = photoshoot.Price,
                    TransportCustomer = photoshoot.TransportCustomer,
                    PickUpAddress = photoshoot?.PickUpAddress,
                    IsSmallProduct = photoshoot!.IsSmallProduct,
                    PhotoShootPhotos = photoshoot?.PhotoShootPhotos,
                    IdentityUserId = userId,
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
        /// Gets all photoshoots for a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>List of PhotoShootsDTO objects containing the required data</returns>
        public async Task<List<MyPhotoShootsDTO>> PhotoShootsByUser(string userId)
        {
            if (userId.IsNullOrEmpty())
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(PhotoShootsByUser)}, {nameof(userId)} is empty");
                return new();
            }

            List<MyPhotoShootsDTO> myPhotoShoots = await _context.PhotoShoots
                .Where(phs => phs.IdentityUserId == userId)
                .Select(phs => new MyPhotoShootsDTO
                {
                    Id = phs.Id,
                    CreatedOn = phs.CreatedOn,
                    PhotoShootType = phs.PhotoShootType,
                    ReservationDate = phs.ReservationDate,
                    ReservationFor = phs.PersonFullName,
                    PhotoDeliveryMethod = phs.PhotoDeliveryMethod,
                    Price = phs.Price,
                    PhotoshootStatus = phs.Status,
                    IsSmallProduct = phs.IsSmallProduct
                })
                .OrderByDescending(phs => phs.ReservationDate)
                .ToListAsync();

            return myPhotoShoots;
        }

        /// <summary>
        /// Creates new photoshoot
        /// </summary>
        /// <param name="dto">DTO with the required data</param>
        /// <returns>Id of the created photoshoot</returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<Guid> Add(CreateRegularPhotoShootDTO dto)
        {
            if (_context.PhotoShoots is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Add)}, {nameof(_context.PhotoShoots)} is null");
                return Guid.Empty;
            }

            try
            {
                string number = GeneratePhotoshootNumber(dto.PersonEmail ?? throw new NullReferenceException($"{nameof(dto.PersonEmail)} is null"), dto.PhotoShootType);
                
                if(number.IsNullOrEmpty())
                {
                    _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Add)}, {nameof(GeneratePhotoshootNumber)} returned empty string");
                    return Guid.Empty;
                }

                PhotoShoot photoShoot = new()
                {
                    NumberOfParticipants = dto.NumberOfParticipants,
                    PersonFirstName = dto.PersonFirstName,
                    PersonLastName = dto.PersonLastName,
                    PersonFullName = dto.PersonFirstName + " " + dto.PersonLastName,
                    PersonEmail = dto.PersonEmail,
                    PersonPhone = dto.PersonPhone,
                    ReservationDate = new DateTime(dto.ReservationDate.Year, dto.ReservationDate.Month, dto.ReservationDate.Day, dto.ReservationTime.Hour, dto.ReservationTime.Minute, 0),
                    PhotoShootType = dto.PhotoShootType,
                    PhotoShootTypeDescription = dto.PhotoShootTypeDescription,
                    CreatedOn = DateTime.Now,
                    IsDecidedByUs = dto.IsDecidedByUs,
                    DoNotUploadPhotos = dto.DoNotUploadPhotos,
                    PhotoDeliveryMethod = dto.PhotoDeliveryMethod,
                    PhotoDeliveryAddress = dto.PhotoDeliveryAddress,
                    UIC = dto.UIC,
                    Price = dto.Price,
                    IdentityUserId = dto.IdentityUserId,
                    Status = PhotoshootStatus.New,
                    PlaceId = dto.PlaceId == Guid.Empty ? null : dto.PlaceId,
                    PhotoshootNumber = number,
                    TransportCustomer = dto.TransportCustomer,
                    PickUpAddress = dto.PickUpAddress
                };

                await _context.AddAsync(photoShoot);
                await _context.SaveChangesAsync();

                return photoShoot.Id;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Adds new photoshoot for a small product
        /// </summary>
        /// <param name="dto">DTO with the required data</param>
        /// <returns>Id of the created photoshoot</returns>
        public async Task<Guid> AddSmallProductPhotoShoot(CreateSmallProductPhotoshootDTO dto)
        {
            try
            {
                string number = GeneratePhotoshootNumber(dto.PersonEmail, dto.PhotoShootType);

                if(number.IsNullOrEmpty())
                {
                    _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(AddSmallProductPhotoShoot)}, {nameof(GeneratePhotoshootNumber)} returned empty string");
                    return Guid.Empty;
                }

                await _context.PhotoShoots.AddAsync(new()
                {
                    PersonFirstName = dto.PersonFirstName,
                    PersonLastName = dto.PersonLastName,
                    PersonFullName = dto.PersonFirstName + " " + dto.PersonLastName,
                    PersonEmail = dto.PersonEmail,
                    PersonPhone = dto.PersonPhone,
                    PhotoShootType = dto.PhotoShootType,
                    PhotoShootTypeDescription = dto.PhotoShootTypeDescription,
                    CreatedOn = DateTime.Now,
                    DoNotUploadPhotos = dto.DoNotUploadPhotos,
                    PhotoDeliveryMethod = dto.PhotoDeliveryMethod,
                    PhotoDeliveryAddress = dto.PhotoDeliveryAddress,
                    Price = dto.Price,
                    IdentityUserId = dto.IdentityUserId,
                    Status = PhotoshootStatus.New,
                    PhotoshootNumber = number,
                    IsSmallProduct = dto.IsSmallProduct
                });

                int result = await _context.SaveChangesAsync();

                PhotoShoot? photoShootResult = await _context.PhotoShoots
                    .OrderByDescending(ph => ph.Id)
                    .FirstOrDefaultAsync();

                if (photoShootResult is null)
                {
                    _logger.LogError($"${nameof(photoShootResult)} is null at {DateTime.UtcNow}, {nameof(AddSmallProductPhotoShoot)}, {nameof(PhotoShootService)}");
                    return Guid.Empty;
                }

                if (result == 0)
                {
                    return Guid.Empty;
                }

                return photoShootResult.Id;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Updates a photoshoot
        /// </summary>
        /// <param name="dto">DTO with the required data</param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> Update(UpdatePhotoShootDTO dto)
        {
            if (dto.Id == Guid.Empty)
            {
                _logger.LogError($"An error occurred at {DateTime.UtcNow}, Service: {nameof(PhotoShootService)}, {nameof(Update)}, {nameof(dto.Id)} is empty");
                return false;
            }

            PhotoShoot? existingPhotoShoot = await _context.PhotoShoots.FindAsync(dto.Id);

            if (existingPhotoShoot == null)
            {
                _logger.LogError($"An error occurred at {DateTime.UtcNow}, Service: {nameof(PhotoShootService)}, {nameof(Update)}, {nameof(existingPhotoShoot)} with id {dto.Id} does not exist");
                return false;
            }

            try
            {
                existingPhotoShoot.PersonEmail = dto.Email;
                existingPhotoShoot.PersonPhone = dto.Phone;
                existingPhotoShoot.ReservationDate = dto.ReservationDate;
                existingPhotoShoot.IsDecidedByUs = dto.IsDecidedByUs;
                existingPhotoShoot.UIC = dto.UIC;
                existingPhotoShoot.Price = dto.Price;
                existingPhotoShoot.DoNotUploadPhotos = dto.DoNotUploadPhotos;
                existingPhotoShoot.PhotoDeliveryMethod = dto.PhotoDeliveryMethod;
                existingPhotoShoot.PhotoDeliveryAddress = dto.PhotoDeliveryAddress;
                existingPhotoShoot.TransportCustomer = dto.TransportCustomer;
                existingPhotoShoot.PickUpAddress = dto.PickUpAddress;
                existingPhotoShoot.IsSmallProduct = dto.IsSmallProduct;
                existingPhotoShoot.PhotoDeliveryMethod = dto.PhotoDeliveryMethod;
                existingPhotoShoot.PhotoDeliveryAddress = dto.PhotoDeliveryAddress;

                await _context.SaveChangesAsync();
                return true;
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        /// <summary>
        /// Gets all dates that have reseved photoshoots
        /// </summary>
        /// <returns>List of DateTime</returns>
        public async Task<IEnumerable<DateTime>> GetReservedDates()
        {
            //Get reservation dates from today's date forward
            try
            {
                return await _context.PhotoShoots
                .Where(p => p.ReservationDate.Date >= DateTime.Now.Date && p.Status != PhotoshootStatus.Declined && p.Status != PhotoshootStatus.Cancelled)
                .Select(ph => ph.ReservationDate)
                .OrderBy(p => p.Date)
                .ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new List<DateTime>();
            }
        }

        /// <summary>
        /// Changes the status of a photoshoot
        /// </summary>
        /// <param name="id">Id of the photoshoot</param>
        /// <param name="status">The new status</param>
        /// <returns>ChangePhotoshootStatusDTO object with the required data</returns>
        public async Task<ChangePhotoshootStatusDTO> ChangeStatus(Guid id, PhotoshootStatus status)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError($"{nameof(id)} is empty at {DateTime.UtcNow} in {nameof(PhotoService)}, {nameof(ChangeStatus)}");
                return new ChangePhotoshootStatusDTO();
            }

            PhotoShoot? photoShoot = await _context.PhotoShoots.FindAsync(id);

            if (photoShoot == null)
            {
                _logger.LogError($"An error occurred at {DateTime.UtcNow}, Service: {nameof(PhotoShootService)}, {nameof(ChangeStatus)}, {nameof(photoShoot)} with id {id} does not exist");
                return new();
            }

            photoShoot.Status = status;
            await _context.SaveChangesAsync();
            
            return new()
            {
                Id = photoShoot.Id,
                PersonEmail = photoShoot.PersonEmail
            };
        }

        /// <summary>
        /// Gets the name of the person from a photoshoot
        /// </summary>
        /// <param name="id">Id of the photoshoot</param>
        /// <returns>Name of the person as string</returns>
        public async Task<string> GetPersonName(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetPersonName)}, {nameof(id)} is empty");
                return string.Empty;
            }

            try
            {
                string? name = await _context.PhotoShoots
                .Where(ps => ps.Id == id)
                .Select(ps => ps.PersonFirstName + ps.PersonLastName)
                .FirstOrDefaultAsync();

                return name ?? string.Empty;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return string.Empty;
            }
        }

        /// <summary>
        /// Generates a number for a photoshoot by combining PH- with the last two digits of the current year, month and day plus the
        /// first three letters from the photoshoot type plus the first three letters of the user email plus three random
        /// letters from the alphabet
        /// </summary>
        /// <param name="email">Email of the user who reserved the photoshoot</param>
        /// <param name="photoShootType">Type of the reserved photoshoot</param>
        /// <returns>the photoshoot number as string</returns>
        private string GeneratePhotoshootNumber(string email, PhotoShootType photoShootType)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //generate three random numbers to get three random letters from the alphabet
            Random random = new();
            int randomNumber1 = random.Next(0, alphabet.Length);
            int randomNumber2 = random.Next(0, alphabet.Length);
            int randomNumber3 = random.Next(0, alphabet.Length);

            try
            {
                string number =
                    "PH-" +
                    DateTime.Now.Year.ToString()[2..] +
                    DateTime.Now.Month +
                    DateTime.Now.Day +
                    photoShootType.ToString()[..3].ToUpper() +
                    email.ToUpper()[..3] + "-" +
                    alphabet[randomNumber1] + alphabet[randomNumber2] + alphabet[randomNumber3];

                return number;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return string.Empty;
            }
        }
    }
}