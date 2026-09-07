using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record ImportLegacyDataRequest(
    [property: Required] string Source,
    string? ImportedByKey,
    [property: Required, MinLength(1)] IReadOnlyList<LegacyImportRowRequest> Rows);
