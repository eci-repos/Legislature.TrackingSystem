using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class WorkTaskAssignmentService : IWorkTaskAssignmentService
{
    private readonly IWorkTaskRepository _repository;
    private readonly INotificationService _notifications;

    public WorkTaskAssignmentService(IWorkTaskRepository repository, INotificationService notifications)
    {
        _repository = repository;
        _notifications = notifications;
    }

    public async Task<TaskAssignmentDto> AssignAsync(AssignWorkTaskCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkTaskId, cancellationToken);

        TaskAssignment assignment = task.AssignUser(
            command.AssigneeKey,
            command.Role,
            command.DueDate,
            command.AssignedByKey,
            DateTimeOffset.UtcNow);

        await _notifications.NotifyAssignmentAsync(command.AssigneeKey, task.Identifier.Value, cancellationToken);

        return ToAssignmentDto(assignment);
    }

    public async Task<TaskAssignmentDto> ReassignAsync(ReassignWorkTaskCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkTaskId, cancellationToken);

        TaskAssignment assignment = task.ReassignUser(
            command.PriorAssigneeKey,
            command.NewAssigneeKey,
            command.Role,
            command.DueDate,
            command.AssignedByKey,
            DateTimeOffset.UtcNow);

        await _notifications.NotifyAssignmentAsync(command.NewAssigneeKey, task.Identifier.Value, cancellationToken);

        return ToAssignmentDto(assignment);
    }

    public async Task<WorkQueueDto> GetWorkQueueAsync(string assigneeKey, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(assigneeKey))
        {
            throw new ArgumentException("A DOR user key is required to load a work queue.", nameof(assigneeKey));
        }

        string key = assigneeKey.Trim();
        IReadOnlyList<WorkTask> tasks = await _repository.GetAllAsync(cancellationToken);

        List<WorkQueueEntryDto> entries = tasks
            .SelectMany(task => task.ActiveAssignments
                .Where(a => string.Equals(a.AssigneeKey, key, StringComparison.OrdinalIgnoreCase))
                .Select(a => new WorkQueueEntryDto(
                    task.Id,
                    task.Identifier.Value,
                    task.Title,
                    task.Type,
                    task.Priority,
                    task.Status,
                    task.DueDate,
                    a.Id,
                    a.Role,
                    a.DueDate,
                    a.IsRework,
                    a.AssignedAt)))
            .OrderBy(e => e.AssignmentDueDate ?? DateOnly.MaxValue)
            .ToList();

        return new WorkQueueDto(key, entries.Count, entries.Count(e => e.IsRework), entries);
    }

    private async Task<WorkTask> GetTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{id}' was not found.");
    }

    private static TaskAssignmentDto ToAssignmentDto(TaskAssignment assignment)
    {
        return new TaskAssignmentDto(
            assignment.Id,
            assignment.AssigneeKey,
            assignment.Role,
            assignment.DueDate,
            assignment.AssignedByKey,
            assignment.AssignedAt,
            assignment.IsRework,
            assignment.IsSuperseded);
    }
}
