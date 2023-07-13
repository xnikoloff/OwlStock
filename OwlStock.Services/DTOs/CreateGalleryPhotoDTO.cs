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

        [Required(ErrorMessage = ModelConstraints.CategoriesRequiredErrorMessage)]
        public IEnumerable<Category> Categories { get; set; }

        [Required(ErrorMessage = ModelConstraints.TagsRequiredErrorMessage)]
        public string? Tags { get; set; }

        [Display(Name = ModelConstraints.PhotoFormFileDisplayName)]
        [Required(ErrorMessage = "Upload a file")]
        public IFormFile? FormFile { get; set; }
    }
}
