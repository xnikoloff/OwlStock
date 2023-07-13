using OwlStock.Domain.Entities;
using OwlStock.Infrastructure.Common.EmailTemplates;
using System.ComponentModel.DataAnnotations;

namespace OwlStock.Web.DTOs.PhotoShootDTOs
{
    public class UpdatePhotoShootPhotosDTO : EmailTemplateBaseDTO
    {
        public UpdatePhotoShootPhotosDTO()
        {
            Files = new HashSet<IFormFile>();
        }

        public Guid PhotoShootId{ get; set; }
        
        public string? PersonFullName{ get; set; }

        [Required]
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
