using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniring.Contracts.Auth
{
    public record RegisterRequest(string UserName, string PhoneNumber, string? Email, string Password);
}
