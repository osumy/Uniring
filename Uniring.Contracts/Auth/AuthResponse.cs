namespace Uniring.Contracts.Auth
{
    public class AuthResponse
    {
        public string? Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public bool Success { get; set; }
    }
}
