using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class DocumentTemplateTests
{
    [Fact]
    public void Create_StartsAtVersionOne()
    {
        DocumentTemplate template = DocumentTemplate.Create("Fiscal Note", null, "Body", false, DateTimeOffset.UtcNow);

        Assert.Equal(1, template.Version);
    }

    [Fact]
    public void UpdateBody_IncrementsVersion()
    {
        DocumentTemplate template = DocumentTemplate.Create("Fiscal Note", null, "Original", false, DateTimeOffset.UtcNow);

        template.UpdateBody("Revised", DateTimeOffset.UtcNow);
        template.UpdateBody("Final", DateTimeOffset.UtcNow);

        Assert.Equal(3, template.Version);
        Assert.Equal("Final", template.Body);
    }

    [Fact]
    public void CreateVersion_CapturesSnapshot()
    {
        DocumentTemplate template = DocumentTemplate.Create("Fiscal Note", null, "Original", false, DateTimeOffset.UtcNow);
        template.UpdateBody("Revised", DateTimeOffset.UtcNow);

        DocumentTemplateVersion version = template.CreateVersion("jdoe");

        Assert.Equal(template.Id, version.TemplateId);
        Assert.Equal(template.Version, version.VersionNumber);
        Assert.Equal("Revised", version.Body);
        Assert.Equal("jdoe", version.UpdatedByKey);
    }
}
