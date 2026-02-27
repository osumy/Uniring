using System.Text.Json;
using Uniring.App.Interfaces;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;
using Uniring.Contracts.Invoice;

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

        public async Task<bool> CreateNewRingAsync(RingRegisterRequest requestModel)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.PostAsJsonAsync("Admin/rings", requestModel);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAccountAsync(string userId)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.DeleteAsync($"Admin/users/{userId}");
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRingAsync(string ringId)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.DeleteAsync($"Admin/rings/{ringId}");
            return res.IsSuccessStatusCode;
        }

        public async Task<List<RingResponse>> GetRingsAsync()
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.GetAsync("Admin/rings");
            if (!res.IsSuccessStatusCode) return new List<RingResponse>();
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<RingResponse>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<RingResponse>();
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

        public async Task<List<LoginResponse>> SearchUsersAsync(string term)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.GetAsync($"Admin/users/search?term={Uri.EscapeDataString(term ?? string.Empty)}&includeGuests=true");
            if (!res.IsSuccessStatusCode) return new List<LoginResponse>();
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<LoginResponse>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<LoginResponse>();
        }

        public async Task<List<InvoiceResponse>> GetRecentInvoicesAsync(int count)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.GetAsync($"Admin/invoices/recent?count={count}");
            if (!res.IsSuccessStatusCode) return new List<InvoiceResponse>();
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<InvoiceResponse>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<InvoiceResponse>();
        }

        public async Task<InvoiceResponse?> GetInvoiceByIdAsync(Guid id)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.GetAsync($"Admin/invoices/{id}");
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<InvoiceResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<InvoiceResponse?> CreateInvoiceAsync(InvoiceCreateRequest requestModel)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.PostAsJsonAsync("Admin/invoices", requestModel);
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<InvoiceResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<InvoiceResponse?> ChangeInvoiceOwnerAsync(Guid id, InvoiceUpdateRequest requestModel)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.PutAsJsonAsync($"Admin/invoices/{id}/owner", requestModel);
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<InvoiceResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> DeleteInvoiceAsync(Guid id)
        {
            var client = _httpFactory.CreateClient("Api");
            var res = await client.DeleteAsync($"Admin/invoices/{id}");
            return res.IsSuccessStatusCode;
        }

    }
}
