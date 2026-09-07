namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record SetWorkTaskCategorizationCommand(
    Guid WorkTaskId,
    bool IsConfidential,
    bool IsExecutiveReview);
