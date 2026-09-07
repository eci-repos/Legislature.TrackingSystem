using Legislature.TrackingSystem.Application.Connectors;

namespace Legislature.TrackingSystem.Domain.Tests.Connectors;

public sealed class ConnectorOptionsTests
{
    [Fact]
    public void LegislativeSource_IsConfigured_FalseWhenBaseUrlEmpty()
    {
        var options = new LegislativeSourceConnectorOptions { BaseUrl = "" };
        Assert.False(options.IsConfigured);
    }

    [Fact]
    public void LegislativeSource_IsConfigured_TrueWhenBaseUrlPresent()
    {
        var options = new LegislativeSourceConnectorOptions { BaseUrl = "https://leg.example.com" };
        Assert.True(options.IsConfigured);
    }

    [Fact]
    public void FiscalDataSource_IsConfigured_FalseWhenBaseUrlEmpty()
    {
        var options = new FiscalDataSourceConnectorOptions { BaseUrl = null };
        Assert.False(options.IsConfigured);
    }

    [Fact]
    public void FiscalDataSource_IsConfigured_TrueWhenBaseUrlPresent()
    {
        var options = new FiscalDataSourceConnectorOptions { BaseUrl = "https://fiscal.example.com" };
        Assert.True(options.IsConfigured);
    }

    [Fact]
    public void M365_IsConfigured_FalseWhenTenantOrClientMissing()
    {
        var options = new M365ConnectorOptions { TenantId = "tenant", ClientId = "" };
        Assert.False(options.IsConfigured);
    }

    [Fact]
    public void M365_IsConfigured_TrueWhenTenantAndClientPresent()
    {
        var options = new M365ConnectorOptions { TenantId = "tenant", ClientId = "client", ClientSecret = "secret" };
        Assert.True(options.IsConfigured);
    }
}
