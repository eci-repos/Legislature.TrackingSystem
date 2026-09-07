namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record BillAmendmentDto(Guid Id, Guid BillId, string AmendmentNumber, string Language, DateTimeOffset CapturedAt, string? Source);
