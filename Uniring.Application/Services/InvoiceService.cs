using Uniring.Application.Interfaces;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Contracts.Invoice;
using Uniring.Domain.Entities;

namespace Uniring.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<List<InvoiceResponse>> GetRecentInvoicesAsync(int count)
        {
            var items = await _invoiceRepository.GetRecentAsync(count);
            return items.Select(MapToResponse).ToList();
        }

        public async Task<InvoiceResponse?> GetByIdAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            return invoice == null ? null : MapToResponse(invoice);
        }

        public async Task<InvoiceResponse> CreateAsync(InvoiceCreateRequest request)
        {
            var now = request.CreatedAtUtc ?? DateTime.UtcNow;

            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                RingId = request.RingId,
                UserId = request.UserId,
                CreatedAtUtc = now
            };

            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();

            var created = await _invoiceRepository.GetByIdAsync(invoice.Id) ?? invoice;
            return MapToResponse(created);
        }

        public async Task<InvoiceResponse> UpdateOwnerAsync(InvoiceUpdateRequest request)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.Id)
                ?? throw new InvalidOperationException("Invoice not found.");

            invoice.UserId = request.UserId;
            await _invoiceRepository.SaveChangesAsync();

            var updated = await _invoiceRepository.GetByIdAsync(invoice.Id) ?? invoice;
            return MapToResponse(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null) return;

            _invoiceRepository.Delete(invoice);
            await _invoiceRepository.SaveChangesAsync();
        }

        private static InvoiceResponse MapToResponse(Invoice invoice)
        {
            return new InvoiceResponse
            {
                Id = invoice.Id,
                RingId = invoice.RingId,
                UserId = invoice.UserId,
                CreatedAtUtc = invoice.CreatedAtUtc,
                RingSerial = invoice.Ring?.Serial ?? string.Empty,
                RingName = invoice.Ring?.Name ?? string.Empty,
                UserDisplayName = invoice.User?.DisplayName ?? string.Empty,
                UserPhoneNumber = invoice.User?.PhoneNumber ?? string.Empty
            };
        }
    }
}
