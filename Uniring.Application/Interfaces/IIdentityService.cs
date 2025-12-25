using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Auth;
using Uniring.Domain.Entities.IdentityEntities;

namespace Uniring.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<LoginResponse>> RegisterUserAsync(RegisterRequest request);
        Task<Result<LoginResponse>> RegisterCustomerAsync(RegisterRequest request);
        Task<Result<LoginResponse>> RegisterAdminAsync(RegisterRequest request);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
        Task SetLastPurchaseAsync(string userId, DateTime purchaseTime);

        Task<List<LoginResponse>> GetUsersInRoleAsync(string roleName);
        Task<Result<LoginResponse>> GetByIdAsync(string id);

        // ForgotPassword, ResetPassword, ...

    }

}
