using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class WorkflowService : IWorkflowService
{
    private readonly IWorkTaskRepository _repository;
    private readonly INotificationService _notifications;

    public WorkflowService(IWorkTaskRepository repository, INotificationService notifications)
    {
        _repository = repository;
        _notifications = notifications;
    }

    public async Task<WorkTaskDto> SubmitForReviewAsync(SubmitWorkItemForReviewCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);

        int required = WorkflowDefinitionCatalog.RequiredReviewerCountFor(task.Type);
        if (command.ReviewerKeys.Count < required)
        {
            throw new InvalidOperationException(
                $"'{task.Identifier.Value}' requires {required} reviewer(s) for type '{task.Type}', but {command.ReviewerKeys.Count} were provided.");
        }

        task.SubmitForReview(command.ReviewerKeys, DateTimeOffset.UtcNow);

        foreach (string reviewerKey in command.ReviewerKeys)
        {
            await _notifications.NotifyAsync(
                reviewerKey,
                $"Review requested for {task.Identifier.Value}.",
                NotificationType.General,
                NotificationTrigger.ReviewRequested,
                cancellationToken);
        }

        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> RecordReviewAsync(RecordWorkflowReviewCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.RecordReview(command.ReviewerKey, command.Decision, command.Comment, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> FinalizeAsync(FinalizeWorkItemCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.Finalize(DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    private async Task<WorkTask> GetTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{id}' was not found.");
    }
}
