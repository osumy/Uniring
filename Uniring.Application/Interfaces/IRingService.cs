using Uniring.Contracts.Ring;

namespace Uniring.Application.Interfaces
{
    public interface IRingService
    {
        public Task<RingResponse?> GetRingByUidAsync(string uid);
        public Task<RingResponse?> GetRingBySerialAsync(string uid);

    }
}
