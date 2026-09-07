using Legislature.TrackingSystem.Application.Connectors;

namespace Legislature.TrackingSystem.Infrastructure.Connectors;

/// <summary>
/// Dev-boundary fake for the external legislative source. Returns no data so the app runs offline
/// without an external endpoint; the ingestion service falls back to command-supplied data.
/// </summary>
internal sealed class FakeLegislativeSourceConnector : ILegislativeSourceConnector
{
    public Task<LegislativeBillSnapshot?> FetchBillAsync(string billNumber, string biennium, CancellationToken cancellationToken)
        => Task.FromResult<LegislativeBillSnapshot?>(null);

    public Task<IReadOnlyList<LegislativeAmendmentSnapshot>> FetchAmendmentsAsync(string billNumber, string biennium, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<LegislativeAmendmentSnapshot>>(Array.Empty<LegislativeAmendmentSnapshot>());
}
