using System.ComponentModel.DataAnnotations;

namespace Uniring.Contracts.Auth
{
    public class UpdateUserRequest
    {
        [Required]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}

