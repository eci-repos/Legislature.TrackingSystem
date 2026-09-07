namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Maintains work tasks: update, cancel, duplicate, set customer due date, and add collaboration
/// comments (US-1.1.1, US-1.3.4, US-1.4.1, US-1.4.2).
/// </summary>
public interface ITaskMaintenanceService
{
    Task<WorkTaskDto> UpdateAsync(UpdateWorkTaskCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> CancelAsync(CancelWorkTaskCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> DuplicateAsync(DuplicateWorkTaskCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> SetCustomerDueDateAsync(SetCustomerDueDateCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> AddCommentAsync(AddWorkTaskCommentCommand command, CancellationToken cancellationToken);
}
