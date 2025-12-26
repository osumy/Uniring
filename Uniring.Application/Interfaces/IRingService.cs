using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Ring;

namespace Uniring.Application.Interfaces
{
    public interface IRingService
    {
        Task<RingResponse?> GetRingByUidAsync(string uid);
        Task<RingResponse?> GetRingBySerialAsync(string uid);


        Task<Result<RingResponse>> CreateRingAsync(RingRegisterRequest request);
        Task<Result<RingResponse>> UpdateRingAsync(RingRegisterRequest request);
        Task<Result<RingResponse?>> DeleteRingAsync(Guid id);

        Task<List<RingResponse>> GetRingsAsync();

    }
}
