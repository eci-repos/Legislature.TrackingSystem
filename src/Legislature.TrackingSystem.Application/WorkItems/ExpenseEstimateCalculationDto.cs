namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ExpenseEstimateCalculationDto(decimal Amount, IReadOnlyList<ExpenseEstimateElementDto> AppliedElements);
