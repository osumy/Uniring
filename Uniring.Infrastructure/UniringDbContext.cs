using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uniring.Domain.Entities;
using Uniring.Infrastructure.Entities;

namespace Uniring.Infrastructure
{
    public class UniringDbContext : IdentityDbContext<ApplicationUser>
    {
        public UniringDbContext(DbContextOptions<UniringDbContext> options) : base(options) { }

        DbSet<Ring> Rings { get; set; }

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
