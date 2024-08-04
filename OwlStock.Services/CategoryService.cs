using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Infrastructure.Common;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly OwlStockDbContext _context;

        public CategoryService(OwlStockDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(IEnumerable<Category> categories, Guid photoId)
        {
            IEnumerable<PhotoCategory> photoCategories = BuildPhotoCateoriesList(categories, photoId);

            if(_context.PhotosCategories is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotosCategories)} is null");
            }

            await _context.PhotosCategories.AddRangeAsync(photoCategories);
            return await _context.SaveChangesAsync();
        }

        private static IEnumerable<PhotoCategory> BuildPhotoCateoriesList(IEnumerable<Category> categories, Guid photoId)
        {
            List<PhotoCategory> photoCategories = new();

            foreach (Category category in categories)
            {
                PhotoCategory photoCategory = new()
                {
                    GalleryPhotoId = photoId,
                    Category = category

                };

                photoCategories.Add(photoCategory);
            }

            return photoCategories;
        }
    }
}
