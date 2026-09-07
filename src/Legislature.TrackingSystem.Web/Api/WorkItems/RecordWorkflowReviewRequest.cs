using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record RecordWorkflowReviewRequest(
    [property: Required] string ReviewerKey,
    WorkflowDecision Decision,
    string? Comment);
