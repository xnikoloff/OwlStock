using OwlStock.Domain.Common;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace OwlStock.Services.DTOs.PhotoShoot
{
    public class CreatePhotoShootDTO
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(ModelConstraints.PersonNameMaxLength)]
        public string? PersonFirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(ModelConstraints.PersonNameMaxLength)]
        public string? PersonLastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [MaxLength(ModelConstraints.PersonEmailMaxLength)]
        public string? PersonEmail { get; set; }

        [Required]
        [Display(Name = "Phone")]
        [MaxLength(ModelConstraints.PersonPhoneMaxLenth)]
        public string? PersonPhone { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime ReservationDate { get; set; }

        [Required]
        [Display(Name = "Type")]
        public PhotoShootType PhotoShootType { get; set; }

        [Display(Name = "Photo Shoot Type Description")]
        [MaxLength(ModelConstraints.PhotoShootTypeDescriptionMaxLength)]
        public string? PhotoShootTypeDescription { get; set; }

        public string? IdentityUserId { get; set; }
    }
}
