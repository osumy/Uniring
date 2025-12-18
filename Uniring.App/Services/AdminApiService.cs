using Uniring.App.Interfaces;
using Uniring.Contracts.Account;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;

namespace Uniring.App.Services
{
    public class AdminApiService : IAdminApiService
    {
        public Task<bool> CreateNewAccountAsync(RegisterRequest requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateNewRingAsync(RingResponse requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAccountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRingAsync()
        {
            throw new NotImplementedException();
        }

        //public Task<AccountResponse?> GetAccountByPhoneNumberAsync(string PhoneNumber)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<AccountListResponse?> GetAccountListAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public Task<RingResponse?> GetRingBySerialAsync(string serial)
        {
            throw new NotImplementedException();
        }

        public Task<RingListResponse?> GetRingListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAccountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRingAsync()
        {
            throw new NotImplementedException();
        }
    }
}
