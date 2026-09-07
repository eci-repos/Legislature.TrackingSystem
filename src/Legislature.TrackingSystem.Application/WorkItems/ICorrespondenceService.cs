namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Tracks correspondence sent to recipients and whether responses were received (US-11.1.1,
/// B.LNP.03).
/// </summary>
public interface ICorrespondenceService
{
    Task<CorrespondenceDto> RecordAsync(RecordCorrespondenceCommand command, CancellationToken cancellationToken);

    Task<CorrespondenceDto> MarkResponseReceivedAsync(MarkResponseReceivedCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<CorrespondenceDto>> ListAsync(CancellationToken cancellationToken);
}
