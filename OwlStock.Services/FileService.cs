using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services
{
    public class FileService : IFileService
    {
        //Add CreatePhotoDTO with photobase and filedata props
        //File paths are missing
        public bool CreatePhotoFile(PhotoBase photo)
        {
            if (File.Exists(photo.FilePath))
            {
                return true;
            }

            if (!Directory.Exists(photo.FilePath))
            {
                Directory.CreateDirectory(photo.FilePath);
            }

            switch (photo)
            {
                case GalleryPhoto:
                {
                    using FileStream streamOriginalSize = File.OpenWrite(Path.Combine(photo.FilePath, $"OriginalSize_{photo.FileName}"));
                    using FileStream streamSmallSize = File.OpenWrite(Path.Combine(photo.FilePath, $"Small_{photo.FileName}"));
                    
                    streamOriginalSize.Write(photo.FileData, 0, photo.FileData.Length);
                    streamSmallSize.Write(photo.FileData, 0, photo.FileData.Length);
                    
                    break;
                }

                case PhotoShootPhoto:
                {
                    using FileStream streamOriginalSize = File.OpenWrite(Path.Combine(photo.FilePath, photo.FileName));
                    streamOriginalSize.Write(photo.FileData, 0, photo.FileData.Length);
                    
                    break;
                }
            }
            
            return false;
        }

        /*private static List<string> GetPaths(PhotoBase photo)
        {
            List<string> paths = new();
            
            switch (photo)
            {
                case GalleryPhoto:
                {
                    string filePathSmall = Path.Combine(webRootPath, ((GalleryPhoto)photo).FilePathSmall);
                    string filePathOriginal = Path.Combine(webRootPath, ((GalleryPhoto)photo).FilePath);
                    
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

                    //Filepath problem
                    string photoShootId = ((PhotoShootPhoto)photo).PhotoShoot.Id.ToString();

                    if (!DirectoryExists(photo.FilePath, photoShootId))
                    {
                        string[] directorySplit = photo.FilePath.Split('\\');

                        string newDirectory = directorySplit[0] + directorySplit[1] + directorySplit[2];
                        Directory.CreateDirectory(Path.Combine(webRootPath, newDirectory));
                    }

                    paths.Add(photo.FilePath);

                    break;
                }
            }

            return paths;
        }*/
    }
}
