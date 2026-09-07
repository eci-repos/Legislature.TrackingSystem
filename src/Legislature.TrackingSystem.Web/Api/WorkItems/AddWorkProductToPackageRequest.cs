namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record AddWorkProductToPackageRequest(Guid WorkItemId, string? AddedByKey);
