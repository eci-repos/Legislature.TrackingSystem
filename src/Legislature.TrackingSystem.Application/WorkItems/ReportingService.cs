using System.Text.RegularExpressions;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ReportingService : IReportingService
{
    private readonly IWorkTaskRepository _tasks;
    private readonly ICustomReportRepository _reports;

    public ReportingService(IWorkTaskRepository tasks, ICustomReportRepository reports)
    {
        _tasks = tasks;
        _reports = reports;
    }

    public async Task<ReportResultDto> RunStandardReportAsync(RunStandardReportCommand command, CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkTask> tasks = await _tasks.GetAllAsync(cancellationToken);
        return command.ReportName switch
        {
            "OutstandingFiscalTasks" => new ReportResultDto(
                command.ReportName,
                tasks
                    .Where(t => t.Status != WorkTaskStatus.Completed && t.Status != WorkTaskStatus.Canceled)
                    .Select(t => new ReportRowDto(new Dictionary<string, string>
                    {
                        ["Identifier"] = t.Identifier.Value,
                        ["Title"] = t.Title,
                        ["Status"] = t.Status.ToString(),
                        ["DueDate"] = t.DueDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    }))
                    .ToList()),
            "WorkProductsByType" => new ReportResultDto(
                command.ReportName,
                tasks
                    .GroupBy(t => t.Type)
                    .Select(g => new ReportRowDto(new Dictionary<string, string>
                    {
                        ["Type"] = g.Key.ToString(),
                        ["Count"] = g.Count().ToString(),
                    }))
                    .ToList()),
            _ => throw new InvalidOperationException($"Unknown standard report '{command.ReportName}'."),
        };
    }

    public async Task<CustomReportDto> CreateCustomReportAsync(CreateCustomReportCommand command, CancellationToken cancellationToken)
    {
        CustomReport report = CustomReport.Create(command.Name, command.OwnerKey, command.Query, DateTimeOffset.UtcNow);
        await _reports.AddAsync(report, cancellationToken);
        return ToDto(report);
    }

    public async Task<IReadOnlyList<CustomReportDto>> ListCustomReportsAsync(string ownerKey, CancellationToken cancellationToken)
    {
        IReadOnlyList<CustomReport> reports = await _reports.GetForOwnerAsync(ownerKey, cancellationToken);
        return reports.Select(ToDto).ToList();
    }

    public async Task<ReportResultDto> RunCustomReportAsync(RunCustomReportCommand command, CancellationToken cancellationToken)
    {
        CustomReport report = await _reports.FindByIdAsync(command.ReportId, cancellationToken)
            ?? throw new InvalidOperationException($"Custom report '{command.ReportId}' was not found.");

        IReadOnlyList<WorkTask> tasks = await _tasks.GetAllAsync(cancellationToken);
        string query = report.Query?.Trim() ?? string.Empty;
        List<ReportRowDto> rows = tasks
            .Where(t => string.IsNullOrWhiteSpace(query)
                || t.Title.Contains(query, StringComparison.OrdinalIgnoreCase)
                || t.Identifier.Value.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(t => new ReportRowDto(new Dictionary<string, string>
            {
                ["Identifier"] = t.Identifier.Value,
                ["Title"] = t.Title,
                ["Status"] = t.Status.ToString(),
            }))
            .ToList();

        return new ReportResultDto(report.Name, rows);
    }

    public async Task<ExtractResultDto> ExtractWorkProductAsync(ExtractWorkProductCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await _tasks.FindByIdAsync(command.WorkItemId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkItemId}' was not found.");

        string format = string.IsNullOrWhiteSpace(command.Format) ? "text" : command.Format.Trim().ToLowerInvariant();
        string content = format switch
        {
            "html" => task.Content ?? string.Empty,
            _ => StripHtml(task.Content ?? string.Empty),
        };

        return new ExtractResultDto(task.Id, task.Identifier.Value, task.Title, content, format);
    }

    private static string StripHtml(string html)
    {
        return Regex.Replace(html, "<[^>]+>", string.Empty);
    }

    private static CustomReportDto ToDto(CustomReport report)
    {
        return new CustomReportDto(report.Id, report.Name, report.OwnerKey, report.Query, report.CreatedAt, report.UpdatedAt);
    }
}
