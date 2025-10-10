using Microsoft.EntityFrameworkCore;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Domain.Entities;

namespace Uniring.Infrastructure.Repositories
{   
    public class FileRepository : IFileRepository
    {
        private readonly UniringDbContext _db;
        
        public FileRepository(UniringDbContext db) {
            _db = db;
        }

        public async Task<FileRecord?> GetFileByIdAsync(Guid id)
        {
            return await _db.Files.FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
