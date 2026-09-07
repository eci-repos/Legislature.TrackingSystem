using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for legislative bills. Implemented by an infrastructure adapter.
/// </summary>
public interface IBillRepository
{
    Task AddAsync(Bill bill, CancellationToken cancellationToken);

    Task<Bill?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Bill?> FindByNumberAsync(string billNumber, CancellationToken cancellationToken);

    Task<Bill?> FindByNumberAndBienniumAsync(string billNumber, string biennium, CancellationToken cancellationToken);

    Task<IReadOnlyList<Bill>> GetAllAsync(CancellationToken cancellationToken);
}
