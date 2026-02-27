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
        Task<List<LoginResponse>> SearchUsersAsync(string term, bool includeGuests);
        Task<Result<LoginResponse>> GetByIdAsync(string id);

        Task<Result<bool>> DeleteUserAsync(string id);
        Task<Result<LoginResponse>> UpdateUserAsync(string id, UpdateUserRequest request);

        Task<Result<bool>> ChangePasswordAsync(string id, string newPassword);

        // ForgotPassword, ResetPassword, ...

    }

}
