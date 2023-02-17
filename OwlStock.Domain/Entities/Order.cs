using Microsoft.AspNetCore.Identity;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public PhotoSize PhotoSize { get; set; }

        [ForeignKey(nameof(Photo))]
        public int? PhotoId { get; set; }

        public Photo? Photo { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        public string? IdentityUserId { get; set; }

        public IdentityUser? IdentityUser { get; set; }
    }
}
