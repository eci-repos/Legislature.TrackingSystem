using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ProductivityIntegrationServiceTests
{
    private static (IProductivityIntegrationService Productivity, IWorkTaskService Tasks, ITemplateService Templates) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IEmailDispatchRepository, FakeEmailDispatchRepository>();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();
        services.AddSingleton<IDocumentTemplateRepository, FakeDocumentTemplateRepository>();
        services.AddSingleton<IGeneratedDocumentRepository, FakeGeneratedDocumentRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IProductivityIntegrationService>(),
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<ITemplateService>());
    }

    [Fact]
    public async Task EmailOutputAsyncRecordsDispatch()
    {
        (IProductivityIntegrationService productivity, _, _) = CreateServices();

        EmailDispatchDto dispatch = await productivity.EmailOutputAsync(
            new EmailOutputCommand("stakeholder@example.com", "Fiscal note", "body", "jdoe"),
            CancellationToken.None);

        Assert.Equal("stakeholder@example.com", dispatch.Recipient);
        Assert.Equal("jdoe", dispatch.SentByKey);
    }

    [Fact]
    public async Task ListEmailDispatchesAsyncReturnsRecordedDispatches()
    {
        (IProductivityIntegrationService productivity, _, _) = CreateServices();
        await productivity.EmailOutputAsync(new EmailOutputCommand("a@example.com", "Subject", "body", "jdoe"), CancellationToken.None);

        IReadOnlyList<EmailDispatchDto> dispatches = await productivity.ListEmailDispatchesAsync(CancellationToken.None);

        Assert.Single(dispatches);
    }

    [Fact]
    public async Task PopulateTemplateAsyncRendersSystemData()
    {
        (IProductivityIntegrationService productivity, IWorkTaskService tasks, ITemplateService templates) = CreateServices();
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Fiscal note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-8.1.1", "B.COM.30", "B", "Exhibit A"),
            CancellationToken.None);
        DocumentTemplateDto template = await templates.CreateTemplateAsync(
            new CreateDocumentTemplateCommand("Fiscal note template", WorkItemType.FiscalNote, "Note for {{Title}}", true),
            CancellationToken.None);

        string body = await productivity.PopulateTemplateAsync(new PopulateTemplateCommand(task.Id, template.Id), CancellationToken.None);

        Assert.Contains("Fiscal note", body);
    }
}
