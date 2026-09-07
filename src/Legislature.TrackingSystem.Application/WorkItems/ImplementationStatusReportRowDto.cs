using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// A row in the legislative implementation-task status report (US-12.1.3, B.LNP.06).
/// </summary>
public sealed record ImplementationStatusReportRowDto(
    Guid TaskId,
    string Title,
    string? AssignedTo,
    string? Division,
    DateOnly? DueDate,
    ImplementationTaskStatus Status);
