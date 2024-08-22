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
        public static WebApplication MigrateContext<TContext>(this WebApplication app) where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            if (!context.Database.EnsureCreated())
                context.Database.Migrate();
            return app;
        }
    }
}
