namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Drives the RFA Executive Review path and step-level workflow management of a fiscal product
/// (F3.2 - RFA Executive Review, US-3.2.1 through US-3.2.4).
/// </summary>
public interface IExecutiveReviewService
{
    Task<WorkTaskDto> StartExecutiveReviewAsync(StartExecutiveReviewCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> BeginExecutiveReviewStepAsync(BeginExecutiveReviewStepCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> AdjustExecutiveReviewAsync(AdjustExecutiveReviewCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> CompleteExecutiveReviewStepAsync(CompleteExecutiveReviewStepCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> SetPriorityAsync(SetWorkTaskPriorityCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> AddStepAsync(AddWorkflowStepCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> SetStepDueDateAsync(SetWorkflowStepDueDateCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> CompleteStepAsync(CompleteWorkflowStepCommand command, CancellationToken cancellationToken);
}
