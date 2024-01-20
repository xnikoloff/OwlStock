using Humanizer;
using Moq;
using OwlStock.Infrastructure;
using OwlStock.Services;
using OwlStock.Services.DTOs;
using Xunit;

namespace OwlStock.Tests.PhotoTests
{
    public class ChangeDownloadPersmissionTests
    {
        [Fact]
        public async Task ChangeDownloadPersmission_WithCorrectData()
        {
            //Arrange
            DataSeeder seeder = new DataSeeder();
            PhotoService photoService = new(await seeder.ArrangeDbContext());

            //Act
            //existing id
            Guid id = new("e7ebb634-4b17-4c64-98a7-1dc08d0b120b");

            PhotoByIdDTO dtoPrevious = await photoService.GetById(id);
            bool previous = dtoPrevious!.Photo!.IsDownloadable;

            await photoService.ChangeDownloadPermissions(id);
            PhotoByIdDTO dtoCurrent = await photoService.GetById(id);
            bool current = dtoCurrent!.Photo!.IsDownloadable;

            //Assert
            Assert.True(previous != current);
        }
    }
}
