namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Provides version history and comparison (F5.2 - Version History and Comparison).
/// </summary>
public interface IVersionService
{
    Task<IReadOnlyList<BillVersionDto>> ListBillVersionsAsync(Guid billId, CancellationToken cancellationToken);

    Task<IReadOnlyList<WorkTaskVersionDto>> ListWorkTaskVersionsAsync(Guid workTaskId, CancellationToken cancellationToken);

    Task<BillComparisonDto> CompareBillVersionsAsync(CompareBillVersionsCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<BillDto>> GetBillHistoryAsync(string billNumber, CancellationToken cancellationToken);
}
