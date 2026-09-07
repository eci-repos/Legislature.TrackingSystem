using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Infrastructure.Connectors;
using Legislature.TrackingSystem.Infrastructure.Persistence;
using Legislature.TrackingSystem.Infrastructure.WorkItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registers the LTS infrastructure adapters. When a PostgreSQL connection string is supplied,
    /// migrated aggregates are persisted through EF Core; otherwise the interim in-memory adapters
    /// keep local development runnable without a database dependency.
    /// </summary>
    public static IServiceCollection AddLtsInfrastructure(
        this IServiceCollection services,
        string? connectionString = null)
    {
        bool usePostgres = !string.IsNullOrWhiteSpace(connectionString);

        if (usePostgres)
        {
            services.AddDbContext<LtsDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddScoped<IBillRepository, EfBillRepository>();
            services.AddScoped<IUserRepository, EfUserRepository>();
            services.AddScoped<IPackageRepository, EfPackageRepository>();
            services.AddScoped<IWorkItemRelationshipRepository, EfWorkItemRelationshipRepository>();
            services.AddScoped<INotificationRepository, EfNotificationRepository>();
            services.AddScoped<IDocumentTemplateRepository, EfDocumentTemplateRepository>();
            services.AddScoped<IGeneratedDocumentRepository, EfGeneratedDocumentRepository>();
            services.AddScoped<ICustomReportRepository, EfCustomReportRepository>();
            services.AddScoped<ICorrespondenceRepository, EfCorrespondenceRepository>();
            services.AddScoped<IImplementationTaskRepository, EfImplementationTaskRepository>();
            services.AddScoped<IExecutiveDiscussionRepository, EfExecutiveDiscussionRepository>();
            services.AddScoped<IFiscalDataRepository, EfFiscalDataRepository>();
            services.AddScoped<IFiscalWorkPaperRepository, EfFiscalWorkPaperRepository>();
            services.AddScoped<IDemographicDataRepository, EfDemographicDataRepository>();
            services.AddScoped<IEmailDispatchRepository, EfEmailDispatchRepository>();
            services.AddScoped<IExpenseEstimateRepository, EfExpenseEstimateRepository>();
            services.AddScoped<IBillFiscalNoteLinkRepository, EfBillFiscalNoteLinkRepository>();
            services.AddScoped<IAccessRestrictionRepository, EfAccessRestrictionRepository>();
            services.AddScoped<ILegacyMigrationRepository, EfLegacyMigrationRepository>();
            services.AddScoped<IWorkTaskRepository, EfWorkTaskRepository>();
        }
        else
        {
            services.AddSingleton<IBillRepository, InMemoryBillRepository>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddSingleton<IPackageRepository, InMemoryPackageRepository>();
            services.AddSingleton<IWorkItemRelationshipRepository, InMemoryWorkItemRelationshipRepository>();
            services.AddSingleton<INotificationRepository, InMemoryNotificationRepository>();
            services.AddSingleton<IDocumentTemplateRepository, InMemoryDocumentTemplateRepository>();
            services.AddSingleton<IGeneratedDocumentRepository, InMemoryGeneratedDocumentRepository>();
            services.AddSingleton<ICustomReportRepository, InMemoryCustomReportRepository>();
            services.AddSingleton<ICorrespondenceRepository, InMemoryCorrespondenceRepository>();
            services.AddSingleton<IImplementationTaskRepository, InMemoryImplementationTaskRepository>();
            services.AddSingleton<IExecutiveDiscussionRepository, InMemoryExecutiveDiscussionRepository>();
            services.AddSingleton<IFiscalDataRepository, InMemoryFiscalDataRepository>();
            services.AddSingleton<IFiscalWorkPaperRepository, InMemoryFiscalWorkPaperRepository>();
            services.AddSingleton<IDemographicDataRepository, InMemoryDemographicDataRepository>();
            services.AddSingleton<IEmailDispatchRepository, InMemoryEmailDispatchRepository>();
            services.AddSingleton<IExpenseEstimateRepository, InMemoryExpenseEstimateRepository>();
            services.AddSingleton<IBillFiscalNoteLinkRepository, InMemoryBillFiscalNoteLinkRepository>();
            services.AddSingleton<IAccessRestrictionRepository, InMemoryAccessRestrictionRepository>();
            services.AddSingleton<ILegacyMigrationRepository, InMemoryLegacyMigrationRepository>();
            services.AddSingleton<IWorkTaskRepository, InMemoryWorkTaskRepository>();
        }

        services.AddSingleton<IWorkItemIdentifierGenerator, WorkItemIdentifierGenerator>();
        services.AddScoped<SeedDataInitializer>();

        return services;
    }

    /// <summary>
    /// Registers the external connector layer. Each connector is selected from configuration: when
    /// its section is configured the HTTP/Graph adapter is registered, otherwise the dev-boundary
    /// fake keeps the app runnable offline. Mirrors the Entra auth boundary selection.
    /// </summary>
    public static IServiceCollection AddLtsConnectors(
        this IServiceCollection services,
        ConnectorOptions options)
    {
        if (options.LegislativeSource.IsConfigured)
        {
            services.AddSingleton<ILegislativeSourceConnector>(_ =>
                new HttpLegislativeSourceConnector(
                    new HttpClient { BaseAddress = new Uri(options.LegislativeSource.BaseUrl!) },
                    options.LegislativeSource));
        }
        else
        {
            services.AddSingleton<ILegislativeSourceConnector, FakeLegislativeSourceConnector>();
        }

        if (options.FiscalDataSource.IsConfigured)
        {
            services.AddSingleton<IFiscalDataSourceConnector>(_ =>
                new HttpFiscalDataSourceConnector(
                    new HttpClient { BaseAddress = new Uri(options.FiscalDataSource.BaseUrl!) },
                    options.FiscalDataSource));
        }
        else
        {
            services.AddSingleton<IFiscalDataSourceConnector, FakeFiscalDataSourceConnector>();
        }

        if (options.M365.IsConfigured)
        {
            services.AddSingleton<IM365Connector>(_ =>
                new GraphM365Connector(
                    new HttpClient { BaseAddress = new Uri("https://graph.microsoft.com/v1.0/") },
                    options.M365));
        }
        else
        {
            services.AddSingleton<IM365Connector, FakeM365Connector>();
        }

        return services;
    }
}
