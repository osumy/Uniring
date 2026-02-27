using Uniring.Contracts.Invoice;

namespace Uniring.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceResponse>> GetRecentInvoicesAsync(int count);
        Task<InvoiceResponse?> GetByIdAsync(Guid id);
        Task<InvoiceResponse> CreateAsync(InvoiceCreateRequest request);
        Task<InvoiceResponse> UpdateOwnerAsync(InvoiceUpdateRequest request);
        Task DeleteAsync(Guid id);
    }
}
