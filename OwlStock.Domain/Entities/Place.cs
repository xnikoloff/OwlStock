using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class Place
    {
        public Place()
        {
            PhotoShoots = new HashSet<PhotoShoot>();
            PhotoBase = new();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? GoogleMapsURL { get; set; }

        public string? ImagePath { get; set; }

        public bool IsPopular { get; set; }

        public DateTime CreatedOn { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public string? CreatedById { get; set; }

        public IdentityUser? CreatedBy { get; set; }

        public DateTime? EditedOn { get; set; }

        [ForeignKey(nameof(EditedBy))]
        public string? EditedById { get; set; }

        public IdentityUser? EditedBy { get; set; }

        [ForeignKey(nameof(PhotoBase))]
        public Guid? PhotoBaseId { get; set; }

        public PhotoBase PhotoBase { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }

        public City? City { get; set; }

        public ICollection<PhotoShoot> PhotoShoots { get; set; }
    }
}
