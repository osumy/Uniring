
using Uniring.Contracts.Ring;
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
        Task<Ring?> GetRingByUidAsync(string uid);

        /// <summary>
        /// Returns a Ring object based on given serial
        /// </summary>
        /// <param name="serial">string serial</param>
        /// <returns>Ring</returns>
        Task<Ring?> GetRingBySerialAsync(string uid);

        Task<List<Ring>> GetAllAsync();

    }
}
