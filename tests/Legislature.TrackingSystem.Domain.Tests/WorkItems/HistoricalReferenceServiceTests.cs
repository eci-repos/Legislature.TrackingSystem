using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class HistoricalReferenceServiceTests
{
    private static (IHistoricalReferenceService Historical, IWorkTaskService Tasks) CreateServices()
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
            provider.GetRequiredService<IHistoricalReferenceService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task ListWorkProductsAsyncReturnsProductsWithinWindow()
    {
        (IHistoricalReferenceService historical, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto recent = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Recent note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-10.2.1", "B.EXP.01", "B", "Exhibit A"),
            CancellationToken.None);

        IReadOnlyList<HistoricalWorkProductDto> products = await historical.ListWorkProductsAsync(10, CancellationToken.None);

        Assert.Contains(products, p => p.Id == recent.Id);
        Assert.Equal(DateTime.UtcNow.Year, products[0].Year);
    }

    [Fact]
    public async Task ListWorkProductsAsyncRejectsInvalidWindow()
    {
        (IHistoricalReferenceService historical, _) = CreateServices();

        await Assert.ThrowsAsync<ArgumentException>(
            () => historical.ListWorkProductsAsync(0, CancellationToken.None));
    }
}
