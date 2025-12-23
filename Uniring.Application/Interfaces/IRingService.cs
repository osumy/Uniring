using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Ring;

namespace Uniring.Application.Interfaces
{
    public interface IRingService
    {
        public Task<RingResponse?> GetRingByUidAsync(string uid);
        public Task<RingResponse?> GetRingBySerialAsync(string uid);


        Task<Result<RingResponse>> CreateRingAsync(RingRegisterRequest request);
        Task<Result<RingResponse>> UpdateRingAsync(RingRegisterRequest request);
        Task<Result<RingResponse?>> DeleteRingAsync(Guid id);
    }
}
