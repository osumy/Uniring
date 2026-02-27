using System.ComponentModel.DataAnnotations;
using Uniring.Domain.Entities.IdentityEntities;

namespace Uniring.Domain.Entities
{
    /// <summary>
    /// Invoice links a ring to a user at a specific point in time.
    /// </summary>
    public class Invoice
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid RingId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CreatedAtUtc { get; set; }

        public Ring Ring { get; set; }

        public ApplicationUser User { get; set; }
    }
}
