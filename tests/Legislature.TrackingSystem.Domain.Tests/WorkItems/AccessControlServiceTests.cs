using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class AccessControlServiceTests
{
    private static (IAccessControlService Access, IWorkTaskService Tasks, IAuthorizationService Auth) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IAccessRestrictionRepository, FakeAccessRestrictionRepository>();
        services.AddSingleton<IUserRepository, FakeUserRepository>();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IAccessControlService>(),
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IAuthorizationService>());
    }

    [Fact]
    public async Task RestrictAsyncRestrictsByDataTypeAndUserType()
    {
        (IAccessControlService access, IWorkTaskService tasks, IAuthorizationService auth) = CreateServices();
        await auth.RegisterUserAsync(new RegisterUserCommand("reader", "Reader", UserRole.ReadOnly), CancellationToken.None);
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Fiscal note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-9.1.2", "B.COM.16", "B", "Exhibit A"),
            CancellationToken.None);

        await access.RestrictAsync(new RestrictAccessCommand(task.Id, "Attachment", UserRole.ReadOnly, null, "jdoe"), CancellationToken.None);

        Assert.False(await access.CanAccessAsync("reader", task.Id, "Attachment", CancellationToken.None));
        Assert.True(await access.CanAccessAsync("reader", task.Id, "Content", CancellationToken.None));
    }

    [Fact]
    public async Task UnauthorizedUserCannotAccessRestrictedInformation()
    {
        (IAccessControlService access, IWorkTaskService tasks, IAuthorizationService auth) = CreateServices();
        await auth.RegisterUserAsync(new RegisterUserCommand("reader", "Reader", UserRole.ReadOnly), CancellationToken.None);
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Fiscal note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-9.1.2", "B.COM.16", "B", "Exhibit A"),
            CancellationToken.None);
        await access.RestrictAsync(new RestrictAccessCommand(task.Id, "Comment", UserRole.ReadOnly, null, "jdoe"), CancellationToken.None);

        IReadOnlyList<AccessRestrictionDto> restrictions = await access.ListForTaskAsync(task.Id, CancellationToken.None);

        Assert.Single(restrictions);
        Assert.Equal("Comment", restrictions[0].DataType);
        Assert.Equal(UserRole.ReadOnly, restrictions[0].RestrictedUserType);
    }
}
