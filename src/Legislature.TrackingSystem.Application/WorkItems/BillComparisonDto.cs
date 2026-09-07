namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record DifferenceDto(string Kind, string Text);

public sealed record BillComparisonDto(Guid BillId, string LeftLabel, string RightLabel, IReadOnlyList<DifferenceDto> Differences);
