using Legislature.TrackingSystem.Application.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class TemplateMergeFieldCatalogTests
{
    [Fact]
    public void Catalog_ContainsKnownFields()
    {
        Assert.Contains("Identifier", TemplateMergeFieldCatalog.All.Select(f => f.Name));
        Assert.Contains("Title", TemplateMergeFieldCatalog.All.Select(f => f.Name));
        Assert.Contains("Type", TemplateMergeFieldCatalog.All.Select(f => f.Name));
        Assert.Contains("StoryId", TemplateMergeFieldCatalog.All.Select(f => f.Name));
        Assert.Contains("RequirementId", TemplateMergeFieldCatalog.All.Select(f => f.Name));
    }

    [Fact]
    public void Contains_IsCaseInsensitive()
    {
        Assert.True(TemplateMergeFieldCatalog.Contains("identifier"));
        Assert.True(TemplateMergeFieldCatalog.Contains("IDENTIFIER"));
        Assert.False(TemplateMergeFieldCatalog.Contains("NotAField"));
    }

    [Fact]
    public void GetUsedFields_ExtractsDistinctFields()
    {
        IReadOnlyList<string> fields = TemplateRenderer.GetUsedFields("{{Title}} and {{Title}} and {{Type}}");

        Assert.Equal(2, fields.Count);
        Assert.Contains("Title", fields);
        Assert.Contains("Type", fields);
    }

    [Fact]
    public void GetUsedFields_ReturnsEmptyForNullOrEmpty()
    {
        Assert.Empty(TemplateRenderer.GetUsedFields(null!));
        Assert.Empty(TemplateRenderer.GetUsedFields(string.Empty));
    }

    [Fact]
    public void GetUnknownFields_ReturnsOnlyUnknownFields()
    {
        IReadOnlyList<string> unknown = TemplateRenderer.GetUnknownFields("{{Title}} and {{BogusField}}");

        Assert.Single(unknown);
        Assert.Equal("BogusField", unknown[0]);
    }

    [Fact]
    public void GetUnknownFields_ReturnsEmptyWhenAllKnown()
    {
        Assert.Empty(TemplateRenderer.GetUnknownFields("{{Title}} {{Identifier}} {{Type}}"));
    }
}
