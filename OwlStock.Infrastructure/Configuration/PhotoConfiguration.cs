using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlStock.Domain.Entities;

namespace OwlStock.Infrastructure.Configuration
{
    public class PhotoConfiguration : ConfigurationBase<Photo>
    {
        public override void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasData(
                new Photo
                {
                    Id = 1,
                    Name = "Test Photo 1",
                    Description = "Description Test Photo 1",
                },

                new Photo
                {
                    Id = 2,
                    Name = "Test Photo 2",
                    Description = "Description Test Photo 2",
                },

                new Photo
                {
                    Id = 3,
                    Name = "Test Photo 3",
                    Description = "Description Test Photo 3",
                }
            );
        }
    }
}
