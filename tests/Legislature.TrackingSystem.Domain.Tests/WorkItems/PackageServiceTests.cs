using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class PackageServiceTests
{
    private static (IWorkTaskService Tasks, IPackageService Packages) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IPackageService>());
    }

    [Fact]
    public async Task CreateAsyncCreatesDraftPackage()
    {
        (_, IPackageService packages) = CreateServices();

        PackageDto dto = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", "Fiscal estimates for HB 1200", "jdoe"),
            CancellationToken.None);

        Assert.Equal("FY2026 Fiscal Package", dto.Name);
        Assert.Equal(PackageStatus.Draft, dto.Status);
        Assert.Empty(dto.Members);
    }

    [Fact]
    public async Task AddWorkProductAsyncAddsMember()
    {
        (IWorkTaskService tasks, IPackageService packages) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        PackageDto package = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", null, "jdoe"),
            CancellationToken.None);

        PackageDto updated = await packages.AddWorkProductAsync(
            new AddWorkProductToPackageCommand(package.Id, task.Id, "jdoe"),
            CancellationToken.None);

        Assert.Single(updated.Members);
        Assert.Equal(task.Id, updated.Members[0].WorkItemId);
    }

    [Fact]
    public async Task AddWorkProductAsyncThrowsForUnknownWorkItem()
    {
        (_, IPackageService packages) = CreateServices();
        PackageDto package = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", null, "jdoe"),
            CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() => packages.AddWorkProductAsync(
            new AddWorkProductToPackageCommand(package.Id, Guid.NewGuid(), "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task AddWorkProductAsyncThrowsForDuplicate()
    {
        (IWorkTaskService tasks, IPackageService packages) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        PackageDto package = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", null, "jdoe"),
            CancellationToken.None);
        await packages.AddWorkProductAsync(
            new AddWorkProductToPackageCommand(package.Id, task.Id, "jdoe"),
            CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() => packages.AddWorkProductAsync(
            new AddWorkProductToPackageCommand(package.Id, task.Id, "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task DeliverAsyncMarksPackageDelivered()
    {
        (IWorkTaskService tasks, IPackageService packages) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        PackageDto package = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", null, "jdoe"),
            CancellationToken.None);
        await packages.AddWorkProductAsync(
            new AddWorkProductToPackageCommand(package.Id, task.Id, "jdoe"),
            CancellationToken.None);

        PackageDto delivered = await packages.DeliverAsync(new DeliverPackageCommand(package.Id), CancellationToken.None);

        Assert.Equal(PackageStatus.Delivered, delivered.Status);
        Assert.Single(delivered.Members);
    }

    [Fact]
    public async Task RemoveWorkProductAsyncRemovesMember()
    {
        (IWorkTaskService tasks, IPackageService packages) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        PackageDto package = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", null, "jdoe"),
            CancellationToken.None);
        await packages.AddWorkProductAsync(
            new AddWorkProductToPackageCommand(package.Id, task.Id, "jdoe"),
            CancellationToken.None);

        PackageDto updated = await packages.RemoveWorkProductAsync(
            new RemoveWorkProductFromPackageCommand(package.Id, task.Id),
            CancellationToken.None);

        Assert.Empty(updated.Members);
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(IWorkTaskService tasks)
    {
        return await tasks.CreateAsync(
            new CreateWorkTaskCommand(
                WorkItemType.FiscalNote,
                "Prepare fiscal note",
                null,
                null,
                TaskPriority.Normal,
                WorkTaskStatus.Proposed,
                null,
                "US-2.2.2",
                "B.RFA.06",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }
}
