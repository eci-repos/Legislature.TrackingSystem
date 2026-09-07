namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// A document to be stored through the Microsoft 365 / SharePoint integration boundary
/// (F8.1 - Productivity Suite Integration).
/// </summary>
public sealed record M365Document(
    string Name,
    string Content,
    string ContentType);
