using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlStock.Domain.Entities;

namespace OwlStock.Infrastructure.Configuration
{
    public class PhotoConfiguration : ConfigurationBase<GalleryPhoto>
    {
        public override void Configure(EntityTypeBuilder<GalleryPhoto> builder)
        {
            builder.HasData(
                new GalleryPhoto
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Photo 1",
                    Description = "Description Test Photo 1",
                },

                new GalleryPhoto
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Photo 2",
                    Description = "Description Test Photo 2",
                },

                new GalleryPhoto
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Photo 3",
                    Description = "Description Test Photo 3",
                }
            );
        }
    }
}
