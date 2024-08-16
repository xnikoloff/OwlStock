using Microsoft.Build.Framework;

namespace OwlStock.Web.DTOs.Identity
{
    public class CreateRoleDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
