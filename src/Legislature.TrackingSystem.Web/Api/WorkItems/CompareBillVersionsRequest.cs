namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record CompareBillVersionsRequest(Guid LeftVersionId, Guid RightVersionId);
