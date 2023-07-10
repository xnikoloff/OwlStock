using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class PhotoCategory
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(GalleryPhoto))]
        public Guid GalleryPhotoId { get; set; }

        public GalleryPhoto? GalleryPhoto { get; set; }

        public Category Category { get; set; }
    }
}
