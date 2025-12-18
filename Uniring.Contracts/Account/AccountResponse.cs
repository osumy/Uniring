namespace Uniring.Contracts.Account
{
    public record AccountResponse(
    string Name,
    string PhoneNumber,
    DateTime RegistrationDateTimeUtc,
    DateTime? LastPurchaseAtUtc
    );
}
