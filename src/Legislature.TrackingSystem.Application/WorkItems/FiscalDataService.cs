using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class FiscalDataService : IFiscalDataService
{
    private readonly IFiscalDataRepository _data;
    private readonly IWorkTaskRepository _tasks;
    private readonly IFiscalDataSourceConnector _connector;

    public FiscalDataService(IFiscalDataRepository data, IWorkTaskRepository tasks, IFiscalDataSourceConnector connector)
    {
        _data = data;
        _tasks = tasks;
        _connector = connector;
    }

    public async Task<IReadOnlyList<FiscalDataDto>> ListAsync(FiscalDataCategory? category, CancellationToken cancellationToken)
    {
        // Refresh from the external fiscal data source when it returns data; the dev-boundary fake
        // returns none, so the repository remains the query source offline.
        IReadOnlyList<FiscalDataPointSnapshot> snapshots = await _connector.FetchFiscalDataAsync(category, cancellationToken);
        foreach (FiscalDataPointSnapshot s in snapshots)
        {
            FiscalData? existing = await _data.FindByCategoryAndNameAsync(s.Category, s.Name, cancellationToken);
            if (existing is null)
            {
                await _data.AddAsync(FiscalData.Create(s.Category, s.Name, s.Value, s.Unit, s.Source, DateTimeOffset.UtcNow), cancellationToken);
            }
            else
            {
                existing.Update(s.Value, s.Source, DateTimeOffset.UtcNow);
            }
        }

        IReadOnlyList<FiscalData> all = await _data.GetAllAsync(category, cancellationToken);
        return all.Select(ToDto).ToList();
    }

    public async Task<FiscalDataDto> UpsertAsync(UpsertFiscalDataCommand command, CancellationToken cancellationToken)
    {
        FiscalData? existing = await _data.FindByCategoryAndNameAsync(command.Category, command.Name, cancellationToken);
        if (existing is null)
        {
            FiscalData data = FiscalData.Create(command.Category, command.Name, command.Value, command.Unit, command.Source, DateTimeOffset.UtcNow);
            await _data.AddAsync(data, cancellationToken);
            return ToDto(data);
        }

        existing.Update(command.Value, command.Source, DateTimeOffset.UtcNow);
        return ToDto(existing);
    }

    public async Task<FiscalCalculationDto> CalculateFiscalNoteAsync(CalculateFiscalNoteCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await _tasks.FindByIdAsync(command.WorkItemId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkItemId}' was not found.");

        IReadOnlyList<FiscalData> fte = await _data.GetAllAsync(FiscalDataCategory.Fte, cancellationToken);
        IReadOnlyList<FiscalData> costRules = await _data.GetAllAsync(FiscalDataCategory.CostRule, cancellationToken);

        decimal amount = 0m;
        foreach (FiscalData f in fte)
        {
            decimal rate = costRules.FirstOrDefault()?.Value ?? 1m;
            amount += f.Value * rate;
        }

        List<FiscalDataDto> applied = fte.Concat(costRules).Select(ToDto).ToList();
        return new FiscalCalculationDto(amount, "dollars", applied);
    }

    private static FiscalDataDto ToDto(FiscalData data)
    {
        return new FiscalDataDto(data.Id, data.Category, data.Name, data.Value, data.Unit, data.Source, data.UpdatedAt);
    }
}
