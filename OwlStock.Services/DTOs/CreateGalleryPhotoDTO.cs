using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Common;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace OwlStock.Services.DTOs
{
    public class CreateGalleryPhotoDTO
    {
        public CreateGalleryPhotoDTO()
        {
            Categories = new HashSet<Category>();
        }

        public GalleryPhoto? GalleryPhoto { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string? Tags { get; set; }

        [Display(Name = ModelConstraints.PhotoFormFileDisplayName)]
        public IFormFile? FormFile { get; set; }
    }
}
