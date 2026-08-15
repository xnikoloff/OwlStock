using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class GalleryService : IGalleryService
    {
        private readonly PhotonicDbContext _context;
        private readonly IPhotoTagService _photoTagService;
        private readonly ILogger<GalleryService> _logger;

        public GalleryService(PhotonicDbContext context, IPhotoTagService photoTagService, ILogger<GalleryService> logger)
        {
            _context = context;
            _photoTagService = photoTagService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all gallery photos
        /// </summary>
        /// <returns>List of all GalleryPhotos</returns>
        public async Task<List<GalleryPhoto>> All()
        {
            if (_context.GalleryPhotos is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(All)}, {nameof(_context.GalleryPhotos)} is null");
                return new();
            }

            try
            {
                List<GalleryPhoto> galleryPhotos = await _context.GalleryPhotos
                    .Include(p => p.PhotoCategories)
                    .Include(p => p.Tags)
                    .ToListAsync();

                return galleryPhotos;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        /// <summary>
        /// Gets all photos that are connected to a photoshoot
        /// </summary>
        /// <param name="photoShootType">Type of the photoshoot</param>
        /// <returns>List of the photoshoot photos</returns>
        private async Task<List<PhotoShootPhoto>> AllPhotoshootPhotos(PhotoShootType photoShootType)
        {
            if (_context.PhotoShootPhotos is null)
            {
                 _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(AllPhotoshootPhotos)}, {nameof(_context.PhotoShootPhotos)} is null");
                return new();
                
            }

            if (_context.PhotoShoots is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(AllPhotoshootPhotos)}, {nameof(_context.PhotoShoots)} is null");
                return new();

            }

            try
            {
                List<PhotoShootPhoto> photos = await _context.PhotoShootPhotos
                    .Include(p => p.PhotoShoot)
                    .Where(p => p.PhotoShoot!.PhotoShootType == photoShootType)
                    .ToListAsync();

                return photos;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        /// <summary>
        /// Builds a dictionary that contains list of all gallery categories and the corresponding photos for each of the categories
        /// </summary>
        /// <returns>Dictionary of gallery categories with their corresponding photos</returns>
        public async Task<Dictionary<Category, List<GalleryPhoto?>>> BuildCategoriesGallery()
        {
            if(_context.PhotosCategories is null)
            {
                _logger.LogError($"{nameof(_context.PhotosCategories)} is null in {nameof(BuildCategoriesGallery)}, {nameof(FileService)}, {DateTime.Now}");
            }

            Dictionary<Category, List<GalleryPhoto?>> categoriesWithPhotos = new();

            //build a list with all enum values of Category
            IEnumerable<Category> categories = Enum.GetValues(typeof(Category)).Cast<Category>();

            try
            {
                foreach (Category category in categories)
                {
                    //get all photos for the current category
                    List<GalleryPhoto?> photos = await _context.PhotosCategories
                        .Where(pc => pc.Category == category && pc.GalleryPhoto.IsDeleted == false)
                        .Select(pc => pc.GalleryPhoto)
                        .ToListAsync() ?? new();

                    categoriesWithPhotos.Add(category, photos);
                }

                return categoriesWithPhotos;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while building categories gallery at {Time}", DateTime.UtcNow);
                return new Dictionary<Category, List<GalleryPhoto?>>();
            }
        }

        /// <summary>
        /// Gets all corresponding photos to a photoshoot type
        /// </summary>
        /// <param name="photoshootType">The photoshoot type</param>
        /// <returns>List of PhotoshootPhoto corresponding to a photoshoot type</returns>
        public async Task<List<PhotoShootPhoto>> AllByPhotoshootType(PhotoShootType photoshootType)
        {
            List<PhotoShootPhoto> photos = await AllPhotoshootPhotos(photoshootType);

            if(photos.Count == 0)
            {
                _logger.LogError(null, "An unspecified error occured while getting photoshoot photos in GalleryService, AllByPhotoshootType()");
                return new List<PhotoShootPhoto>();
            }

            return photos;
        }

        /// <summary>
        /// Gets all corresponding gallery photos to a gallery category 
        /// </summary>
        /// <param name="category">The gallery category</param>
        /// <returns>List of GalleryPhotos corresponding to a gallery category</returns>
        public async Task<List<GalleryPhoto>> AllByCategory(Category category)
        {
            List<GalleryPhoto> galleryPhotos = await All();

            return galleryPhotos
                .Where(gp => gp.PhotoCategories.Select(gp => gp.Category).Contains(category) && gp.IsDeleted == false)
                .ToList();
        }

        /// <summary>
        /// Gets all corresponding gallery photos to a search tag 
        /// </summary>
        /// <param name="tagText">The tag that is requested</param>
        /// <returns>List of GalleryPhotos corresponding to a search tag</returns>
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
