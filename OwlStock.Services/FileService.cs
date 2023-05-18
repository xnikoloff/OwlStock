using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class FileService : IFileService
    {
        private readonly IPhotoResizer _photoResizer;

        public FileService(IPhotoResizer photoResizer)
        {
            _photoResizer = photoResizer;
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
                    string filePath = Path.Combine(uploadsFolder, size.ToString() + "_" + file.Name);

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

        private static byte[] ConvertFormFileToByteArray(IFormFile file)
        {
            using MemoryStream stream = new();
            file.CopyTo(stream);
            return stream.ToArray();
        }
    }
}
