
using Uniring.Contracts.Ring;
using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces
{
    public interface IRingService
    {
        public Task<RingResponse?> GetRingByUidAsync(string uid);
        public Task<RingResponse?> GetRingBySerialAsync(string uid);

    }
}
