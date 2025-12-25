namespace Uniring.Contracts.Ring
{
    public class RingRegisterRequest
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string OwnerPhoneNumber { get; set; }

        public required List<Guid> MediaIds { get; set; }
    }
}
