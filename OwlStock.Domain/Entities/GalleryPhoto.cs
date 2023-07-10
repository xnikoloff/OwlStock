using Microsoft.AspNetCore.Identity;
using OwlStock.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class GalleryPhoto : PhotoBase
    {
        public GalleryPhoto()
        {
            PhotoCategories = new HashSet<PhotoCategory>();
            Tags = new HashSet<Tag>();
        }

        [Required]
        [MaxLength(ModelConstraints.PictureNameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(ModelConstraints.PictureDescriptionMaxLength)]
        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public bool IsFree { get; set; }

        public string? FilePathSmall { get; set; }

        [Required]
        public ICollection<PhotoCategory> PhotoCategories { get; set; }

        public ICollection<Tag> Tags { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        public string? IdentityUserId { get; set; }
        
        public IdentityUser? IdentityUser { get; set; }

        [ForeignKey(nameof(PhotoBase))]
        public Guid PhotoBaseId { get; set; }

        PhotoBase? PhotoBase { get; set; }
        
    }
}