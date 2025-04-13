using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Souq.Core.Entites.Identity;

namespace Souq.Infrastructure.Data.DbContexts
{
    public class SouqeIdentityDbContext : IdentityDbContext<AppUser>
    {
        public SouqeIdentityDbContext(DbContextOptions options)
            :base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
