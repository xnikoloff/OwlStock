using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class Place
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? GoogleMapsURL { get; set; }

        public string? ImagePath { get; set; }

        public bool IsPopular { get; set; }

        [ForeignKey(nameof(PhotoBase))]
        public Guid? PhotoBaseId { get; set; }

        public PhotoBase? PhotoBase { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }

        public City? City { get; set; }
    }
}
