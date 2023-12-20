using OwlStock.Domain.Entities;

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
    }
}
