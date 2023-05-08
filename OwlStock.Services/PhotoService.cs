using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly OwlStockDbContext _context;
        private readonly IPhotoResizer _photoResizer;
        private readonly IPhotoTagService _photoTagService;
        
        public PhotoService(OwlStockDbContext context, IPhotoResizer photoResizer, IPhotoTagService photoTagService)
        {
            _context = context;
            _photoResizer = photoResizer;
            _photoTagService = photoTagService;
        }

        public async Task<List<AllPhotosDTO>> All()
        {
            if(_context.Photos is not null)
            {
                List<AllPhotosDTO> allPhotosDTO = await _context.Photos
                    .Include(p => p.PhotoCategories)
                    .Include(p => p.Tags)
                    .Select(p => new AllPhotosDTO
                    {
                        Id = p.Id,
                        PhotoName = p.Name,
                        Categories = p.PhotoCategories.Select(e => e.Category).ToList(),
                        Tags = p.Tags.ToList(),
                        FileName = p.FileName,
                        UserId = p.IdentityUserId,
                    })
                    .ToListAsync();

                return allPhotosDTO;
            }

            throw new NullReferenceException($"{_context.Photos} is null");
        }

        public async Task<List<AllPhotosDTO>> All(string? userId)
        {
            List<AllPhotosDTO> allPhotosDTO = await All();
            return allPhotosDTO.Where(dto => dto.UserId == userId).ToList();
        }

        public async Task<List<AllPhotosDTO>> AllByCategory(Category category)
        {
            List<AllPhotosDTO> allPhotosDTO = await All();

            return allPhotosDTO
                .Where(dto => dto.Categories.Contains(category))
                .ToList();
        }

        public async Task<List<AllPhotosDTO>> AllByTags(string tagText)
        {
            List<Tag>? tags = await _photoTagService.GetByText(tagText);

            if (tags == null || tags.Count == 0)
            {
                return new List<AllPhotosDTO>();
            }

            string? text = tags?.FirstOrDefault()?.Text;

            List<AllPhotosDTO> allPhotosDTO = await All();  

            List<AllPhotosDTO> photosByTags = allPhotosDTO
                .Where(dto => dto.Tags != null && dto.Tags.Select(t => t.Text).Contains(text))
                .ToList();

            return photosByTags;
        }

        public async Task<PhotoByIdDTO> GetById(int? id)
        {
            if(id is null)
            {
                throw new NullReferenceException($"{nameof(id)} is null");
            }

            if(_context.Photos is null)
            {
                throw new NullReferenceException($"{nameof(_context.Photos)} is null");
            }

            Photo? photo = await _context.Photos.FindAsync(id) ?? 
                throw new NullReferenceException($"{nameof(photo)} is null");
            
            return new PhotoByIdDTO
            {
                Photo = photo,
                PhotoSize = PhotoSize.Large
            };
        }

        public async Task<int> Create(CreatePhotoDTO? createPhotoDto)
        {
            if(createPhotoDto is null)
            {
                throw new NullReferenceException($"{nameof(createPhotoDto)} is null");
            }

            Photo photo = new()
            {
                Name = createPhotoDto?.Name,
                IsFree = createPhotoDto.IsFree,
                Price = createPhotoDto.Price,
                Description = createPhotoDto?.Description,
                FileName = createPhotoDto?.FormFile?.FileName,
                FileType = createPhotoDto?.FormFile?.ContentType,
                IdentityUserId = createPhotoDto?.UserId,
            };

            
            List<PhotoCategory> photoCategories = GetCategories(createPhotoDto?.Categories, photo);

            photo.PhotoCategories = photoCategories;

            string fileName = GetFileName(createPhotoDto?.Name, createPhotoDto?.FormFile?.FileName);
            UploadeFile(createPhotoDto?.FormFile, photo, createPhotoDto?.WebRootPath, PhotoSize.OriginalSize, fileName);
            UploadeFile(createPhotoDto?.FormFile, photo, createPhotoDto?.WebRootPath, PhotoSize.Small, fileName);

            photo.FileName = fileName;

            await _context.AddAsync(photo);
            int saveChanges = await _context.SaveChangesAsync();

            await _photoTagService.Add(createPhotoDto?.Tags, photo.Id);

            return saveChanges;
        }

        private static List<PhotoCategory> GetCategories(List<Category> categories, Photo photo)
        {
            List<PhotoCategory> photoCategories = new();

            foreach(Category category in categories)
            {
                photoCategories.Add(new()
                {
                    Category = category,
                    Photo = photo
                });
            }

            return photoCategories;
        }

        public void UploadeFile(IFormFile? file, Photo photo, string? webRootPath, PhotoSize size, string fileName)
        {
            if (file != null && photo?.Name != null && webRootPath != null)
            {
                string uploadsFolder = Path.Combine(webRootPath, "images");
                string filePath = Path.Combine(uploadsFolder, size.ToString() + "_" + fileName);

                //iformfile to byte array
                byte[] data = ConvertFormFileToByteArray(file);

                //resize
                byte[] bytes = _photoResizer.Resize(data, size);
                byte[] resired = bytes;
                using FileStream stream = File.OpenWrite(filePath);
                stream.Write(resired, 0, resired.Length);
            }
        }

        private static byte[] ConvertFormFileToByteArray(IFormFile file)
        {
            using MemoryStream stream = new();
            file.CopyTo(stream);
            return stream.ToArray();
        }

        private static string GetFileName(string photoName, string fileName)
        {
             return Guid.NewGuid().ToString() + "_" + photoName + "_" + fileName;
        }
    }
}