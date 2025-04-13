using Microsoft.AspNetCore.Identity;

namespace Souq.Core.Entites.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public UserAddress Address { get; set; } = null!;
    }
}
