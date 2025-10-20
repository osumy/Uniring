namespace Uniring.Contracts.Auth
{
    public record RegisterResponse(
        string Token,
        DateTime? ExpiresAt,
        DateTime? LastPurchaseAtUtc
    );
}
