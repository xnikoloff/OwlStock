using OwlStock.Services;
using OwlStock.Services.Interfaces;
using Xunit;
using OwlStock.Domain;
using OwlStock.Infrastructure;
using Microsoft.AspNetCore.Http;
using OwlStock.Services.DTOs;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.IO;

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


            CreatePhotoDTO dto = new()
            {
                Name = "Test Photo",
                Description = "Test Descr",
                FormFile = formFile,
                WebRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            };

            IPhotoService service = null;

            //Act
            int recordsCountBefore = 0;

            if(context.Photos is not null)
            {
                recordsCountBefore = await context.Photos.CountAsync();
            }

            await service.Create(dto);

            int recordsCountAfter = await context.Photos.CountAsync();

            //Assert
            Assert.True(recordsCountAfter == ++recordsCountBefore);
        }
    }
}
