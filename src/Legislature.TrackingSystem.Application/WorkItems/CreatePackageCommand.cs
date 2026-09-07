namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record CreatePackageCommand(string Name, string? Description, string? CreatedByKey);
