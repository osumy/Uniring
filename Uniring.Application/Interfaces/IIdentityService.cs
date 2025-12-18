using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Auth;

namespace Uniring.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, IEnumerable<string>? Errors)> RegisterUserAsync(RegisterRequest request);
        Task<(bool Succeeded, IEnumerable<string>? Errors)> RegisterAdminAsync(RegisterRequest request);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
        Task SetLastPurchaseAsync(string userId, DateTime purchaseTime);

        // ForgotPassword, ResetPassword, GetUserById ...

    }

}
