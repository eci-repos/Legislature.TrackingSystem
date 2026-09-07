using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// A work product surfaced for historical reference (US-10.2.1, B.EXP.01). Carries the year and
/// biennium so authorized users can view products across the required 10-year period.
/// </summary>
public sealed record HistoricalWorkProductDto(
    Guid Id,
    string Identifier,
    WorkItemType Type,
    string Title,
    WorkTaskStatus Status,
    int Year,
    string Biennium,
    DateTimeOffset CreatedAt);
