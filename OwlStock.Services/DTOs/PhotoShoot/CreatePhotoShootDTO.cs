using OwlStock.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlStock.Services.DTOs.PhotoShoot
{
    public class CreatePhotoShootDTO
    {
        [Display(Name = "First Name")]
        public string? PersonFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? PersonLastName { get; set; }

        [Display(Name = "Email")]
        public string? PersonEmail { get; set; }

        [Display(Name = "Phone")]
        public string? PersonPhone { get; set; }

        [Display(Name = "Date")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Type")]
        public PhotoShootType PhotoShootType { get; set; }

        [Display(Name = "Photo Shoot Type Description")]
        public string? PhotoShootTypeDescription { get; set; }
        public string? IdentityUserId { get; set; }
    }
}
