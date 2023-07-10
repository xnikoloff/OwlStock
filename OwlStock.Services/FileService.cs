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
        //Add CreatePhotoDTO with photobase and filedata props
        public bool CreatePhotoFile(PhotoBase photo, string webRootPath)
        {
            List<string> paths = GetPaths(photo, webRootPath);

            foreach(string path in paths)
            {
                if (File.Exists(path))
                {
                    return false;
                }

                using FileStream stream = File.OpenWrite(path);
                stream.Write(photo.FileData, 0,photo.FileData.Length);

            }

            return true;
        }

        private static List<string> GetPaths(PhotoBase photo, string webRootPath)
        {
            List<string> paths = new();
            string uploadsFolder = Path.Combine(webRootPath, "images");

            switch (photo)
            {
                case GalleryPhoto:
                {
                    string filePathSmall = Path.Combine(uploadsFolder, PhotoSize.Small.ToString() + "_" + photo.FileName);
                    string filePathOriginal = Path.Combine(uploadsFolder, PhotoSize.OriginalSize.ToString() + "_" + photo.FileName);

                    paths.Add(filePathSmall);
                    paths.Add(filePathOriginal);

                    break;
                }

                case PhotoShootPhoto:
                {
                    //Think of a way to save file path to db for both photoshoot photos abd gallery photos
                    string filePath = Path.Combine(uploadsFolder + "\\photoshoots", photo.FileName);
                    paths.Add(filePath);

                    break;
                }
            }

            return paths;
        }
    }
}
