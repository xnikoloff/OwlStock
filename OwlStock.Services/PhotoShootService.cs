using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot;
using OwlStock.Services.Common.HelperClasses;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class PhotoShootService : IPhotoShootService
    {
        private readonly OwlStockDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ICalendarService _calendarService;
        //private readonly ICalculationsService _calculationsService;
        //private readonly ISettlementService _settlementService;

        public PhotoShootService(OwlStockDbContext context, IEmailService emailService, ICalendarService calendarService
            /*ICalculationsService calculationsService, ISettlementService settlementService*/)
        {
            _context = context;
            _emailService = emailService;
            _calendarService = calendarService;
            //_calculationsService = calculationsService;
            //_settlementService = settlementService;
        }

        public async Task<PhotoShoot> PhotoShootById(Guid id)
        {
            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            //List<string> files = await _fileService.GetFilesNamesForPhotoShoot(id);

            PhotoShoot? dto = await _context.PhotoShoots
                .Include(phs => phs.PhotoShootPhotos)
                .Where(phs => phs.Id == id)
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                throw new NullReferenceException($"{nameof(dto)} is null");
            }

            return dto;
        }

        public async Task<List<MyPhotoShootsDTO>> MyPhotoShoots(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            List<MyPhotoShootsDTO> myPhotoShoots = await _context.PhotoShoots
                .Where(phs => phs.IdentityUserId == userId)
                .Select(phs => new MyPhotoShootsDTO
                {
                    Id = phs.Id,
                    CreatedOn = phs.CreatedOn,
                    PhotoShootType = phs.PhotoShootType,
                    ReservationDate = phs.ReservationDate,
                    ReservationFor = phs.PersonFullName

                })
                .ToListAsync();

            return myPhotoShoots;
        }

        public async Task<PhotoShoot> Add(CreatePhotoShootDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if(_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            //not used for now
            //double[] settlementLatAndLon = await _settlementService.GetLatitudeAndLongitude(dto.SettlementName ?? 
            //    throw new NullReferenceException($"{nameof(dto.SettlementName)} is null or empty"));

            PhotoShoot photoShoot = new()
            {
                PersonFirstName = dto.PersonFirstName,
                PersonLastName = dto.PersonLastName,
                PersonFullName = dto.PersonFirstName + " " + dto.PersonLastName,
                PersonEmail = dto.PersonEmail,
                PersonPhone = dto.PersonPhone,
                ReservationDate = new DateTime(dto.ReservationDate.Year, dto.ReservationDate.Month, dto.ReservationDate.Day, dto.ReservationTime.Hour, dto.ReservationTime.Minute, 0),
                PhotoShootType = dto.PhotoShootType,
                PhotoShootTypeDescription = dto.PhotoShootTypeDescription,
                CreatedOn = DateTime.Now,
                UserPlace = dto.UserPlace,
                GoogleMapsLink = dto.GoogleMapsLink,
                IsDecidedByUs = dto.IsDecidedByUs,
                Price = dto.Price,
                IdentityUserId = dto.IdentityUserId
            };

            //not used for now
            //double timeForTravel = _calculationsService.CalculateTimeForTravel(DefaultValue.DefaultSettlementLatitude, DefaultValue.DefaultSettlementLongitude,
            //    settlementLatAndLon[0], settlementLatAndLon[1]);

            await _context.AddAsync(photoShoot);
            await _context.SaveChangesAsync();

            PhotoShoot? photoShootResult = await _context.PhotoShoots
                .OrderByDescending(ph => ph.Id)
                .LastOrDefaultAsync() ?? 
                    throw new NullReferenceException($"No records found");

            PhotoShootEmailTemplateDTO emailDto = new()
            {
                Date = new DateTime(dto.ReservationDate.Year, dto.ReservationDate.Month, dto.ReservationDate.Day, dto.ReservationTime.Hour, dto.ReservationTime.Minute, 0),
                Recipient = dto.PersonEmail,
                Type = dto.PhotoShootType,
                PersonFullName = dto.PersonFirstName + " " + dto.PersonLastName,
                EmailTemplate = EmailTemplate.CreatePhotoShoot
            };

            await _emailService.Send(emailDto);

            return photoShootResult;
        }

        public async Task<Dictionary<DateOnly, IEnumerable<TimeSlot>>> GetPhotoShootsCalendar()
        {
            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            //Get reservation dates from today's date forward
            List<DateTime> reservationDates = await _context.PhotoShoots
                .Where(p => p.ReservationDate.Date >= DateTime.Now.Date)
                .Select(ph => ph.ReservationDate)
                .OrderBy(p => p.Date)
                .ToListAsync();

            return _calendarService.GetPhotoShootsCalendar(reservationDates);
        }
    }
}