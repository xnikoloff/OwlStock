using Microsoft.AspNetCore.Identity;
using OwlStock.Domain.Common;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class GiftCard
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ModelConstraints.PersonNameMaxLength)]
        public string? Receiver { get; set; }

        [Required]
        public PhotoShootType PhotoShootType { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? GiftCardNumber { get; set; }

        public GiftCardStatus Status { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        public string? IdentityUserId { get; set; }

        public IdentityUser? IdentityUser { get; set; }
    }
}
