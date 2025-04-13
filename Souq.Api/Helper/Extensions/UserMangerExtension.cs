using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Souq.Core.Entites.Identity;
using System.Security.Claims;

namespace Souq.Api.Helper.Extensions
{
    public static class UserMangerExtension
    {
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
        {
            var Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Where(e=>e.Email==Email).Include(A => A.Address).FirstOrDefaultAsync();
            return user;

        }
    }
}
