using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;

namespace OwlStock.Services
{
    public class FileService : IFileService
    {
        public bool CreatePhotoFile(PhotoBase photo)
        {
            if (File.Exists(photo.FilePath))
            {
                return true;
            }

            if (!Directory.Exists(photo.FilePath))
            {
                Directory.CreateDirectory(photo.FilePath ?? throw new NullReferenceException($"{photo.FilePath} is null"));
            }

            if(photo.FileName is null)
            {
                throw new NullReferenceException($"{nameof(photo.FileName)}");
            }

            if(photo.FileData is null)
            {
                throw new NullReferenceException($"{nameof(photo.FileData)} is null");
            }

            switch (photo)
            {
                case GalleryPhoto:
                {
                    using FileStream streamOriginalSize = File.OpenWrite(Path.Combine(photo.FilePath, $"OriginalSize_{photo.FileName}").Replace('\\', '/'));
                    using FileStream streamSmallSize = File.OpenWrite(Path.Combine(photo.FilePath, $"Small_{photo.FileName}").Replace('\\', '/'));
                    
                    streamOriginalSize.Write(photo.FileData, 0, photo.FileData.Length);
                    streamSmallSize.Write(photo.FileData, 0, photo.FileData.Length);
                    
                    break;
                }

                case PhotoShootPhoto:
                {
                    using FileStream streamOriginalSize = File.OpenWrite(Path.Combine(photo.FilePath, photo.FileName).Replace('\\', '/'));
                    streamOriginalSize.Write(photo.FileData, 0, photo.FileData.Length);
                    
                    break;
                }
            }
            
            return false;
        }

        public async Task<bool> CreatePlacePhotoFile(CreatePlacePhotoFileDTO dto)
        {
            //photobase is null when updating place with new photo
            if (dto.PhotoBase == null)
            {
                throw new NullReferenceException($"{nameof(dto.PhotoBase)} is null");
            }

            int lastIndexOfSlash = dto.PhotoBase.FilePath.LastIndexOf('\\');
            string directoryPath = dto.PhotoBase.FilePath.Substring(0, lastIndexOfSlash);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            byte[]? bytes = dto.FileData;
            using FileStream streamSmallSize = File.OpenWrite(dto?.PhotoBase?.FilePath);
            streamSmallSize.Write(bytes, 0, bytes.Length);

            return true;
        }

        public async Task CreateIFormFile(IFormFile file, string webRootPath)
        {
            if(file == null) 
            { 
                throw new NullReferenceException($"{nameof(file)} is null");
            }

            string filePath = Path.Combine(webRootPath, "resources", "images", file.FileName);
            if (file.Length > 0)
            {
                using Stream fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
        }
    }
}
