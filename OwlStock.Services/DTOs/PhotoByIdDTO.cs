using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace OwlStock.Services.DTOs
{
    public class PhotoByIdDTO
    {
        public GalleryPhoto? Photo { get; set; }
        [Display(Name = "Select Size")]
        public PhotoSize PhotoSize { get; set; }
    }
}
