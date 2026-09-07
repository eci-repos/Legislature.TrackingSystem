using System.Text.RegularExpressions;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Renders a template body by replacing <c>{{Field}}</c> merge fields with work-product data
/// (US-4.3.1, B.COM.24; US-4.3.3, B.COM.26). Merge fields are explicit data contracts validated
/// against <see cref="TemplateMergeFieldCatalog"/>.
/// </summary>
internal static class TemplateRenderer
{
    private static readonly Regex MergeFieldPattern = new(@"\{\{\s*([A-Za-z0-9_]+)\s*\}\}", RegexOptions.Compiled);

    /// <summary>Returns the distinct merge-field names referenced by a template body.</summary>
    public static IReadOnlyList<string> GetUsedFields(string body)
    {
        if (string.IsNullOrEmpty(body))
        {
            return Array.Empty<string>();
        }

        return MergeFieldPattern.Matches(body)
            .Select(m => m.Groups[1].Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>Returns the merge-field names referenced by a body that are not in the catalog.</summary>
    public static IReadOnlyList<string> GetUnknownFields(string body) =>
        GetUsedFields(body).Where(f => !TemplateMergeFieldCatalog.Contains(f)).ToList();

    public static string Render(string body, WorkTask task)
    {
        if (string.IsNullOrEmpty(body))
        {
            return string.Empty;
        }

        Dictionary<string, string> values = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Identifier"] = task.Identifier.Value,
            ["Title"] = task.Title,
            ["Type"] = task.Type.ToString(),
            ["Description"] = task.Description ?? string.Empty,
            ["DueDate"] = task.DueDate?.ToString("yyyy-MM-dd") ?? string.Empty,
            ["Priority"] = task.Priority.ToString(),
            ["Status"] = task.Status.ToString(),
            ["Owner"] = task.Owner ?? string.Empty,
            ["Content"] = task.Content ?? string.Empty,
            ["StoryId"] = task.SourceTrace.StoryId,
            ["RequirementId"] = task.SourceTrace.RequirementId,
            ["CreatedAt"] = task.CreatedAt.ToString("yyyy-MM-dd"),
            ["UpdatedAt"] = task.UpdatedAt.ToString("yyyy-MM-dd"),
        };

        return MergeFieldPattern.Replace(body, match =>
            values.TryGetValue(match.Groups[1].Value, out string? value) ? value : match.Value);
    }
}
