using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;
using Uniring.Contracts.Invoice;

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

        Task<List<LoginResponse>> SearchUsersAsync(string term);
        Task<List<InvoiceResponse>> GetRecentInvoicesAsync(int count);
        Task<InvoiceResponse?> GetInvoiceByIdAsync(Guid id);
        Task<InvoiceResponse?> CreateInvoiceAsync(InvoiceCreateRequest requestModel);
        Task<InvoiceResponse?> ChangeInvoiceOwnerAsync(Guid id, InvoiceUpdateRequest requestModel);
        Task<bool> DeleteInvoiceAsync(Guid id);

        Task<LoginResponse?> GetUserByIdAsync(string userId);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest requestModel);

    }
}
