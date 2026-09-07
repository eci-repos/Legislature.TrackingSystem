namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Stores and retrieves supporting documentation for fiscal work (US-7.1.2, B.COM.37).
/// </summary>
public interface IFiscalWorkPaperService
{
    Task<FiscalWorkPaperDto> AddAsync(AddFiscalWorkPaperCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<FiscalWorkPaperDto>> ListForTaskAsync(Guid workTaskId, CancellationToken cancellationToken);
}
