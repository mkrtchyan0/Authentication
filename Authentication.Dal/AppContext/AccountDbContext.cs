using Authentication.Dal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.Dal.AppContext
{
    public class AccountDbContext(IConfiguration configuration, DbContextOptions<AccountDbContext> options) 
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {                             //IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Postgres"));
        }
    }
}
