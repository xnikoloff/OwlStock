using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class PhotoService : IPhotoService
    {
        private readonly PhotonicDbContext _context;
        private readonly ILogger<PhotoService> _logger;
        
        public PhotoService(PhotonicDbContext context, ILogger<PhotoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gets Photo by ID
        /// </summary>
        /// <param name="id">Id of the photo</param>
        /// <returns>PhotoById DTO with the required data</returns>
        public async Task<PhotoByIdDTO> GetById(Guid id)
        {
            if(id == Guid.Empty)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetById)}, {nameof(PhotoService)}, {nameof(id)} was empty");
                return new();
            }

            try
            {
                GalleryPhoto? photo = await _context.GalleryPhotos
                                .Include(gf => gf.Tags)
                                .Include(gf => gf.PhotoCategories)
                                .Include(gf => gf.Gear)
                                .FirstOrDefaultAsync(gf => gf.Id == id);

                return new PhotoByIdDTO
                {
                    Photo = photo,
                    PhotoSize = PhotoSize.Large //set default size to large
                };
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, $"An error occurred at {DateTime.UtcNow}, {nameof(GetById)}, {nameof(PhotoService)}, {ex.Message}");
                return new PhotoByIdDTO();
            }
        }

        /// <summary>
        /// Gets PhotoBase by ID
        /// </summary>
        /// <param name="id">Id of the PhotoBase</param>
        /// <returns>PhotoBase entity with the required data</returns>
        public async Task<PhotoBase> GetPhotoBaseById(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError(null, $"An error occurred at {DateTime.UtcNow}, {nameof(GetPhotoBaseById)}, {nameof(PhotoService)}, {nameof(id)} was empty");
                return new();
            }

            try
            {
                return await _context.PhotosBase.FirstOrDefaultAsync(gf => gf.Id == id) ?? new();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred at {DateTime.UtcNow}, {nameof(GetPhotoBaseById)}, {nameof(PhotoService)}, {ex.Message}");
                return new();
            }
        }

        /// <summary>
        /// Gets list of Gear for a photo
        /// </summary>
        /// <returns>List of Gear</returns>
        public async Task<IEnumerable<Gear>> GetPhotoGears()
        {
            try
            {
                return await _context.Gear.ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred at {DateTime.UtcNow}, {nameof(GetPhotoGears)}, {nameof(PhotoService)}, {ex.Message}");
                return new List<Gear>();
            }
        }
        
        /// <summary>
        /// Creates new Photo
        /// </summary>
        /// <param name="photo">PhotoBase entity</param>
        /// <param name="userId">Id of the current user</param>
        /// <returns>PhotoBase object</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PhotoBase> Create(PhotoBase? photo, string userId)
        {
            if(photo is null)
            {
                _logger.LogError($"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(PhotoService)}, {nameof(photo)} was null");
                return new();
            }

            if (string.IsNullOrEmpty(photo.FilePath))
            {
                _logger.LogError($"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(PhotoService)}, {nameof(photo.FilePath)} was null");
                return new();
            }

            if (string.IsNullOrEmpty(photo.FileName))
            {
                _logger.LogError($"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(PhotoService)}, {nameof(photo.FileName)} was null");
                return new();
            }

            try
            {
                photo.CreatedOn = DateTime.Now;
                photo.CreatedById = userId;

                switch (photo)
                {
                    case GalleryPhoto:
                    {
                        //create new Gear if GearId is empty and set the gear id to the photo
                        if (((GalleryPhoto)photo).GearId == Guid.Empty || ((GalleryPhoto)photo).GearId == null)
                        {
                            await _context.Gear!.AddAsync
                            (
                                new()
                                {
                                    CameraBrand = ((GalleryPhoto)photo).Gear?.CameraBrand,
                                    CameraModel = ((GalleryPhoto)photo).Gear?.CameraModel,
                                    CameraLens = ((GalleryPhoto)photo).Gear?.CameraLens,
                                    AdditionalInformation = ((GalleryPhoto)photo).Gear?.AdditionalInformation
                                }
                            );

                            await _context.SaveChangesAsync();
                            Gear? newGear = await _context.Gear.OrderByDescending(g =>g.Id).FirstOrDefaultAsync();
                        
                            if(newGear == null)
                            {
                                _logger.LogError($"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(PhotoService)}, {nameof(newGear)} was null");
                                return new();
                            }

                            ((GalleryPhoto)photo).GearId = newGear.Id;
                            ((GalleryPhoto)photo).Gear = null;
                        }

                        //if GearId is not empty, keep the gear that was set from the list in the View 
                        else
                        {
                            ((GalleryPhoto)photo).GearId = ((GalleryPhoto)photo).GearId;
                            ((GalleryPhoto)photo).Gear = null;
                        }

                        photo.FilePath = Path.Combine("gallery-photos", PhotoSize.OriginalSize.ToString() + "_" + photo.FileName).Replace('\\', '/');
                        ((GalleryPhoto)photo).FilePathSmall = Path.Combine("gallery-photos", PhotoSize.Small.ToString() + "_" + photo.FileName).Replace('\\', '/');
                        
                        await _context.GalleryPhotos!.AddAsync((GalleryPhoto)photo);
                        break;
                    }

                    case PhotoShootPhoto:
                    {
                        string extractedPath = ExtractPath(photo!.FilePath);

                        if (extractedPath.IsNullOrEmpty())
                        {
                            return new();
                        }

                        photo.FilePath = extractedPath;
                        ((PhotoShootPhoto)photo).PhotoShootId = ((PhotoShootPhoto)photo).PhotoShoot.Id;
                        ((PhotoShootPhoto)photo).PhotoShoot = null;

                        await _context.PhotoShootPhotos!.AddAsync((PhotoShootPhoto)photo);
                        break;
                    }

                    case PhotoBase:
                    {
                        await _context.PhotosBase!.AddAsync(photo);
                        break;
                    }

                    default:
                        {
                            _logger.LogError($"An invalid option was provided to the swtich statement at {DateTime.UtcNow}, {nameof(Create)}, {nameof(PhotoService)}");
                            return new();
                        }
                }
                    
                await _context.SaveChangesAsync();

                await UpdateBasePhotoId(photo);

                return photo;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred at {DateTime.UtcNow}, {nameof(Create)}, {nameof(PhotoService)}, {ex.Message}");
                return new();
            }
        }

        /// <summary>
        /// Sets IsDeleted property to true
        /// </summary>
        /// <param name="photo">A PhotoBase object</param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> Delete(PhotoBase photo)
        {
            if (photo is null)
            {
                _logger.LogError($"An error occurred at {DateTime.UtcNow}, {nameof(Delete)}, {nameof(PhotoService)}, {nameof(photo)} was null");
                return false;
            }

            try
            {
                PhotoBase? photoBase = await _context.PhotosBase.FindAsync(photo.Id);

                if(photoBase is null)
                {
                    _logger.LogError($"${nameof(photoBase)} is null at {DateTime.UtcNow}, {nameof(Delete)}, {nameof(PhotoService)}");
                    return false;
                }

                photoBase.IsDeleted = true;
                await _context.SaveChangesAsync();

                return true;
            }
            
            catch(Exception ex)
            {
                _logger.LogError(ex, $"An error occurred at {DateTime.UtcNow}, {nameof(Delete)}, {nameof(PhotoService)}, {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Swaps the IsDownloadable property from true to false and vice versa
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> ChangeDownloadPermissions(Guid photoId)
        {
            if(_context.GalleryPhotos is null)
            {
                _logger.LogError($"An error occurred at {DateTime.UtcNow}, {nameof(ChangeDownloadPermissions)}, {nameof(PhotoService)}, {nameof(_context.GalleryPhotos)} is null");
                return false;
            }

            try
            {
                GalleryPhoto? photo = await _context.GalleryPhotos.FindAsync(photoId);
                
                if (photo is null)
                {
                    _logger.LogError($"${nameof(photo)} is null at {DateTime.UtcNow}, {nameof(Delete)}, {nameof(PhotoService)}");
                    return false;
                }

                photo.IsDownloadable = !photo.IsDownloadable;

                await _context.SaveChangesAsync();

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred at {DateTime.UtcNow}, {nameof(ChangeDownloadPermissions)}, {nameof(PhotoService)}, {ex.Message}");
                return false;
            }
        }

        private string ExtractPath(string filePath)
        {
            //find word "images" in the path string
            //check each 5 indexex, 0-5 -- 5-10 -- 10--15 till the end of the array
            //until word "image" is found
            //search each 5 chars because image has 5 letters
            //when word image is found -> save the index of 'i' in word "images"
            //that's the index where the path should be substringed
            //the substringed path goes to db as FilePath in PhotoBase

            int position = FindStartIndexPossition(filePath);

            if(position != -1)
            {
                return filePath.Substring(position);
            }

            return string.Empty;
        }

        private int FindStartIndexPossition(string filePath)
        {
            try
            {
                int position = 0;

                for (int i = 0; i < filePath.Length; i++)
                {
                    string word = "";

                    for (int j = i; j < i + 11; j++)
                    {
                        word += filePath[j];
                    }

                    if (word.Equals("photoshoots"))
                    {
                        position = i;
                        break;
                    }

                }

                return position;
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred at {DateTime.UtcNow}, {nameof(FindStartIndexPossition)}, {nameof(PhotoService)}, {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Updates the Id of PhotoBase for a photo
        /// </summary>
        /// <param name="photo">PhotoBase object</param>
        /// <returns>True if successful, else false</returns>
        private async Task<bool> UpdateBasePhotoId(PhotoBase photo)
        {
            try
            {
                Guid basePhotoId = await _context.PhotosBase!
                .OrderByDescending(pb => pb.Id)
                .Select(pb => pb.Id)
                .FirstOrDefaultAsync();

                switch (photo)
                {
                    case GalleryPhoto:
                    {
                        GalleryPhoto? galleryPhoto = await _context.GalleryPhotos!.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
                        
                        if (galleryPhoto is null)
                        {
                            _logger.LogError($"${nameof(galleryPhoto)} is null at {DateTime.UtcNow}, {nameof(Delete)}, {nameof(PhotoService)}");
                            return false;
                        }

                        galleryPhoto.PhotoBaseId = basePhotoId;
                        break;
                    }

                    case PhotoShootPhoto:
                    {
                        PhotoShootPhoto? photoShootPhoto = await _context.PhotoShootPhotos!.OrderByDescending(p => p.Id).FirstOrDefaultAsync();

                        if (photoShootPhoto is null)
                        {
                            _logger.LogError($"${nameof(photoShootPhoto)} is null at {DateTime.UtcNow}, {nameof(Delete)}, {nameof(PhotoService)}");
                            return false;
                        }

                        photoShootPhoto.PhotoBaseId = basePhotoId;
                        break;
                    }

                    default:
                    {
                        return false;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred at {DateTime.UtcNow}, {nameof(UpdateBasePhotoId)}, {nameof(PhotoService)}, {ex.Message}");
                return false;
            }
        }
    }
}