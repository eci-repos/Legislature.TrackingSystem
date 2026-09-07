using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class PackageService : IPackageService
{
    private readonly IPackageRepository _repository;
    private readonly IWorkTaskRepository _workTaskRepository;

    public PackageService(IPackageRepository repository, IWorkTaskRepository workTaskRepository)
    {
        _repository = repository;
        _workTaskRepository = workTaskRepository;
    }

    public async Task<PackageDto> CreateAsync(CreatePackageCommand command, CancellationToken cancellationToken)
    {
        Package package = Package.Create(command.Name, command.Description, command.CreatedByKey, DateTimeOffset.UtcNow);
        await _repository.AddAsync(package, cancellationToken);
        return ToDto(package);
    }

    public async Task<PackageDto> AddWorkProductAsync(AddWorkProductToPackageCommand command, CancellationToken cancellationToken)
    {
        Package package = await GetPackageAsync(command.PackageId, cancellationToken);

        if (await _workTaskRepository.FindByIdAsync(command.WorkItemId, cancellationToken) is null)
        {
            throw new InvalidOperationException($"Work item '{command.WorkItemId}' was not found.");
        }

        package.AddWorkProduct(command.WorkItemId, command.AddedByKey, DateTimeOffset.UtcNow);
        await _repository.UpdateAsync(package, cancellationToken);
        return ToDto(package);
    }

    public async Task<PackageDto> RemoveWorkProductAsync(RemoveWorkProductFromPackageCommand command, CancellationToken cancellationToken)
    {
        Package package = await GetPackageAsync(command.PackageId, cancellationToken);
        package.RemoveWorkProduct(command.WorkItemId, DateTimeOffset.UtcNow);
        await _repository.UpdateAsync(package, cancellationToken);
        return ToDto(package);
    }

    public async Task<PackageDto> DeliverAsync(DeliverPackageCommand command, CancellationToken cancellationToken)
    {
        Package package = await GetPackageAsync(command.PackageId, cancellationToken);
        package.Deliver(DateTimeOffset.UtcNow);
        await _repository.UpdateAsync(package, cancellationToken);
        return ToDto(package);
    }

    public async Task<PackageDto> AddRecipientAsync(AddPackageRecipientCommand command, CancellationToken cancellationToken)
    {
        Package package = await GetPackageAsync(command.PackageId, cancellationToken);
        package.AddRecipient(command.Name, command.Kind, DateTimeOffset.UtcNow);
        await _repository.UpdateAsync(package, cancellationToken);
        return ToDto(package);
    }

    public async Task<PackageDto> FinalizePackageAsync(FinalizePackageCommand command, CancellationToken cancellationToken)
    {
        Package package = await GetPackageAsync(command.PackageId, cancellationToken);

        foreach (PackageMember member in package.Members)
        {
            WorkTask? task = await _workTaskRepository.FindByIdAsync(member.WorkItemId, cancellationToken);
            if (task is null)
            {
                throw new InvalidOperationException(
                    $"Package '{command.PackageId}' contains unknown work item '{member.WorkItemId}'.");
            }

            if (task.WorkflowStatus != WorkflowStatus.Approved)
            {
                throw new InvalidOperationException(
                    $"Work item '{task.Identifier.Value}' must be approved before the package can be finalized (workflow state '{task.WorkflowStatus}').");
            }
        }

        package.Finalize(DateTimeOffset.UtcNow);
        await _repository.UpdateAsync(package, cancellationToken);
        return ToDto(package);
    }

    public async Task<PackageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Package? package = await _repository.FindByIdAsync(id, cancellationToken);
        return package is null ? null : ToDto(package);
    }

    public async Task<IReadOnlyList<PackageDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<Package> packages = await _repository.GetAllAsync(cancellationToken);
        return packages.Select(ToDto).ToList();
    }

    private async Task<Package> GetPackageAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Package '{id}' was not found.");
    }

    private static PackageDto ToDto(Package package)
    {
        return new PackageDto(
            package.Id,
            package.Name,
            package.Description,
            package.Status,
            package.CreatedByKey,
            package.CreatedAt,
            package.UpdatedAt,
            package.Members.Select(m => new PackageMemberDto(m.WorkItemId, m.AddedByKey, m.AddedAt)).ToList(),
            package.Recipients.Select(r => new PackageRecipientDto(r.Id, r.Name, r.Kind, r.AddedAt)).ToList());
    }
}
