using Microsoft.AspNetCore.Identity;

namespace OwlStock.Web.DTOs.Identity
{
    public class ChangeUserRoleDTO
    {
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? SelectedRole { get; set; }   
        public IEnumerable<IdentityRole>? Roles { get; set; }
    }
}
