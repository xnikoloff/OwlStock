using Braintree;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services
{
    public class FileService : IFileService
    {
        //Add CreatePhotoDTO with photobase and filedata props
        public string CreatePhotoFile(PhotoBase photo, string webRootPath)
        {
            List<string> paths = GetPaths(photo, webRootPath);
            
            foreach(string path in paths)
            {
                if (!File.Exists(path))
                {
                    //Throws an exception when next day comes
                    
                    using FileStream stream = File.OpenWrite(path);
                    stream.Write(photo.FileData, 0, photo.FileData.Length);
                    return path;
                }
            }

            return "";
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
                    if(photo == null)
                    {
                        throw new NullReferenceException($"{((PhotoShootPhoto)photo).PhotoShoot} is null");
                    }

                    if (((PhotoShootPhoto)photo).PhotoShoot == null)
                    {
                        throw new NullReferenceException($"{((PhotoShootPhoto)photo).PhotoShoot} is null");
                    }

                    string photoShootId = ((PhotoShootPhoto)photo).PhotoShoot.Id.ToString();
                    string personFullName = ((PhotoShootPhoto)photo).PhotoShoot.PersonFullName.ToString();
                    string[] photoShootSubDirectories = Directory.GetDirectories(uploadsFolder + "\\photoshoots");
                    string directoryPath = uploadsFolder + "\\photoshoots\\" + personFullName + '_' + photoShootId;
                    string filePath = Path.Combine(directoryPath, photo.FileName);

                    if (!DirectoryExists(photoShootSubDirectories, photoShootId))
                    {
                        Directory.CreateDirectory(Path.Combine(directoryPath));
                    }

                    paths.Add(filePath);

                    break;
                }
            }

            return paths;
        }

        private static bool DirectoryExists(string[] photoShootSubDirectories, string photoShootId)
        {
            for(int i = 0; i < photoShootSubDirectories.Length; i++) 
            {
                string[] subdirectoryPathSplit = photoShootSubDirectories[i].Split('\\');

                //[^1] is index operator for array.Length - 1
                string[] subdirectoryNameSplit = subdirectoryPathSplit[^1].Split('_');

                if(subdirectoryNameSplit.Length < 2)
                {
                    continue;
                }

                if (subdirectoryNameSplit[1].Contains(photoShootId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
