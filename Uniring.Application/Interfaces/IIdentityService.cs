using Uniring.Contracts.Auth;

namespace Uniring.Application.Interfaces
{

    public interface IIdentityService
    {
        Task<(bool Succeeded, IEnumerable<string>? Errors)> RegisterAsync(RegisterRequest request, string? role = null);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task SignOutAsync(); // For cookie signout in UI host
        Task SetLastPurchaseAsync(string userId, DateTime purchaseTime);
        // ForgotPassword, ResetPassword, GetUserById ...
    }

}
