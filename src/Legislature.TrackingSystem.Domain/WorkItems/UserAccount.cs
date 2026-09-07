namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A DOR user account with an assigned role (US-9.1.1, B.COM.06). The role determines the
/// permissions the user holds; restricted functions are enforced against these permissions.
/// </summary>
public sealed class UserAccount
{
    private UserAccount(Guid id, string userKey, string displayName, UserRole role, bool isActive)
    {
        Id = id;
        UserKey = userKey;
        DisplayName = displayName;
        Role = role;
        IsActive = isActive;
    }

    public Guid Id { get; }

    public string UserKey { get; }

    public string DisplayName { get; }

    public UserRole Role { get; private set; }

    public bool IsActive { get; private set; }

    public static UserAccount Create(string userKey, string displayName, UserRole role, bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(userKey))
        {
            throw new ArgumentException("A user key is required.", nameof(userKey));
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("A display name is required.", nameof(displayName));
        }

        return new UserAccount(Guid.NewGuid(), userKey.Trim(), displayName.Trim(), role, isActive);
    }

    public void SetRole(UserRole role)
    {
        Role = role;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
