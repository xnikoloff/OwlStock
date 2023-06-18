using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class FileService : IFileService
    {
        private readonly OwlStockDbContext _context;

        public FileService(IPhotoResizer photoResizer, OwlStockDbContext context)
        {
            _context = context;
        }

        public void Create(byte[] file, string? webRootPath, string filePath)
        {
            if (file != null && webRootPath != null)
            {
                using FileStream stream = File.OpenWrite(filePath);
                stream.Write(file, 0, file.Length);
            }
            
        }

        public async Task<List<string>> GetFilesNamesForPhotoShoot(Guid photoShootId)
        {
            if(_context.PhotoShootFiles is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShootFiles)} is null");
            }

            List<string> paths = await _context.PhotoShootFiles
                .Where(f => f.PhotoShootId == photoShootId)
                .Select(f => f.FileName ?? "")
                .ToListAsync();

            return paths;
        }

        public async Task<int> CreatePhotoShootFiles(List<IFormFile> files, Guid photoShootId, string webRootPath)
        {
            
            foreach(IFormFile file in files)
            {
                
                PhotoShootFile photoShootFile = new()
                {
                    FileName = file.FileName,
                    PhotoShootId = photoShootId,
                    FilePath = ""
                };

                if(_context.PhotoShootFiles is null)
                {
                    throw new NullReferenceException($"{nameof(_context.PhotoShootFiles)} is null");
                }

                await _context.PhotoShootFiles.AddAsync(photoShootFile);
            }

            return await _context.SaveChangesAsync();
        }

        public byte[] ConvertFormFileToByteArray(IFormFile file)
        {
            using MemoryStream stream = new();
            file.CopyTo(stream);
            return stream.ToArray();
        }
    }
}
