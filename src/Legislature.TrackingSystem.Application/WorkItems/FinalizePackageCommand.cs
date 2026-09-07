namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Finalizes a package for submission after every member work product is approved
/// (US-3.1.3, B.COM.20).
/// </summary>
public sealed record FinalizePackageCommand(Guid PackageId);
