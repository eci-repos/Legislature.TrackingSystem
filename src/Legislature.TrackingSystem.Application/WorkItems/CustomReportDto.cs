namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record CustomReportDto(Guid Id, string Name, string OwnerKey, string Query, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
