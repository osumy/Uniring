
using Microsoft.AspNetCore.Identity;

namespace Uniring.Domain.Entities.IdentityEntities
{

    /// <summary>
    /// Primary login identifier is PhoneNumber.
    /// Stores registration and last purchase timestamps (UTC).
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }

        // Store the user's registration time (UTC)
        public DateTime RegistrationDateTimeUtc { get; set; }

        // Store the UTC time of last purchase
        public DateTime? LastPurchaseAtUtc { get; set; }

    }
}
