using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record SubmitWorkItemForReviewRequest(
    [property: Required, MinLength(1)] IReadOnlyList<string> ReviewerKeys,
    string? SubmittedByKey);
