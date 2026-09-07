using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record AddWorkflowStepRequest(
    [property: Required] string Name,
    DateOnly? DueDate);
