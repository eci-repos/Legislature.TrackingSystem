using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ReuseService : IReuseService
{
    private readonly IWorkTaskRepository _repository;

    public ReuseService(IWorkTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkTaskDto> ReuseContentAsync(ReuseContentCommand command, CancellationToken cancellationToken)
    {
        WorkTask target = await _repository.FindByIdAsync(command.TargetWorkItemId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.TargetWorkItemId}' was not found.");
        WorkTask source = await _repository.FindByIdAsync(command.SourceWorkItemId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.SourceWorkItemId}' was not found.");

        target.ReuseContentFrom(source, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(target);
    }
}
