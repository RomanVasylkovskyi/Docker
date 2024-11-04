using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiStore.Data.Entities.Identity
{
    public class UserEntity : IdentityUser<int>
    {
        [StringLength(255)]
        public string ? Image { get; set; }
        [StringLength(100)]
        public string ? Lastname { get; set; }
        [StringLength(100)]
        public string? Firstname { get; set; }
        public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
    }
}
