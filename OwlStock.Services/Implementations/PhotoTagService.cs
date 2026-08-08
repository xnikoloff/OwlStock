using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class PhotoTagService : IPhotoTagService
    {
        private readonly PhotonicDbContext _context;
        private readonly ILogger<PhotoTagService> _logger;

        public PhotoTagService(PhotonicDbContext context, ILogger<PhotoTagService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Add(string tags, Guid photoId)
        {
            List<string> tagsSplit = SplitTags(tags);

            if (tagsSplit.Count == 0)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(Add)}, {nameof(tagsSplit)} is empty");
                return false;
            }

            try
            {
                for (int i = 0; i < tagsSplit.Count; i++)
                {
                    Tag tag = new()
                    {
                        PhotoId = photoId,
                        Text = tagsSplit[i]
                    };

                    await _context.AddAsync(tag);
                    await _context.SaveChangesAsync();
                }
                
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        public async Task<List<Guid>> GetPhotoIdListByTag(string tagText)
        {
            try
            {
                List<Guid> ids = await _context.Tags
                .Where(t => t.Text.Contains(tagText))
                .Select(t => t.PhotoId)
                .ToListAsync();

                return ids;
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving photo IDs by tag at {Time}", DateTime.UtcNow);
                return new List<Guid>();
            }
        }
        
        private List<string> SplitTags(string tags)
        {
            try
            {
                List<string> tagsSplit = tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> tagsCleaned = new();

                for (int i = 0; i < tagsSplit.Count; i++)
                {
                    tagsCleaned.Add(tagsSplit[i].TrimStart());
                }

                return tagsCleaned;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while splitting tags at {Time}", DateTime.UtcNow);
                return new List<string>();
            }
        }
    }
}
