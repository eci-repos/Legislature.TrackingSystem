namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Represents Microsoft 365 productivity integration boundaries (F8.1 - Productivity Suite
/// Integration). Real tenant integration is deferred; the POC records template population and
/// email dispatches.
/// </summary>
public interface IProductivityIntegrationService
{
    Task<string> PopulateTemplateAsync(PopulateTemplateCommand command, CancellationToken cancellationToken);

    Task<EmailDispatchDto> EmailOutputAsync(EmailOutputCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<EmailDispatchDto>> ListEmailDispatchesAsync(CancellationToken cancellationToken);
}
