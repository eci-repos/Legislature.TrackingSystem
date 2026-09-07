using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Retrieves, calculates, and updates fiscal data from internal DOR systems (F7.1 - Fiscal Data
/// Integration).
/// </summary>
public interface IFiscalDataService
{
    Task<IReadOnlyList<FiscalDataDto>> ListAsync(FiscalDataCategory? category, CancellationToken cancellationToken);

    Task<FiscalDataDto> UpsertAsync(UpsertFiscalDataCommand command, CancellationToken cancellationToken);

    Task<FiscalCalculationDto> CalculateFiscalNoteAsync(CalculateFiscalNoteCommand command, CancellationToken cancellationToken);
}
