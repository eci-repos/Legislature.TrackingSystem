namespace Legislature.TrackingSystem.Application.WorkItems;

public interface IWorkItemQueryService
{
    Task<WorkItemQueryResultDto> QueryAsync(WorkItemQuery query, CancellationToken cancellationToken);
}
