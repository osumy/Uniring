using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice?> GetByIdAsync(Guid id);
        Task<List<Invoice>> GetRecentAsync(int count);
        Task AddAsync(Invoice invoice);
        void Delete(Invoice invoice);
        Task SaveChangesAsync();
    }
}
