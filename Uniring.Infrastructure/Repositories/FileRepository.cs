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

        public Task<bool> SaveFileAsync(FileRecord record) 
        {
            _db.Files.Add(record);
            //await _db.SaveChangesAsync();
            return null; 
        }

        public Task<bool> DeleteFileAsync(FileRecord record)
        {
            try
            {
                if (File.Exists(record.Path)) File.Delete(record.Path);
                _db.Files.Remove(record);
                _db.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }
    }
}
