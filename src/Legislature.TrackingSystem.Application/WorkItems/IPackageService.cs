namespace Legislature.TrackingSystem.Application.WorkItems;

public interface IPackageService
{
    Task<PackageDto> CreateAsync(CreatePackageCommand command, CancellationToken cancellationToken);

    Task<PackageDto> AddWorkProductAsync(AddWorkProductToPackageCommand command, CancellationToken cancellationToken);

    Task<PackageDto> RemoveWorkProductAsync(RemoveWorkProductFromPackageCommand command, CancellationToken cancellationToken);

    Task<PackageDto> DeliverAsync(DeliverPackageCommand command, CancellationToken cancellationToken);

    Task<PackageDto> AddRecipientAsync(AddPackageRecipientCommand command, CancellationToken cancellationToken);

    Task<PackageDto> FinalizePackageAsync(FinalizePackageCommand command, CancellationToken cancellationToken);

    Task<PackageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<PackageDto>> GetAllAsync(CancellationToken cancellationToken);
}
