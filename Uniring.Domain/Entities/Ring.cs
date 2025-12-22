using System.ComponentModel.DataAnnotations;

namespace Uniring.Domain.Entities
{
    public class Ring
    {
        [Key]
        public required Guid Id { get; set; }

        [Required]
        public required string Uid { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Serial { get; set; }

        [Required]
        public required int Price { get; set; }

        public string? Description { get; set; }

    }
}
