using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        [ForeignKey(nameof(Photo))]
        public int PhotoId { get; set; }
        public Photo? Photo { get; set; }
    }
}
