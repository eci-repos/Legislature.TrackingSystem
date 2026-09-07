using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record BeginExecutiveReviewStepRequest(
    [property: Required] string ReviewerKey);
