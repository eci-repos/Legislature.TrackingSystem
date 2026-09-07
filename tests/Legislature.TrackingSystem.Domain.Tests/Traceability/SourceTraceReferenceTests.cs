using Legislature.TrackingSystem.Domain.Traceability;

namespace Legislature.TrackingSystem.Domain.Tests.Traceability;

public sealed class SourceTraceReferenceTests
{
    [Fact]
    public void CreateTrimsExplicitTraceValues()
    {
        SourceTraceReference trace = SourceTraceReference.Create(
            " TS-1.5 ",
            " TR-107 ",
            " M ",
            " Technical Requirements.docx ");

        Assert.Equal("TS-1.5", trace.StoryId);
        Assert.Equal("TR-107", trace.RequirementId);
        Assert.Equal("M", trace.RequirementType);
        Assert.Equal("Technical Requirements.docx", trace.SourceDocument);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateRejectsMissingStoryId(string storyId)
    {
        Assert.Throws<ArgumentException>(() => SourceTraceReference.Create(
            storyId,
            "TR-107",
            "M",
            "Technical Requirements.docx"));
    }
}
