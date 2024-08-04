using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly OwlStockDbContext _context;
        private readonly IPhotoTagService _photoTagService;

        public GalleryService(OwlStockDbContext context, IPhotoTagService photoTagService)
        {
            _context = context;
            _photoTagService = photoTagService;
        }

        public async Task<List<GalleryPhoto>> All()
        {
            if(_context.GalleryPhotos is not null)
            {
                List<GalleryPhoto> galleryPhotos = await _context.GalleryPhotos
                    .Include(p => p.PhotoCategories)
                    .Include(p => p.Tags)
                    .ToListAsync();

                return galleryPhotos;
            }

            throw new NullReferenceException($"{_context.GalleryPhotos} is null");
        }

        private async Task<List<PhotoShootPhoto>> AllPhotoshootPhotos(PhotoShootType photoShootType)
        {
            if (_context.PhotoShootPhotos is not null)
            {
                List<PhotoShootPhoto> photos = await _context.PhotoShootPhotos
                    .Include(p => p.PhotoShoot)
                    .Where(p => p.PhotoShoot.PhotoShootType == photoShootType)
                    .ToListAsync();

                return photos;
            }

            throw new NullReferenceException($"{_context.PhotoShootPhotos} is null");
        }

        public async Task<List<PhotoShootPhoto>> AllByPhotoshootType(PhotoShootType photoshootType)
        {
            List<PhotoShootPhoto> photos = await AllPhotoshootPhotos(photoshootType);
            return photos;
        }

        public async Task<List<GalleryPhoto>> AllByCategory(Category category)
        {
            List<GalleryPhoto> galleryPhotos = await All();

            return galleryPhotos
                .Where(gp => gp.PhotoCategories.Select(gp => gp.Category).Contains(category))
                .ToList();
        }

        public async Task<List<GalleryPhoto>> AllByTags(string tagText)
        {
            List<Guid> idList = await _photoTagService.GetPhotoIdListByTag(tagText);
            List<GalleryPhoto> photosByTags = new();

            if (idList.Count == 0)
            {
                return new List<GalleryPhoto>();
            }

            List<GalleryPhoto> galleryPhotos = await All();  
            
             for(int i = 0; i < idList.Count; i++)
             {
                GalleryPhoto? galleryPhoto = galleryPhotos.Where(dto => dto.Id == idList[i]).FirstOrDefault();
               
                if (galleryPhoto != null)
                {
                    if (!photosByTags.Contains(galleryPhoto))
                    {
                        photosByTags.Add(galleryPhoto);
                    }
                }
             }

            return photosByTags;
        }
    }
}
