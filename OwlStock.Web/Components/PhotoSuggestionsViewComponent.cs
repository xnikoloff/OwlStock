using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;

namespace OwlStock.Web.Components
{
    public class PhotoSuggestionsViewComponent : ViewComponent
    {
        private readonly OwlStockDbContext _context;

        public PhotoSuggestionsViewComponent(OwlStockDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<Category> categories)
        {
            if(_context.GalleryPhotos is null)
            {
                throw new NullReferenceException($"{nameof(_context.GalleryPhotos)} is null");
            }

            List<GalleryPhoto> photos = await _context.GalleryPhotos
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            List<GalleryPhoto> photosByCategory = new();

            foreach(Category category in categories)
            {
                photosByCategory.AddRange
                (
                    photos.Where(p => 
                        p.PhotoCategories
                        .Select(phc => phc.Category)
                        .Contains(category)

                    )
                );
            }

            return View(photos.Distinct().Take(3).ToList());
        }
    }
}
