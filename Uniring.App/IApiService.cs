using Uniring.Contracts.Ring;

namespace Uniring.App
{
    public interface IApiService
    {
        Task<RingResponse?> GetRingByUidAsync(string uid);
        Task<RingResponse?> GetRingBySerialAsync(string serial);

    }
}
