namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// An email message to be dispatched through the Microsoft 365 / Outlook integration boundary
/// (F8.1 - Productivity Suite Integration).
/// </summary>
public sealed record M365EmailMessage(
    string Recipient,
    string Subject,
    string Body);
