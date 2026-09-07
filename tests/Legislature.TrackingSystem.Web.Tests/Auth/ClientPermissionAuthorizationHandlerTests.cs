using System.Security.Claims;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Web.Client.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Legislature.TrackingSystem.Web.Tests.Auth;

/// <summary>
/// Covers the WebAssembly client-side <see cref="PermissionAuthorizationHandler"/> so the client's
/// role-to-permission enforcement (US-9.1.1, B.COM.06) is asserted by automated tests, mirroring the
/// server-side handler coverage.
/// </summary>
public sealed class ClientPermissionAuthorizationHandlerTests
{
    private static ClaimsPrincipal UserWithRole(UserRole role) =>
        new(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, role.ToString()) }, "test"));

    private static async Task<bool> SatisfiesAsync(UserRole role, Permission permission)
    {
        var handler = new PermissionAuthorizationHandler();
        var requirement = new PermissionRequirement(permission);
        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            UserWithRole(role),
            resource: null);

        await handler.HandleAsync(context);

        return context.HasSucceeded;
    }

    [Fact]
    public async Task SecurityAdministratorSatisfiesAdministerRequirement()
    {
        Assert.True(await SatisfiesAsync(UserRole.SecurityAdministrator, Permission.Administer));
    }

    [Fact]
    public async Task SecurityAdministratorSatisfiesMigrateRequirement()
    {
        Assert.True(await SatisfiesAsync(UserRole.SecurityAdministrator, Permission.Migrate));
    }

    [Fact]
    public async Task AnalystSatisfiesPrepareRequirement()
    {
        Assert.True(await SatisfiesAsync(UserRole.Analyst, Permission.Prepare));
    }

    [Fact]
    public async Task AnalystDoesNotSatisfyAdministerRequirement()
    {
        Assert.False(await SatisfiesAsync(UserRole.Analyst, Permission.Administer));
    }

    [Fact]
    public async Task FinancialUserSatisfiesViewHistoricalRequirement()
    {
        Assert.True(await SatisfiesAsync(UserRole.FinancialUser, Permission.ViewHistorical));
    }

    [Fact]
    public async Task ReadOnlyUserDoesNotSatisfyPrepareRequirement()
    {
        Assert.False(await SatisfiesAsync(UserRole.ReadOnly, Permission.Prepare));
    }

    [Fact]
    public async Task ReviewerSatisfiesApproveRequirement()
    {
        Assert.True(await SatisfiesAsync(UserRole.Reviewer, Permission.Approve));
    }

    [Fact]
    public async Task ReadOnlyUserDoesNotSatisfyApproveRequirement()
    {
        Assert.False(await SatisfiesAsync(UserRole.ReadOnly, Permission.Approve));
    }

    [Fact]
    public async Task UserWithoutRoleClaimDoesNotSatisfyAnyRequirement()
    {
        var handler = new PermissionAuthorizationHandler();
        var requirement = new PermissionRequirement(Permission.ReadOnly);
        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            new ClaimsPrincipal(new ClaimsIdentity()),
            resource: null);

        await handler.HandleAsync(context);

        Assert.False(context.HasSucceeded);
    }
}
