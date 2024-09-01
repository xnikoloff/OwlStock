using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlStock.Domain.Entities;

namespace OwlStock.Infrastructure.Configuration
{
    internal class PlaceConfiguration : ConfigurationBase<Place>
    {
        public override void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.HasData(
                new Place
                {
                    Id = Guid.NewGuid(),
                    Name = "Метоха",
                    Description = "Местност Метоха в Асеновград",
                    IsPopular = true,
                    GoogleMapsURL = "https://www.google.bg/maps/place/%D0%9C%D0%B5%D1%82%D0%BE%D1%85%D0%B0/@42.0009383,24.8695654,17z/data=!3m1!4b1!4m6!3m5!1s0x14acd8cf73ab2aaf:0xbcb985c2cfe76039!8m2!3d42.0009383!4d24.8721403!16s%2Fg%2F11g7nrn5qb?entry=ttu&g_ep=EgoyMDI0MDgyOC4wIKXMDSoASAFQAw%3D%3D",
                    CityId = 8443
                },

                new Place
                {
                    Id = Guid.NewGuid(),
                    Name = "Асеновата крепост",
                    Description = "Асеновата крепост в Асеновград",
                    GoogleMapsURL = "https://www.google.bg/maps/place/%D0%90%D1%81%D0%B5%D0%BD%D0%BE%D0%B2%D0%B0+%D0%BA%D1%80%D0%B5%D0%BF%D0%BE%D1%81%D1%82/@41.9863671,24.8703,17z/data=!3m1!4b1!4m6!3m5!1s0x14acdf34198aa463:0xd61aeb51093571e1!8m2!3d41.9863671!4d24.8728749!16zL20vMGNfcXo3?entry=ttu&g_ep=EgoyMDI0MDgyOC4wIKXMDSoASAFQAw%3D%3D",
                    IsPopular = true,
                    CityId = 8443
                },
                new Place
                {
                    Id = Guid.NewGuid(),
                    Name = "Старият град на Пловдив",
                    Description = "Старият град на Пловдив",
                    GoogleMapsURL = "https://www.google.bg/maps/place/%D0%A1%D1%82%D0%B0%D1%80%D0%B8%D1%8F+%D0%B3%D1%80%D0%B0%D0%B4%D0%9F%D0%BB%D0%BE%D0%B2%D0%B4%D0%B8%D0%B2+%D0%A6%D0%B5%D0%BD%D1%82%D1%8A%D1%80,+4000+%D0%9F%D0%BB%D0%BE%D0%B2%D0%B4%D0%B8%D0%B2/@42.1490439,24.7463858,16z/data=!3m1!4b1!4m6!3m5!1s0x14acd1a2e85b2bf7:0xe7d9efa93577ca7e!8m2!3d42.1488072!4d24.7521373!16s%2Fg%2F11ys_h_xy?entry=ttu&g_ep=EgoyMDI0MDgyOC4wIKXMDSoASAFQAw%3D%3D",
                    IsPopular = true,
                    CityId = 12590
                },
                new Place
                {
                    Id = Guid.NewGuid(),
                    Name = "Дворец Кричим",
                    Description = "Царският Дворец в село Куртово Конаре",
                    GoogleMapsURL = "https://www.google.bg/maps/place/%D0%A6%D0%B0%D1%80%D1%81%D0%BA%D0%B8+%D0%B4%D0%B2%D0%BE%D1%80%D0%B5%D1%86+%D0%9A%D1%80%D0%B8%D1%87%D0%B8%D0%BC/@42.0992886,24.5157877,17z/data=!3m1!4b1!4m6!3m5!1s0x14acc7810a3d0e1d:0x4f0a59b440e765c0!8m2!3d42.0992886!4d24.5183626!16s%2Fg%2F11gcxykv2k?entry=ttu&g_ep=EgoyMDI0MDgyOC4wIKXMDSoASAFQAw%3D%3D",
                    IsPopular = true,
                    CityId = 11545
                }
            );
        }
    }
}
