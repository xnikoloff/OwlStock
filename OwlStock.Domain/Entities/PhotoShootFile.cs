using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class PhotoShootFile
    {
        [Key]
        public int Id { get; set; }
        public string? FileName { get; set; }
        public IFormFile? FileData { get; set; }

        [ForeignKey(nameof(PhotoShoot))]
        public int PhotoShootId { get; set; }
        public PhotoShoot? PhotoShoot { get; set; }
    }
}
