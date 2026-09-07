using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Duplicates a work task into a new, independently identifiable task (US-1.4.1, B.COM.18).
/// </summary>
public sealed record DuplicateWorkTaskCommand(
    Guid WorkItemId,
    string? Title,
    string? ByKey);
