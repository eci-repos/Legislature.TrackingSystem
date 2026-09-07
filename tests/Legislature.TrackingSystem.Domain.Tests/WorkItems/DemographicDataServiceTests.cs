using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class DemographicDataServiceTests
{
    private static IDemographicDataService CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IDemographicDataRepository, FakeDemographicDataRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IDemographicDataService>();
    }

    [Fact]
    public async Task UpsertAsyncStoresAndUpdatesBySessionAndCategory()
    {
        IDemographicDataService service = CreateServices();

        DemographicDataDto created = await service.UpsertAsync(
            new UpsertDemographicDataCommand("2025-2026", "Population", 1000m, 2025),
            CancellationToken.None);
        DemographicDataDto updated = await service.UpsertAsync(
            new UpsertDemographicDataCommand("2025-2026", "Population", 1100m, 2025),
            CancellationToken.None);

        Assert.Equal(created.Id, updated.Id);
        Assert.Equal(1100m, updated.Value);
    }

    [Fact]
    public async Task ListForSessionAsyncReturnsSessionData()
    {
        IDemographicDataService service = CreateServices();
        await service.UpsertAsync(new UpsertDemographicDataCommand("2025-2026", "Population", 1000m, 2025), CancellationToken.None);
        await service.UpsertAsync(new UpsertDemographicDataCommand("2025-2026", "Enrollment", 500m, 2025), CancellationToken.None);

        IReadOnlyList<DemographicDataDto> data = await service.ListForSessionAsync("2025-2026", CancellationToken.None);

        Assert.Equal(2, data.Count);
    }

    [Fact]
    public async Task ListSessionsAsyncReturnsDistinctSessions()
    {
        IDemographicDataService service = CreateServices();
        await service.UpsertAsync(new UpsertDemographicDataCommand("2025-2026", "Population", 1000m, 2025), CancellationToken.None);
        await service.UpsertAsync(new UpsertDemographicDataCommand("2027-2028", "Population", 1100m, 2027), CancellationToken.None);

        IReadOnlyList<string> sessions = await service.ListSessionsAsync(CancellationToken.None);

        Assert.Equal(2, sessions.Count);
    }
}
