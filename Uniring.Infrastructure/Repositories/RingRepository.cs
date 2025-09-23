using Microsoft.EntityFrameworkCore;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Domain.Entities;

namespace Uniring.Infrastructure.Repositories
{
    public class RingRepository : IRingRepository
    {
        private readonly UniringDbContext _db;

        public RingRepository(UniringDbContext db)
        {
            _db = db;
        }

        public async Task<Ring?> GetRingByUid(string uid)
        {
            return await _db.Rings.FirstOrDefaultAsync(r => r.Uid == uid);
        }
    }
}
