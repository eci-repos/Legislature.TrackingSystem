using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record FiscalDataDto(Guid Id, FiscalDataCategory Category, string Name, decimal Value, string Unit, string? Source, DateTimeOffset UpdatedAt);
