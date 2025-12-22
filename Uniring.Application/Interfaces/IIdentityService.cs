using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Auth;

namespace Uniring.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<LoginResponse>> RegisterUserAsync(RegisterRequest request);
        Task<Result<LoginResponse>> RegisterAdminAsync(RegisterRequest request);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
        Task SetLastPurchaseAsync(string userId, DateTime purchaseTime);

        // ForgotPassword, ResetPassword, GetUserById ...

    }

}
