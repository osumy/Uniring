using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Uniring.Domain.Entities.IdentityEntities;

namespace Uniring.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<UniringDbContext>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                //var logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();

                // Apply any pending migrations
                await context.Database.MigrateAsync();
                //logger.LogInformation("Database migrations applied successfully.");

                // Seed roles
                await SeedRolesAsync(roleManager);

                // You can also seed admin users here if needed
                // await SeedAdminUserAsync(userManager, roleManager, logger);
            }
            catch (Exception ex)
            {
                //var logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();
                //logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            string[] roleNames = { "guest", "admin", "user" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                    //logger.LogInformation($"Role '{roleName}' created successfully.");
                }
                else
                {
                    //logger.LogInformation($"Role '{roleName}' already exists.");
                }
            }
        }

        // Optional: Seed an admin user
        private static async Task SeedAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger logger)
        {
            var adminEmail = "admin@uniring.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    PhoneNumber = "+1234567890",
                    RegistrationDateTimeUtc = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "admin");
                    logger.LogInformation("Admin user created successfully.");
                }
                else
                {
                    logger.LogError($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}