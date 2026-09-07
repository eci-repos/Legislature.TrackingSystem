namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// A known template merge field (US-4.3.1, B.COM.24; US-4.3.3, B.COM.26). Merge fields are explicit
/// data contracts: a template body may reference only fields in the catalog.
/// </summary>
public sealed record TemplateMergeField(string Name, string Description);

/// <summary>
/// The catalog of known template merge fields. Template bodies are validated against this catalog so
/// unknown fields are surfaced instead of silently left unresolved.
/// </summary>
public static class TemplateMergeFieldCatalog
{
    public static IReadOnlyList<TemplateMergeField> All { get; } =
    [
        new("Identifier", "The work item identifier."),
        new("Title", "The work item title."),
        new("Type", "The work item type."),
        new("Description", "The work item description."),
        new("DueDate", "The work item due date (yyyy-MM-dd)."),
        new("Priority", "The work item priority."),
        new("Status", "The work item status."),
        new("Owner", "The work item owner."),
        new("Content", "The work item content."),
        new("StoryId", "The source user story id."),
        new("RequirementId", "The source requirement id."),
        new("CreatedAt", "The work item creation date (yyyy-MM-dd)."),
        new("UpdatedAt", "The work item last-updated date (yyyy-MM-dd)."),
    ];

    /// <summary>Returns whether the named field is a known merge field (case-insensitive).</summary>
    public static bool Contains(string name) =>
        All.Any(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));
}
