using OwlStock.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class Municipality
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? NameLatin { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? Slug { get; set; }

        [ForeignKey(nameof(Region))]
        public int RegionId { get; set; }

        public Region? Region { get; set; }
    }
}
