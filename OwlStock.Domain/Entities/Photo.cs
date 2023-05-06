using Microsoft.AspNetCore.Identity;
using OwlStock.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class Photo
    {
        public Photo()
        {
            this.PhotoCategories = new HashSet<PhotoCategory>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstraints.PictureNameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(ModelConstraints.PictureDescriptionMaxLength)]
        public string? Description { get; set; }

        public string? FileName { get; set; }

        public string? FileType { get; set; }

        public byte[]? FileData { get; set; }

        public decimal? Price { get; set; }
        public bool IsFree { get; set; }

        [Required]
        public ICollection<PhotoCategory> PhotoCategories { get; set; }

        public List<Tag> Tags { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        public string? IdentityUserId { get; set; }

        public IdentityUser? IdentityUser { get; set; }
    }
}