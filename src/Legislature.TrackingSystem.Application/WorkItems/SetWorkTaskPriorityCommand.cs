using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Sets the priority of a work product for workload management (US-3.2.4, B.RFA.05).
/// </summary>
public sealed record SetWorkTaskPriorityCommand(Guid WorkItemId, TaskPriority Priority);
