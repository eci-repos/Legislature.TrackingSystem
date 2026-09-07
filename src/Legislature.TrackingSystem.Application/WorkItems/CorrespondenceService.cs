using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class CorrespondenceService : ICorrespondenceService
{
    private readonly ICorrespondenceRepository _correspondence;

    public CorrespondenceService(ICorrespondenceRepository correspondence)
    {
        _correspondence = correspondence;
    }

    public async Task<CorrespondenceDto> RecordAsync(RecordCorrespondenceCommand command, CancellationToken cancellationToken)
    {
        Correspondence item = Correspondence.Create(
            command.BillId,
            command.WorkTaskId,
            command.Recipient,
            command.Subject,
            command.Body,
            command.SentByKey,
            DateTimeOffset.UtcNow);
        await _correspondence.AddAsync(item, cancellationToken);
        return ToDto(item);
    }

    public async Task<CorrespondenceDto> MarkResponseReceivedAsync(MarkResponseReceivedCommand command, CancellationToken cancellationToken)
    {
        Correspondence item = await _correspondence.FindByIdAsync(command.CorrespondenceId, cancellationToken)
            ?? throw new InvalidOperationException($"Correspondence '{command.CorrespondenceId}' was not found.");

        item.MarkResponseReceived(DateTimeOffset.UtcNow);
        return ToDto(item);
    }

    public async Task<IReadOnlyList<CorrespondenceDto>> ListAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<Correspondence> items = await _correspondence.GetAllAsync(cancellationToken);
        return items.Select(ToDto).ToList();
    }

    private static CorrespondenceDto ToDto(Correspondence item)
    {
        return new CorrespondenceDto(
            item.Id,
            item.BillId,
            item.WorkTaskId,
            item.Recipient,
            item.Subject,
            item.Body,
            item.SentByKey,
            item.SentAt,
            item.ResponseReceived);
    }
}
