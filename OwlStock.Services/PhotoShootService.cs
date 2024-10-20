﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ICalculationsService _calculationsService;

        public PhotoShootService(OwlStockDbContext context, IEmailService emailService, ICalendarService calendarService, ICalculationsService calculationsService)
        {
            _context = context;
            _emailService = emailService;
            _calendarService = calendarService;
            _calculationsService = calculationsService;
        }

        public async Task<IEnumerable<PhotoShoot>> GetAll()
        {
            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            return await _context.PhotoShoots
                .Include(ph => ph.IdentityUser)
                .OrderByDescending(ph => ph.Id)
                .ToListAsync();
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
                .Include(phs => phs.Place)
                    .ThenInclude(p => p.City)
                    .ThenInclude(c => c.Municipality)
                    .ThenInclude(m => m.Region)
                .Where(phs => phs.Id == id)
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                throw new NullReferenceException($"{nameof(dto)} is null");
            }

            return dto;
        }

        public async Task<PhotoShoot?> PhotoShootById(Guid id, string userId)
        {
            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            //List<string> files = await _fileService.GetFilesNamesForPhotoShoot(id);

            PhotoShoot? dto = await _context.PhotoShoots
                .Include(phs => phs.PhotoShootPhotos)
                .Include(phs => phs.Place)
                    .ThenInclude(p => p.City)
                    .ThenInclude(c => c.Region)
                .Where(phs => phs.Id == id && phs.IdentityUserId!.Equals(userId))
                .FirstOrDefaultAsync();

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
                    ReservationFor = phs.PersonFullName,
                    PhotoDeliveryMethod = phs.PhotoDeliveryMethod
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

            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            decimal totalPrice = _calculationsService.CalculatePhotoshootPrice(dto.PhotoShootType, dto.FuelPrice);
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
                //UserPlace = dto.UserPlace, //not used anymore
                //GoogleMapsLink = dto.GoogleMapsLink, //not used anymore
                IsDecidedByUs = dto.IsDecidedByUs,
                DoNotUploadPhotos = dto.DoNotUploadPhotos,
                PhotoDeliveryMethod = dto.PhotoDeliveryMethod,
                PhotoDeliveryAddress = dto.PhotoDeliveryAddress,
                Price = totalPrice,
                IdentityUserId = dto.IdentityUserId,
                Status = PhotoshootStatus.New,
                PlaceId = dto.PlaceId == Guid.Empty ? null : dto.PlaceId
            };

            //not used anymore
            //now the PhotoShoot has PlaceId that contains the 
            //full information about Place's City and Region
            /*if(dto.IsPlace)
            {
                photoShoot.PlaceId = new Guid(dto.SelectedSettlementId ?? throw new ArgumentNullException(dto.SelectedSettlementId));
            }

            else
            {
                photoShoot.CityId = (Convert.ToInt32(dto.SelectedSettlementId));
            }*/

            //not used for now
            //double timeForTravel = _calculationsService.CalculateTimeForTravel(DefaultValue.DefaultSettlementLatitude, DefaultValue.DefaultSettlementLongitude,
            //    settlementLatAndLon[0], settlementLatAndLon[1]);

            await _context.AddAsync(photoShoot);
            await _context.SaveChangesAsync();

            PhotoShoot? photoShootResult = await _context.PhotoShoots
                .OrderByDescending(ph => ph.Id)
                .FirstOrDefaultAsync() ??
                    throw new NullReferenceException($"No records found");

            PhotoShootEmailTemplateDTO emailDto = new()
            {
                Date = new DateTime(dto.ReservationDate.Year, dto.ReservationDate.Month, dto.ReservationDate.Day, dto.ReservationTime.Hour, dto.ReservationTime.Minute, 0),
                Topic = "Успешна резервация",
                Recipient = dto.PersonEmail,
                Type = dto.PhotoShootType,
                PersonFullName = dto.PersonFirstName + " " + dto.PersonLastName,
                EmailTemplate = EmailTemplate.CreatePhotoShoot,
                PhotoShootId = photoShootResult.Id
            };

            await _emailService.Send(emailDto);

            return photoShootResult;
        }

        public async Task<PhotoShoot> Update(ManagePhotoshootDTO dto)
        {
            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            if (dto.Id == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(dto.Id)}");
            }

            PhotoShoot? existingPhotoShoot = await _context.PhotoShoots.FindAsync(dto.Id) ??
                throw new NullReferenceException($"{nameof(existingPhotoShoot)} with id ${dto?.Id} does not exists");

            existingPhotoShoot.PersonFullName = dto.PersonFullName;
            existingPhotoShoot.ReservationDate = dto.ReservationDate;
            existingPhotoShoot.PersonPhone = dto.PersonPhone;
            existingPhotoShoot.PhotoShootType = dto.PhotoShootType;
            //existingPhotoShoot.UserPlace = dto.UserPlace;
            //existingPhotoShoot.GoogleMapsLink = dto.GoogleMapsLink;
            existingPhotoShoot.Price = dto.Price;
            existingPhotoShoot.PhotoDeliveryMethod = dto.PhotoDeliveryMethod;
            existingPhotoShoot.PhotoDeliveryAddress = dto.PhotoDeliveryAddress;

            await _context.SaveChangesAsync();

            return existingPhotoShoot;
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

        public async Task<PhotoShoot> ChangeStatus(Guid id, PhotoshootStatus status)
        {
            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", $"{nameof(id)}");
            }

            PhotoShoot? photoShoot = await _context.PhotoShoots.FindAsync(id) ??
                throw new NullReferenceException($"{nameof(photoShoot)} with id ${id} does not exists");


            UpdatePhotoShootEmailTemplateDTO photoShootEmailTemplateDTO = new()
            {
                Recipient = photoShoot.PersonEmail,
                PhotoShootId = photoShoot.Id
            };

            //set status and topic for DTO
            switch (status)
            {
                case PhotoshootStatus.Declined:
                {
                    photoShootEmailTemplateDTO.EmailTemplate = EmailTemplate.DeclinePhotoShoot;
                    photoShootEmailTemplateDTO.Topic = "Отхвърлена фотосесия";
                    break;
                }

                case PhotoshootStatus.Cancelled:
                {
                    photoShootEmailTemplateDTO.EmailTemplate = EmailTemplate.CancelPhotoShoot;
                    photoShootEmailTemplateDTO.Topic = "Отказана фотосесия";
                    break;
                }
            }

            photoShoot.Status = status;
            await _context.SaveChangesAsync();

            await _emailService.Send(photoShootEmailTemplateDTO);

            return photoShoot;
        }

        public async Task<string> GetPersonName(Guid id)
        {
            if (_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", $"{nameof(id)}");
            }

            string? name = await _context.PhotoShoots
                .Where(ps => ps.Id == id)
                .Select(ps => ps.PersonFirstName + ps.PersonLastName)
                .FirstOrDefaultAsync();

            return name ?? throw new NullReferenceException($"{nameof(name)} is null or empty");
        }
    }
}