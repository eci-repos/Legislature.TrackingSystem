namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Provides a consolidated executive bill view with the most important bill information,
/// including analysis and fiscal notes or estimates, on one screen (US-13.2.1, B.EXEC.03).
/// </summary>
public interface IExecutiveBillViewService
{
    Task<ExecutiveBillViewDto> GetBillViewAsync(Guid billId, CancellationToken cancellationToken);
}
