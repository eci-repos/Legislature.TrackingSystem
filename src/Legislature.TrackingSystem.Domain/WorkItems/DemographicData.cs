namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A demographic data point stored by legislative session (US-7.2.2, B.RFA.08). Current and
/// historical values are retained so expense differences and demographic trends can be compared.
/// </summary>
public sealed class DemographicData
{
    private DemographicData(Guid id, string session, string category, decimal value, int year, DateTimeOffset updatedAt)
    {
        Id = id;
        Session = session;
        Category = category;
        Value = value;
        Year = year;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }

    public string Session { get; }

    public string Category { get; }

    public decimal Value { get; private set; }

    public int Year { get; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static DemographicData Create(string session, string category, decimal value, int year, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(session))
        {
            throw new ArgumentException("Demographic data requires a session.", nameof(session));
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Demographic data requires a category.", nameof(category));
        }

        return new DemographicData(Guid.NewGuid(), session.Trim(), category.Trim(), value, year, at);
    }

    public void Update(decimal value, DateTimeOffset at)
    {
        Value = value;
        UpdatedAt = at;
    }
}
