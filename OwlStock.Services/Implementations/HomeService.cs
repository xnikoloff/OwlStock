using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs.HomePage;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly PhotonicDbContext _context;
        private readonly ILogger<HomeService> _logger;

        public HomeService(PhotonicDbContext context, ILogger<HomeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HomePageDTO> GetHomeData(IEnumerable<Article> articles, IEnumerable<Testimony> testimonies)
        {
            return new HomePageDTO()
            {
                Photo = await ChooseHomePagePhoto(),
                Articles = articles,
                Testimonies = testimonies
            };
        }

        private async Task<string> ChooseHomePagePhoto()
        {
            if(_context.GalleryPhotos is null)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(ChooseHomePagePhoto)}, {nameof(_context.GalleryPhotos)} is null");
                return string.Empty;
            }

            try
            {
                List<GalleryPhoto> photos = await _context.GalleryPhotos.ToListAsync();

                if (photos.Count == 0)
                {
                    return string.Empty;
                }

                Random random = new();
                int randomNumber = random.Next(0, _context.GalleryPhotos.Count());

                return photos[randomNumber].FilePathSmall ?? throw new NullReferenceException($"FilePath is null");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while choosing home page photo at {Time}", DateTime.UtcNow);
                return string.Empty;
            }
        }
    }
}
