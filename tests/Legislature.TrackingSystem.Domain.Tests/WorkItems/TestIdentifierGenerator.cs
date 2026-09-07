using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// Deterministic identifier generator that produces a unique identifier per call.
/// </summary>
internal sealed class TestIdentifierGenerator : IWorkItemIdentifierGenerator
{
    private int _sequence;

    public WorkItemIdentifier Generate(WorkItemType type)
    {
        _sequence++;
        return WorkItemIdentifier.Create($"LTS-{type}-DET{_sequence:000}");
    }
}
