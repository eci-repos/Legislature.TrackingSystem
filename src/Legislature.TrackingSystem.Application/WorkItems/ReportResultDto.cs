namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ReportRowDto(IReadOnlyDictionary<string, string> Values);

public sealed record ReportResultDto(string Name, IReadOnlyList<ReportRowDto> Rows);
