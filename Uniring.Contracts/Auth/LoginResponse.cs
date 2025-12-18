namespace Uniring.Contracts.Auth
{
    public class LoginResponse
    {
        public required string Id { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Role { get; set; }
        public string? Token { get; set; }

    }
}
