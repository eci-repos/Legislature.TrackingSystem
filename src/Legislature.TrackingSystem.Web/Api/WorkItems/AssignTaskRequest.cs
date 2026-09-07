using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record AssignTaskRequest(
    [property: Required] string AssigneeKey,
    AssignmentRole Role,
    DateOnly? DueDate,
    string? AssignedByKey);
