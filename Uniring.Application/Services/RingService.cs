using Uniring.Application.Interfaces;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Domain.Entities;

namespace Uniring.Application.Services
{
    public class RingService : IRingService
    {
        private readonly IRingRepository _ringRepository;

        public RingService(IRingRepository ringRepository)
        {
            _ringRepository = ringRepository;
        }

        public async Task<Ring?> GetRingByUid(string uid)
        {
            var result = await _ringRepository.GetRingByUid(uid);

            return result;
        }
    }
}
