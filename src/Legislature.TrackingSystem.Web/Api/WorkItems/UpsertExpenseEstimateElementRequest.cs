using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record UpsertExpenseEstimateElementRequest(
    [property: Required] string Name,
    ExpenseEstimateElementKind Kind,
    decimal Value,
    DateOnly EffectiveDate,
    [property: Required] string UpdatedByKey);
