using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class PhotoShootPhoto : PhotoBase
    {
        public bool IsPublic { get; set; }

        [ForeignKey(nameof(PhotoShoot))]
        public Guid PhotoShootId { get; set; }

        public PhotoShoot? PhotoShoot { get; set; }

        [ForeignKey(nameof(PhotoBase))]
        public Guid PhotoBaseId { get; set; }

        PhotoBase? PhotoBase { get; set; }
    }
}
