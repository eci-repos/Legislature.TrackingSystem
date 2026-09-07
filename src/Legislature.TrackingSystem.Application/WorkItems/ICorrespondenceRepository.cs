using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for correspondence items (US-11.1.1, B.LNP.03). Implemented by an
/// infrastructure adapter.
/// </summary>
public interface ICorrespondenceRepository
{
    Task AddAsync(Correspondence correspondence, CancellationToken cancellationToken);

    Task<Correspondence?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Correspondence>> GetAllAsync(CancellationToken cancellationToken);
}
