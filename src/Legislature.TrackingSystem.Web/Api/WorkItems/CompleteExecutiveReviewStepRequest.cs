using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record CompleteExecutiveReviewStepRequest(
    [property: Required] string ReviewerKey,
    string? Comment);
