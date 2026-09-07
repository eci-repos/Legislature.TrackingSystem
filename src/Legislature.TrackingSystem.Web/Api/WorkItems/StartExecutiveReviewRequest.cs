using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record StartExecutiveReviewRequest(
    [property: Required, MinLength(1)] IReadOnlyList<string> ReviewerKeys,
    string? StartedByKey);
