using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class WorkflowDefinitionCatalogTests
{
    [Fact]
    public void Catalog_DefinesReviewerCountPerType()
    {
        Assert.Equal(1, WorkflowDefinitionCatalog.RequiredReviewerCountFor(WorkItemType.BillAnalysis));
        Assert.Equal(2, WorkflowDefinitionCatalog.RequiredReviewerCountFor(WorkItemType.FiscalNote));
        Assert.Equal(2, WorkflowDefinitionCatalog.RequiredReviewerCountFor(WorkItemType.FiscalEstimate));
    }

    [Fact]
    public void RequiredReviewerCountFor_DefaultsToOneForUndefinedType()
    {
        Assert.Equal(1, WorkflowDefinitionCatalog.RequiredReviewerCountFor((WorkItemType)999));
    }
}
