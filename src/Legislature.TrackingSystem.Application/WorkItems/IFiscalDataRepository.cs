using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for fiscal data points. Implemented by an infrastructure adapter.
/// </summary>
public interface IFiscalDataRepository
{
    Task AddAsync(FiscalData data, CancellationToken cancellationToken);

    Task<FiscalData?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<FiscalData?> FindByCategoryAndNameAsync(FiscalDataCategory category, string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<FiscalData>> GetAllAsync(FiscalDataCategory? category, CancellationToken cancellationToken);
}
