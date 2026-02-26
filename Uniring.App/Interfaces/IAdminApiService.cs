using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;

namespace Uniring.App.Interfaces
{
    public interface IAdminApiService
    {
        Task<RingListResponse?> GetRingListAsync();
        Task<RingResponse?> GetRingBySerialAsync(string serial);
        //Task<AccountListResponse?> GetAccountListAsync();
        //Task<AccountResponse?> GetAccountByPhoneNumberAsync(string PhoneNumber);

        Task<bool> CreateNewAccountAsync(RegisterRequest requestModel);
        Task<bool> CreateNewRingAsync(RingRegisterRequest requestModel);

        Task<bool> UpdateAccountAsync(string userId, UpdateUserRequest requestModel);
        Task<bool> UpdateRingAsync();

        Task<bool> DeleteAccountAsync(string userId);
        Task<bool> DeleteRingAsync(string ringId);

        Task<List<LoginResponse>> GetUsersAsync();
        Task<List<RingResponse>> GetRingsAsync();

        Task<LoginResponse?> GetUserByIdAsync(string userId);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest requestModel);

    }
}
