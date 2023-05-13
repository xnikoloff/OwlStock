using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.DTOs.PhotoShoot
{
    public class MyPhotoShootsDTO
    {
        public DateTime ReservationDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public PhotoShootType PhotoShootType { get; set; }
        public string? ReservationFor { get; set; }
    }
}
