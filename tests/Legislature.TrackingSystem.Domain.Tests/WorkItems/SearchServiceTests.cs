using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class SearchServiceTests
{
    private static (ISearchService Search, IWorkTaskService Tasks, ILegislativeIngestionService Ingestion) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();
        services.AddSingleton<IBillRepository, FakeBillRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<ISearchService>(),
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<ILegislativeIngestionService>());
    }

    [Fact]
    public async Task SearchAsyncFindsWorkProductsByTitle()
    {
        (ISearchService search, IWorkTaskService tasks, _) = CreateServices();
        await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Prepare fiscal note for HB 1001", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-6.1.1", "B.COM.09", "B", "Exhibit A"),
            CancellationToken.None);

        SearchResultDto result = await search.SearchAsync(new SearchCommand("fiscal note"), CancellationToken.None);

        Assert.Equal(1, result.TotalCount);
        Assert.Contains(result.Hits, h => h.Type == "WorkProduct");
    }

    [Fact]
    public async Task SearchAsyncFindsBillsByNumber()
    {
        (ISearchService search, _, ILegislativeIngestionService ingestion) = CreateServices();
        await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        SearchResultDto result = await search.SearchAsync(new SearchCommand("HB 1001"), CancellationToken.None);

        Assert.Equal(1, result.TotalCount);
        Assert.Contains(result.Hits, h => h.Type == "Bill");
    }

    [Fact]
    public async Task SearchAsyncReturnsEmptyForNoMatch()
    {
        (ISearchService search, _, _) = CreateServices();

        SearchResultDto result = await search.SearchAsync(new SearchCommand("nonexistent"), CancellationToken.None);

        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Hits);
    }
}
