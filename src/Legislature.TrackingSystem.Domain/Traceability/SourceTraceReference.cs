namespace Legislature.TrackingSystem.Domain.Traceability;

public sealed record SourceTraceReference
{
    private SourceTraceReference(
        string storyId,
        string requirementId,
        string requirementType,
        string sourceDocument)
    {
        StoryId = storyId;
        RequirementId = requirementId;
        RequirementType = requirementType;
        SourceDocument = sourceDocument;
    }

    public string StoryId { get; }

    public string RequirementId { get; }

    public string RequirementType { get; }

    public string SourceDocument { get; }

    public static SourceTraceReference Create(
        string storyId,
        string requirementId,
        string requirementType,
        string sourceDocument)
    {
        return new SourceTraceReference(
            RequireValue(storyId, nameof(storyId)),
            RequireValue(requirementId, nameof(requirementId)),
            RequireValue(requirementType, nameof(requirementType)),
            RequireValue(sourceDocument, nameof(sourceDocument)));
    }

    private static string RequireValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Source trace values must be explicit.", parameterName);
        }

        return value.Trim();
    }
}
