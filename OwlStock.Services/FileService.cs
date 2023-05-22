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
        private readonly IPhotoResizer _photoResizer;
        private readonly OwlStockDbContext _context;

        public FileService(IPhotoResizer photoResizer, OwlStockDbContext context)
        {
            _photoResizer = photoResizer;
            _context = context;
        }

        public Task<int> Create(IFormFile? file, Photo photo, string? webRootPath, PhotoSize size, string fileName)
        {
            throw new NotImplementedException();
        }

        public void Create(List<IFormFile> files, string? webRootPath, PhotoSize size)
        {
            foreach(IFormFile file in files)
            {
                if (file != null && webRootPath != null)
                {
                    string uploadsFolder = Path.Combine(webRootPath, "images/photoshoots");
                    string filePath = Path.Combine(uploadsFolder, size.ToString() + "_" + file.FileName);

                    //iformfile to byte array
                    byte[] data = ConvertFormFileToByteArray(file);

                    //resize
                    byte[] bytes = _photoResizer.Resize(data, size);
                    byte[] resired = bytes;
                    using FileStream stream = File.OpenWrite(filePath);
                    stream.Write(resired, 0, resired.Length);
                }
            }
            
        }

        public async Task<List<string>> GetFilesPathsForPhotoShoot(int photoShootId)
        {
            if(_context.PhotoShootFiles is null)
            {
                throw new NullReferenceException($"{nameof(_context.PhotoShootFiles)} is null");
            }
            
            return await _context.PhotoShootFiles
                .Where(f => f.PhotoShootId == photoShootId)
                .Select(f => f.FilePath ?? "")
                .ToListAsync();
        }

        private static byte[] ConvertFormFileToByteArray(IFormFile file)
        {
            using MemoryStream stream = new();
            file.CopyTo(stream);
            return stream.ToArray();
        }
    }
}
