namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// Transport boundary to an external legislative source (F5.1 - External Legislative Updates).
/// Implemented by an HTTP adapter when configured and a dev-boundary fake otherwise.
/// </summary>
public interface ILegislativeSourceConnector
{
    Task<LegislativeBillSnapshot?> FetchBillAsync(string billNumber, string biennium, CancellationToken cancellationToken);

    Task<IReadOnlyList<LegislativeAmendmentSnapshot>> FetchAmendmentsAsync(string billNumber, string biennium, CancellationToken cancellationToken);
}
