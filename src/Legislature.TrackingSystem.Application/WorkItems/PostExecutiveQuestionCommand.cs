namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Posts an executive discussion question on a bill page (US-13.2.2, B.EXEC.04).
/// </summary>
public sealed record PostExecutiveQuestionCommand(
    Guid BillId,
    string AuthorKey,
    string Question,
    Guid? AssociatedWorkTaskId);
