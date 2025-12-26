using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using System.Reflection.Emit;
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

            // Fix potential comparer issues for value types
            builder.Entity<Media>()
                .Property(m => m.Id)
                .HasConversion<Guid>();

            builder.Entity<Media>()
                .Property(m => m.CreatedAt)
                .HasConversion<DateTime>();

            builder.Entity<Ring>()
                .Property(r => r.Id)
                .HasConversion<Guid>();

            builder.Entity<Ring>()
                .HasMany(r => r.Medias)
                .WithOne(m => m.Ring)
                .HasForeignKey(m => m.RingId)
                .OnDelete(DeleteBehavior.SetNull);

            //// Optional: Ensure Ring.Uid is unique (if needed)
            //builder.Entity<Ring>()
            //    .HasIndex(r => r.Uid)
            //    .IsUnique();
        }
    }
}
