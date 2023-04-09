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
            if(_context.Photos is null)
            {
                throw new NullReferenceException($"{nameof(_context.Photos)} is null");
            }

            List<Photo> photos = await _context.Photos.ToListAsync();

            Random random = new();
            int randomNumber = random.Next(0, _context.Photos.Count());

            return photos[randomNumber].FileName;

        }
    }
}
