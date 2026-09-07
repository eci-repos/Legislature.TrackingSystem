using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Infrastructure.Tests;

public sealed class ConnectorRegistrationTests
{
    [Fact]
    public void AddLtsConnectors_WithEmptyOptions_RegistersDevFakes()
    {
        ServiceCollection services = new();
        services.AddLtsConnectors(new ConnectorOptions());

        using ServiceProvider provider = services.BuildServiceProvider();
        Assert.Equal("FakeLegislativeSourceConnector", provider.GetRequiredService<ILegislativeSourceConnector>().GetType().Name);
        Assert.Equal("FakeFiscalDataSourceConnector", provider.GetRequiredService<IFiscalDataSourceConnector>().GetType().Name);
        Assert.Equal("FakeM365Connector", provider.GetRequiredService<IM365Connector>().GetType().Name);
    }

    [Fact]
    public void AddLtsConnectors_WithConfiguredOptions_RegistersHttpAdapters()
    {
        ServiceCollection services = new();
        var options = new ConnectorOptions
        {
            LegislativeSource = new LegislativeSourceConnectorOptions { BaseUrl = "https://leg.example.com" },
            FiscalDataSource = new FiscalDataSourceConnectorOptions { BaseUrl = "https://fiscal.example.com" },
            M365 = new M365ConnectorOptions { TenantId = "tenant", ClientId = "client", ClientSecret = "secret" },
        };
        services.AddLtsConnectors(options);

        using ServiceProvider provider = services.BuildServiceProvider();
        Assert.Equal("HttpLegislativeSourceConnector", provider.GetRequiredService<ILegislativeSourceConnector>().GetType().Name);
        Assert.Equal("HttpFiscalDataSourceConnector", provider.GetRequiredService<IFiscalDataSourceConnector>().GetType().Name);
        Assert.Equal("GraphM365Connector", provider.GetRequiredService<IM365Connector>().GetType().Name);
    }
}
