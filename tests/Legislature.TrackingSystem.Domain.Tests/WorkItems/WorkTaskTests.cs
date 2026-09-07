using Legislature.TrackingSystem.Domain.Traceability;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class WorkTaskTests
{
    private static readonly SourceTraceReference Trace = SourceTraceReference.Create(
        "US-1.3.1",
        "B.COM.11",
        "B",
        "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System");

    [Fact]
    public void CreateAssignsIdentifierAndAttributes()
    {
        WorkItemIdentifier identifier = WorkItemIdentifier.Create("LTS-Task-ABC123");
        var createdAt = new DateTimeOffset(2026, 9, 4, 12, 0, 0, TimeSpan.Zero);

        WorkTask task = WorkTask.Create(
            identifier,
            WorkItemType.Task,
            "Prepare fiscal note",
            "Analyze the fiscal impact of the bill.",
            new DateOnly(2026, 9, 20),
            TaskPriority.High,
            WorkTaskStatus.Assigned,
            "jdoe",
            Trace,
            createdAt);

        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal("LTS-Task-ABC123", task.Identifier.Value);
        Assert.Equal(WorkItemType.Task, task.Type);
        Assert.Equal("Prepare fiscal note", task.Title);
        Assert.Equal(TaskPriority.High, task.Priority);
        Assert.Equal(WorkTaskStatus.Assigned, task.Status);
        Assert.Equal("jdoe", task.Owner);
        Assert.Equal(createdAt, task.CreatedAt);
        Assert.Equal(createdAt, task.UpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateRejectsMissingTitle(string title)
    {
        Assert.Throws<ArgumentException>(() => WorkTask.Create(
            WorkItemIdentifier.Create("LTS-Task-ABC123"),
            WorkItemType.Task,
            title,
            null,
            null,
            TaskPriority.Normal,
            WorkTaskStatus.Proposed,
            null,
            Trace,
            DateTimeOffset.UtcNow));
    }

    [Fact]
    public void OverrideIdentifierUpdatesIdentifierAndUpdatedAt()
    {
        WorkTask task = WorkTask.Create(
            WorkItemIdentifier.Create("LTS-Task-ABC123"),
            WorkItemType.Task,
            "Prepare fiscal note",
            null,
            null,
            TaskPriority.Normal,
            WorkTaskStatus.Proposed,
            null,
            Trace,
            DateTimeOffset.UtcNow);

        var updatedAt = new DateTimeOffset(2026, 9, 5, 8, 0, 0, TimeSpan.Zero);
        task.OverrideIdentifier(WorkItemIdentifier.Create("LTS-FiscalNote-OVR999"), updatedAt);

        Assert.Equal("LTS-FiscalNote-OVR999", task.Identifier.Value);
        Assert.Equal(updatedAt, task.UpdatedAt);
    }
}
