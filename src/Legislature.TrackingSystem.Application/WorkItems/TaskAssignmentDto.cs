using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record TaskAssignmentDto(
    Guid Id,
    string AssigneeKey,
    AssignmentRole Role,
    DateOnly? DueDate,
    string? AssignedByKey,
    DateTimeOffset AssignedAt,
    bool IsRework,
    bool IsSuperseded);
