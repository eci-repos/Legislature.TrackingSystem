namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record BillVersionDto(Guid Id, Guid BillId, string VersionLabel, string Language, DateTimeOffset CapturedAt, string? Source);
