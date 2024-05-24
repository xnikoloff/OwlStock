using System.ComponentModel.DataAnnotations;

namespace OwlStock.Domain.Entities
{
    public class DynamicContent
    {
        [Key]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public string? Content { get; set; }
        public bool IsVisible { get; set; }
        public bool ShowInTopPosition { get; set; }
    }
}
