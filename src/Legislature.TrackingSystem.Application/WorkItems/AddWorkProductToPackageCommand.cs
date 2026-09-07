namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record AddWorkProductToPackageCommand(Guid PackageId, Guid WorkItemId, string? AddedByKey);
