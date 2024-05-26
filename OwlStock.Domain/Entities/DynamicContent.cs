using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class DynamicContent
    {
        [Key]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public string? Content { get; set; }
        public bool IsVisible { get; set; }
        public bool ShowInTopPosition { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int ReadingTime { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public string? CreatedById { get; set; }
        public IdentityUser? CreatedBy { get; set; }

        [ForeignKey(nameof(EditedBy))]
        public string? EditedById { get; set; }
        public IdentityUser? EditedBy { get; set; }

        [ForeignKey(nameof(DeletedBy))]
        public string? DeletedById { get; set; }
        public IdentityUser? DeletedBy { get; set; }
    }
}
