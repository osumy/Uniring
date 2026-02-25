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

        public async Task<bool> DeleteAccountAsync(string userId)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.DeleteAsync($"Admin/users/{userId}");
            return res.IsSuccessStatusCode;
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

        public async Task<bool> UpdateAccountAsync(string userId, UpdateUserRequest requestModel)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.PutAsJsonAsync($"Admin/users/{userId}", requestModel);
            return res.IsSuccessStatusCode;
        }

        public Task<bool> UpdateRingAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<LoginResponse>> GetUsersAsync()
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.GetAsync("Admin/users");
            if (!res.IsSuccessStatusCode) return new List<LoginResponse>();
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<LoginResponse>>(json) ?? new List<LoginResponse>();
        }

        public async Task<LoginResponse?> GetUserByIdAsync(string userId)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.GetAsync($"Admin/users/{userId}");
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(json);
        }

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest requestModel)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.PostAsJsonAsync($"Admin/users/{userId}/change-password", requestModel);
            return res.IsSuccessStatusCode;
        }

    }
}
