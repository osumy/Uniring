using System.ComponentModel.DataAnnotations;

namespace Uniring.Domain.Entities
{
    public class Media
    {
        [Key]
        public required Guid Id { get; set; }
        [Required]
        public required string OriginalFileName { get; set; }
        [Required]
        public required string ContentType { get; set; }
        [Required]
        public required long Size { get; set; }
        [Required]
        public required string Path { get; set; }
        [Required]
        public required DateTime CreatedAt { get; set; }

        public Guid? RingId { get; set; }
        //public Ring? Ring { get; set; }
    }
}
