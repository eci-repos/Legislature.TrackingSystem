using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ContentServiceTests
{
    private static (IContentService Content, IWorkTaskService Tasks) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IContentService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task SetContentAsyncSavesContentAndLastSavedAt()
    {
        (IContentService content, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto updated = await content.SetContentAsync(
            new SetWorkItemContentCommand(task.Id, "<p>Fiscal note draft</p>"),
            CancellationToken.None);

        Assert.Equal("<p>Fiscal note draft</p>", updated.Content);
        Assert.NotNull(updated.LastSavedAt);
    }

    [Fact]
    public async Task SetContentAsyncAllowsSavingIncompleteWork()
    {
        (IContentService content, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        // Saving content must not require completion or approval (US-4.2.2).
        WorkTaskDto updated = await content.SetContentAsync(
            new SetWorkItemContentCommand(task.Id, "partial draft"),
            CancellationToken.None);

        Assert.Equal("partial draft", updated.Content);
        Assert.Equal(WorkTaskStatus.Proposed, updated.Status);
    }

    [Fact]
    public async Task AddAttachmentAsyncAddsAttachment()
    {
        (IContentService content, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto updated = await content.AddAttachmentAsync(
            new AddAttachmentCommand(task.Id, "fiscal-note.pdf", "application/pdf", 1024, "jdoe"),
            CancellationToken.None);

        Assert.Single(updated.Attachments);
        Assert.Equal("fiscal-note.pdf", updated.Attachments[0].FileName);
        Assert.Equal("application/pdf", updated.Attachments[0].ContentType);
    }

    [Fact]
    public async Task AddAttachmentAsyncRejectsEmptyFileName()
    {
        (IContentService content, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        await Assert.ThrowsAsync<ArgumentException>(() => content.AddAttachmentAsync(
            new AddAttachmentCommand(task.Id, "  ", "application/pdf", 1024, "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task RemoveAttachmentAsyncRemovesAttachment()
    {
        (IContentService content, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        WorkTaskDto withAttachment = await content.AddAttachmentAsync(
            new AddAttachmentCommand(task.Id, "fiscal-note.pdf", "application/pdf", 1024, "jdoe"),
            CancellationToken.None);
        Guid attachmentId = withAttachment.Attachments[0].Id;

        WorkTaskDto updated = await content.RemoveAttachmentAsync(
            new RemoveAttachmentCommand(task.Id, attachmentId),
            CancellationToken.None);

        Assert.Empty(updated.Attachments);
    }

    [Fact]
    public async Task RemoveAttachmentAsyncThrowsForUnknownAttachment()
    {
        (IContentService content, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        await Assert.ThrowsAsync<InvalidOperationException>(() => content.RemoveAttachmentAsync(
            new RemoveAttachmentCommand(task.Id, Guid.NewGuid()),
            CancellationToken.None));
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
                "US-4.1.1",
                "B.COM.17",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }
}
