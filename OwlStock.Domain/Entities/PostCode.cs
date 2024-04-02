using OwlStock.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace OwlStock.Domain.Entities
{
    public class PostCode
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(ModelConstraints.PostCodeMaxLength)]
        public string? Code { get; set; }
    }
}
