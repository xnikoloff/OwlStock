using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class PhotoShootService : IPhotoShootService
    {
        private readonly OwlStockDbContext _context;

        public PhotoShootService(OwlStockDbContext context)
        {
            _context = context;
        }

        public async Task<List<PhotoShoot>> AllReservations()
        {
            if(_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }
            
            return await _context.PhotoShoots.ToListAsync();
        }

        public async Task<PhotoShoot> ReservationById(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException($"{nameof(id)} is or less than 0");
            }

            if(_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            PhotoShoot? photoShoot = await _context.PhotoShoots
                .Where(phs => phs.Id == id)
                .FirstOrDefaultAsync();

            if(photoShoot == null) 
            {
                throw new NullReferenceException($"{nameof(photoShoot)} is null");
            }

            return photoShoot;
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

        public async Task<int> Reserve(CreatePhotoShootDTO dto)
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
