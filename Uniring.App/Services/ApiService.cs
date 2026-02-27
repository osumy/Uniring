using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using Uniring.App.Interfaces;
using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;

namespace Uniring.App.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpFactory;

        public ApiService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<RingResponse?> GetRingBySerialAsync(string serial)
        {
            var client = _httpFactory.CreateClient("Api");
            var result = await client.GetAsync($"Ring/serial/{serial}");

            if (!result.IsSuccessStatusCode) return null;

            var json = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RingResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        //public async Task<Result<RingResponse>> GetRingByUidAsync(string uid)
        //{
        //    var client = _httpFactory.CreateClient("Api");
        //    var result = await client.GetAsync($"Ring/uid/{uid}");

        //    if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        return Result<RingResponse>.Success(null); // واقعاً پیدا نشد

        //    if (!result.IsSuccessStatusCode)
        //        return Result<RingResponse>.Error($"API Error: {result.StatusCode}");

        //    var json = await result.Content.ReadAsStringAsync();
        //    var data = JsonSerializer.Deserialize<RingResponse>(json);

        //    return Result<RingResponse>.Success(data);
        //}

        public async Task<RingResponse?> GetRingByUidAsync(string uid)
        {
            var client = _httpFactory.CreateClient("Api");
            var result = await client.GetAsync($"Ring/uid/{uid}");

            if (!result.IsSuccessStatusCode) return null;

            var json = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RingResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<LoginResponse> RegisterAsync(RegisterRequest requestModel)
        {
            var client = _httpFactory.CreateClient("Api");
            var result = await client.PostAsJsonAsync("Account/register", requestModel);
            var raw = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                try
                {
                    // Try to parse expected success DTO
                    var data = JsonSerializer.Deserialize<LoginResponse>(raw);

                    return data;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }


            return null;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest requestModel)
        {
            var client = _httpFactory.CreateClient("Api");
            var result = await client.PostAsJsonAsync("Account/login", requestModel);
            var raw = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                try
                {
                    // Try to parse expected success DTO
                    var data = JsonSerializer.Deserialize<LoginResponse>(raw);

                    return data;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }


            return null;
        }
    }
}
