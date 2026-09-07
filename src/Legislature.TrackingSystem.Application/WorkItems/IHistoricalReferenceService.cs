namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Surfaces work products across a historical period so authorized users can view and use prior
/// analysis to inform current work (US-10.2.1, B.EXP.01).
/// </summary>
public interface IHistoricalReferenceService
{
    Task<IReadOnlyList<HistoricalWorkProductDto>> ListWorkProductsAsync(int years, CancellationToken cancellationToken);
}
