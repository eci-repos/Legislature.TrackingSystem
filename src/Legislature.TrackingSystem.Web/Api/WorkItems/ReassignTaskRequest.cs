using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record ReassignTaskRequest(
    [property: Required] string PriorAssigneeKey,
    [property: Required] string NewAssigneeKey,
    AssignmentRole Role,
    DateOnly? DueDate,
    string? AssignedByKey);
