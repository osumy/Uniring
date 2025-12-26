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

        public async Task<Ring?> GetRingBySerialAsync(string serial)
        {
            return await _db.Rings
                .Include(r => r.Medias)
                .FirstOrDefaultAsync(r => r.Serial == serial);
        }

        public async Task<Ring?> GetRingByUidAsync(string uid)
        {
            return await _db.Rings
                .Include(r => r.Medias)
                .FirstOrDefaultAsync(r => r.Uid == uid);
        }

        public async Task<Ring?> GetByIdAsync(Guid id)
        {
            return await _db.Rings
                .Include(r => r.Medias) // Include related media
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Ring ring)
        {
            await _db.Rings.AddAsync(ring);
        }

        public void Delete(Ring ring)
        {
            _db.Rings.Remove(ring);
        }

        public async Task<bool> ExistsBySerialAsync(string serial)
        {
            return await _db.Rings.AnyAsync(r => r.Serial == serial);
        }

        public async Task<List<Ring>> GetAllAsync()
        {
            return await _db.Rings.Include(r => r.Medias).ToListAsync();
        }

        //// Don't forget to implement UnitOfWork property
        //public IUnitOfWork UnitOfWork => _db;
    }
}
