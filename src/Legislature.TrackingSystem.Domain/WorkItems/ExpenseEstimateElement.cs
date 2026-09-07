namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// An auditable expense-estimate element entered or updated by Budget Office users (US-14.1.1,
/// B.BGT.01). Supported elements include costs of goods and services and percentages of salary.
/// </summary>
public sealed class ExpenseEstimateElement
{
    private ExpenseEstimateElement(Guid id, string name, ExpenseEstimateElementKind kind, decimal value, DateOnly effectiveDate, string updatedByKey, DateTimeOffset updatedAt)
    {
        Id = id;
        Name = name;
        Kind = kind;
        Value = value;
        EffectiveDate = effectiveDate;
        UpdatedByKey = updatedByKey;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }

    public string Name { get; }

    public ExpenseEstimateElementKind Kind { get; }

    public decimal Value { get; private set; }

    public DateOnly EffectiveDate { get; }

    public string UpdatedByKey { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static ExpenseEstimateElement Create(string name, ExpenseEstimateElementKind kind, decimal value, DateOnly effectiveDate, string updatedByKey, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("An expense-estimate element requires a name.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(updatedByKey))
        {
            throw new ArgumentException("An expense-estimate element requires an updater.", nameof(updatedByKey));
        }

        return new ExpenseEstimateElement(Guid.NewGuid(), name.Trim(), kind, value, effectiveDate, updatedByKey.Trim(), at);
    }

    public void Update(decimal value, string updatedByKey, DateTimeOffset at)
    {
        Value = value;
        UpdatedByKey = updatedByKey;
        UpdatedAt = at;
    }
}
