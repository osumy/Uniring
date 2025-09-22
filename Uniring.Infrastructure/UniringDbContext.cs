using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uniring.Infrastructure.Entities;

namespace Uniring.Infrastructure
{
    public class UniringDbContext : IdentityDbContext<ApplicationUser>
    {
        public UniringDbContext(DbContextOptions<UniringDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Unique index on PhoneNumber to prevent duplicate phone registrations.
            // Note: ensure phone normalization before storing (E.164).
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }
    }
}
