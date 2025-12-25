using System.Text.Json;
using Uniring.App.Interfaces;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;

namespace Uniring.App.Services
{
    public class AdminApiService : IAdminApiService
    {
        private readonly IHttpClientFactory _httpFactory;

        public AdminApiService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<bool> CreateNewAccountAsync(RegisterRequest requestModel)
        {
            var client = _httpFactory.CreateClient("Api");
            var result = await client.PostAsJsonAsync("Admin/register-user", requestModel);
            var raw = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
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
