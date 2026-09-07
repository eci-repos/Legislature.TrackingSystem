namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Emails an applicable output to a stakeholder (US-8.1.1, B.COM.30).
/// </summary>
public sealed record EmailOutputCommand(string Recipient, string Subject, string Body, string SentByKey);
