using Microsoft.EntityFrameworkCore;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Domain.Entities;

namespace Uniring.Infrastructure.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly UniringDbContext _db;

        public MediaRepository(UniringDbContext db)
        {
            _db = db;
        }

        public async Task<Media?> GetFileByIdAsync(Guid id)
        {
            return await _db.Medias.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> SaveFileAsync(Media record)
        {
            try
            {
                _db.Medias.Add(record);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(Media record)
        {
            try
            {
                _db.Medias.Remove(record);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
