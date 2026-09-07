using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class NotificationServiceTests
{
    private static INotificationService CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<INotificationService>();
    }

    [Fact]
    public async Task NotifyAssignmentAsyncCreatesNotification()
    {
        INotificationService notifications = CreateServices();

        await notifications.NotifyAssignmentAsync("asmith", "LTS-FiscalNote-001", CancellationToken.None);

        IReadOnlyList<NotificationDto> list = await notifications.ListForUserAsync("asmith", CancellationToken.None);
        Assert.Single(list);
        Assert.Equal(NotificationType.Assignment, list[0].Type);
        Assert.Contains("LTS-FiscalNote-001", list[0].Message);
    }

    [Fact]
    public async Task ListForUserAsyncReturnsOnlyThatUsersNotifications()
    {
        INotificationService notifications = CreateServices();
        await notifications.NotifyAssignmentAsync("asmith", "LTS-FiscalNote-001", CancellationToken.None);
        await notifications.NotifyAssignmentAsync("jdoe", "LTS-FiscalNote-002", CancellationToken.None);

        IReadOnlyList<NotificationDto> list = await notifications.ListForUserAsync("asmith", CancellationToken.None);

        Assert.Single(list);
        Assert.Equal("asmith", list[0].RecipientKey);
    }

    [Fact]
    public async Task MarkReadAsyncMarksNotificationRead()
    {
        INotificationService notifications = CreateServices();
        await notifications.NotifyAssignmentAsync("asmith", "LTS-FiscalNote-001", CancellationToken.None);
        IReadOnlyList<NotificationDto> list = await notifications.ListForUserAsync("asmith", CancellationToken.None);
        Guid id = list[0].Id;

        NotificationDto updated = await notifications.MarkReadAsync(id, CancellationToken.None);

        Assert.True(updated.IsRead);
    }

    [Fact]
    public async Task MarkReadAsyncThrowsForUnknownNotification()
    {
        INotificationService notifications = CreateServices();

        await Assert.ThrowsAsync<InvalidOperationException>(() => notifications.MarkReadAsync(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task NotifyAssignmentAsync_DefaultsToInAppChannelAndAssignmentTrigger()
    {
        INotificationService notifications = CreateServices();

        await notifications.NotifyAssignmentAsync("asmith", "LTS-FiscalNote-001", CancellationToken.None);

        IReadOnlyList<NotificationDto> list = await notifications.ListForUserAsync("asmith", CancellationToken.None);
        Assert.Equal(NotificationChannel.InApp, list[0].Channel);
        Assert.Equal(NotificationTrigger.Assignment, list[0].Trigger);
    }

    [Fact]
    public async Task NotifyAsync_WithExplicitChannelAndTrigger()
    {
        INotificationService notifications = CreateServices();

        await notifications.NotifyAsync("asmith", "Review requested", NotificationType.General, CancellationToken.None);

        IReadOnlyList<NotificationDto> list = await notifications.ListForUserAsync("asmith", CancellationToken.None);
        Assert.Equal(NotificationChannel.InApp, list[0].Channel);
        Assert.Equal(NotificationTrigger.General, list[0].Trigger);
    }
}
