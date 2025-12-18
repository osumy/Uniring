namespace Uniring.Contracts.Auth
{
    /// <summary>
    /// Login request: PhoneNumber + Password only.
    /// </summary>
    public record LoginRequest(
        string PhoneNumber,
        string Password
        );
}
