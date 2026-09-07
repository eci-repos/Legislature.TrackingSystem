namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Assigns a legislative implementation task to an individual across DOR (US-12.1.1, B.LNP.04;
/// US-12.1.5, B.LNP.08).
/// </summary>
public sealed record AssignImplementationTaskCommand(
    Guid BillId,
    string Title,
    string? AssignedTo,
    string? Division,
    string? RequiredWork,
    DateOnly? DueDate,
    string? AssignedByKey);
