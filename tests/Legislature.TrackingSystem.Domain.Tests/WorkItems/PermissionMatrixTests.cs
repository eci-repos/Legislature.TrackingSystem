using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class PermissionMatrixTests
{
    [Fact]
    public void SecurityAdministratorHoldsAdministrationPermissions()
    {
        IReadOnlyList<Permission> permissions = PermissionMatrix.GetPermissionsForRole(UserRole.SecurityAdministrator);

        Assert.Contains(Permission.Administer, permissions);
        Assert.Contains(Permission.ManageAccess, permissions);
        Assert.Contains(Permission.Migrate, permissions);
        Assert.Contains(Permission.ViewHistorical, permissions);
        Assert.Contains(Permission.ReadOnly, permissions);
    }

    [Fact]
    public void AnalystHoldsPrepareButNotAdministrationPermissions()
    {
        IReadOnlyList<Permission> permissions = PermissionMatrix.GetPermissionsForRole(UserRole.Analyst);

        Assert.Contains(Permission.Prepare, permissions);
        Assert.Contains(Permission.ReadOnly, permissions);
        Assert.DoesNotContain(Permission.Administer, permissions);
        Assert.DoesNotContain(Permission.ManageAccess, permissions);
        Assert.DoesNotContain(Permission.Migrate, permissions);
    }

    [Fact]
    public void ReadOnlyHoldsOnlyReadOnly()
    {
        IReadOnlyList<Permission> permissions = PermissionMatrix.GetPermissionsForRole(UserRole.ReadOnly);

        Assert.Equal(new[] { Permission.ReadOnly }, permissions);
    }

    [Fact]
    public void FinancialUserHoldsPrepareAndViewHistorical()
    {
        IReadOnlyList<Permission> permissions = PermissionMatrix.GetPermissionsForRole(UserRole.FinancialUser);

        Assert.Contains(Permission.Prepare, permissions);
        Assert.Contains(Permission.ViewHistorical, permissions);
        Assert.DoesNotContain(Permission.Administer, permissions);
    }
}
