using Microsoft.AspNetCore.Identity;
using OwlStock.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain
{
    public class Photo
    {
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

        [ForeignKey(nameof(IdentityUser))]
        public string? IdentityUserId { get; set; }

        public IdentityUser? IdentityUser { get; set; }
    }
}