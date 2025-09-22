using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uniring.Application.Interfaces;
using Uniring.Infrastructure.Entities;
using Uniring.Infrastructure.Services;
using Uniring.Infrastructure.Validators;

namespace Uniring.Infrastructure
{
    public static class UniringServiceRegistration
    {
        public static IServiceCollection AddUniringIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            //// Identity options
            //services.AddIdentityCore<ApplicationUser>(options =>
            //{
            //    // Do not force email usage
            //    options.User.RequireUniqueEmail = false;
            //    // Password policy
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireLowercase = false;
            //    // SignIn options
            //    options.SignIn.RequireConfirmedEmail = false; // we don't rely on email
            //                                                  // We won't use AllowedUserNameCharacters (we'll register a validator below)
            //})
            //.AddRoles<IdentityRole>()
            //.AddEntityFrameworkStores<UniringDbContext>()
            //.AddSignInManager()
            //.AddDefaultTokenProviders();

            // Register full Identity (includes SignInManager, UserManager, RoleManager, cookie defaults, etc.)
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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

            // Register your identity service implementation
            services.AddScoped<IIdentityService, IdentityService>();

            // JWT config: if the API will use JWT, register the JwtBearer in the Api host (not here)
            return services;
        }
    }
}
