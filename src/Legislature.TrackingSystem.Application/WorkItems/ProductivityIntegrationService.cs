using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ProductivityIntegrationService : IProductivityIntegrationService
{
    private readonly ITemplateService _templates;
    private readonly IEmailDispatchRepository _dispatches;
    private readonly IM365Connector _connector;

    public ProductivityIntegrationService(ITemplateService templates, IEmailDispatchRepository dispatches, IM365Connector connector)
    {
        _templates = templates;
        _dispatches = dispatches;
        _connector = connector;
    }

    public async Task<string> PopulateTemplateAsync(PopulateTemplateCommand command, CancellationToken cancellationToken)
    {
        GeneratedDocumentDto document = await _templates.GenerateDocumentAsync(
            new GenerateDocumentCommand(command.WorkItemId, command.TemplateId, null),
            cancellationToken);
        return document.Body;
    }

    public async Task<EmailDispatchDto> EmailOutputAsync(EmailOutputCommand command, CancellationToken cancellationToken)
    {
        // Dispatch through the Microsoft 365 connector (the dev fake records nothing), then persist
        // the dispatch record.
        await _connector.SendEmailAsync(new M365EmailMessage(command.Recipient, command.Subject, command.Body), cancellationToken);
        EmailDispatch dispatch = EmailDispatch.Create(command.Recipient, command.Subject, command.Body, command.SentByKey, DateTimeOffset.UtcNow);
        await _dispatches.AddAsync(dispatch, cancellationToken);
        return ToDto(dispatch);
    }

    public async Task<IReadOnlyList<EmailDispatchDto>> ListEmailDispatchesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<EmailDispatch> all = await _dispatches.GetAllAsync(cancellationToken);
        return all.Select(ToDto).ToList();
    }

    private static EmailDispatchDto ToDto(EmailDispatch dispatch)
    {
        return new EmailDispatchDto(dispatch.Id, dispatch.Recipient, dispatch.Subject, dispatch.Body, dispatch.SentByKey, dispatch.SentAt);
    }
}
