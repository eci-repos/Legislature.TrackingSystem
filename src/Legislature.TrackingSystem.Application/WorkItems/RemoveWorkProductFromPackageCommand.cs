namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record RemoveWorkProductFromPackageCommand(Guid PackageId, Guid WorkItemId);
