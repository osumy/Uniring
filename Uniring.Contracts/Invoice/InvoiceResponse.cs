namespace Uniring.Contracts.Invoice
{
    public class InvoiceResponse
    {
        public Guid Id { get; set; }
        public Guid RingId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        // For display
        public string RingSerial { get; set; } = string.Empty;
        public string RingName { get; set; } = string.Empty;
        public string UserDisplayName { get; set; } = string.Empty;
        public string UserPhoneNumber { get; set; } = string.Empty;
    }
}
