namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record WorkTaskVersionDto(Guid Id, Guid WorkTaskId, int VersionNumber, string? Content, DateTimeOffset CapturedAt, string? CapturedByKey);
