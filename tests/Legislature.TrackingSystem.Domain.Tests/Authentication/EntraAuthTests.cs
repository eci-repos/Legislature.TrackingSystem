using System.Security.Claims;
using Legislature.TrackingSystem.Application.Authentication;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.Authentication;

public sealed class EntraAuthOptionsTests
{
    [Fact]
    public void IsConfiguredIsFalseWhenRequiredSettingsMissing()
    {
        var options = new EntraAuthOptions();

        Assert.False(options.IsConfigured);
    }

    [Fact]
    public void IsConfiguredIsTrueWhenRequiredSettingsPresent()
    {
        var options = new EntraAuthOptions
        {
            Instance = "https://login.microsoftonline.com/",
            TenantId = "tenant-id",
            ClientId = "client-id",
        };

        Assert.True(options.IsConfigured);
    }

    [Fact]
    public void IssuerAndAuthorityUseTenantV2Endpoint()
    {
        var options = new EntraAuthOptions
        {
            Instance = "https://login.microsoftonline.com/",
            TenantId = "tenant-id",
            ClientId = "client-id",
        };

        Assert.Equal("https://login.microsoftonline.com/tenant-id/v2.0", options.Issuer);
        Assert.Equal("https://login.microsoftonline.com/tenant-id/v2.0", options.Authority);
    }
}

public sealed class EntraClaimsMapperTests
{
    private static EntraClaimsMapper CreateMapper()
    {
        return new EntraClaimsMapper(new EntraAuthOptions
        {
            RoleMappings = new Dictionary<string, UserRole>
            {
                ["LTS.Analyst"] = UserRole.Analyst,
                ["LTS.SecurityAdministrator"] = UserRole.SecurityAdministrator,
            },
            GroupMappings = new Dictionary<string, UserRole>
            {
                ["group-reviewer"] = UserRole.Reviewer,
            },
        });
    }

    [Fact]
    public void TryGetUserKeyPrefersOid()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("oid", "object-id"),
            new Claim("upn", "user@example.com"),
            new Claim("preferred_username", "user@example.com"),
        }));

        Assert.Equal("object-id", CreateMapper().TryGetUserKey(principal));
    }

    [Fact]
    public void TryGetUserKeyFallsBackToUpn()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("upn", "user@example.com"),
        }));

        Assert.Equal("user@example.com", CreateMapper().TryGetUserKey(principal));
    }

    [Fact]
    public void TryGetUserKeyReturnsNullWhenNoIdentityClaim()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity());

        Assert.Null(CreateMapper().TryGetUserKey(principal));
    }

    [Fact]
    public void MapRoleMapsRolesClaim()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("roles", "LTS.Analyst"),
        }));

        Assert.Equal(UserRole.Analyst, CreateMapper().MapRole(principal));
    }

    [Fact]
    public void MapRoleMapsGroupsClaim()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("groups", "group-reviewer"),
        }));

        Assert.Equal(UserRole.Reviewer, CreateMapper().MapRole(principal));
    }

    [Fact]
    public void MapRoleReturnsNullWhenNoMappingMatches()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("roles", "LTS.Unknown"),
        }));

        Assert.Null(CreateMapper().MapRole(principal));
    }

    [Fact]
    public void ApplyRoleClaimAddsRoleClaim()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("roles", "LTS.SecurityAdministrator"),
        }));

        ClaimsPrincipal mapped = CreateMapper().ApplyRoleClaim(principal);

        Assert.Equal(UserRole.SecurityAdministrator.ToString(), mapped.FindFirst(ClaimTypes.Role)?.Value);
    }

    [Fact]
    public void ApplyRoleClaimLeavesExistingRoleClaimUnchanged()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, UserRole.ReadOnly.ToString()),
            new Claim("roles", "LTS.Analyst"),
        }));

        ClaimsPrincipal mapped = CreateMapper().ApplyRoleClaim(principal);

        Assert.Equal(UserRole.ReadOnly.ToString(), mapped.FindFirst(ClaimTypes.Role)?.Value);
    }

    [Fact]
    public void ApplyRoleClaimReturnsUnchangedWhenNoMappingMatches()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("roles", "LTS.Unknown"),
        }));

        ClaimsPrincipal mapped = CreateMapper().ApplyRoleClaim(principal);

        Assert.Null(mapped.FindFirst(ClaimTypes.Role)?.Value);
    }
}
