using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.DTOs.PhotoShoot
{
    public class ManagePhotoshootDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ReservationDate { get; set; }
        public string? PersonFullName { get; set; }
        public string? PersonEmail { get; set; }
        public string? PersonPhone { get; set; }
        public PhotoShootType PhotoShootType { get; set; }
        public string? PhotoShootTypeDescription { get; set; }
        public string? UserPlace { get; set; }
        public string? GoogleMapsLink { get; set; }
        public decimal Price { get; set; }
        public PhotoDeliveryMethod? PhotoDeliveryMethod { get; set; }
        public string? PhotoDeliveryAddress { get; set; }
    }
}
