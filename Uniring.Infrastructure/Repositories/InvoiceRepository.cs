using Microsoft.EntityFrameworkCore;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Domain.Entities;

namespace Uniring.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly UniringDbContext _db;

        public InvoiceRepository(UniringDbContext db)
        {
            _db = db;
        }

        public async Task<Invoice?> GetByIdAsync(Guid id)
        {
            return await _db.Invoices
                .Include(i => i.Ring)
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Invoice>> GetRecentAsync(int count)
        {
            return await _db.Invoices
                .Include(i => i.Ring)
                .Include(i => i.User)
                .OrderByDescending(i => i.CreatedAtUtc)
                .Take(count)
                .ToListAsync();
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _db.Invoices.AddAsync(invoice);
        }

        public void Delete(Invoice invoice)
        {
            _db.Invoices.Remove(invoice);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
