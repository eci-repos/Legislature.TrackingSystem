namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A legislative bill tracked in the solution (F5.1, F5.2). External bill language and status
/// changes are ingested and applied to the corresponding bill, and prior versions are retained.
/// </summary>
public sealed class Bill
{
    private readonly List<BillVersion> _versions = new();
    private readonly List<BillAmendment> _amendments = new();

    private Bill(
        Guid id,
        string billNumber,
        string title,
        BillStatus status,
        string currentVersion,
        string currentLanguage,
        int year,
        string biennium,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        Id = id;
        BillNumber = billNumber;
        Title = title;
        Status = status;
        CurrentVersion = currentVersion;
        CurrentLanguage = currentLanguage;
        Year = year;
        Biennium = biennium;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }

    public string BillNumber { get; private set; }

    public string Title { get; private set; }

    public BillStatus Status { get; private set; }

    public string CurrentVersion { get; private set; }

    public string CurrentLanguage { get; private set; }

    public int Year { get; private set; }

    public string Biennium { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// Whether this bill is flagged as part of the DOR budget (US-7.2.1, B.RFA.07).
    /// </summary>
    public bool IsBudgetBill { get; private set; }

    /// <summary>
    /// Whether this bill requires legislative implementation (US-12.1.4, B.LNP.07). The
    /// indication remains associated with the bill.
    /// </summary>
    public bool RequiresImplementation { get; private set; }

    public IReadOnlyList<BillVersion> Versions => _versions;

    public IReadOnlyList<BillAmendment> Amendments => _amendments;

    public static Bill Create(
        string billNumber,
        string title,
        BillStatus status,
        string currentVersion,
        string currentLanguage,
        int year,
        string biennium,
        DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(billNumber))
        {
            throw new ArgumentException("A bill requires a bill number.", nameof(billNumber));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A bill requires a title.", nameof(title));
        }

        Bill bill = new(Guid.NewGuid(), billNumber.Trim(), title.Trim(), status, currentVersion ?? string.Empty, currentLanguage ?? string.Empty, year, biennium ?? string.Empty, at, at);
        bill._versions.Add(new BillVersion(Guid.NewGuid(), bill.Id, bill.CurrentVersion, bill.CurrentLanguage, at, null));
        return bill;
    }

    /// <summary>
    /// Applies an external bill-language update, retaining the prior version (US-5.1.1, B.COM.31;
    /// US-5.2.1, B.COM.33). The new state is captured as a version so every state is comparable.
    /// </summary>
    public void UpdateLanguage(string title, string versionLabel, string language, string? source, DateTimeOffset at)
    {
        _versions.Add(new BillVersion(Guid.NewGuid(), Id, versionLabel, language ?? string.Empty, at, source));
        Title = title.Trim();
        CurrentVersion = versionLabel;
        CurrentLanguage = language ?? string.Empty;
        UpdatedAt = at;
    }

    /// <summary>
    /// Applies an external bill-status change (US-5.1.2, B.COM.32).
    /// </summary>
    public void UpdateStatus(BillStatus status, DateTimeOffset at)
    {
        Status = status;
        UpdatedAt = at;
    }

    /// <summary>
    /// Imports and tracks a proposed or unadopted amendment (US-5.1.3, B.LNP.01).
    /// </summary>
    public BillAmendment AddAmendment(string amendmentNumber, string language, string? source, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(amendmentNumber))
        {
            throw new ArgumentException("An amendment requires an amendment number.", nameof(amendmentNumber));
        }

        BillAmendment amendment = new(Guid.NewGuid(), Id, amendmentNumber.Trim(), language ?? string.Empty, at, source);
        _amendments.Add(amendment);
        UpdatedAt = at;
        return amendment;
    }

    /// <summary>
    /// Flags or unflags this bill as part of the DOR budget (US-7.2.1, B.RFA.07).
    /// </summary>
    public void SetBudgetBillFlag(bool isBudgetBill, DateTimeOffset at)
    {
        IsBudgetBill = isBudgetBill;
        UpdatedAt = at;
    }

    /// <summary>
    /// Marks whether this bill requires legislative implementation (US-12.1.4, B.LNP.07).
    /// </summary>
    public void SetRequiresImplementation(bool requiresImplementation, DateTimeOffset at)
    {
        RequiresImplementation = requiresImplementation;
        UpdatedAt = at;
    }
}
