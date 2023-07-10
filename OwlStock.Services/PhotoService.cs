using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly OwlStockDbContext _context;
        
        public PhotoService(OwlStockDbContext context)
        {
            _context = context;
        }

        public async Task<PhotoByIdDTO> GetById(Guid? id)
        {
            if(id is null)
            {
                throw new NullReferenceException($"{nameof(id)} is null");
            }

            if(_context.GalleryPhotos is null)
            {
                throw new NullReferenceException($"{nameof(_context.GalleryPhotos)} is null");
            }

            GalleryPhoto? photo = await _context.GalleryPhotos.FindAsync(id) ?? 
                throw new NullReferenceException($"{nameof(photo)} is null");
            
            return new PhotoByIdDTO
            {
                Photo = photo,
                PhotoSize = PhotoSize.Large
            };
        }

        public async Task<Guid> Create(PhotoBase? photo)
        {
            if(photo is null)
            {
                throw new NullReferenceException($"{nameof(photo)} is null");
            }

            if (string.IsNullOrEmpty(photo.FileName))
            {
                throw new NullReferenceException($"{nameof(photo.FileName)} is null or empty");
            }

            switch (photo)
            {
                case GalleryPhoto:
                {
                    photo.FilePath = Path.Combine("images", PhotoSize.OriginalSize.ToString() + "_" + photo.FileName);
                    ((GalleryPhoto)photo).FilePathSmall = Path.Combine("images", PhotoSize.Small.ToString() + "_" + photo.FileName);
                        await _context.GalleryPhotos!.AddAsync((GalleryPhoto)photo);
                    break;
                }

                case PhotoShootPhoto:
                {
                    photo.FilePath = Path.Combine("\\images\\photoshoots", photo.FileName);
                    await _context.PhotoShootPhotos!.AddAsync((PhotoShootPhoto)photo);
                    break;
                }

                default: throw new ArgumentException($"{nameof(photo)} has invalid type");
                
            }

            await _context.SaveChangesAsync();

            await UpdateBasePhotoId(photo);

            return photo.Id;
        }

        private async Task UpdateBasePhotoId(PhotoBase photo)
        {
            Guid basePhotoId = await _context.PhotosBase!
                .OrderByDescending(pb => pb.Id)
                .Select(pb => pb.Id)
                .FirstOrDefaultAsync();

            switch(photo)
            {
                case GalleryPhoto:
                {
                    GalleryPhoto galleryPhoto = await _context.GalleryPhotos!.OrderByDescending(p => p.Id).FirstOrDefaultAsync() ?? 
                        throw new NullReferenceException("No Gallery Photos are found");

                    galleryPhoto.PhotoBaseId = basePhotoId;
                    break;
                }

                case PhotoShootPhoto:
                {
                    PhotoShootPhoto photoShootPhoto = await _context.PhotoShootPhotos!.OrderByDescending(p => p.Id).FirstOrDefaultAsync()??
                        throw new NullReferenceException("No Photo Shoot Photos are found"); ;
                    
                    photoShootPhoto.PhotoBaseId = basePhotoId;
                    break;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}