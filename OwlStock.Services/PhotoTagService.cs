﻿using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class PhotoTagService : IPhotoTagService
    {
        private readonly OwlStockDbContext _context;

        public PhotoTagService(OwlStockDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(string tags, int photoId)
        {
            List<string> tagsSplit = SplitTags(tags);

            for(int i = 0; i < tagsSplit.Count; i++)
            {
                Tag tag = new()
                {
                    PhotoId = photoId,
                    Text = tagsSplit[i]
                };

                await _context.AddAsync(tag);
            }

            return await _context.SaveChangesAsync();
        }

        private List<string> SplitTags(string tags)
        {
            List<string> tagsSplit = tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> tagsCleaned = new();

            for (int i = 0; i < tagsSplit.Count; i++)
            {
                tagsCleaned.Add(tagsSplit[i].TrimStart());
            }

            return tagsCleaned;
        }
    }

    
}
