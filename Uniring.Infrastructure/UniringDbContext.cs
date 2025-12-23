using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uniring.Domain.Entities;
using Uniring.Domain.Entities.IdentityEntities;

namespace Uniring.Infrastructure
{
    public class UniringDbContext : 
        IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public UniringDbContext(DbContextOptions<UniringDbContext> options) : base(options) { }

        public DbSet<Ring> Rings { get; set; }
        public DbSet<Media> Medias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Data
            builder.Entity<Ring>().HasData( new Ring
            {
                Uid = "UID", Name = "انگشتر عقیق", Serial = "R2732874204", Id = Guid.NewGuid()
            });

            // Unique index on PhoneNumber to prevent duplicate phone registrations.
            // Note: ensure phone normalization before storing (E.164).
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }

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
    }
}
