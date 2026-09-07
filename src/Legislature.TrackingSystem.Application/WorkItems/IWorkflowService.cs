namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Drives the review and approval workflow of a work item (F3.1 - Configurable Workflow,
/// US-3.1.1, US-3.1.2, US-3.1.3).
/// </summary>
public interface IWorkflowService
{
    Task<WorkTaskDto> SubmitForReviewAsync(SubmitWorkItemForReviewCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> RecordReviewAsync(RecordWorkflowReviewCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> FinalizeAsync(FinalizeWorkItemCommand command, CancellationToken cancellationToken);
}
