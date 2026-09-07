namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The legislative lifecycle status of a bill (US-5.1.2, B.COM.32). Lifecycle changes such as
/// HB to SHB can be represented (US-5.1.1, B.COM.31).
/// </summary>
public enum BillStatus
{
    Introduced,
    InCommittee,
    PassedHouse,
    PassedSenate,
    Enacted,
    Vetoed,
    Dead,
}
