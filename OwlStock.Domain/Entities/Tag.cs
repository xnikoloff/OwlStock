using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string? Text { get; set; }

        [ForeignKey(nameof(Photo))]
        public Guid PhotoId { get; set; }
        public GalleryPhoto? Photo { get; set; }
    }
}
