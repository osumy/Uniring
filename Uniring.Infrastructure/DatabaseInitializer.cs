using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uniring.Domain.Entities.IdentityEntities;

namespace Uniring.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            string[] roleNames = { "guest", "admin", "user" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }
        }

        public static async Task InitializeAdminsAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            var admins = new[]
            {
                new { Phone = "+989129999999", DisplayName = "admin", Password = "pass@admin123" },
            };

            foreach (var admin in admins)
            {
                var user = await userManager.Users
                                            .FirstOrDefaultAsync(u => u.PhoneNumber == admin.Phone);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        PhoneNumber = admin.Phone,
                        UserName = admin.Phone,
                        DisplayName = admin.DisplayName,
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var result = await userManager.CreateAsync(user, admin.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "admin");
                    }
                }
                else if (!await userManager.IsInRoleAsync(user, "admin"))
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
        }
    }
}