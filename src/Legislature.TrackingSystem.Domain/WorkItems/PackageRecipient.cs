namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A named recipient of a finalized package, classified as internal or external to DOR
/// (US-3.1.3, B.COM.20).
/// </summary>
public sealed record PackageRecipient(
    Guid Id,
    string Name,
    PackageRecipientKind Kind,
    DateTimeOffset AddedAt)
{
    public static PackageRecipient Create(string name, PackageRecipientKind kind, DateTimeOffset addedAt)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A package recipient requires a name.", nameof(name));
        }

        return new PackageRecipient(Guid.NewGuid(), name.Trim(), kind, addedAt);
    }
}
