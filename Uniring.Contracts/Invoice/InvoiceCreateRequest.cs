namespace Uniring.Contracts.Invoice
{
    public class InvoiceCreateRequest
    {
        public Guid RingId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
    }
}
