using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Creates or updates an expense-estimate element (US-14.1.1, B.BGT.01).
/// </summary>
public sealed record UpsertExpenseEstimateElementCommand(string Name, ExpenseEstimateElementKind Kind, decimal Value, DateOnly EffectiveDate, string UpdatedByKey);
