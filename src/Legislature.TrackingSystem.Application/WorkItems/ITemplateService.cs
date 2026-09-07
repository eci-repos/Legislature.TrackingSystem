namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Manages document templates and generated documents (F4.3 - Templates and Generated Documents).
/// </summary>
public interface ITemplateService
{
    Task<IReadOnlyList<DocumentTemplateDto>> ListTemplatesAsync(CancellationToken cancellationToken);

    Task<DocumentTemplateDto> CreateTemplateAsync(CreateDocumentTemplateCommand command, CancellationToken cancellationToken);

    Task<DocumentTemplateDto> UpdateTemplateAsync(UpdateDocumentTemplateCommand command, CancellationToken cancellationToken);

    Task<DocumentTemplateDto> SetTemplateSharedAsync(SetDocumentTemplateSharedCommand command, CancellationToken cancellationToken);

    Task<GeneratedDocumentDto> GenerateDocumentAsync(GenerateDocumentCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<GeneratedDocumentDto>> ListGeneratedDocumentsAsync(Guid workItemId, CancellationToken cancellationToken);
}
