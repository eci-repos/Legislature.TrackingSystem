namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record DemographicDataDto(Guid Id, string Session, string Category, decimal Value, int Year, DateTimeOffset UpdatedAt);
