using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlStock.Domain.Entities;

namespace OwlStock.Infrastructure.Configuration
{
    public class ArticleCateogyConfiguration : ConfigurationBase<ArticleCategory>
    {
        public override void Configure(EntityTypeBuilder<ArticleCategory> builder)
        {
            builder.HasData(
                new ArticleCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Технологии",
                    CreatedOn = DateTime.UtcNow,
                },

                new ArticleCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Фототехника",
                    CreatedOn = DateTime.UtcNow,
                },

                new ArticleCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Образователни",
                    CreatedOn = DateTime.UtcNow,
                }
            );
        }
    }
}
