using Moq;
using OwlStock.Domain.Entities;
using OwlStock.Services;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using Xunit;

namespace OwlStock.Tests.PhotoshootTests
{
    public class GetPhotoShootTests
    {
        [Fact]
        public async Task GetPhotoshootById_WithCorrectData_ShouldReturnPhotoshoot()
        {
            //Arange
            DataSeeder seeder = new();
            Mock<IEmailService> emailServiceMock = new();
            Mock<ICalculationsService> calculationServiceMock = new();
            Mock<ISettlementService> settlementServiceMock = new();
            CalendarService calendarService = new();
            PhotoShootService service = new(await seeder.ArrangeDbContext(), emailServiceMock.Object, calendarService);

            //Act
            //existing guid in database
            Guid guid = new("b0337f80-e186-48cf-a471-63d8c40a4bc3");
            PhotoShoot photoShoot = await service.PhotoShootById(guid);

            //Assert
            Assert.True(photoShoot.Id == new Guid("b0337f80-e186-48cf-a471-63d8c40a4bc3"));
        }

        [Fact]
        public async Task GetPhotoshootById_WithWrongId_ShouldThrowException()
        {
            //Arange
            DataSeeder seeder = new();
            Mock<IEmailService> emailServiceMock = new();
            Mock<ICalculationsService> calculationServiceMock = new();
            Mock<ISettlementService> settlementServiceMock = new();
            CalendarService calendarService = new();
            PhotoShootService service = new(await seeder.ArrangeDbContext(), emailServiceMock.Object, calendarService);

            //Act
            //not existing guid
            Guid guid = new("f0337f80-e186-48cf-a471-63d8c40a4bc3");
            
            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.PhotoShootById(guid));
        }

        [Fact]
        public async Task GetMyPhotoshoots_WithCorrectData_ShouldReturnPhotoshootsForUser()
        {
            //Arrange
            DataSeeder seeder = new();
            Mock<IEmailService> emailServiceMock = new();
            Mock<ICalendarService> calendarServiceMock = new();
            Mock<ISettlementService> settlementServiceMock = new();
            Mock<ICalculationsService> calculationServiceMock = new();
            PhotoShootService service = new(await seeder.ArrangeDbContext(), emailServiceMock.Object, calendarServiceMock.Object);
            
            //Act
            //existing Id
            string userId = "1";
            List<MyPhotoShootsDTO> photoShoots = await service.MyPhotoShoots(userId);

            //Assert
            Assert.True(photoShoots.Count == 2);
        }
    }
}
