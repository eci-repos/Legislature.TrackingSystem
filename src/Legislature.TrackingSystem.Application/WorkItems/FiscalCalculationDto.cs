namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record FiscalCalculationDto(decimal Amount, string Unit, IReadOnlyList<FiscalDataDto> AppliedData);
