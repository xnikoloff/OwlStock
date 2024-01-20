using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services;
using Xunit;

namespace OwlStock.Tests.PhotoTests
{
    public class DeletePhotoTests
    {
        [Fact]
        public async Task DeletePhoto_WithCorrectData_ShouldSetIsDeletedToTrue()
        {
            //Arrange
            DataSeeder seeder = new();
            OwlStockDbContext context = await seeder.ArrangeDbContext();
            PhotoService service = new(context);

            //existing guid
            Guid id = new("e7ebb634-4b17-4c64-98a7-1dc08d0b120b");
            PhotoBase? photo = await context!.PhotosBase!.FindAsync(id);

            //Act
            await service.Delete(photo!);
            
            //Assert
            Assert.True(photo?.IsDeleted);
        }
    }
}
