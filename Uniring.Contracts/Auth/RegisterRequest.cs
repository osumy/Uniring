using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniring.Contracts.Auth
{
    /// <summary>
    /// Registration request:
    /// - DisplayName is required and used only as display (not for login)
    /// - PhoneNumber is required and used for login
    /// - Password is required
    /// </summary>
    public record RegisterRequest(
        string DisplayName,
        string PhoneNumber,
        string Password
        );
}
