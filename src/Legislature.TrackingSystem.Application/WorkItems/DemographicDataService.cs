using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class DemographicDataService : IDemographicDataService
{
    private readonly IDemographicDataRepository _data;

    public DemographicDataService(IDemographicDataRepository data)
    {
        _data = data;
    }

    public async Task<DemographicDataDto> UpsertAsync(UpsertDemographicDataCommand command, CancellationToken cancellationToken)
    {
        DemographicData? existing = await _data.FindBySessionAndCategoryAsync(command.Session, command.Category, cancellationToken);
        if (existing is null)
        {
            DemographicData data = DemographicData.Create(command.Session, command.Category, command.Value, command.Year, DateTimeOffset.UtcNow);
            await _data.AddAsync(data, cancellationToken);
            return ToDto(data);
        }

        existing.Update(command.Value, DateTimeOffset.UtcNow);
        return ToDto(existing);
    }

    public async Task<IReadOnlyList<DemographicDataDto>> ListForSessionAsync(string session, CancellationToken cancellationToken)
    {
        IReadOnlyList<DemographicData> all = await _data.GetForSessionAsync(session, cancellationToken);
        return all.Select(ToDto).ToList();
    }

    public async Task<IReadOnlyList<string>> ListSessionsAsync(CancellationToken cancellationToken)
    {
        return await _data.ListSessionsAsync(cancellationToken);
    }

    private static DemographicDataDto ToDto(DemographicData data)
    {
        return new DemographicDataDto(data.Id, data.Session, data.Category, data.Value, data.Year, data.UpdatedAt);
    }
}
