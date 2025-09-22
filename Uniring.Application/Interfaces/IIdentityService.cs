using Uniring.Contracts.Auth;

namespace Uniring.Application.Interfaces
{

    public interface IIdentityService
    {
        Task<(bool Succeeded, IEnumerable<string>? Errors)> RegisterAsync(RegisterRequest request, string? role = null);
        Task<(bool Success, string? Token, DateTime? ExpiresAt, IEnumerable<string>? Errors)> LoginAsync(LoginRequest request);
        Task SignOutAsync(); // for Cookie sign-out in UI
        Task<bool> ConfirmEmailAsync(string userId, string token);
        // ForgotPassword, ResetPassword, GetUserById ...
    }

}
