using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class TemplateService : ITemplateService
{
    private readonly IDocumentTemplateRepository _templates;
    private readonly IWorkTaskRepository _tasks;
    private readonly IGeneratedDocumentRepository _generatedDocuments;

    public TemplateService(
        IDocumentTemplateRepository templates,
        IWorkTaskRepository tasks,
        IGeneratedDocumentRepository generatedDocuments)
    {
        _templates = templates;
        _tasks = tasks;
        _generatedDocuments = generatedDocuments;
    }

    public async Task<IReadOnlyList<DocumentTemplateDto>> ListTemplatesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<DocumentTemplate> templates = await _templates.GetAllAsync(cancellationToken);
        return templates.Select(ToDto).ToList();
    }

    public async Task<DocumentTemplateDto> CreateTemplateAsync(CreateDocumentTemplateCommand command, CancellationToken cancellationToken)
    {
        DocumentTemplate template = DocumentTemplate.Create(
            command.Name,
            command.ApplicableWorkType,
            command.Body,
            command.IsShared,
            DateTimeOffset.UtcNow);
        await _templates.AddAsync(template, cancellationToken);
        return ToDto(template);
    }

    public async Task<DocumentTemplateDto> UpdateTemplateAsync(UpdateDocumentTemplateCommand command, CancellationToken cancellationToken)
    {
        DocumentTemplate template = await GetTemplateAsync(command.TemplateId, cancellationToken);
        template.UpdateBody(command.Body, DateTimeOffset.UtcNow);
        return ToDto(template);
    }

    public async Task<DocumentTemplateDto> SetTemplateSharedAsync(SetDocumentTemplateSharedCommand command, CancellationToken cancellationToken)
    {
        DocumentTemplate template = await GetTemplateAsync(command.TemplateId, cancellationToken);
        template.SetShared(command.IsShared, DateTimeOffset.UtcNow);
        return ToDto(template);
    }

    public async Task<GeneratedDocumentDto> GenerateDocumentAsync(GenerateDocumentCommand command, CancellationToken cancellationToken)
    {
        DocumentTemplate template = await GetTemplateAsync(command.TemplateId, cancellationToken);
        WorkTask task = await _tasks.FindByIdAsync(command.WorkItemId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkItemId}' was not found.");

        string body = TemplateRenderer.Render(template.Body, task);
        string title = $"{template.Name} - {task.Identifier.Value}";
        GeneratedDocument document = GeneratedDocument.Create(
            task.Id,
            template.Id,
            title,
            body,
            DateTimeOffset.UtcNow,
            command.GeneratedByKey);
        await _generatedDocuments.AddAsync(document, cancellationToken);
        return ToDto(document);
    }

    public async Task<IReadOnlyList<GeneratedDocumentDto>> ListGeneratedDocumentsAsync(Guid workItemId, CancellationToken cancellationToken)
    {
        IReadOnlyList<GeneratedDocument> documents = await _generatedDocuments.GetForWorkItemAsync(workItemId, cancellationToken);
        return documents.Select(ToDto).ToList();
    }

    private static GeneratedDocumentDto ToDto(GeneratedDocument document)
    {
        return new GeneratedDocumentDto(
            document.Id,
            document.WorkItemId,
            document.TemplateId,
            document.Title,
            document.Body,
            document.GeneratedAt,
            document.GeneratedByKey);
    }

    private async Task<DocumentTemplate> GetTemplateAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _templates.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Document template '{id}' was not found.");
    }

    private static DocumentTemplateDto ToDto(DocumentTemplate template)
    {
        return new DocumentTemplateDto(
            template.Id,
            template.Name,
            template.ApplicableWorkType,
            template.Body,
            template.IsShared,
            template.CreatedAt,
            template.UpdatedAt);
    }
}
