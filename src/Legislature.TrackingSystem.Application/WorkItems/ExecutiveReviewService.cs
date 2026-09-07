using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ExecutiveReviewService : IExecutiveReviewService
{
    private readonly IWorkTaskRepository _repository;

    public ExecutiveReviewService(IWorkTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkTaskDto> StartExecutiveReviewAsync(StartExecutiveReviewCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);

        foreach (string key in command.ReviewerKeys)
        {
            bool designated = task.ActiveAssignments.Any(a =>
                a.Role == AssignmentRole.ExecutiveReviewer &&
                string.Equals(a.AssigneeKey, key, StringComparison.OrdinalIgnoreCase));

            if (!designated)
            {
                throw new InvalidOperationException(
                    $"User '{key}' is not a designated executive reviewer for '{task.Identifier.Value}'.");
            }
        }

        task.StartExecutiveReview(command.ReviewerKeys, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> BeginExecutiveReviewStepAsync(BeginExecutiveReviewStepCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.BeginExecutiveReviewStep(command.ReviewerKey, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> AdjustExecutiveReviewAsync(AdjustExecutiveReviewCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.AdjustExecutiveReview(command.ReviewerKey, command.Note, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> CompleteExecutiveReviewStepAsync(CompleteExecutiveReviewStepCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.CompleteExecutiveReviewStep(command.ReviewerKey, command.Comment, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> SetPriorityAsync(SetWorkTaskPriorityCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.SetPriority(command.Priority, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> AddStepAsync(AddWorkflowStepCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.AddStep(command.Name, command.DueDate, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> SetStepDueDateAsync(SetWorkflowStepDueDateCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.SetStepDueDate(command.StepId, command.DueDate, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> CompleteStepAsync(CompleteWorkflowStepCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.CompleteStep(command.StepId, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    private async Task<WorkTask> GetTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{id}' was not found.");
    }
}
