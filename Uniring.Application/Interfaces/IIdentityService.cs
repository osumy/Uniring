using Uniring.Contracts.Auth;

namespace Uniring.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, IEnumerable<string>? Errors)> RegisterUserAsync(RegisterRequest request);
        Task<(bool Succeeded, IEnumerable<string>? Errors)> RegisterAdminAsync(RegisterRequest request);
        Task<RegisterResponse> LoginAsync(LoginRequest request);
        Task SignOutAsync(); // For cookie signout in UI host
        Task SetLastPurchaseAsync(string userId, DateTime purchaseTime);

        // ForgotPassword, ResetPassword, GetUserById ...

    }

}
