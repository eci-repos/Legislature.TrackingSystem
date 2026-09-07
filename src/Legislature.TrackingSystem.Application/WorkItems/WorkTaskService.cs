using Legislature.TrackingSystem.Domain.Traceability;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class WorkTaskService : IWorkTaskService
{
    private readonly IWorkTaskRepository _repository;
    private readonly IWorkItemIdentifierGenerator _identifierGenerator;

    public WorkTaskService(IWorkTaskRepository repository, IWorkItemIdentifierGenerator identifierGenerator)
    {
        _repository = repository;
        _identifierGenerator = identifierGenerator;
    }

    public async Task<WorkTaskDto> CreateAsync(CreateWorkTaskCommand command, CancellationToken cancellationToken)
    {
        WorkItemIdentifier identifier = _identifierGenerator.Generate(command.Type);

        SourceTraceReference sourceTrace = SourceTraceReference.Create(
            command.StoryId,
            command.RequirementId,
            command.RequirementType,
            command.SourceDocument);

        WorkTask task = WorkTask.Create(
            identifier,
            command.Type,
            command.Title,
            command.Description,
            command.DueDate,
            command.Priority,
            command.Status,
            command.Owner,
            sourceTrace,
            DateTimeOffset.UtcNow);

        await _repository.AddAsync(task, cancellationToken);

        return ToDto(task);
    }

    public async Task<WorkTaskDto> OverrideIdentifierAsync(OverrideWorkItemIdentifierCommand command, CancellationToken cancellationToken)
    {
        WorkTask? task = await _repository.FindByIdAsync(command.WorkTaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkTaskId}' was not found.");

        WorkItemIdentifier newIdentifier = WorkItemIdentifier.Create(command.NewIdentifier);

        bool duplicate = await _repository.ExistsWithIdentifierAsync(newIdentifier.Value, cancellationToken);
        if (duplicate)
        {
            throw new InvalidOperationException($"Identifier '{newIdentifier.Value}' is already in use.");
        }

        task.OverrideIdentifier(newIdentifier, DateTimeOffset.UtcNow);

        return ToDto(task);
    }

    public async Task<WorkTaskDto> SetCategorizationAsync(SetWorkTaskCategorizationCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await _repository.FindByIdAsync(command.WorkTaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkTaskId}' was not found.");

        task.SetCategorization(command.IsConfidential, command.IsExecutiveReview, DateTimeOffset.UtcNow);

        return ToDto(task);
    }

    public async Task<WorkTaskDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        WorkTask? task = await _repository.FindByIdAsync(id, cancellationToken);
        return task is null ? null : ToDto(task);
    }

    private static WorkTaskDto ToDto(WorkTask task)
    {
        return WorkTaskDtoMapper.ToDto(task);
    }
}
