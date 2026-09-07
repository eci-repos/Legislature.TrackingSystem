using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record UpdateWorkTaskRequest(string? Title, string? Description, DateOnly? DueDate, TaskPriority? Priority, string? ByKey);
