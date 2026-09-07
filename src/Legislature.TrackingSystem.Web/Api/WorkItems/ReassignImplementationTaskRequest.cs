namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record ReassignImplementationTaskRequest(string? AssignedTo, string? Division, DateOnly? DueDate);
