using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniring.Domain.Entities
{
    public class Ring
    {
        [Key]
        public Guid Id { get; set; }

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
