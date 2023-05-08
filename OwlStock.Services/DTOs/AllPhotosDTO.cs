using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.DTOs
{
    public class AllPhotosDTO
    {
        public int Id { get; set; }
        public string? PhotoName { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Tag>? Tags { get; set; }
        public string? FileName { get; set; }
        public string? UserId { get; set; }
    }
}
