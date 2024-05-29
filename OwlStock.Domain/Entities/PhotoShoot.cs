using Microsoft.AspNetCore.Identity;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class PhotoShoot
    {
        public PhotoShoot()
        {
            PhotoShootPhotos = new HashSet<PhotoShootPhoto>();
        }

        [Key]
        public Guid Id { get; set; }
        public string? PersonFirstName { get; set; }
        public string? PersonLastName { get; set; }
        public string? PersonFullName { get; set; }
        public string? PersonEmail { get; set; }
        public string? PersonPhone { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public PhotoShootType PhotoShootType { get; set; }
        public string? PhotoShootTypeDescription { get; set; }
        public string? UserPlace { get; set; }
        public string? GoogleMapsLink { get; set; }
        public bool IsDecidedByUs { get; set; }
        public decimal Price { get; set; }
        public bool DoNotUploadPhotos { get; set; }
        public PhotoDeliveryMethod? PhotoDeliveryMethod { get; set; }
        public string? PhotoDeliveryAddress { get; set; }
        public City? City { get; set; }

        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }
        
        [ForeignKey(nameof(IdentityUser))]
        public string? IdentityUserId { get; set; }
        public IdentityUser? IdentityUser { get; set; }

        public ICollection<PhotoShootPhoto> PhotoShootPhotos { get; set; }
    }
}
