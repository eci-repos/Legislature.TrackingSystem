using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.Connectors;

/// <summary>
/// Dev-boundary fakes for the external connector layer, used by the domain test suite so the
/// application services resolve their connector dependencies without an external endpoint.
/// </summary>
public static class TestConnectorRegistrations
{
    public static IServiceCollection AddLtsTestConnectors(this IServiceCollection services)
    {
        services.AddSingleton<ILegislativeSourceConnector, FakeLegislativeSourceConnector>();
        services.AddSingleton<IFiscalDataSourceConnector, FakeFiscalDataSourceConnector>();
        services.AddSingleton<IM365Connector, FakeM365Connector>();
        return services;
    }
}

public sealed class FakeLegislativeSourceConnector : ILegislativeSourceConnector
{
    public Task<LegislativeBillSnapshot?> FetchBillAsync(string billNumber, string biennium, CancellationToken cancellationToken)
        => Task.FromResult<LegislativeBillSnapshot?>(null);

    public Task<IReadOnlyList<LegislativeAmendmentSnapshot>> FetchAmendmentsAsync(string billNumber, string biennium, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<LegislativeAmendmentSnapshot>>(Array.Empty<LegislativeAmendmentSnapshot>());
}

public sealed class FakeFiscalDataSourceConnector : IFiscalDataSourceConnector
{
    public Task<IReadOnlyList<FiscalDataPointSnapshot>> FetchFiscalDataAsync(FiscalDataCategory? category, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<FiscalDataPointSnapshot>>(Array.Empty<FiscalDataPointSnapshot>());
}

public sealed class FakeM365Connector : IM365Connector
{
    public Task SendEmailAsync(M365EmailMessage message, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task<string> StoreDocumentAsync(M365Document document, CancellationToken cancellationToken)
        => Task.FromResult(document.Name);
}
