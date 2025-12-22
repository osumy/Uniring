using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;

namespace Uniring.App.Interfaces
{
    public interface IApiService
    {
        Task<RingResponse?> GetRingByUidAsync(string uid);
        Task<RingResponse?> GetRingBySerialAsync(string serial);
        Task<LoginResponse> RegisterAsync(RegisterRequest requestModel);
        Task<LoginResponse> LoginAsync(LoginRequest requestModel);

    }
}
