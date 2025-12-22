using Uniring.Application.Interfaces;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Contracts.Ring;
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

        public async Task<RingResponse?> GetRingBySerialAsync(string serial)
        {
            var result = await _ringRepository.GetRingBySerialAsync(serial);

            if (result == null) 
            {
                return null;
            }

            RingResponse response = new RingResponse
            {
                Uid = result.Uid,
                Serial = result.Serial,
                Name = result.Name,
                Price = result.Price,
                Id = result.Id,
                Description = result.Description
            };

            return response;
        }

        public async Task<RingResponse?> GetRingByUidAsync(string uid)
        {
            var result = await _ringRepository.GetRingByUidAsync(uid);

            if (result == null)
            {
                return null;
            }

            RingResponse response = new RingResponse 
            { 
                Uid = result.Uid,
                Serial = result.Serial,
                Name = result.Name,
                Price = result.Price,
                Id = result.Id,
                Description = result.Description
            };

            return response;
        }

    }
}
