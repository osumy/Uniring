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
        Task<bool> CreateNewRingAsync(RingResponse requestModel);

        Task<bool> UpdateAccountAsync();
        Task<bool> UpdateRingAsync();

        Task<bool> DeleteAccountAsync();
        Task<bool> DeleteRingAsync();

        Task<List<LoginResponse>> GetUsersAsync();

    }
}
