using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;

namespace OwlStock.Tests
{
    public class DataSeeder
    {
        protected PhotonicDbContext BuildDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PhotonicDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            PhotonicDbContext context = new(optionsBuilder);
            return context;

        }

        public async Task<PhotonicDbContext> ArrangeDbContext()
        {
            PhotonicDbContext context = BuildDbContext();

            List<IdentityUser> users = new()
            {
                new()
                {
                    Id = "1",
                    Email = "test@test.test",
                    EmailConfirmed = true,
                    PasswordHash = "hash",
                }
            };

            List<Gear> gears = new()
            {
                new Gear
                {
                    Id = new Guid("d4f1c4e2-3c5b-4f6a-9f7e-2b8c9d0e1f2a"),
                    CameraBrand = "Canon",
                    CameraModel = "EOS 1100D",
                    CameraLens = "EF-S 18-55mm IS II",
                    AdditionalInformation = ""
                },

                new Gear                 {
                    Id = new Guid("a1b2c3d4-e5f6-4789-0abc-def123456789"),
                    CameraBrand = "Nikon",
                    CameraModel = "D3500",
                    CameraLens = "AF-P DX NIKKOR 18-55mm f/3.5-5.6G VR",
                    AdditionalInformation = ""
                }
            };

            List<Place> places = new()
            {
                new Place
                {
                    Id = new Guid("f1e2d3c4-b5a6-4789-0abc-def123456789"),
                    Name = "Central Park",
                    Description = "A large public park in New York City.",
                    GoogleMapsURL = "https://maps.google.com/?q=Central+Park",
                    ImagePath = "central_park.jpg",
                    IsPopular = true,
                    CreatedOn = DateTime.Now,
                    CreatedById = "1"
                },

                new Place
                {
                    Id = new Guid("0a1b2c3d-4e5f-6789-0abc-def123456789"),
                    Name = "Eiffel Tower",
                    Description = "An iconic landmark in Paris, France.",
                    GoogleMapsURL = "https://maps.google.com/?q=Eiffel+Tower",
                    ImagePath = "eiffel_tower.jpg",
                    IsPopular = true,
                    CreatedOn = DateTime.Now,
                    CreatedById = "1"
                }
            };

            var photos = new List<GalleryPhoto>
            {
                new GalleryPhoto
                {
                    Id = new Guid("e7ebb634-4b17-4c64-98a7-1dc08d0b120b"),
                    Name = "Test Photo 1",
                    Description = null,
                    CreatedById = "1",
                    CreatedOn = DateTime.Now,
                    FileName = "testphoto1.jpg",
                    GearId = new Guid("d4f1c4e2-3c5b-4f6a-9f7e-2b8c9d0e1f2a"),
                    
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
