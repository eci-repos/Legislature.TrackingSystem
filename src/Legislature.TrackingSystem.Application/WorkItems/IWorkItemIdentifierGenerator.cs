using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Produces a unique business identifier for a work item of a given type.
/// </summary>
public interface IWorkItemIdentifierGenerator
{
    WorkItemIdentifier Generate(WorkItemType type);
}
