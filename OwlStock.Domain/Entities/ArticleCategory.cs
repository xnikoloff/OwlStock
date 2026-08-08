using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class ArticleCategory
    {
        public ArticleCategory()
        {
           Articles = new HashSet<Article>(); 
        }

        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public ICollection<Article>? Articles { get; set; }

        public DateTime CreatedOn { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public string? CreatedById { get; set; }

        public IdentityUser? CreatedBy { get; set; }
    }
}
