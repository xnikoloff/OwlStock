using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class HomeService : IHomeService
    {
        private readonly OwlStockDbContext _context;

        public HomeService(OwlStockDbContext context)
        {
            _context = context;
        }

        public async Task<string> ChooseHomePagePhoto()
        {
            if(_context.GalleryPhotos is null)
            {
                throw new NullReferenceException($"{nameof(_context.GalleryPhotos)} is null");
            }

            List<GalleryPhoto> photos = await _context.GalleryPhotos.ToListAsync();

            if(photos.Count == 0)
            {
                return string.Empty;
            }

            Random random = new();
            int randomNumber = random.Next(0, _context.GalleryPhotos.Count());

            return photos[randomNumber].FilePath ?? throw new NullReferenceException($"FilePath is null");

        }
    }
}
