namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A saved custom query or report (US-6.2.2, B.COM.39). Authorized users can create, save, and
/// reuse custom queries and reports.
/// </summary>
public sealed class CustomReport
{
    private CustomReport(Guid id, string name, string ownerKey, string query, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    {
        Id = id;
        Name = name;
        OwnerKey = ownerKey;
        Query = query;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }

    public string Name { get; private set; }

    public string OwnerKey { get; }

    public string Query { get; private set; }

    public DateTimeOffset CreatedAt { get; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static CustomReport Create(string name, string ownerKey, string query, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A custom report requires a name.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(ownerKey))
        {
            throw new ArgumentException("A custom report requires an owner.", nameof(ownerKey));
        }

        return new CustomReport(Guid.NewGuid(), name.Trim(), ownerKey.Trim(), query ?? string.Empty, at, at);
    }

    public void UpdateQuery(string query, DateTimeOffset at)
    {
        Query = query ?? string.Empty;
        UpdatedAt = at;
    }
}
