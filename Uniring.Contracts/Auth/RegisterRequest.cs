using System.ComponentModel.DataAnnotations;

namespace Uniring.Contracts.Auth
{
    /// <summary>
    /// Registration request:
    /// - DisplayName is required and used only as display (not for login)
    /// - PhoneNumber is required and used for login
    /// - Password is required
    /// </summary>
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Name Can't be blank")]
        public required string DisplayName { get; set; }

        [Required(ErrorMessage = "Phone Number Can't be blank")]
        // TODO: Regex 
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password Can't be blank")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
