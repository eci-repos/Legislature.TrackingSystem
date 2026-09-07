using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record AdjustExecutiveReviewRequest(
    [property: Required] string ReviewerKey,
    [property: Required] string Note);
