using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record DocumentTemplateDto(
    Guid Id,
    string Name,
    WorkItemType? ApplicableWorkType,
    string Body,
    bool IsShared,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
