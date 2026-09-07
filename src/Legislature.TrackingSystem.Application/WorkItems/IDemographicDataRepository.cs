using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for demographic data by legislative session. Implemented by an
/// infrastructure adapter.
/// </summary>
public interface IDemographicDataRepository
{
    Task AddAsync(DemographicData data, CancellationToken cancellationToken);

    Task<DemographicData?> FindBySessionAndCategoryAsync(string session, string category, CancellationToken cancellationToken);

    Task<IReadOnlyList<DemographicData>> GetForSessionAsync(string session, CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> ListSessionsAsync(CancellationToken cancellationToken);
}
