namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// Transport boundary to Microsoft 365 productivity integration (F8.1 - Productivity Suite
/// Integration). Implemented by a Microsoft Graph adapter when configured and a dev-boundary fake
/// otherwise.
/// </summary>
public interface IM365Connector
{
    Task SendEmailAsync(M365EmailMessage message, CancellationToken cancellationToken);

    Task<string> StoreDocumentAsync(M365Document document, CancellationToken cancellationToken);
}
