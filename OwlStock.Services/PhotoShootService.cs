using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using System.Xml;

namespace OwlStock.Services
{
    public class PhotoShootService : IPhotoShootService
    {
        private readonly OwlStockDbContext _context;
        private IFileService _fileService;

        public PhotoShootService(OwlStockDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;

        }

        public async Task<List<PhotoShoot>> AllPhotoShoots()
        {
            if(_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }
            
            return await _context.PhotoShoots.ToListAsync();
        }

        public async Task<PhotoShootByIdDTO> PhotoShootById(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException($"{nameof(id)} is or less than 0");
            }

            if(_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            List<string> files = await _fileService.GetFilesPathsForPhotoShoot(id);

            PhotoShootByIdDTO? dto = await _context.PhotoShoots
                .Select(phs => new PhotoShootByIdDTO
                {
                    Id = phs.Id,
                    PersonFullName = phs.PersonFullName,
                    ReservationDate = phs.ReservationDate,
                    PhotoShootType = phs.PhotoShootType,
                    PhotoShootTypeDescription = phs.PhotoShootTypeDescription,
                    CreatedOn = phs.CreatedOn,
                    IdentityUserId = phs.IdentityUserId,
                    FilePaths = files

                })
                .Where(phs => phs.Id == id)
                .FirstOrDefaultAsync();

            if(dto == null) 
            {
                throw new NullReferenceException($"{nameof(dto)} is null");
            }

            return dto;
        }

        public async Task<List<MyPhotoShootsDTO>> MyPhotoShoots(string userId)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if(_context.PhotoShoots is null)
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

        public async Task<int> Add(CreatePhotoShootDTO dto)
        {
            if(dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            PhotoShoot photoShoot = new()
            {
                PersonFirstName = dto.PersonFirstName,
                PersonLastName = dto.PersonLastName,
                PersonFullName = dto.PersonFirstName + " " + dto.PersonLastName,
                PersonEmail = dto.PersonEmail,
                PersonPhone = dto.PersonPhone,
                ReservationDate = dto.ReservationDate,
                PhotoShootType = dto.PhotoShootType,
                PhotoShootTypeDescription = dto.PhotoShootTypeDescription,
                CreatedOn = DateTime.Now,
                IdentityUserId = dto.IdentityUserId
            };

            await _context.AddAsync(photoShoot);

            return await _context.SaveChangesAsync();
        }

        public Task<List<PhotoShoot>> ShowAvailableSlots()
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
