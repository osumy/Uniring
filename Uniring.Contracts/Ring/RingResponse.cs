namespace Uniring.Contracts.Ring
{
    public class RingResponse
    {
        public required Guid Id { get; set; }

        public required string Uid { get; set; }

        public required string Name { get; set; }

        public required string Serial { get; set; }

        public string? Description { get; set; }

        public required List<Guid> mediaIds { get; set; }

    }
}
