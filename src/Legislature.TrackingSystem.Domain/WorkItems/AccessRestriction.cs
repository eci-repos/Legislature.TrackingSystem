namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A restriction on access to a portion of a work task or product (US-9.1.2, B.COM.16).
/// Restrictions are scoped by data type and by the user type (role) that is denied access, so
/// sensitive information can be restricted by data type or user type.
/// </summary>
public sealed class AccessRestriction
{
    private AccessRestriction(
        Guid id,
        Guid workTaskId,
        string dataType,
        UserRole restrictedUserType,
        string? note,
        string? setByKey,
        DateTimeOffset setAt)
    {
        Id = id;
        WorkTaskId = workTaskId;
        DataType = dataType;
        RestrictedUserType = restrictedUserType;
        Note = note;
        SetByKey = setByKey;
        SetAt = setAt;
    }

    public Guid Id { get; }

    public Guid WorkTaskId { get; }

    /// <summary>
    /// The portion of the work task or product that is restricted (for example a field, section,
    /// document, attachment, or comment).
    /// </summary>
    public string DataType { get; }

    /// <summary>
    /// The user type (role) that is denied access to the restricted portion.
    /// </summary>
    public UserRole RestrictedUserType { get; }

    public string? Note { get; }

    public string? SetByKey { get; }

    public DateTimeOffset SetAt { get; }

    public static AccessRestriction Create(
        Guid workTaskId,
        string dataType,
        UserRole restrictedUserType,
        string? note,
        string? setByKey,
        DateTimeOffset setAt)
    {
        if (string.IsNullOrWhiteSpace(dataType))
        {
            throw new ArgumentException("A data type is required.", nameof(dataType));
        }

        return new AccessRestriction(
            Guid.NewGuid(),
            workTaskId,
            dataType.Trim(),
            restrictedUserType,
            note,
            setByKey,
            setAt);
    }
}
