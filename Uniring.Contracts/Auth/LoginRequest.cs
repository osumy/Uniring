using System.ComponentModel.DataAnnotations;

namespace Uniring.Contracts.Auth
{
    /// <summary>
    /// Login request: PhoneNumber + Password only.
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "Phone Number Can't be blank")]
        // TODO: Regex 
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password Can't be blank")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
