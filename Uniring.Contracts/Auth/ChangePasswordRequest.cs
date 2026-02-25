using System.ComponentModel.DataAnnotations;

namespace Uniring.Contracts.Auth
{
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and confirmation do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

