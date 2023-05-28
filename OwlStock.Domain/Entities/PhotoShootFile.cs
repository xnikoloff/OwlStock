using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class PhotoShootFile
    {
        [Key]
        public Guid Id { get; set; }

        public string? FileName { get; set; }

        public string? FilePath { get; set; }

        [ForeignKey(nameof(PhotoShoot))]
        public Guid PhotoShootId { get; set; }

        public PhotoShoot? PhotoShoot { get; set; }
    }
}
