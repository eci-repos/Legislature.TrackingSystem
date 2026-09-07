namespace Legislature.TrackingSystem.Application.WorkItems;

public interface IWorkTaskService
{
    Task<WorkTaskDto> CreateAsync(CreateWorkTaskCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> OverrideIdentifierAsync(OverrideWorkItemIdentifierCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> SetCategorizationAsync(SetWorkTaskCategorizationCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
