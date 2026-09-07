using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Add an internal or external recipient to a package (US-3.1.3, B.COM.20).
/// </summary>
public sealed record AddPackageRecipientCommand(
    Guid PackageId,
    string Name,
    PackageRecipientKind Kind,
    string? AddedByKey);
