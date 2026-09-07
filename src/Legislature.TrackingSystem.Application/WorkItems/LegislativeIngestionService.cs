using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class LegislativeIngestionService : ILegislativeIngestionService
{
    private readonly IBillRepository _bills;
    private readonly ILegislativeSourceConnector _connector;

    public LegislativeIngestionService(IBillRepository bills, ILegislativeSourceConnector connector)
    {
        _bills = bills;
        _connector = connector;
    }

    public async Task<BillDto> IngestBillUpdateAsync(IngestBillUpdateCommand command, CancellationToken cancellationToken)
    {
        LegislativeBillSnapshot? snapshot = await _connector.FetchBillAsync(command.BillNumber, command.Biennium, cancellationToken);
        string title = snapshot?.Title ?? command.Title;
        string versionLabel = snapshot?.VersionLabel ?? command.VersionLabel;
        string language = snapshot?.Language ?? command.Language;
        string? source = snapshot?.Source ?? command.Source;

        Bill? bill = await _bills.FindByNumberAndBienniumAsync(command.BillNumber, command.Biennium, cancellationToken);
        if (bill is null)
        {
            bill = Bill.Create(
                command.BillNumber,
                title,
                BillStatus.Introduced,
                versionLabel,
                language,
                command.Year,
                command.Biennium,
                DateTimeOffset.UtcNow);
            await _bills.AddAsync(bill, cancellationToken);
        }
        else
        {
            bill.UpdateLanguage(title, versionLabel, language, source, DateTimeOffset.UtcNow);
        }

        return ToDto(bill);
    }

    public async Task<BillDto> IngestBillStatusAsync(IngestBillStatusCommand command, CancellationToken cancellationToken)
    {
        Bill bill = await GetByNumberAsync(command.BillNumber, cancellationToken);
        bill.UpdateStatus(command.Status, DateTimeOffset.UtcNow);
        return ToDto(bill);
    }

    public async Task<BillDto> IngestAmendmentAsync(IngestAmendmentCommand command, CancellationToken cancellationToken)
    {
        Bill bill = await GetByNumberAsync(command.BillNumber, cancellationToken);

        IReadOnlyList<LegislativeAmendmentSnapshot> snapshots = await _connector.FetchAmendmentsAsync(command.BillNumber, bill.Biennium, cancellationToken);
        if (snapshots.Count > 0)
        {
            foreach (LegislativeAmendmentSnapshot s in snapshots)
            {
                bill.AddAmendment(s.AmendmentNumber, s.Language, s.Source, DateTimeOffset.UtcNow);
            }
        }
        else
        {
            bill.AddAmendment(command.AmendmentNumber, command.Language, command.Source, DateTimeOffset.UtcNow);
        }

        return ToDto(bill);
    }

    private async Task<Bill> GetByNumberAsync(string billNumber, CancellationToken cancellationToken)
    {
        return await _bills.FindByNumberAsync(billNumber, cancellationToken)
            ?? throw new InvalidOperationException($"Bill '{billNumber}' was not found.");
    }

    private static BillDto ToDto(Bill bill)
    {
        return new BillDto(
            bill.Id,
            bill.BillNumber,
            bill.Title,
            bill.Status,
            bill.CurrentVersion,
            bill.CurrentLanguage,
            bill.Year,
            bill.Biennium,
            bill.CreatedAt,
            bill.UpdatedAt,
            bill.IsBudgetBill,
            bill.RequiresImplementation,
            bill.Versions.Select(v => new BillVersionDto(v.Id, v.BillId, v.VersionLabel, v.Language, v.CapturedAt, v.Source)).ToList(),
            bill.Amendments.Select(a => new BillAmendmentDto(a.Id, a.BillId, a.AmendmentNumber, a.Language, a.CapturedAt, a.Source)).ToList());
    }
}
