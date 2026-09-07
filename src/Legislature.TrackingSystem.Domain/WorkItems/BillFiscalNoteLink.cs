namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// Associates a fiscal note (work product) with a bill so the comparison relationship remains
/// associated with the bill (US-7.2.1, B.RFA.07).
/// </summary>
public sealed class BillFiscalNoteLink
{
    private BillFiscalNoteLink(Guid id, Guid billId, Guid workTaskId, string linkedByKey, DateTimeOffset linkedAt)
    {
        Id = id;
        BillId = billId;
        WorkTaskId = workTaskId;
        LinkedByKey = linkedByKey;
        LinkedAt = linkedAt;
    }

    public Guid Id { get; }

    public Guid BillId { get; }

    public Guid WorkTaskId { get; }

    public string LinkedByKey { get; }

    public DateTimeOffset LinkedAt { get; }

    public static BillFiscalNoteLink Create(Guid billId, Guid workTaskId, string linkedByKey, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(linkedByKey))
        {
            throw new ArgumentException("A fiscal-note link requires a linker.", nameof(linkedByKey));
        }

        return new BillFiscalNoteLink(Guid.NewGuid(), billId, workTaskId, linkedByKey.Trim(), at);
    }
}
