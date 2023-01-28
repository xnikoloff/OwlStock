using Microsoft.EntityFrameworkCore;
using OwlStock.Domain;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;
using System.Reflection;

namespace OwlStock.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly OwlStockDbContext _context;

        public PhotoService(OwlStockDbContext context)
        {
            _context = context;
        }

        public async Task<List<Photo>> All()
        {
            if(_context.Photos is not null)
            {
                return await _context.Photos.ToListAsync();
            }

            throw new NullReferenceException($"{_context.Photos} is null");
        }

        public async Task<Photo> GetById(int? id)
        {
            if(id is null)
            {
                throw new NullReferenceException($"{nameof(id)} is null");
            }

            if(_context.Photos is null)
            {
                throw new NullReferenceException($"{nameof(_context.Photos)} is null");
            }

            Photo photo = await _context.Photos.FindAsync(id) ?? 
                throw new NullReferenceException($"{nameof(photo)} is null");

            return photo;
        }

        public async Task<int> Create(Photo? photo)
        {
            if(photo is null)
            {
                throw new NullReferenceException($"{nameof(photo)} is null");
            }

            await _context.AddAsync(photo);
            return await _context.SaveChangesAsync();
        }
    }
}