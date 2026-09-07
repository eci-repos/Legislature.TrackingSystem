using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class WorkItemIdentifierTests
{
    [Fact]
    public void CreateTrimsAndRetainsValue()
    {
        WorkItemIdentifier identifier = WorkItemIdentifier.Create("  LTS-Task-ABC123  ");

        Assert.Equal("LTS-Task-ABC123", identifier.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateRejectsMissingValue(string value)
    {
        Assert.Throws<ArgumentException>(() => WorkItemIdentifier.Create(value));
    }

    [Fact]
    public void CreateRejectsValueLongerThan64Characters()
    {
        string tooLong = new('A', 65);

        Assert.Throws<ArgumentException>(() => WorkItemIdentifier.Create(tooLong));
    }
}
