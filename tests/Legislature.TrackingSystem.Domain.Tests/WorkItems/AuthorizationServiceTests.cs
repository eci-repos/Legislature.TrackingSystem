using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class AuthorizationServiceTests
{
    private static IAuthorizationService CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IUserRepository, FakeUserRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IAuthorizationService>();
    }

    [Fact]
    public async Task RegisterUserAsyncAssignsRoleAndPermissions()
    {
        IAuthorizationService authorization = CreateServices();

        UserAccountDto user = await authorization.RegisterUserAsync(
            new RegisterUserCommand("jdoe", "Jane Doe", UserRole.Analyst),
            CancellationToken.None);

        Assert.Equal(UserRole.Analyst, user.Role);
        Assert.Contains(Permission.Prepare, user.Permissions);
        Assert.DoesNotContain(Permission.Approve, user.Permissions);
    }

    [Fact]
    public async Task CanAsyncDistinguishesPreparationApprovalAndDelivery()
    {
        IAuthorizationService authorization = CreateServices();
        await authorization.RegisterUserAsync(new RegisterUserCommand("analyst", "Analyst", UserRole.Analyst), CancellationToken.None);
        await authorization.RegisterUserAsync(new RegisterUserCommand("reviewer", "Reviewer", UserRole.Reviewer), CancellationToken.None);

        Assert.True(await authorization.CanAsync("analyst", Permission.Prepare, CancellationToken.None));
        Assert.False(await authorization.CanAsync("analyst", Permission.Approve, CancellationToken.None));
        Assert.True(await authorization.CanAsync("reviewer", Permission.Approve, CancellationToken.None));
        Assert.False(await authorization.CanAsync("reviewer", Permission.Prepare, CancellationToken.None));
    }

    [Fact]
    public async Task RequireAsyncThrowsForRestrictedFunction()
    {
        IAuthorizationService authorization = CreateServices();
        await authorization.RegisterUserAsync(new RegisterUserCommand("analyst", "Analyst", UserRole.Analyst), CancellationToken.None);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => authorization.RequireAsync("analyst", Permission.Deliver, CancellationToken.None));
    }

    [Fact]
    public async Task ReadOnlyRoleCannotPerformRestrictedFunctions()
    {
        IAuthorizationService authorization = CreateServices();
        await authorization.RegisterUserAsync(new RegisterUserCommand("reader", "Reader", UserRole.ReadOnly), CancellationToken.None);

        Assert.True(await authorization.CanAsync("reader", Permission.ReadOnly, CancellationToken.None));
        Assert.False(await authorization.CanAsync("reader", Permission.Prepare, CancellationToken.None));
        Assert.False(await authorization.CanAsync("reader", Permission.Approve, CancellationToken.None));
    }
}
