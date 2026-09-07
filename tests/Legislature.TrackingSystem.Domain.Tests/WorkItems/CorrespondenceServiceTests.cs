using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class CorrespondenceServiceTests
{
    private static ICorrespondenceService CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<ICorrespondenceRepository, FakeCorrespondenceRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ICorrespondenceService>();
    }

    [Fact]
    public async Task RecordAsyncStoresSentCorrespondence()
    {
        ICorrespondenceService service = CreateServices();

        CorrespondenceDto item = await service.RecordAsync(
            new RecordCorrespondenceCommand(null, null, "recipient@example.com", "Fiscal note request", "body", "jdoe"),
            CancellationToken.None);

        Assert.Equal("recipient@example.com", item.Recipient);
        Assert.False(item.ResponseReceived);
    }

    [Fact]
    public async Task MarkResponseReceivedAsyncIndicatesResponse()
    {
        ICorrespondenceService service = CreateServices();
        CorrespondenceDto item = await service.RecordAsync(
            new RecordCorrespondenceCommand(null, null, "recipient@example.com", "Subject", "body", "jdoe"),
            CancellationToken.None);

        CorrespondenceDto updated = await service.MarkResponseReceivedAsync(new MarkResponseReceivedCommand(item.Id), CancellationToken.None);

        Assert.True(updated.ResponseReceived);
    }

    [Fact]
    public async Task ListAsyncReturnsRecordedCorrespondence()
    {
        ICorrespondenceService service = CreateServices();
        await service.RecordAsync(new RecordCorrespondenceCommand(null, null, "a@example.com", "Subject", "body", "jdoe"), CancellationToken.None);

        IReadOnlyList<CorrespondenceDto> items = await service.ListAsync(CancellationToken.None);

        Assert.Single(items);
    }
}
