using OwlStock.Domain.Common;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace OwlStock.Domain.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? NameLatin { get; set; }

        [MaxLength(ModelConstraints.SettlementNameMaxLength)]
        public string? Slug { get; set; }

        [MaxLength(ModelConstraints.LatLongMaxLength)]
        public string? Latitude { get; set; }

        [MaxLength(ModelConstraints.LatLongMaxLength)]
        public string? Longitude { get; set; }

        [MaxLength(ModelConstraints.PostCodeMaxLength)]
        public string? PostCode { get; set; }

        [ForeignKey(nameof(Region))]
        public int RegionId { get; set; }

        public Region? Region { get; set; }

        [ForeignKey(nameof(Municipality))]
        public int MunicipalityId { get; set; }

        public Municipality? Municipality { get; set; }

        public SettlementType? SettlementType { get; set; }

    }
}
