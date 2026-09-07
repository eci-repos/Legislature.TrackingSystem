using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record RunStandardReportRequest(
    [property: Required] string ReportName);
