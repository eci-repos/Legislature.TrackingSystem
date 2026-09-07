using Legislature.TrackingSystem.Application.Authentication;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.Authentication;

public sealed class EntraAuthOptionsValidatorTests
{
    private static EntraAuthOptions ValidOptions() => new()
    {
        Instance = "https://login.microsoftonline.com/",
        TenantId = "tenant-id",
        ClientId = "client-id",
        RoleMappings = new Dictionary<string, UserRole>
        {
            ["LTS.Analyst"] = UserRole.Analyst,
        },
    };

    private static IReadOnlyList<string> Validate(EntraAuthOptions options) =>
        new EntraAuthOptionsValidator().Validate(options);

    [Fact]
    public void ValidOptionsProduceNoErrors()
    {
        Assert.Empty(Validate(ValidOptions()));
    }

    [Fact]
    public void MissingInstanceProducesError()
    {
        EntraAuthOptions options = ValidOptions();
        options.Instance = null;

        Assert.Contains(Validate(options), e => e.Contains("Instance", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void NonHttpsInstanceProducesError()
    {
        EntraAuthOptions options = ValidOptions();
        options.Instance = "http://login.microsoftonline.com/";

        Assert.Contains(Validate(options), e => e.Contains("HTTPS", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void MissingTenantIdProducesError()
    {
        EntraAuthOptions options = ValidOptions();
        options.TenantId = null;

        Assert.Contains(Validate(options), e => e.Contains("TenantId", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void MissingClientIdProducesError()
    {
        EntraAuthOptions options = ValidOptions();
        options.ClientId = null;

        Assert.Contains(Validate(options), e => e.Contains("ClientId", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void InvalidRoleMappingValueProducesError()
    {
        EntraAuthOptions options = ValidOptions();
        options.RoleMappings["LTS.Bad"] = (UserRole)999;

        Assert.Contains(Validate(options), e => e.Contains("RoleMappings", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void InvalidGroupMappingValueProducesError()
    {
        EntraAuthOptions options = ValidOptions();
        options.RoleMappings.Clear();
        options.GroupMappings["group-bad"] = (UserRole)999;

        Assert.Contains(Validate(options), e => e.Contains("GroupMappings", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void NoRoleOrGroupMappingsProducesError()
    {
        EntraAuthOptions options = ValidOptions();
        options.RoleMappings.Clear();
        options.GroupMappings.Clear();

        Assert.Contains(Validate(options), e => e.Contains("RoleMappings", StringComparison.OrdinalIgnoreCase));
    }
}
