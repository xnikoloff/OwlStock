using Moq;
using OwlStock.Domain.Entities;
using OwlStock.Services;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using Xunit;

namespace OwlStock.Tests.PhotoshootTests
{
    public class CreatePhotoShootTests
    {
        [Fact]
        public async Task CreatePhotoShoot_WithCorrectData_ShouldCreatePhotoshoot()
        {
            //Arrange
            DataSeeder seeder = new();
            Mock<IEmailService> emailServiceMock = new();
            Mock<ICalculationsService> calculationServiceMock = new();
            CalendarService calendarService = new();
            PhotoShootService photoShootService = new(await seeder.ArrangeDbContext(), emailServiceMock.Object, calendarService);

            CreatePhotoShootDTO dto = new()
            {
                PersonFirstName = "Test",
                PersonLastName = "User",
                PersonEmail = "email@email.com",
                PersonPhone = "0878131828",
                //PhotoShootType = PhotoShootType.Event,
                ReservationDate = DateTime.Now,
                ReservationTime = new TimeOnly(),
                IdentityUserId = new Guid().ToString()
            };


            //Act
            PhotoShoot result = await photoShootService.Add(dto);

            //Assert
            Assert.True(result?.PersonFirstName?.Equals(dto?.PersonFirstName));
            Assert.True(result?.PersonLastName?.Equals(dto?.PersonLastName));
            Assert.True(result?.PersonEmail?.Equals(dto?.PersonEmail));
            Assert.True(result?.PersonPhone?.Equals(dto?.PersonPhone));
            Assert.True(result?.PhotoShootType == dto?.PhotoShootType);
            Assert.True(result?.ReservationDate.Date == dto?.ReservationDate.Date);
            Assert.True(result?.IdentityUserId == dto?.IdentityUserId);
        }
    }
}
