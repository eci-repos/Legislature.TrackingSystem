using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.Readiness;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.Readiness;

public sealed class SprintReadinessServiceTests
{
    [Fact]
    public void ApplicationRegistrationResolvesReadinessServiceByInterface()
    {
        ServiceCollection services = new();

        services.AddLtsApplication();
        services.AddLtsTestConnectors();

        using ServiceProvider provider = services.BuildServiceProvider();
        ISprintReadinessService readinessService = provider.GetRequiredService<ISprintReadinessService>();

        SprintReadinessSummary readiness = readinessService.GetReadiness();

        Assert.Equal("Sprint 1 - POC Foundation and Architecture Scaffold", readiness.Sprint);
        Assert.Contains(readiness.TraceabilityItems, item => item.StoryId == "TS-1.5" && item.RequirementId == "TR-107");
        Assert.Contains(readiness.TraceabilityItems, item => item.StoryId == "TS-6.1" && item.RequirementId == "TR-601");
    }
}
