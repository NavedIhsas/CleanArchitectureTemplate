using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace persistent.Context
{
    public class IdentityDatabaseContext:IdentityDbContext<User>
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUser<string>>().ToTable("Users","identity");
            builder.Entity<IdentityUser<string>>().ToTable("Roles","identity");
            builder.Entity<IdentityUser<string>>().ToTable("UserClaims","identity");
            builder.Entity<IdentityUser<string>>().ToTable("RoleClaims","identity");
            builder.Entity<IdentityUser<string>>().ToTable("UserLogins","identity");
            builder.Entity<IdentityUser<string>>().ToTable("UserTokens","identity");
            builder.Entity<IdentityUser<string>>().ToTable("UserRoles","identity");

            builder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.LoginProvider, x.Name });
          // base.OnModelCreating(builder);
        }
    }
}
