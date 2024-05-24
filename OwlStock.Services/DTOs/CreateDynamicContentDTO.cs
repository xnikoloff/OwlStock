using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;

namespace OwlStock.Services.DTOs
{
    public class CreateDynamicContentDTO
    {
        public DynamicContent? DynamicContent { get; set; }
        public IFormFile? Image { get; set; }
        public string? WebRootPath { get; set; }
    }
}
