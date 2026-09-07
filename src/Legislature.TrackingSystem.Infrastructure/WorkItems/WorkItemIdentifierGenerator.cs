using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Generates a globally unique business identifier for a work item. The GUID-derived
/// suffix guarantees uniqueness across work types, years, and biennia for the POC.
/// </summary>
internal sealed class WorkItemIdentifierGenerator : IWorkItemIdentifierGenerator
{
    public WorkItemIdentifier Generate(WorkItemType type)
    {
        string suffix = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        return WorkItemIdentifier.Create($"LTS-{type}-{suffix}");
    }
}
