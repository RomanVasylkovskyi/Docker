using Microsoft.AspNetCore.Identity;

namespace WebHalk.Data.Entities.Identity
{
    public class RoleEntity : IdentityRole<int>
    {
        public ICollection<UserRoleEntity>? UserRoles { get; set; }
    }
}