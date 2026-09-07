namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Stores and retrieves demographic data by legislative session (US-7.2.2, B.RFA.08).
/// </summary>
public interface IDemographicDataService
{
    Task<DemographicDataDto> UpsertAsync(UpsertDemographicDataCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<DemographicDataDto>> ListForSessionAsync(string session, CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> ListSessionsAsync(CancellationToken cancellationToken);
}
