using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;

namespace OwlStock.Tests
{
    public class DataSeeder
    {
        protected OwlStockDbContext BuildDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OwlStockDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            OwlStockDbContext context = new(optionsBuilder);
            return context;

        }

        public async Task<OwlStockDbContext> ArrangeDbContext()
        {
            OwlStockDbContext context = BuildDbContext();

            List<IdentityUser> users = new()
            {
                new()
                {
                    Id = "1",
                    Email = "test@test.test",
                    EmailConfirmed = false,
                    PasswordHash = "hash",
                    PhoneNumber = "0123456789"
                }
            };

            var photos = new List<GalleryPhoto>
            {
                new GalleryPhoto
                {
                    Id = new Guid("e7ebb634-4b17-4c64-98a7-1dc08d0b120b"),
                    Name = "Test Photo 1",
                    Description = null
                }
            };

            List<PhotoShoot> photoShoots = new()
            {
                new PhotoShoot()
                {
                    Id = new("e7ebb634-4b17-4c64-98a7-1dc08d0b120b"),
                    IdentityUserId = "1",
                    CreatedOn = DateTime.Now,
                    PersonFirstName = "Test",
                    PersonLastName = "Test",
                    PersonFullName = "Test",
                    ReservationDate = DateTime.Now,
                    //PhotoShootType = PhotoShootType.Event,
                    PersonPhone = "0123456789",
                    PersonEmail = "test@test.test"
                },
                new PhotoShoot()
                {
                    Id = new("b0337f80-e186-48cf-a471-63d8c40a4bc3"),
                    IdentityUserId = "1",
                    CreatedOn = DateTime.Now,
                    PersonFirstName = "Test 2",
                    PersonLastName = "Test 2",
                    PersonFullName = "Test 2",
                    ReservationDate = DateTime.Now,
                    //PhotoShootType = PhotoShootType.Event,
                    PersonPhone = "0123456789",
                    PersonEmail = "test@test.test"
                }
            };

            await context.AddRangeAsync(photos);
            await context.AddRangeAsync(photoShoots);
            await context.SaveChangesAsync();
            return context;
        }
    }
}
