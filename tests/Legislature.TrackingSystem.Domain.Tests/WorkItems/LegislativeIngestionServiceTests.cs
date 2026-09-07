using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class LegislativeIngestionServiceTests
{
    private static (ILegislativeIngestionService Ingestion, IBillRepository Bills) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IBillRepository, FakeBillRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<ILegislativeIngestionService>(),
            provider.GetRequiredService<IBillRepository>());
    }

    [Fact]
    public async Task IngestBillUpdateAsyncCreatesBill()
    {
        (ILegislativeIngestionService ingestion, IBillRepository bills) = CreateServices();

        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "Section 1 text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        Assert.Equal("HB 1001", bill.BillNumber);
        Assert.Equal(BillStatus.Introduced, bill.Status);
        Assert.Equal("Original", bill.CurrentVersion);
        Assert.NotNull(await bills.FindByNumberAsync("HB 1001", CancellationToken.None));
    }

    [Fact]
    public async Task IngestBillUpdateAsyncRetainsPriorVersion()
    {
        (ILegislativeIngestionService ingestion, _) = CreateServices();
        await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "Section 1 text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        BillDto updated = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill (amended)", "Substitute", "Section 1 amended text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        Assert.Equal("Substitute", updated.CurrentVersion);
        Assert.Equal(2, updated.Versions.Count);
        Assert.Equal("Original", updated.Versions[0].VersionLabel);
        Assert.Equal("Section 1 text", updated.Versions[0].Language);
        Assert.Equal("Substitute", updated.Versions[1].VersionLabel);
    }

    [Fact]
    public async Task IngestBillStatusAsyncUpdatesStatus()
    {
        (ILegislativeIngestionService ingestion, _) = CreateServices();
        await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        BillDto bill = await ingestion.IngestBillStatusAsync(new IngestBillStatusCommand("HB 1001", BillStatus.Enacted), CancellationToken.None);

        Assert.Equal(BillStatus.Enacted, bill.Status);
    }

    [Fact]
    public async Task IngestAmendmentAsyncTracksAmendment()
    {
        (ILegislativeIngestionService ingestion, _) = CreateServices();
        await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        BillDto bill = await ingestion.IngestAmendmentAsync(
            new IngestAmendmentCommand("HB 1001", "A1", "Amendment language", "WA Legislature"),
            CancellationToken.None);

        Assert.Single(bill.Amendments);
        Assert.Equal("A1", bill.Amendments[0].AmendmentNumber);
    }
}
