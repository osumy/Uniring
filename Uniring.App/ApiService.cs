using System.Text.Json;
using Uniring.Contracts.Ring;

namespace Uniring.App
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

        public async Task<RingResponse?> GetRingByUidAsync(string uid)
        {
            var client = _httpFactory.CreateClient("Api");
            var result = await client.GetAsync($"Ring/uid/{uid}");

            if (!result.IsSuccessStatusCode) return null;

            var json = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RingResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
