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

            GalleryPhoto? photo = await _context.GalleryPhotos
                .Include(gf => gf.Tags)
                .Include(gf => gf.PhotoCategories)
                .Include(gf => gf.Gear)
                .FirstOrDefaultAsync(gf => gf.Id == id) ?? 
                throw new NullReferenceException($"{nameof(photo)} is null");
            
            return new PhotoByIdDTO
            {
                Photo = photo,
                PhotoSize = PhotoSize.Large //set default size to large
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
                    photo.FilePath = Path.Combine("images", PhotoSize.OriginalSize.ToString() + "_" + photo.FileName).Replace('\\', '/');
                    ((GalleryPhoto)photo).FilePathSmall = Path.Combine("images", PhotoSize.Small.ToString() + "_" + photo.FileName).Replace('\\', '/');
                        await _context.GalleryPhotos!.AddAsync((GalleryPhoto)photo);
                    break;
                }

                case PhotoShootPhoto:
                {
                        //photoshootId is 0000-0000-0000
                    photo.FilePath = ExtractPath(photo.FilePath);
                    ((PhotoShootPhoto)photo).PhotoShootId = ((PhotoShootPhoto)photo).PhotoShoot.Id;
                    ((PhotoShootPhoto)photo).PhotoShoot = null;

                    await _context.PhotoShootPhotos!.AddAsync((PhotoShootPhoto)photo);
                    break;
                }

                default: throw new ArgumentException($"{nameof(photo)} has invalid type");
                
            }

            await _context.SaveChangesAsync();

            await UpdateBasePhotoId(photo);

            return photo.Id;
        }

        public async Task<PhotoBase> Delete(PhotoBase photo)
        {
            if (photo is null)
            {
                throw new NullReferenceException($"{nameof(photo)} is null");
            }

            if(_context.PhotosBase is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotosBase)} is null");
            }

            PhotoBase? photoBase = await _context.PhotosBase.FindAsync(photo.Id) ?? 
                throw new NullReferenceException($"{nameof(PhotoBase)} with Id {photo.Id} cannot be found");

            photoBase.IsDeleted = true;
            await _context.SaveChangesAsync();

            return photoBase;
        }

        public async Task<Guid> ChangeDownloadPermissions(Guid photoId)
        {
            if(_context.GalleryPhotos is null)
            {
                throw new NullReferenceException($"{_context.GalleryPhotos} is null");
            }

            GalleryPhoto? photo = await _context.GalleryPhotos.FindAsync(photoId) ?? 
                throw new NullReferenceException($"{nameof(GalleryPhoto)} with Id {photoId} cannot be found");

            photo.IsDownloadable = !photo.IsDownloadable;

            await _context.SaveChangesAsync();

            return photoId;
        }

        private static string ExtractPath(string filePath)
        {
            //find word "images" in the path string
            //check each 5 indexex, 0-5 -- 5-10 -- 10--15 till the end of the array
            //until word "image" is found
            //search each 5 chars because image has 5 letters
            //when word image is found -> save the index of 'i' in word "images"
            //that's the index where the path should be substringed
            //the substringed path goes to db as FilePath in PhotoBase

            int position = FindStartIndexPossition(filePath);

            return filePath.Substring(position);
        }

        private static int FindStartIndexPossition(string filePath)
        {
            int position = 0;

            for (int i = 0; i < filePath.Length; i++)
            {
                string word = "";

                for (int j = i; j < i + 11; j++)
                {
                    word += filePath[j];
                }

                if (word.Equals("photoshoots"))
                {
                    position = i;
                    break;
                }

            }

            return position;
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