using Legislature.TrackingSystem.Application.Connectors;

namespace Legislature.TrackingSystem.Infrastructure.Connectors;

/// <summary>
/// Dev-boundary fake for Microsoft 365 productivity integration. Records the dispatch locally so
/// the app runs offline without a tenant; the productivity service persists the dispatch.
/// </summary>
internal sealed class FakeM365Connector : IM365Connector
{
    public Task SendEmailAsync(M365EmailMessage message, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task<string> StoreDocumentAsync(M365Document document, CancellationToken cancellationToken)
        => Task.FromResult(document.Name);
}
