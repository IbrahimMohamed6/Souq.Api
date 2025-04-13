using Microsoft.AspNetCore.Identity;
using Souq.Core.Entites.Identity;

namespace Souq.Infrastructure.DataSeed.Identity
{
   public static class IdentityDataSeed
    {
       
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Ibrahim Mohamed",
                    Email = "hema@gmail.com",
                    UserName = "hema",
                    PhoneNumber = "01094898422"

                };
                await userManager.CreateAsync(User,"Pa$$word"); 
            }
        }
    }
}
