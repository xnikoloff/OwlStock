using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class PhotoBase
    {
        [Key]
        public Guid Id { get; set; }

        public string? FileName { get; set; }

        public string? FileType { get; set; }

        public string? FilePath { get; set; }

        [NotMapped]
        public byte[]? FileData { get; set; }
    }
}
