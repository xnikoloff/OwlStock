using OwlStock.Services.Interfaces;
using Xunit;
using OwlStock.Infrastructure;
using Microsoft.AspNetCore.Http;
using OwlStock.Services.DTOs;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Services;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Tests.PhotoTests
{
    public class CreatePhotoTests
    {
        [Fact]
        public async Task CreatePhoto_WithCorrectData_ShouldCreatePhoto()
        {
            //Arrage
            DataSeeder seeder = new();
            OwlStockDbContext context = await seeder.ArrangeDbContext();
            
            byte[] bytes = await File.ReadAllBytesAsync("./testImage.jpg");
            MemoryStream stream = new(bytes);

            var formFile = new FormFile(stream, 0, stream.Length, "test", "testImage")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            GalleryPhoto galleryPhoto = new()
            {
                Name = "test",
                Description = "test",
                FileName = "success",
                FileData = stream.ToArray(),
                IdentityUserId = "1",
                IsFree = true,
                Tags = new List<Tag>() { new(){ Id = new Guid(), PhotoId = new Guid(), Text = "tag" } },
            };

            PhotoService service = new(context);

            //Act
            int recordsCountBefore = 0;

            if(context.GalleryPhotos is not null)
            {
                recordsCountBefore = await context.GalleryPhotos.CountAsync();
            }

            await service.Create(galleryPhoto);

            int recordsCountAfter = await context.GalleryPhotos.CountAsync();

            //Assert
            Assert.True(recordsCountAfter == ++recordsCountBefore);
        }
    }
}
