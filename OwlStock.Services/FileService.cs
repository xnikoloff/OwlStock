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

        public void Create(List<IFormFile> files, string? webRootPath, PhotoSize? size)
        {
            string filePath = "";

            foreach (IFormFile file in files)
            {

                if (file != null && webRootPath != null)
                {
                        
                    filePath = size != null ? BuildFilePath(file, webRootPath, size.Value) : 
                        BuildFilePath(file, webRootPath);
                    
                    //iformfile to byte array
                    byte[] data = ConvertFormFileToByteArray(file);
                    byte[]? resised = null;

                    //resize
                    if(size != null)
                    {
                        byte[] bytes = _photoResizer.Resize(data, size.Value);
                        resised = bytes;
                    }

                    using FileStream stream = File.OpenWrite(filePath);

                    if(resised != null)
                    {
                        stream.Write(resised, 0, resised.Length);
                    }

                    else
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
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

        private static string BuildFilePath(IFormFile file, string webRootPath, PhotoSize size)
        {
            string uploadsFolder = Path.Combine(webRootPath, "images");
            string filePath = Path.Combine(uploadsFolder, size.ToString() + "_" + file.FileName);

            return filePath;
        }

        private static string BuildFilePath(IFormFile file, string webRootPath)
        {
            string uploadsFolder = Path.Combine(webRootPath, "images/photoshoots");
            string filePath = Path.Combine(uploadsFolder,  file.FileName);

            return filePath;
        }

        private static byte[] ConvertFormFileToByteArray(IFormFile file)
        {
            using MemoryStream stream = new();
            file.CopyTo(stream);
            return stream.ToArray();
        }
    }
}
