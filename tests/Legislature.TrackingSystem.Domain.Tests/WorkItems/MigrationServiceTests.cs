using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class MigrationServiceTests
{
    private static (IMigrationService Migration, IWorkTaskService Tasks) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<ILegacyMigrationRepository, FakeLegacyMigrationRepository>();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IMigrationService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task ImportAsyncMigratesLegacyRecordsIntoUsableWorkTasks()
    {
        (IMigrationService migration, IWorkTaskService tasks) = CreateServices();

        LegacyMigrationBatchDto batch = await migration.ImportAsync(
            new ImportLegacyDataCommand(
                "LegacySystem",
                "jdoe",
                new[]
                {
                    new LegacyImportRow("LEG-001", "Legacy fiscal note", WorkItemType.FiscalNote, 2018, "2017-2018", "US-10.1.1", "B.COM.41"),
                    new LegacyImportRow("LEG-002", "Legacy estimate", WorkItemType.FiscalEstimate, 2019, "2019-2020", "US-10.1.1", "B.COM.41"),
                }),
            CancellationToken.None);

        Assert.Equal(MigrationBatchStatus.Completed, batch.Status);
        Assert.Equal(2, batch.ImportedCount);
        Assert.Equal(0, batch.FailedCount);
        Assert.All(batch.Records, r => Assert.NotNull(r.TargetWorkTaskId));
    }

    [Fact]
    public async Task ImportAsyncRecordsFailuresAndMarksBatchFailed()
    {
        (IMigrationService migration, _) = CreateServices();

        LegacyMigrationBatchDto batch = await migration.ImportAsync(
            new ImportLegacyDataCommand(
                "LegacySystem",
                "jdoe",
                new[]
                {
                    new LegacyImportRow("LEG-001", "", WorkItemType.FiscalNote, 2018, "2017-2018", "US-10.1.1", "B.COM.41"),
                }),
            CancellationToken.None);

        Assert.Equal(MigrationBatchStatus.Failed, batch.Status);
        Assert.Equal(1, batch.FailedCount);
    }

    [Fact]
    public async Task RollbackAsyncRollsBackCompletedBatch()
    {
        (IMigrationService migration, _) = CreateServices();
        LegacyMigrationBatchDto batch = await migration.ImportAsync(
            new ImportLegacyDataCommand(
                "LegacySystem",
                "jdoe",
                new[] { new LegacyImportRow("LEG-001", "Legacy note", WorkItemType.FiscalNote, 2018, "2017-2018", "US-10.1.1", "B.COM.41") }),
            CancellationToken.None);

        LegacyMigrationBatchDto rolledBack = await migration.RollbackAsync(new RollbackMigrationCommand(batch.Id), CancellationToken.None);

        Assert.Equal(MigrationBatchStatus.RolledBack, rolledBack.Status);
    }
}
