using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniring.Contracts.Auth
{
    /// <summary>
    /// Login request: PhoneNumber + Password only.
    /// </summary>
    public record LoginRequest(
        string PhoneNumber,
        string Password
        );
}
