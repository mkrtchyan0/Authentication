
using Authentication.Api.JwtTokenProviders;
using Authentication.Dal.AppContext;
using Authentication.Dal.Models;
using Authentication.Extansions;
using Authentication.Api.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAccountContext(builder);
            builder.Services.AddScoped<IApplicationInterface, UserRepository>();
            builder.Services.AddSingleton<JwtTokenProvider>();

            //builder.Services.AddIdentity<ApplicationUserDal, IdentityRole>()
            //.AddEntityFrameworkStores<AccountDbContext>()
            //.AddDefaultTokenProviders();               
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                 //.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
                 //.AddUserManager<UserManager<IdentityUser<Guid>>>()
                 //.AddSignInManager<SignInManager<IdentityUser<Guid>>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<AccountDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoles<IdentityRole<Guid>>()
                .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
                .AddDefaultTokenProviders();                

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
            });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MigrateContext<AccountDbContext>()
                .Run();
        }
    }
}
