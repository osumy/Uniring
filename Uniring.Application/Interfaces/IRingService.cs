
using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces
{
    public interface IRingService
    {
        public Task<Ring?> GetRingByUid(string uid);
    }
}
