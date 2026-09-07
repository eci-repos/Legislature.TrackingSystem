using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record AssignImplementationTaskRequest(
    [property: Required] string Title,
    string? AssignedTo,
    string? Division,
    string? RequiredWork,
    DateOnly? DueDate,
    string? AssignedByKey);
