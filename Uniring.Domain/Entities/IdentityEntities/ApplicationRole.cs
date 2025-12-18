using Microsoft.AspNetCore.Identity;

namespace Uniring.Domain.Entities.IdentityEntities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
        public ApplicationRole()
        {
        }
    }
}
