
using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces.Repositories
{
    public interface IRingRepository
    {
        /// <summary>
        /// Returns a Ring object based on given uid (NanoId)
        /// </summary>
        /// <param name="uid">string NanoId</param>
        /// <returns>Ring</returns>
        public Task<Ring?> GetRingByUid(string uid);
    }
}
