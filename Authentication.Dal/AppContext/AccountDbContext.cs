using Authentication.Dal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.Dal.AppContext
{
    public class AccountDbContext : IdentityDbContext
    {
        private readonly IConfiguration _configuration;
        public AccountDbContext(IConfiguration configuration, DbContextOptions<AccountDbContext> options) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<ApplicationUserDal> Costumers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgres"));
        }
    }
}
