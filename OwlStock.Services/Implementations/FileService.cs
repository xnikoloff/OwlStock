using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IPhotoResizer _photoResizer;
        private readonly ILogger<FileService> _logger;

        public FileService(IPhotoResizer photoResizer, ILogger<FileService> logger)
        {
            _photoResizer = photoResizer;
            _logger = logger;
        }

        /// <summary>
        /// Creates a photo file
        /// </summary>
        /// <param name="photo">Contains information that is required for creating the photo file</param>
        /// <returns>True if successful, else false</returns>
        /// <exception cref="NullReferenceException">Thrown when the file path is null</exception>
        public bool CreatePhotoFile(PhotoBase photo)
        {
            if (File.Exists(Path.Combine(photo.FilePath, photo.FileName)))
            {
                _logger.LogError("File {FileName}/{FilePath} already exists. {Method}, {Class}, {DateTime}", photo.FileName, photo.FilePath, nameof(CreatePhotoFile), nameof(AdministrationService), DateTime.Now);
                return false;
            }

            if (!Directory.Exists(photo.FilePath))
            {
                Directory.CreateDirectory(photo.FilePath ?? throw new NullReferenceException($"{photo.FilePath} is null"));
            }

            if(photo.FileName is null)
            {
                _logger.LogError("{FileName} is null in {Method}, {Class}, {DateTime}", nameof(photo.FileName), nameof(CreatePhotoFile), nameof(AdministrationService), DateTime.Now);
                return false;
            }

            if(photo.FileData is null)
            {
                _logger.LogError("{FileData} is null in {Method}, {Class}, {DateTime}", nameof(photo.FileData), nameof(CreatePhotoFile), nameof(AdministrationService), DateTime.Now);
                return false;
            }

            try
            {
                switch (photo)
                {
                    case GalleryPhoto:
                    {
                        using FileStream streamOriginalSize = File.OpenWrite(Path.Combine(photo.FilePath, $"OriginalSize_{photo.FileName}").Replace('\\', '/'));
                        using FileStream streamSmallSize = File.OpenWrite(Path.Combine(photo.FilePath, $"Small_{photo.FileName}").Replace('\\', '/'));

                        streamOriginalSize.Write(photo.FileData, 0, photo.FileData.Length);
                        byte[] resized = _photoResizer.Resize(photo.FileData, PhotoSize.Small);
                        streamSmallSize.Write(resized, 0, resized.Length);

                        break;
                    }

                    case PhotoShootPhoto:
                    {
                        using FileStream streamOriginalSize = File.OpenWrite(Path.Combine(photo.FilePath, photo.FileName).Replace('\\', '/'));
                        streamOriginalSize.Write(photo.FileData, 0, photo.FileData.Length);

                        break;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating photo file at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        /// <summary>
        /// Creates a photo file for a photoshoot location
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool CreatePlacePhotoFile(CreatePlacePhotoFileDTO dto)
        {
            if (dto.PhotoBase == null)
            {
                _logger.LogInformation("{FileName} is null in {Method}, {Class}, {DateTime}", nameof(dto.PhotoBase), nameof(CreatePlacePhotoFile), nameof(AdministrationService), DateTime.Now);
                return false;
            }

            if (dto.PhotoBase.FilePath == null)
            {
                _logger.LogInformation("{FileName} is null in {Method}, {Class}, {DateTime}", nameof(dto.PhotoBase.FilePath), nameof(CreatePlacePhotoFile), nameof(AdministrationService), DateTime.Now);
                return false;
            }

            try
            {
                int lastIndexOfSlash = dto.PhotoBase.FilePath.LastIndexOf('\\');
                string directoryPath = dto.PhotoBase.FilePath[..lastIndexOfSlash];

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (dto.FileData == null)
                {
                    throw new NullReferenceException(nameof(dto.FileData));
                }

                byte[]? bytes = dto.FileData;
                using FileStream streamSmallSize = File.OpenWrite(dto?.PhotoBase?.FilePath ?? "");
                streamSmallSize.Write(bytes, 0, bytes.Length);

                return true;
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating place photo file at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        /// <summary>
        /// Creates a photo file from an IFormFile
        /// </summary>
        /// <param name="file">The IFormFile</param>
        /// <param name="webRootPath">Path to the root directory of the project</param>
        /// <returns></returns>
        public async Task<bool> CreateIFormFile(IFormFile file, string webRootPath)
        {
            if (file == null)
            {
                _logger.LogError("File is null in {Method}, {Class}, {DateTime}", nameof(CreateIFormFile), nameof(FileService), DateTime.Now);
                return false;
            }

            try
            {
                string filePath = Path.Combine(webRootPath, "resources", "images", file.FileName);
                if (file.Length > 0)
                {
                    using Stream fileStream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fileStream);
                }

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating IFormFile at {Time}", DateTime.UtcNow);
                return false;
            }
        }
    }
}
