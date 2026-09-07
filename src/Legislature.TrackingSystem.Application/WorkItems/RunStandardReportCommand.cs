namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Runs a standard report (US-6.2.1, B.COM.38).
/// </summary>
public sealed record RunStandardReportCommand(string ReportName);
