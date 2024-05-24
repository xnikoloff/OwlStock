using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class DynamicContentService : IDynamicContentService
    {
        private readonly OwlStockDbContext _context;
        private readonly IFileService _fileService;
        private readonly IPhotoResizer _photoResizer;

        public DynamicContentService(OwlStockDbContext context, IFileService fileService, IPhotoResizer photoResizer)
        {
            _context = context;
            _fileService = fileService;
            _photoResizer = photoResizer;

        }

        public async Task<DynamicContent> Create(CreateDynamicContentDTO dto)
        {
            if(_context.DynamicContents is null)
            {
                throw new NullReferenceException($"{nameof(_context.DynamicContents)} is null");
            }

            if (dto == null)
            {
                throw new NullReferenceException($"{nameof(dto)} is null");
            }

            if (dto.DynamicContent == null)
            {
                throw new NullReferenceException($"{nameof(dto.DynamicContent)} is null");
            }

            if (dto.Image == null)
            {
                throw new NullReferenceException($"{nameof(dto.Image)} is null");
            }

            if (dto.WebRootPath == null)
            {
                throw new NullReferenceException($"{nameof(dto.WebRootPath)} is null");
            }

            dto.DynamicContent.ImageName = dto?.Image?.FileName;

            await _context.AddAsync(dto!.DynamicContent);
            await _context.SaveChangesAsync();
            await _fileService.CreateIFormFile(dto!.Image, dto!.WebRootPath);

            return await _context.DynamicContents.OrderBy(dc => dc.Id).FirstOrDefaultAsync() ?? 
                throw new NullReferenceException($"Cannot find DynamicContent");
        }

        public async Task Delete(Guid id)
        {
            if (_context.DynamicContents is null)
            {
                throw new NullReferenceException($"{nameof(_context.DynamicContents)} is null");
            }

            DynamicContent dynamicContent = await _context.DynamicContents.FindAsync(id) ??
                throw new NullReferenceException($"DynamicContent with id {id} does not exists");

            _context.DynamicContents.Remove(dynamicContent);
            await _context.SaveChangesAsync();
        }

        public async Task<DynamicContent> GetById(Guid id)
        {
            if (_context.DynamicContents is null)
            {
                throw new NullReferenceException($"{nameof(_context.DynamicContents)} is null");
            }

            return await _context.DynamicContents.FindAsync(id) ?? 
                throw new NullReferenceException($"DynamicContent with id {id} does not exists");
        }

        public async Task<IEnumerable<DynamicContent>> GetAll()
        {
            if (_context.DynamicContents is null)
            {
                throw new NullReferenceException($"{nameof(_context.DynamicContents)} is null");
            }

            return await _context.DynamicContents.ToListAsync();
        }
    }
}
