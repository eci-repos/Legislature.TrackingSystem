using Legislature.TrackingSystem.Application.Connectors;

namespace Legislature.TrackingSystem.Domain.Tests.Connectors;

public sealed class ConnectorOptionsValidatorTests
{
    private static IReadOnlyList<string> Validate(ConnectorOptions options) =>
        new ConnectorOptionsValidator().Validate(options);

    [Fact]
    public void UnconfiguredOptionsProduceNoErrors()
    {
        Assert.Empty(Validate(new ConnectorOptions()));
    }

    [Fact]
    public void ConfiguredLegislativeSourceWithHttpsAndApiKeyProducesNoErrors()
    {
        var options = new ConnectorOptions
        {
            LegislativeSource = new LegislativeSourceConnectorOptions
            {
                BaseUrl = "https://legislature.example.gov/api/",
                ApiKey = "secret",
            },
        };

        Assert.Empty(Validate(options));
    }

    [Fact]
    public void NonHttpsLegislativeSourceProducesError()
    {
        var options = new ConnectorOptions
        {
            LegislativeSource = new LegislativeSourceConnectorOptions
            {
                BaseUrl = "http://legislature.example.gov/api/",
                ApiKey = "secret",
            },
        };

        Assert.Contains(Validate(options), e => e.Contains("LegislativeSource", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void LegislativeSourceWithoutApiKeyProducesError()
    {
        var options = new ConnectorOptions
        {
            LegislativeSource = new LegislativeSourceConnectorOptions
            {
                BaseUrl = "https://legislature.example.gov/api/",
            },
        };

        Assert.Contains(Validate(options), e => e.Contains("ApiKey", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void NonHttpsFiscalDataSourceProducesError()
    {
        var options = new ConnectorOptions
        {
            FiscalDataSource = new FiscalDataSourceConnectorOptions
            {
                BaseUrl = "http://fiscal.example.gov/api/",
                ApiKey = "secret",
            },
        };

        Assert.Contains(Validate(options), e => e.Contains("FiscalDataSource", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void PartiallyConfiguredM365ProducesError()
    {
        var options = new ConnectorOptions
        {
            M365 = new M365ConnectorOptions
            {
                TenantId = "tenant-id",
                ClientId = "client-id",
            },
        };

        IReadOnlyList<string> errors = Validate(options);

        Assert.Contains(errors, e => e.Contains("ClientSecret", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void FullyConfiguredM365ProducesNoErrors()
    {
        var options = new ConnectorOptions
        {
            M365 = new M365ConnectorOptions
            {
                TenantId = "tenant-id",
                ClientId = "client-id",
                ClientSecret = "secret",
            },
        };

        Assert.Empty(Validate(options));
    }
}
