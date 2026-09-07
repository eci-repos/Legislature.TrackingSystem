using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class TaskMaintenanceService : ITaskMaintenanceService
{
    private readonly IWorkTaskRepository _repository;
    private readonly IWorkItemIdentifierGenerator _identifierGenerator;

    public TaskMaintenanceService(IWorkTaskRepository repository, IWorkItemIdentifierGenerator identifierGenerator)
    {
        _repository = repository;
        _identifierGenerator = identifierGenerator;
    }

    public async Task<WorkTaskDto> UpdateAsync(UpdateWorkTaskCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.UpdateTask(command.Title, command.Description, command.DueDate, command.Priority, command.ByKey, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> CancelAsync(CancelWorkTaskCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.CancelTask(command.ByKey, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> DuplicateAsync(DuplicateWorkTaskCommand command, CancellationToken cancellationToken)
    {
        WorkTask source = await GetTaskAsync(command.WorkItemId, cancellationToken);
        WorkItemIdentifier newIdentifier = _identifierGenerator.Generate(source.Type);
        WorkTask duplicate = WorkTask.Create(
            newIdentifier,
            source.Type,
            command.Title ?? source.Title,
            source.Description,
            source.DueDate,
            source.Priority,
            WorkTaskStatus.Proposed,
            source.Owner,
            source.SourceTrace,
            DateTimeOffset.UtcNow);
        await _repository.AddAsync(duplicate, cancellationToken);
        return WorkTaskDtoMapper.ToDto(duplicate);
    }

    public async Task<WorkTaskDto> SetCustomerDueDateAsync(SetCustomerDueDateCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.SetCustomerDueDate(command.CustomerDueDate, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> AddCommentAsync(AddWorkTaskCommentCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.AddComment(command.AuthorKey, command.Body, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    private async Task<WorkTask> GetTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{id}' was not found.");
    }
}
