using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ExpenseEstimateElementDto(Guid Id, string Name, ExpenseEstimateElementKind Kind, decimal Value, DateOnly EffectiveDate, string UpdatedByKey, DateTimeOffset UpdatedAt);
