using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class TemplateServiceTests
{
    private static (ITemplateService Templates, IWorkTaskService Tasks) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<IDocumentTemplateRepository, FakeDocumentTemplateRepository>();
        services.AddSingleton<IGeneratedDocumentRepository, FakeGeneratedDocumentRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<ITemplateService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task CreateTemplateAsyncCreatesTemplate()
    {
        (ITemplateService templates, _) = CreateServices();

        DocumentTemplateDto created = await templates.CreateTemplateAsync(
            new CreateDocumentTemplateCommand("Fiscal Note", WorkItemType.FiscalNote, "Body", true),
            CancellationToken.None);

        Assert.Equal("Fiscal Note", created.Name);
        Assert.Equal(WorkItemType.FiscalNote, created.ApplicableWorkType);
        Assert.True(created.IsShared);
    }

    [Fact]
    public async Task CreateTemplateAsyncRejectsEmptyName()
    {
        (ITemplateService templates, _) = CreateServices();

        await Assert.ThrowsAsync<ArgumentException>(() => templates.CreateTemplateAsync(
            new CreateDocumentTemplateCommand("  ", null, "Body", false),
            CancellationToken.None));
    }

    [Fact]
    public async Task UpdateTemplateAsyncUpdatesBody()
    {
        (ITemplateService templates, _) = CreateServices();
        DocumentTemplateDto created = await templates.CreateTemplateAsync(
            new CreateDocumentTemplateCommand("Fiscal Note", null, "Original", false),
            CancellationToken.None);

        DocumentTemplateDto updated = await templates.UpdateTemplateAsync(
            new UpdateDocumentTemplateCommand(created.Id, "Revised"),
            CancellationToken.None);

        Assert.Equal("Revised", updated.Body);
    }

    [Fact]
    public async Task SetTemplateSharedAsyncTogglesShared()
    {
        (ITemplateService templates, _) = CreateServices();
        DocumentTemplateDto created = await templates.CreateTemplateAsync(
            new CreateDocumentTemplateCommand("Fiscal Note", null, "Body", false),
            CancellationToken.None);

        DocumentTemplateDto shared = await templates.SetTemplateSharedAsync(
            new SetDocumentTemplateSharedCommand(created.Id, true),
            CancellationToken.None);

        Assert.True(shared.IsShared);
    }

    [Fact]
    public async Task GenerateDocumentAsyncRendersMergeFields()
    {
        (ITemplateService templates, IWorkTaskService tasks) = CreateServices();
        DocumentTemplateDto template = await templates.CreateTemplateAsync(
            new CreateDocumentTemplateCommand("Fiscal Note", WorkItemType.FiscalNote, "{{Identifier}} - {{Title}} ({{Type}})", false),
            CancellationToken.None);
        WorkTaskDto task = await CreateTaskAsync(tasks);

        GeneratedDocumentDto generated = await templates.GenerateDocumentAsync(
            new GenerateDocumentCommand(task.Id, template.Id, "jdoe"),
            CancellationToken.None);

        Assert.Contains(task.Identifier, generated.Body);
        Assert.Contains(task.Title, generated.Body);
        Assert.Contains(WorkItemType.FiscalNote.ToString(), generated.Body);
        Assert.Equal("jdoe", generated.GeneratedByKey);
    }

    [Fact]
    public async Task GenerateDocumentAsyncThrowsForUnknownTemplate()
    {
        (ITemplateService templates, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        await Assert.ThrowsAsync<InvalidOperationException>(() => templates.GenerateDocumentAsync(
            new GenerateDocumentCommand(task.Id, Guid.NewGuid(), null),
            CancellationToken.None));
    }

    [Fact]
    public async Task GenerateDocumentAsyncPersistsAndListReturnsIt()
    {
        (ITemplateService templates, IWorkTaskService tasks) = CreateServices();
        DocumentTemplateDto template = await templates.CreateTemplateAsync(
            new CreateDocumentTemplateCommand("Fiscal Note", WorkItemType.FiscalNote, "{{Identifier}}", false),
            CancellationToken.None);
        WorkTaskDto task = await CreateTaskAsync(tasks);

        GeneratedDocumentDto generated = await templates.GenerateDocumentAsync(
            new GenerateDocumentCommand(task.Id, template.Id, "jdoe"),
            CancellationToken.None);

        IReadOnlyList<GeneratedDocumentDto> listed = await templates.ListGeneratedDocumentsAsync(task.Id, CancellationToken.None);

        Assert.Single(listed);
        Assert.Equal(generated.Id, listed[0].Id);
        Assert.Equal(task.Id, listed[0].WorkItemId);
        Assert.Equal(template.Id, listed[0].TemplateId);
        Assert.Equal("jdoe", listed[0].GeneratedByKey);
    }

    [Fact]
    public async Task ListGeneratedDocumentsAsyncReturnsEmptyForUnknownWorkItem()
    {
        (ITemplateService templates, _) = CreateServices();

        IReadOnlyList<GeneratedDocumentDto> listed = await templates.ListGeneratedDocumentsAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Empty(listed);
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
                "US-4.3.1",
                "B.COM.24",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }
}
