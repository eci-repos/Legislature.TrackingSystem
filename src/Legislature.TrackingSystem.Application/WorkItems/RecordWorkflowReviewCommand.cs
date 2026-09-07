using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Records a single reviewer's decision (approve/reject) on a work item (US-3.1.2, B.COM.19).
/// </summary>
public sealed record RecordWorkflowReviewCommand(
    Guid WorkItemId,
    string ReviewerKey,
    WorkflowDecision Decision,
    string? Comment);
