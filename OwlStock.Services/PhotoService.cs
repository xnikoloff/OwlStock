using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs;
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

        public async Task<List<AllPhotosDTO>> All()
        {
            if(_context.Photos is not null)
            {
                return await _context.Photos
                    .Select(p => new AllPhotosDTO
                    {
                        Id = p.Id,
                        PhotoName = p.Name,
                        FileName = p.FileName
                    })
                    .ToListAsync();
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

        public async Task<int> Create(CreatePhotoDTO? createPhotoDto)
        {
            if(createPhotoDto is null)
            {
                throw new NullReferenceException($"{nameof(createPhotoDto)} is null");
            }

            byte[]? fileData = null;

            if (createPhotoDto?.FormFile?.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    createPhotoDto.FormFile.CopyTo(ms);
                    fileData = ms.ToArray();
                }
            }

            string fileName = UploadeFile(createPhotoDto?.FormFile, createPhotoDto?.Name, createPhotoDto?.WebRootPath);

            Photo photo = new()
            {
                Name = createPhotoDto?.Name,
                Description = createPhotoDto?.Description,
                FileName = fileName,
                FileType = createPhotoDto?.FormFile?.ContentType,
                FileData = fileData
            };

            await _context.AddAsync(photo);
            int saveChanges = await _context.SaveChangesAsync();


            return saveChanges;
        }

        public string UploadeFile(IFormFile? file, string? photoName, string? webRootPath)
        {
            string? uniqueFileName = null;

            if (file != null && photoName != null && webRootPath != null)
            {
                string uploadsFolder = Path.Combine(webRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + "_" + photoName + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName ?? "";
        }
    }
}