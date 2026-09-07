using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ExecutiveDiscussionServiceTests
{
    private static (IExecutiveDiscussionService Discussion, ILegislativeIngestionService Ingestion, INotificationService Notifications) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IExecutiveDiscussionRepository, FakeExecutiveDiscussionRepository>();
        services.AddSingleton<IBillRepository, FakeBillRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IExecutiveDiscussionService>(),
            provider.GetRequiredService<ILegislativeIngestionService>(),
            provider.GetRequiredService<INotificationService>());
    }

    [Fact]
    public async Task PostQuestionAndAnswerAsyncTracksDiscussion()
    {
        (IExecutiveDiscussionService discussion, ILegislativeIngestionService ingestion, _) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 6001", "Executive bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        ExecutiveDiscussionDto question = await discussion.PostQuestionAsync(
            new PostExecutiveQuestionCommand(bill.Id, "jdoe", "What is the fiscal impact?", null),
            CancellationToken.None);
        ExecutiveDiscussionDto answered = await discussion.PostAnswerAsync(
            new PostExecutiveAnswerCommand(question.Id, "No fiscal impact.", "jane"),
            CancellationToken.None);

        Assert.Equal("What is the fiscal impact?", answered.Question);
        Assert.Equal("No fiscal impact.", answered.Answer);
    }

    [Fact]
    public async Task PostQuestionNotifiesParticipants()
    {
        (IExecutiveDiscussionService discussion, ILegislativeIngestionService ingestion, INotificationService notifications) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 6001", "Executive bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        await discussion.PostQuestionAsync(new PostExecutiveQuestionCommand(bill.Id, "jdoe", "Question?", null), CancellationToken.None);
        IReadOnlyList<NotificationDto> notificationsForParticipants = await notifications.ListForUserAsync("executive-participants", CancellationToken.None);

        Assert.Contains(notificationsForParticipants, n => n.Message.Contains("HB 6001"));
    }

    [Fact]
    public async Task ListForBillAsyncReturnsDiscussion()
    {
        (IExecutiveDiscussionService discussion, ILegislativeIngestionService ingestion, _) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 6001", "Executive bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);
        await discussion.PostQuestionAsync(new PostExecutiveQuestionCommand(bill.Id, "jdoe", "Question?", null), CancellationToken.None);

        IReadOnlyList<ExecutiveDiscussionDto> items = await discussion.ListForBillAsync(bill.Id, CancellationToken.None);

        Assert.Single(items);
    }
}
