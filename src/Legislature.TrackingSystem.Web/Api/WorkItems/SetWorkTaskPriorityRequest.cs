using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record SetWorkTaskPriorityRequest(TaskPriority Priority);
