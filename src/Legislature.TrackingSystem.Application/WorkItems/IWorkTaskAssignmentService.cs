namespace Legislature.TrackingSystem.Application.WorkItems;

public interface IWorkTaskAssignmentService
{
    Task<TaskAssignmentDto> AssignAsync(AssignWorkTaskCommand command, CancellationToken cancellationToken);

    Task<TaskAssignmentDto> ReassignAsync(ReassignWorkTaskCommand command, CancellationToken cancellationToken);

    Task<WorkQueueDto> GetWorkQueueAsync(string assigneeKey, CancellationToken cancellationToken);
}
