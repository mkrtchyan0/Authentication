using Microsoft.EntityFrameworkCore;
using Authentication.Dal.AppContext;

namespace Authentication.Extansions
{
    public static class AddDbContextExtansion
    {
        public static IServiceCollection AddAccountContext(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AccountDbContext>(options =>
             options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

            return services;
        }
    }
}
