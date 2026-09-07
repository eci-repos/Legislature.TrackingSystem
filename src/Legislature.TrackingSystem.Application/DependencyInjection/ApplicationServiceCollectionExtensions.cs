using Legislature.TrackingSystem.Application.Readiness;
using Legislature.TrackingSystem.Application.WorkItems;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddLtsApplication(this IServiceCollection services)
    {
        services.AddSingleton<ISprintReadinessService, SprintReadinessService>();
        services.AddScoped<IWorkTaskService, WorkTaskService>();
        services.AddScoped<IWorkTaskAssignmentService, WorkTaskAssignmentService>();
        services.AddScoped<IWorkItemRelationshipService, WorkItemRelationshipService>();
        services.AddScoped<IPackageService, PackageService>();
        services.AddScoped<IWorkItemQueryService, WorkItemQueryService>();
        services.AddScoped<IWorkflowService, WorkflowService>();
        services.AddScoped<IExecutiveReviewService, ExecutiveReviewService>();
        services.AddScoped<IContentService, ContentService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<IReuseService, ReuseService>();
        services.AddScoped<ITaskMaintenanceService, TaskMaintenanceService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ILegislativeIngestionService, LegislativeIngestionService>();
        services.AddScoped<IVersionService, VersionService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IReportingService, ReportingService>();
        services.AddScoped<IFiscalDataService, FiscalDataService>();
        services.AddScoped<IFiscalWorkPaperService, FiscalWorkPaperService>();
        services.AddScoped<IBudgetBillService, BudgetBillService>();
        services.AddScoped<IDemographicDataService, DemographicDataService>();
        services.AddScoped<IProductivityIntegrationService, ProductivityIntegrationService>();
        services.AddScoped<IExpenseEstimateService, ExpenseEstimateService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IAccessControlService, AccessControlService>();
        services.AddScoped<IMigrationService, MigrationService>();
        services.AddScoped<IHistoricalReferenceService, HistoricalReferenceService>();
        services.AddScoped<ICorrespondenceService, CorrespondenceService>();
        services.AddScoped<IImplementationTaskService, ImplementationTaskService>();
        services.AddScoped<IExecutiveBillViewService, ExecutiveBillViewService>();
        services.AddScoped<IExecutiveDiscussionService, ExecutiveDiscussionService>();

        return services;
    }
}
