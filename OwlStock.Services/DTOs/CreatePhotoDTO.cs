using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Common;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace OwlStock.Services.DTOs
{
    public class CreatePhotoDTO
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Display(Name = ModelConstraints.PhotoFormFileDisplayName)]
        public IFormFile? FormFile { get; set; }

        public string? WebRootPath { get; set; }
    }
}
