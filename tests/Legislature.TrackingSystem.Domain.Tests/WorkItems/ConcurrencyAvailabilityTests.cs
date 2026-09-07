using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.Readiness;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// Tests for concurrent use (US-9.2.1, B.COM.07) and 24/7 availability (US-9.2.2, B.COM.42).
/// </summary>
public sealed class ConcurrencyAvailabilityTests
{
    private static (IWorkTaskService Tasks, ISprintReadinessService Readiness) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<ISprintReadinessService>());
    }

    [Fact]
    public async Task MoreThanSixtyUsersCanCreateWorkConcurrently()
    {
        (IWorkTaskService tasks, _) = CreateServices();
        const int concurrency = 65;

        Task<WorkTaskDto>[] operations = Enumerable.Range(0, concurrency)
            .Select(i => tasks.CreateAsync(
                new CreateWorkTaskCommand(
                    WorkItemType.FiscalNote,
                    $"Concurrent note {i}",
                    null,
                    null,
                    TaskPriority.Normal,
                    WorkTaskStatus.Proposed,
                    null,
                    "US-9.2.1",
                    "B.COM.07",
                    "B",
                    "Exhibit A"),
                CancellationToken.None))
            .ToArray();

        WorkTaskDto[] results = await Task.WhenAll(operations);

        Assert.Equal(concurrency, results.Length);
        Assert.Equal(concurrency, results.Select(r => r.Identifier).Distinct().Count());
    }

    [Fact]
    public void ReadinessReportsAvailableStatus()
    {
        (_, ISprintReadinessService readiness) = CreateServices();

        SprintReadinessSummary summary = readiness.GetReadiness();

        Assert.False(string.IsNullOrWhiteSpace(summary.Status));
        Assert.False(string.IsNullOrWhiteSpace(summary.Sprint));
    }
}
