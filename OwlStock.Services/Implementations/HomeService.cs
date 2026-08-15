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

        /// <summary>
        /// Get all the data that is required to load the home page
        /// </summary>
        /// <param name="articles">List of articles displayed on the home page</param>
        /// <param name="testimonies">List of testimonials displayed on the home page</param>
        /// <returns>DTO with the data that is required to load the home page</returns>
        public async Task<HomePageDTO> GetHomeData(IEnumerable<Article> articles, IEnumerable<Testimony> testimonies)
        {
            return new HomePageDTO()
            {
                Photo = await ChooseHomePagePhoto(),
                Articles = articles,
                Testimonies = testimonies
            };
        }

        /// <summary>
        /// Gets the path to a random photo file for the home page carousel
        /// </summary>
        /// <returns>Path to the photo file. Returns an empty string if there are no photos found</returns>
        /// <exception cref="NullReferenceException">Thrown when tha path to the small photo file is null</exception>
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
