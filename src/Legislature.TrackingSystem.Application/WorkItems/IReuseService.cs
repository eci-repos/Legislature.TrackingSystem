namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Transfers applicable work from an existing product into a new product without copy-and-paste
/// (F4.4 - Reuse Existing Work).
/// </summary>
public interface IReuseService
{
    Task<WorkTaskDto> ReuseContentAsync(ReuseContentCommand command, CancellationToken cancellationToken);
}
