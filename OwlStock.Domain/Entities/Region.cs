using OwlStock.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace OwlStock.Domain.Entities
{
    public class Region
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? NameLatin { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? Slug { get; set; }
    }
}
