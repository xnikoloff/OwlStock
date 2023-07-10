using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class PhotoShootService : IPhotoShootService
    {
        private readonly OwlStockDbContext _context;
        private readonly IEmailService _emailService;

        public PhotoShootService(OwlStockDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<PhotoShoot>> AllPhotoShoots()
        {
            if(_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }
            
            return await _context.PhotoShoots.ToListAsync();
        }

        public async Task<PhotoShoot> PhotoShootById(Guid id)
        { 
            if(_context.PhotoShoots is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShoots)} is null");
            }

            //List<string> files = await _fileService.GetFilesNamesForPhotoShoot(id);

            PhotoShoot? dto = await _context.PhotoShoots
                .Include(phs => phs.PhotoShootPhotos)
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

            int result = await _context.SaveChangesAsync();

            PhotoShootEmailTemplateDTO emailDto = new()
            {
                Date = dto.ReservationDate,
                Recipient = dto.PersonEmail,
                Type = dto.PhotoShootType,
                PersonFullName = dto.PersonFirstName + " " + dto.PersonLastName,
                EmailTemplate = EmailTemplate.CreatePhotoShoot
            };

            await _emailService.Send(emailDto);

            return result;
        }

        /*public async Task AddFiles(Guid photoShootId, List<IFormFile> files, string? webRootPath, PhotoSize? size)
        {
            string photoShootsFolder = "";
            if(webRootPath == null)
            {
                throw new NullReferenceException($"{nameof(webRootPath)} is null");
            }
            
            foreach (IFormFile file in files)
            {
                photoShootsFolder = Path.Combine(webRootPath, size == null ? "images/photoshoots" : "images");
                string filePath = Path.Combine(photoShootsFolder, size == null ? file.FileName : size.ToString() + "_" + file.FileName);

                //iformfile to byte array
                byte[] data =  _fileService.ConvertFormFileToByteArray(file);
                byte[]? resised = null;

                //resize
                if (size != null)
                {
                    byte[] bytes = _photoResizer.Resize(data, size.Value);
                    resised = bytes;
                }

                _fileService.Create(resised ?? data, webRootPath, filePath);
                
            }

            await _fileService.CreatePhotoShootFiles(files, photoShootId, webRootPath);

            PhotoShoot? photoShoot = await _context.PhotoShoots!
                .Where(ps => ps.Id == photoShootId).FirstOrDefaultAsync() ?? 
                    throw new NullReferenceException($"{nameof(PhotoShoot)} with id {photoShootId} cannot be found");

            UpdatePhotoShootEmailTemplateDTO dto = new()
            {
                EmailTemplate = EmailTemplate.UpdatePhotosForPhotoShoot,
                PersonFullName = photoShoot.PersonFullName,
                Recipient = photoShoot.PersonEmail,
                Url = $"https:///flashstudio.com/photoshoot/{photoShootId}/"
            };

            await _emailService.Send(dto);
        }*/

        public Task<List<PhotoShoot>> ShowAvailableSlots()
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
