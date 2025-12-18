using Uniring.Contracts.Auth;

namespace Uniring.Api.Authentication
{
    public interface IJwtGenerator
    {
        string GenerateToken(LoginResponse loginResponse);

    }
}
