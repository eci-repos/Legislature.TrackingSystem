namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A fiscal data point (FTE, cost rule, revenue fund, or revenue source) retrieved, calculated, or
/// updated from internal DOR systems (US-7.1.1, B.COM.36).
/// </summary>
public sealed class FiscalData
{
    private FiscalData(Guid id, FiscalDataCategory category, string name, decimal value, string unit, string? source, DateTimeOffset updatedAt)
    {
        Id = id;
        Category = category;
        Name = name;
        Value = value;
        Unit = unit;
        Source = source;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }

    public FiscalDataCategory Category { get; }

    public string Name { get; }

    public decimal Value { get; private set; }

    public string Unit { get; }

    public string? Source { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static FiscalData Create(FiscalDataCategory category, string name, decimal value, string unit, string? source, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Fiscal data requires a name.", nameof(name));
        }

        return new FiscalData(Guid.NewGuid(), category, name.Trim(), value, unit ?? string.Empty, source, at);
    }

    public void Update(decimal value, string? source, DateTimeOffset at)
    {
        Value = value;
        Source = source;
        UpdatedAt = at;
    }
}
