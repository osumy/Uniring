using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uniring.Application.Interfaces;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Application.Services;
using Uniring.Domain.Entities.IdentityEntities;
using Uniring.Infrastructure.Repositories;
using Uniring.Infrastructure.Validators;

namespace Uniring.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddRepositories();
            services.AddUniringDbContext(configuration.GetConnectionString("DefaultConnection"));
            services.AddUniringIdentity(configuration);

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRingRepository, RingRepository>();

            return services;
        }

        private static IServiceCollection AddUniringDbContext(this IServiceCollection services, string? conn)
        {
            services.AddDbContext<UniringDbContext> (
                options =>
                {
                    options.UseSqlServer(conn);
                });

            return services;
        }

        private static IServiceCollection AddUniringIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Do not force email usage
                options.User.RequireUniqueEmail = false;

                // Password policy
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // SignIn options (we don't force email confirmation)
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<UniringDbContext>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole,
            UniringDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, UniringDbContext, Guid>>()
            .AddDefaultTokenProviders();


            // Replace username validator with our own to allow Unicode names
            services.AddScoped<IUserValidator<ApplicationUser>, AnyCharsUserValidator<ApplicationUser>>();

            // Configure cookie for UI host (if using cookie)
            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = "/Account/Login";
                opts.Cookie.Name = "Uniring.Auth.Cookie";
                opts.ExpireTimeSpan = TimeSpan.FromDays(1);
                opts.SlidingExpiration = true;
            });

            services.AddScoped<IIdentityService, IdentityService>();

            // JWT config: if the API will use JWT, register the JwtBearer in the Api host (not here)
            return services;
        }

    }
}
