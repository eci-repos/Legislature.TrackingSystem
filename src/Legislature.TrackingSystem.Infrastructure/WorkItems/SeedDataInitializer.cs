using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.Traceability;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Loads realistic DOR seed data into the stores so the POC is demonstrable without manual
/// entry. Seeding is idempotent: it only runs when the work task store is empty. The data is a
/// coherent follow-up of the core HB 1200 / SB 88 legislative thread so search, listing, and
/// related features can be showcased end to end.
/// </summary>
public sealed class SeedDataInitializer
{
    private readonly IWorkTaskRepository _workTaskRepository;
    private readonly IWorkItemRelationshipRepository _relationshipRepository;
    private readonly IPackageRepository _packageRepository;
    private readonly IWorkItemIdentifierGenerator _identifierGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IBillRepository _billRepository;
    private readonly IFiscalDataRepository _fiscalDataRepository;
    private readonly IDemographicDataRepository _demographicDataRepository;
    private readonly IDocumentTemplateRepository _templateRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICorrespondenceRepository _correspondenceRepository;
    private readonly IImplementationTaskRepository _implementationTaskRepository;
    private readonly IExecutiveDiscussionRepository _executiveDiscussionRepository;
    private readonly ICustomReportRepository _customReportRepository;
    private readonly IAccessRestrictionRepository _accessRestrictionRepository;
    private readonly IFiscalWorkPaperRepository _fiscalWorkPaperRepository;
    private readonly IExpenseEstimateRepository _expenseEstimateRepository;
    private readonly IBillFiscalNoteLinkRepository _billFiscalNoteLinkRepository;
    private readonly IEmailDispatchRepository _emailDispatchRepository;
    private readonly IGeneratedDocumentRepository _generatedDocumentRepository;
    private readonly ILegacyMigrationRepository _legacyMigrationRepository;

    public SeedDataInitializer(
        IWorkTaskRepository workTaskRepository,
        IWorkItemRelationshipRepository relationshipRepository,
        IPackageRepository packageRepository,
        IWorkItemIdentifierGenerator identifierGenerator,
        IUserRepository userRepository,
        IBillRepository billRepository,
        IFiscalDataRepository fiscalDataRepository,
        IDemographicDataRepository demographicDataRepository,
        IDocumentTemplateRepository templateRepository,
        INotificationRepository notificationRepository,
        ICorrespondenceRepository correspondenceRepository,
        IImplementationTaskRepository implementationTaskRepository,
        IExecutiveDiscussionRepository executiveDiscussionRepository,
        ICustomReportRepository customReportRepository,
        IAccessRestrictionRepository accessRestrictionRepository,
        IFiscalWorkPaperRepository fiscalWorkPaperRepository,
        IExpenseEstimateRepository expenseEstimateRepository,
        IBillFiscalNoteLinkRepository billFiscalNoteLinkRepository,
        IEmailDispatchRepository emailDispatchRepository,
        IGeneratedDocumentRepository generatedDocumentRepository,
        ILegacyMigrationRepository legacyMigrationRepository)
    {
        _workTaskRepository = workTaskRepository;
        _relationshipRepository = relationshipRepository;
        _packageRepository = packageRepository;
        _identifierGenerator = identifierGenerator;
        _userRepository = userRepository;
        _billRepository = billRepository;
        _fiscalDataRepository = fiscalDataRepository;
        _demographicDataRepository = demographicDataRepository;
        _templateRepository = templateRepository;
        _notificationRepository = notificationRepository;
        _correspondenceRepository = correspondenceRepository;
        _implementationTaskRepository = implementationTaskRepository;
        _executiveDiscussionRepository = executiveDiscussionRepository;
        _customReportRepository = customReportRepository;
        _accessRestrictionRepository = accessRestrictionRepository;
        _fiscalWorkPaperRepository = fiscalWorkPaperRepository;
        _expenseEstimateRepository = expenseEstimateRepository;
        _billFiscalNoteLinkRepository = billFiscalNoteLinkRepository;
        _emailDispatchRepository = emailDispatchRepository;
        _generatedDocumentRepository = generatedDocumentRepository;
        _legacyMigrationRepository = legacyMigrationRepository;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<WorkTask> existing = await _workTaskRepository.GetAllAsync(cancellationToken);
        if (existing.Count > 0)
        {
            return;
        }

        DateTimeOffset now = DateTimeOffset.UtcNow;

        await SeedUsersAsync(cancellationToken);
        IReadOnlyList<Bill> bills = await SeedBillsAsync(now, cancellationToken);
        IReadOnlyList<WorkTask> tasks = await SeedWorkTasksAsync(now, cancellationToken);
        await SeedPackagesAsync(tasks, now, cancellationToken);
        await SeedRelationshipsAsync(tasks, cancellationToken);
        await SeedFiscalDataAsync(now, cancellationToken);
        await SeedDemographicsAsync(now, cancellationToken);
        IReadOnlyList<DocumentTemplate> templates = await SeedTemplatesAsync(now, cancellationToken);
        await SeedNotificationsAsync(tasks, now, cancellationToken);
        await SeedCorrespondenceAsync(bills, tasks, now, cancellationToken);
        await SeedImplementationTasksAsync(bills, now, cancellationToken);
        await SeedExecutiveDiscussionsAsync(bills, tasks, now, cancellationToken);
        await SeedCustomReportsAsync(now, cancellationToken);
        await SeedAccessRestrictionsAsync(tasks, now, cancellationToken);
        await SeedFiscalWorkPapersAsync(tasks, now, cancellationToken);
        await SeedExpenseEstimateElementsAsync(now, cancellationToken);
        await SeedBillFiscalNoteLinksAsync(bills, tasks, now, cancellationToken);
        await SeedEmailDispatchesAsync(now, cancellationToken);
        await SeedGeneratedDocumentsAsync(tasks, templates, now, cancellationToken);
        await SeedMigrationAsync(tasks, now, cancellationToken);
    }

    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        await EnsureUserAsync("admin", "System Administrator", UserRole.SecurityAdministrator, cancellationToken);
        await EnsureUserAsync("jdoe", "Jane Doe", UserRole.Analyst, cancellationToken);
        await EnsureUserAsync("asmith", "Alex Smith", UserRole.Analyst, cancellationToken);
        await EnsureUserAsync("bchen", "Bo Chen", UserRole.Analyst, cancellationToken);
        await EnsureUserAsync("cduke", "Cara Duke", UserRole.Analyst, cancellationToken);
        await EnsureUserAsync("coordinator", "RFA Coordinator", UserRole.Reviewer, cancellationToken);
        await EnsureUserAsync("mrivera", "Mia Rivera", UserRole.Reviewer, cancellationToken);
        await EnsureUserAsync("snguyen", "Sam Nguyen", UserRole.Approver, cancellationToken);
        await EnsureUserAsync("kpatel", "Kiran Patel", UserRole.ExecutiveReviewer, cancellationToken);
        await EnsureUserAsync("lgonzalez", "Luis Gonzalez", UserRole.FinancialUser, cancellationToken);
        await EnsureUserAsync("rthomas", "Riley Thomas", UserRole.ReadOnly, cancellationToken);
    }

    private async Task EnsureUserAsync(string key, string displayName, UserRole role, CancellationToken cancellationToken)
    {
        if (await _userRepository.FindByKeyAsync(key, cancellationToken) is null)
        {
            await _userRepository.AddAsync(UserAccount.Create(key, displayName, role), cancellationToken);
        }
    }

    private async Task<IReadOnlyList<Bill>> SeedBillsAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        List<Bill> bills = new();

        Bill hb1200 = Bill.Create(
            "HB 1200", "Department of Revenue fiscal impact", BillStatus.InCommittee, "Original",
            "Establishes the fiscal impact of the Department of Revenue operations for the 2025-2026 biennium.", 2026, "2025-2026", now);
        hb1200.UpdateLanguage("Department of Revenue fiscal impact", "Substitute", "Establishes the fiscal impact of DOR operations, including tax administration modernization.", "legislative-source", now);
        hb1200.AddAmendment("HB 1200 A1", "Adds a reporting requirement for tax administration modernization.", "legislative-source", now);
        hb1200.SetBudgetBillFlag(true, now);
        hb1200.SetRequiresImplementation(true, now);
        await _billRepository.AddAsync(hb1200, cancellationToken);
        bills.Add(hb1200);

        Bill sb88 = Bill.Create(
            "SB 88", "Sales tax administration modernization", BillStatus.PassedSenate, "Original",
            "Modernizes sales tax administration and reporting.", 2026, "2025-2026", now);
        sb88.UpdateLanguage("Sales tax administration modernization", "Engrossed", "Modernizes sales tax administration, reporting, and remittance.", "legislative-source", now);
        sb88.AddAmendment("SB 88 A1", "Clarifies remittance thresholds for small businesses.", "legislative-source", now);
        sb88.SetRequiresImplementation(true, now);
        await _billRepository.AddAsync(sb88, cancellationToken);
        bills.Add(sb88);

        Bill hb1201 = Bill.Create(
            "HB 1201", "Working families tax credit", BillStatus.Introduced, "Original",
            "Establishes a refundable working families tax credit.", 2026, "2025-2026", now);
        hb1201.SetBudgetBillFlag(true, now);
        await _billRepository.AddAsync(hb1201, cancellationToken);
        bills.Add(hb1201);

        Bill sb89 = Bill.Create(
            "SB 89", "Property tax relief program", BillStatus.InCommittee, "Original",
            "Establishes a property tax relief program for qualifying homeowners.", 2026, "2025-2026", now);
        sb89.AddAmendment("SB 89 A1", "Adjusts the income eligibility threshold.", "legislative-source", now);
        await _billRepository.AddAsync(sb89, cancellationToken);
        bills.Add(sb89);

        Bill hb1300 = Bill.Create(
            "HB 1300", "Business license modernization", BillStatus.PassedHouse, "Original",
            "Modernizes the business license application and renewal process.", 2026, "2025-2026", now);
        hb1300.UpdateLanguage("Business license modernization", "Substitute", "Modernizes the business license application, renewal, and fee collection process.", "legislative-source", now);
        hb1300.SetRequiresImplementation(true, now);
        await _billRepository.AddAsync(hb1300, cancellationToken);
        bills.Add(hb1300);

        Bill sb101 = Bill.Create(
            "SB 101", "Corporate income tax apportionment", BillStatus.Introduced, "Original",
            "Updates the corporate income tax apportionment formula.", 2026, "2025-2026", now);
        await _billRepository.AddAsync(sb101, cancellationToken);
        bills.Add(sb101);

        Bill hb1400 = Bill.Create(
            "HB 1400", "Fuel tax indexing", BillStatus.InCommittee, "Original",
            "Indexes the motor fuel tax to inflation.", 2026, "2025-2026", now);
        hb1400.SetBudgetBillFlag(true, now);
        await _billRepository.AddAsync(hb1400, cancellationToken);
        bills.Add(hb1400);

        Bill sb110 = Bill.Create(
            "SB 110", "Data center sales tax exemption", BillStatus.Dead, "Original",
            "Provides a sales tax exemption for qualifying data centers.", 2026, "2025-2026", now);
        await _billRepository.AddAsync(sb110, cancellationToken);
        bills.Add(sb110);

        return bills;
    }

    private async Task<IReadOnlyList<WorkTask>> SeedWorkTasksAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        List<WorkTask> tasks = new();

        WorkTask analyzeHb1200 = CreateTask(
            WorkItemType.FiscalNote, "Analyze HB 1200 fiscal impact", "Estimate the fiscal impact of HB 1200.",
            new DateOnly(2026, 9, 20), TaskPriority.High, WorkTaskStatus.Assigned, "jdoe", now);
        analyzeHb1200.AssignUser("jdoe", AssignmentRole.Owner, new DateOnly(2026, 9, 20), "coordinator", now);
        analyzeHb1200.SetContent("HB 1200 establishes the fiscal impact of DOR operations. Preliminary estimate: $12.4M over the biennium.", now);
        analyzeHb1200.AddComment("asmith", "Please include the tax administration modernization costs.", now);
        await _workTaskRepository.AddAsync(analyzeHb1200, cancellationToken);
        tasks.Add(analyzeHb1200);

        WorkTask fiscalNoteSb88 = CreateTask(
            WorkItemType.FiscalNote, "Prepare fiscal note for SB 88", null,
            new DateOnly(2026, 9, 25), TaskPriority.Normal, WorkTaskStatus.InProgress, "asmith", now);
        fiscalNoteSb88.AssignUser("asmith", AssignmentRole.Analyst, new DateOnly(2026, 9, 25), "coordinator", now);
        fiscalNoteSb88.SetContent("SB 88 modernizes sales tax administration. Draft fiscal note in progress.", now);
        await _workTaskRepository.AddAsync(fiscalNoteSb88, cancellationToken);
        tasks.Add(fiscalNoteSb88);

        WorkTask billAnalysis = CreateTask(
            WorkItemType.BillAnalysis, "Draft bill analysis for HB 1200", null,
            new DateOnly(2026, 9, 30), TaskPriority.High, WorkTaskStatus.Proposed, "bchen", now);
        billAnalysis.AssignUser("bchen", AssignmentRole.Analyst, new DateOnly(2026, 9, 30), "coordinator", now);
        await _workTaskRepository.AddAsync(billAnalysis, cancellationToken);
        tasks.Add(billAnalysis);

        WorkTask dataRequest = CreateTask(
            WorkItemType.DataRequest, "Data request: prior-year revenue", "Request prior-year revenue data for the budget office.",
            new DateOnly(2026, 10, 5), TaskPriority.Normal, WorkTaskStatus.OnHold, "cduke", now);
        dataRequest.AssignUser("cduke", AssignmentRole.Analyst, new DateOnly(2026, 10, 5), "coordinator", now);
        dataRequest.SetCategorization(isConfidential: false, isExecutiveReview: true, now);
        await _workTaskRepository.AddAsync(dataRequest, cancellationToken);
        tasks.Add(dataRequest);

        WorkTask fiscalEstimate = CreateTask(
            WorkItemType.FiscalEstimate, "Fiscal estimate for HB 1200", null,
            new DateOnly(2026, 9, 22), TaskPriority.Critical, WorkTaskStatus.Assigned, "asmith", now);
        fiscalEstimate.AssignUser("asmith", AssignmentRole.Analyst, new DateOnly(2026, 9, 22), "coordinator", now);
        fiscalEstimate.AssignUser("jdoe", AssignmentRole.Reviewer, new DateOnly(2026, 9, 24), "coordinator", now);
        fiscalEstimate.SetCategorization(isConfidential: true, isExecutiveReview: false, now);
        fiscalEstimate.SetContent("Fiscal estimate for HB 1200: $12.4M over the 2025-2026 biennium.", now);
        await _workTaskRepository.AddAsync(fiscalEstimate, cancellationToken);
        tasks.Add(fiscalEstimate);

        WorkTask hearingReport = CreateTask(
            WorkItemType.WorkProduct, "Hearing report for SB 88", null,
            new DateOnly(2026, 10, 1), TaskPriority.Low, WorkTaskStatus.Submitted, "jdoe", now);
        hearingReport.AssignUser("jdoe", AssignmentRole.Owner, new DateOnly(2026, 10, 1), "coordinator", now);
        await _workTaskRepository.AddAsync(hearingReport, cancellationToken);
        tasks.Add(hearingReport);

        // Approved fiscal note for HB 1201.
        WorkTask fiscalNoteHb1201 = CreateTask(
            WorkItemType.FiscalNote, "Fiscal note for HB 1201 working families tax credit", "Estimate the cost of the working families tax credit.",
            new DateOnly(2026, 9, 18), TaskPriority.High, WorkTaskStatus.Approved, "bchen", now);
        fiscalNoteHb1201.AssignUser("bchen", AssignmentRole.Analyst, new DateOnly(2026, 9, 18), "coordinator", now);
        fiscalNoteHb1201.SetContent("Working families tax credit estimated at $85M annually.", now);
        SubmitAndApprove(fiscalNoteHb1201, new[] { "coordinator", "snguyen" }, now);
        await _workTaskRepository.AddAsync(fiscalNoteHb1201, cancellationToken);
        tasks.Add(fiscalNoteHb1201);

        // Under review bill analysis for SB 89.
        WorkTask billAnalysisSb89 = CreateTask(
            WorkItemType.BillAnalysis, "Bill analysis for SB 89 property tax relief", null,
            new DateOnly(2026, 9, 28), TaskPriority.Normal, WorkTaskStatus.InProgress, "cduke", now);
        billAnalysisSb89.AssignUser("cduke", AssignmentRole.Analyst, new DateOnly(2026, 9, 28), "coordinator", now);
        billAnalysisSb89.SetContent("SB 89 establishes a property tax relief program for qualifying homeowners.", now);
        SubmitForReview(billAnalysisSb89, new[] { "coordinator" }, now);
        billAnalysisSb89.RecordReview("coordinator", WorkflowDecision.Approved, "Analysis looks complete.", now);
        await _workTaskRepository.AddAsync(billAnalysisSb89, cancellationToken);
        tasks.Add(billAnalysisSb89);

        // Finalized fiscal estimate for HB 1300.
        WorkTask fiscalEstimateHb1300 = CreateTask(
            WorkItemType.FiscalEstimate, "Fiscal estimate for HB 1300 business license modernization", null,
            new DateOnly(2026, 9, 15), TaskPriority.High, WorkTaskStatus.Completed, "asmith", now);
        fiscalEstimateHb1300.AssignUser("asmith", AssignmentRole.Analyst, new DateOnly(2026, 9, 15), "coordinator", now);
        fiscalEstimateHb1300.SetContent("HB 1300 modernization estimated at $3.1M implementation cost.", now);
        SubmitAndApprove(fiscalEstimateHb1300, new[] { "coordinator", "snguyen" }, now);
        fiscalEstimateHb1300.Finalize(now);
        CompleteExecutiveReview(fiscalEstimateHb1300, new[] { "kpatel" }, now);
        await _workTaskRepository.AddAsync(fiscalEstimateHb1300, cancellationToken);
        tasks.Add(fiscalEstimateHb1300);

        // Draft data request for SB 101.
        WorkTask dataRequestSb101 = CreateTask(
            WorkItemType.DataRequest, "Data request: corporate apportionment data", "Request corporate apportionment data for SB 101 analysis.",
            new DateOnly(2026, 10, 10), TaskPriority.Normal, WorkTaskStatus.Proposed, "bchen", now);
        dataRequestSb101.AssignUser("bchen", AssignmentRole.Analyst, new DateOnly(2026, 10, 10), "coordinator", now);
        await _workTaskRepository.AddAsync(dataRequestSb101, cancellationToken);
        tasks.Add(dataRequestSb101);

        // Pending review work product for HB 1400.
        WorkTask workProductHb1400 = CreateTask(
            WorkItemType.WorkProduct, "Fuel tax indexing work product for HB 1400", null,
            new DateOnly(2026, 9, 26), TaskPriority.Normal, WorkTaskStatus.Submitted, "cduke", now);
        workProductHb1400.AssignUser("cduke", AssignmentRole.Analyst, new DateOnly(2026, 9, 26), "coordinator", now);
        workProductHb1400.SetContent("Fuel tax indexing analysis for HB 1400.", now);
        SubmitForReview(workProductHb1400, new[] { "coordinator" }, now);
        await _workTaskRepository.AddAsync(workProductHb1400, cancellationToken);
        tasks.Add(workProductHb1400);

        // Rejected fiscal note for SB 110.
        WorkTask fiscalNoteSb110 = CreateTask(
            WorkItemType.FiscalNote, "Fiscal note for SB 110 data center exemption", null,
            new DateOnly(2026, 9, 12), TaskPriority.Low, WorkTaskStatus.Submitted, "jdoe", now);
        fiscalNoteSb110.AssignUser("jdoe", AssignmentRole.Analyst, new DateOnly(2026, 9, 12), "coordinator", now);
        fiscalNoteSb110.SetContent("SB 110 data center exemption draft.", now);
        SubmitForReview(fiscalNoteSb110, new[] { "coordinator" }, now);
        fiscalNoteSb110.RecordReview("coordinator", WorkflowDecision.Rejected, "Revenue impact not fully documented.", now);
        await _workTaskRepository.AddAsync(fiscalNoteSb110, cancellationToken);
        tasks.Add(fiscalNoteSb110);

        // Approved bill analysis for HB 1200.
        WorkTask billAnalysisHb1200 = CreateTask(
            WorkItemType.BillAnalysis, "Bill analysis for HB 1200", null,
            new DateOnly(2026, 9, 10), TaskPriority.High, WorkTaskStatus.Approved, "bchen", now);
        billAnalysisHb1200.AssignUser("bchen", AssignmentRole.Analyst, new DateOnly(2026, 9, 10), "coordinator", now);
        billAnalysisHb1200.SetContent("Comprehensive bill analysis for HB 1200.", now);
        SubmitAndApprove(billAnalysisHb1200, new[] { "coordinator", "snguyen" }, now);
        await _workTaskRepository.AddAsync(billAnalysisHb1200, cancellationToken);
        tasks.Add(billAnalysisHb1200);

        // In-progress fiscal estimate for SB 88 with executive review started.
        WorkTask fiscalEstimateSb88 = CreateTask(
            WorkItemType.FiscalEstimate, "Fiscal estimate for SB 88", null,
            new DateOnly(2026, 9, 24), TaskPriority.High, WorkTaskStatus.InProgress, "asmith", now);
        fiscalEstimateSb88.AssignUser("asmith", AssignmentRole.Analyst, new DateOnly(2026, 9, 24), "coordinator", now);
        fiscalEstimateSb88.SetContent("SB 88 fiscal estimate in progress.", now);
        StartExecutiveReview(fiscalEstimateSb88, new[] { "kpatel" }, now);
        await _workTaskRepository.AddAsync(fiscalEstimateSb88, cancellationToken);
        tasks.Add(fiscalEstimateSb88);

        return tasks;
    }

    private async Task SeedPackagesAsync(IReadOnlyList<WorkTask> tasks, DateTimeOffset now, CancellationToken cancellationToken)
    {
        WorkTask analyzeHb1200 = tasks[0];
        WorkTask fiscalNoteSb88 = tasks[1];
        WorkTask fiscalEstimate = tasks[4];

        Package package = Package.Create(
            "FY2026 HB 1200 Fiscal Package",
            "Fiscal estimates and notes for HB 1200.",
            "coordinator",
            now);
        package.AddWorkProduct(analyzeHb1200.Id, "coordinator", now);
        package.AddWorkProduct(fiscalNoteSb88.Id, "coordinator", now);
        package.AddWorkProduct(fiscalEstimate.Id, "coordinator", now);
        package.SetStatus(PackageStatus.InProgress, now);
        await _packageRepository.AddAsync(package, cancellationToken);

        Package sb88Package = Package.Create(
            "FY2026 SB 88 Fiscal Package",
            "Fiscal notes and estimates for SB 88.",
            "coordinator",
            now);
        sb88Package.AddWorkProduct(fiscalNoteSb88.Id, "coordinator", now);
        sb88Package.SetStatus(PackageStatus.Draft, now);
        await _packageRepository.AddAsync(sb88Package, cancellationToken);
    }

    private async Task SeedRelationshipsAsync(IReadOnlyList<WorkTask> tasks, CancellationToken cancellationToken)
    {
        WorkTask analyzeHb1200 = tasks[0];
        WorkTask fiscalNoteSb88 = tasks[1];
        WorkTask billAnalysis = tasks[2];
        WorkTask fiscalEstimate = tasks[4];
        WorkTask hearingReport = tasks[5];

        await _relationshipRepository.AddAsync(
            WorkItemRelationship.Create(analyzeHb1200.Id, billAnalysis.Id, WorkItemRelationshipType.LegislativeIdentifier, "coordinator", DateTimeOffset.UtcNow),
            cancellationToken);
        await _relationshipRepository.AddAsync(
            WorkItemRelationship.Create(analyzeHb1200.Id, fiscalEstimate.Id, WorkItemRelationshipType.LegislativeIdentifier, "coordinator", DateTimeOffset.UtcNow),
            cancellationToken);
        await _relationshipRepository.AddAsync(
            WorkItemRelationship.Create(fiscalNoteSb88.Id, hearingReport.Id, WorkItemRelationshipType.LegislativeIdentifier, "coordinator", DateTimeOffset.UtcNow),
            cancellationToken);
    }

    private async Task SeedFiscalDataAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        await _fiscalDataRepository.AddAsync(FiscalData.Create(FiscalDataCategory.Fte, "RFA FTE - HB 1200", 2.5m, "count", "DOR fiscal system", now), cancellationToken);
        await _fiscalDataRepository.AddAsync(FiscalData.Create(FiscalDataCategory.Fte, "Tax Systems FTE - SB 88", 4.0m, "count", "DOR fiscal system", now), cancellationToken);
        await _fiscalDataRepository.AddAsync(FiscalData.Create(FiscalDataCategory.CostRule, "Software licensing cost rule", 250000m, "dollars", "DOR fiscal system", now), cancellationToken);
        await _fiscalDataRepository.AddAsync(FiscalData.Create(FiscalDataCategory.CostRule, "Training cost rule", 50000m, "dollars", "DOR fiscal system", now), cancellationToken);
        await _fiscalDataRepository.AddAsync(FiscalData.Create(FiscalDataCategory.RevenueFund, "General Fund revenue - HB 1200", 12400000m, "dollars", "DOR fiscal system", now), cancellationToken);
        await _fiscalDataRepository.AddAsync(FiscalData.Create(FiscalDataCategory.RevenueFund, "Working families credit fund", 85000000m, "dollars", "DOR fiscal system", now), cancellationToken);
        await _fiscalDataRepository.AddAsync(FiscalData.Create(FiscalDataCategory.RevenueSource, "Sales tax revenue - SB 88", 320000000m, "dollars", "DOR fiscal system", now), cancellationToken);
        await _fiscalDataRepository.AddAsync(FiscalData.Create(FiscalDataCategory.RevenueSource, "Corporate income tax - SB 101", 145000000m, "dollars", "DOR fiscal system", now), cancellationToken);
    }

    private async Task SeedDemographicsAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        await _demographicDataRepository.AddAsync(DemographicData.Create("2025-2026", "Population", 7_900_000m, 2026, now), cancellationToken);
        await _demographicDataRepository.AddAsync(DemographicData.Create("2025-2026", "Households", 3_100_000m, 2026, now), cancellationToken);
        await _demographicDataRepository.AddAsync(DemographicData.Create("2025-2026", "Businesses", 620_000m, 2026, now), cancellationToken);
        await _demographicDataRepository.AddAsync(DemographicData.Create("2025-2026", "Median household income", 78_500m, 2026, now), cancellationToken);
        await _demographicDataRepository.AddAsync(DemographicData.Create("2027-2028", "Population", 8_100_000m, 2028, now), cancellationToken);
        await _demographicDataRepository.AddAsync(DemographicData.Create("2027-2028", "Households", 3_200_000m, 2028, now), cancellationToken);
        await _demographicDataRepository.AddAsync(DemographicData.Create("2027-2028", "Businesses", 650_000m, 2028, now), cancellationToken);
        await _demographicDataRepository.AddAsync(DemographicData.Create("2027-2028", "Median household income", 82_000m, 2028, now), cancellationToken);
    }

    private async Task<IReadOnlyList<DocumentTemplate>> SeedTemplatesAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        List<DocumentTemplate> templates = new();

        DocumentTemplate fiscalNote = DocumentTemplate.Create(
            "Fiscal Note", WorkItemType.FiscalNote,
            "Fiscal Note\nBill: {{BillNumber}}\nTitle: {{Title}}\nEstimated Impact: {{Impact}}", true, now);
        await _templateRepository.AddAsync(fiscalNote, cancellationToken);
        templates.Add(fiscalNote);

        DocumentTemplate billAnalysis = DocumentTemplate.Create(
            "Bill Analysis", WorkItemType.BillAnalysis,
            "Bill Analysis\nBill: {{BillNumber}}\nSummary: {{Summary}}", true, now);
        await _templateRepository.AddAsync(billAnalysis, cancellationToken);
        templates.Add(billAnalysis);

        DocumentTemplate hearingReport = DocumentTemplate.Create(
            "Hearing Report", WorkItemType.WorkProduct,
            "Hearing Report\nBill: {{BillNumber}}\nDate: {{Date}}\nFindings: {{Findings}}", true, now);
        await _templateRepository.AddAsync(hearingReport, cancellationToken);
        templates.Add(hearingReport);

        DocumentTemplate dataRequest = DocumentTemplate.Create(
            "Data Request", WorkItemType.DataRequest,
            "Data Request\nRequested By: {{RequestedBy}}\nData Needed: {{DataNeeded}}", false, now);
        await _templateRepository.AddAsync(dataRequest, cancellationToken);
        templates.Add(dataRequest);

        return templates;
    }

    private async Task SeedNotificationsAsync(IReadOnlyList<WorkTask> tasks, DateTimeOffset now, CancellationToken cancellationToken)
    {
        WorkTask analyzeHb1200 = tasks[0];
        WorkTask fiscalEstimate = tasks[4];

        await _notificationRepository.AddAsync(Notification.Create("jdoe", "You have been assigned to analyze HB 1200 fiscal impact.", NotificationType.Assignment, now, NotificationChannel.InApp, NotificationTrigger.Assignment), cancellationToken);
        await _notificationRepository.AddAsync(Notification.Create("asmith", "Fiscal estimate for HB 1200 is due soon.", NotificationType.General, now, NotificationChannel.InApp, NotificationTrigger.General), cancellationToken);
        await _notificationRepository.AddAsync(Notification.Create("admin", "HB 1200 fiscal note has been submitted for review.", NotificationType.BillChange, now, NotificationChannel.InApp, NotificationTrigger.ReviewRequested), cancellationToken);
        await _notificationRepository.AddAsync(Notification.Create("coordinator", "Review requested for HB 1200 bill analysis.", NotificationType.General, now, NotificationChannel.InApp, NotificationTrigger.ReviewRequested), cancellationToken);
        await _notificationRepository.AddAsync(Notification.Create("snguyen", "HB 1201 fiscal note approved.", NotificationType.General, now, NotificationChannel.InApp, NotificationTrigger.ReviewCompleted), cancellationToken);
        await _notificationRepository.AddAsync(Notification.Create("kpatel", "Executive review started for SB 88 fiscal estimate.", NotificationType.General, now, NotificationChannel.InApp, NotificationTrigger.ReviewRequested), cancellationToken);
    }

    private async Task SeedCorrespondenceAsync(IReadOnlyList<Bill> bills, IReadOnlyList<WorkTask> tasks, DateTimeOffset now, CancellationToken cancellationToken)
    {
        Bill hb1200 = bills[0];
        Bill sb88 = bills[1];
        WorkTask analyzeHb1200 = tasks[0];

        await _correspondenceRepository.AddAsync(Correspondence.Create(hb1200.Id, analyzeHb1200.Id, "legislative@example.gov", "HB 1200 fiscal impact request", "Please provide the fiscal impact estimate for HB 1200.", "jdoe", now), cancellationToken);
        await _correspondenceRepository.AddAsync(Correspondence.Create(sb88.Id, null, "budget.office@example.gov", "SB 88 sales tax data request", "Requesting sales tax administration data for SB 88.", "asmith", now), cancellationToken);
        await _correspondenceRepository.AddAsync(Correspondence.Create(hb1200.Id, null, "fiscal.committee@example.gov", "HB 1200 hearing scheduling", "Confirming the hearing date for HB 1200.", "coordinator", now), cancellationToken);
    }

    private async Task SeedImplementationTasksAsync(IReadOnlyList<Bill> bills, DateTimeOffset now, CancellationToken cancellationToken)
    {
        Bill hb1200 = bills[0];
        Bill sb88 = bills[1];
        Bill hb1300 = bills[4];

        ImplementationTask hb1200Task = ImplementationTask.Create(hb1200.Id, "Update tax system for HB 1200", "asmith", "Tax Systems", "Update system configuration for HB 1200.", new DateOnly(2026, 12, 1), "coordinator", now);
        await _implementationTaskRepository.AddAsync(hb1200Task, cancellationToken);

        ImplementationTask sb88Task = ImplementationTask.Create(sb88.Id, "Configure sales tax reporting for SB 88", "bchen", "Tax Systems", "Configure sales tax reporting and remittance.", new DateOnly(2026, 12, 15), "coordinator", now);
        await _implementationTaskRepository.AddAsync(sb88Task, cancellationToken);

        ImplementationTask hb1300Task = ImplementationTask.Create(hb1300.Id, "Update business license forms for HB 1300", "cduke", "Business Licensing", "Update business license application forms.", new DateOnly(2026, 11, 20), "coordinator", now);
        await _implementationTaskRepository.AddAsync(hb1300Task, cancellationToken);
    }

    private async Task SeedExecutiveDiscussionsAsync(IReadOnlyList<Bill> bills, IReadOnlyList<WorkTask> tasks, DateTimeOffset now, CancellationToken cancellationToken)
    {
        Bill hb1200 = bills[0];
        Bill sb88 = bills[1];
        WorkTask fiscalEstimate = tasks[4];

        ExecutiveDiscussion hb1200Discussion = ExecutiveDiscussion.Create(hb1200.Id, "kpatel", "What is the projected revenue impact of HB 1200?", fiscalEstimate.Id, now);
        hb1200Discussion.PostAnswer("The projected impact is $12.4M over the biennium.", "asmith", now);
        await _executiveDiscussionRepository.AddAsync(hb1200Discussion, cancellationToken);

        await _executiveDiscussionRepository.AddAsync(ExecutiveDiscussion.Create(sb88.Id, "kpatel", "When will the sales tax modernization be complete?", null, now), cancellationToken);
    }

    private async Task SeedCustomReportsAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        await _customReportRepository.AddAsync(CustomReport.Create("High-priority fiscal notes", "jdoe", "type=FiscalNote priority=High", now), cancellationToken);
        await _customReportRepository.AddAsync(CustomReport.Create("Items under executive review", "asmith", "executiveReview=true", now), cancellationToken);
        await _customReportRepository.AddAsync(CustomReport.Create("Overdue work items", "coordinator", "dueDate<today status!=Completed", now), cancellationToken);
    }

    private async Task SeedAccessRestrictionsAsync(IReadOnlyList<WorkTask> tasks, DateTimeOffset now, CancellationToken cancellationToken)
    {
        WorkTask fiscalEstimate = tasks[4];

        await _accessRestrictionRepository.AddAsync(AccessRestriction.Create(fiscalEstimate.Id, "Content", UserRole.ReadOnly, "Confidential fiscal estimate content.", "coordinator", now), cancellationToken);
        await _accessRestrictionRepository.AddAsync(AccessRestriction.Create(fiscalEstimate.Id, "Attachments", UserRole.ReadOnly, "Confidential attachments.", "coordinator", now), cancellationToken);
    }

    private async Task SeedFiscalWorkPapersAsync(IReadOnlyList<WorkTask> tasks, DateTimeOffset now, CancellationToken cancellationToken)
    {
        WorkTask fiscalEstimate = tasks[4];
        WorkTask analyzeHb1200 = tasks[0];

        await _fiscalWorkPaperRepository.AddAsync(FiscalWorkPaper.Create(fiscalEstimate.Id, "HB 1200 revenue model", "Revenue model used for the HB 1200 fiscal estimate.", "asmith", now), cancellationToken);
        await _fiscalWorkPaperRepository.AddAsync(FiscalWorkPaper.Create(analyzeHb1200.Id, "HB 1200 cost assumptions", "Cost assumptions for the HB 1200 fiscal impact analysis.", "jdoe", now), cancellationToken);
    }

    private async Task SeedExpenseEstimateElementsAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        await _expenseEstimateRepository.AddAsync(ExpenseEstimateElement.Create("Software licensing", ExpenseEstimateElementKind.GoodsServices, 250000m, new DateOnly(2026, 7, 1), "lgonzalez", now), cancellationToken);
        await _expenseEstimateRepository.AddAsync(ExpenseEstimateElement.Create("Staff training", ExpenseEstimateElementKind.GoodsServices, 50000m, new DateOnly(2026, 7, 1), "lgonzalez", now), cancellationToken);
        await _expenseEstimateRepository.AddAsync(ExpenseEstimateElement.Create("Analyst salary percentage", ExpenseEstimateElementKind.SalaryPercentage, 0.25m, new DateOnly(2026, 7, 1), "lgonzalez", now), cancellationToken);
    }

    private async Task SeedBillFiscalNoteLinksAsync(IReadOnlyList<Bill> bills, IReadOnlyList<WorkTask> tasks, DateTimeOffset now, CancellationToken cancellationToken)
    {
        Bill hb1200 = bills[0];
        Bill sb88 = bills[1];
        WorkTask analyzeHb1200 = tasks[0];
        WorkTask fiscalNoteSb88 = tasks[1];

        await _billFiscalNoteLinkRepository.AddAsync(BillFiscalNoteLink.Create(hb1200.Id, analyzeHb1200.Id, "coordinator", now), cancellationToken);
        await _billFiscalNoteLinkRepository.AddAsync(BillFiscalNoteLink.Create(sb88.Id, fiscalNoteSb88.Id, "coordinator", now), cancellationToken);
    }

    private async Task SeedEmailDispatchesAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        await _emailDispatchRepository.AddAsync(EmailDispatch.Create("legislative@example.gov", "HB 1200 fiscal note", "Please find the HB 1200 fiscal note attached.", "jdoe", now), cancellationToken);
        await _emailDispatchRepository.AddAsync(EmailDispatch.Create("budget.office@example.gov", "SB 88 sales tax data", "Sales tax administration data for SB 88.", "asmith", now), cancellationToken);
    }

    private async Task SeedGeneratedDocumentsAsync(IReadOnlyList<WorkTask> tasks, IReadOnlyList<DocumentTemplate> templates, DateTimeOffset now, CancellationToken cancellationToken)
    {
        WorkTask analyzeHb1200 = tasks[0];
        DocumentTemplate fiscalNote = templates[0];

        await _generatedDocumentRepository.AddAsync(GeneratedDocument.Create(analyzeHb1200.Id, fiscalNote.Id, "HB 1200 Fiscal Note", "Fiscal Note\nBill: HB 1200\nEstimated Impact: $12.4M", now, "jdoe"), cancellationToken);
    }

    private async Task SeedMigrationAsync(IReadOnlyList<WorkTask> tasks, DateTimeOffset now, CancellationToken cancellationToken)
    {
        WorkTask analyzeHb1200 = tasks[0];
        WorkTask fiscalNoteSb88 = tasks[1];

        LegacyMigrationBatch batch = LegacyMigrationBatch.Create("Legacy LTS export 2025", "coordinator", now);
        batch.AddRecord("LEG-001", analyzeHb1200.Id, MigrationRecordStatus.Imported, null);
        batch.AddRecord("LEG-002", fiscalNoteSb88.Id, MigrationRecordStatus.Imported, null);
        batch.AddRecord("LEG-003", null, MigrationRecordStatus.Failed, "Duplicate source key.");
        batch.Complete();
        await _legacyMigrationRepository.AddBatchAsync(batch, cancellationToken);
    }

    private WorkTask CreateTask(
        WorkItemType type,
        string title,
        string? description,
        DateOnly? dueDate,
        TaskPriority priority,
        WorkTaskStatus status,
        string? owner,
        DateTimeOffset now)
    {
        WorkItemIdentifier identifier = _identifierGenerator.Generate(type);
        SourceTraceReference sourceTrace = SourceTraceReference.Create(
            "US-1.3.1",
            "B.COM.11",
            "B",
            "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System");

        return WorkTask.Create(identifier, type, title, description, dueDate, priority, status, owner, sourceTrace, now);
    }

    private static void SubmitForReview(WorkTask task, IReadOnlyList<string> reviewers, DateTimeOffset at)
    {
        task.SubmitForReview(reviewers, at);
    }

    private static void SubmitAndApprove(WorkTask task, IReadOnlyList<string> reviewers, DateTimeOffset at)
    {
        task.SubmitForReview(reviewers, at);
        foreach (string reviewer in reviewers)
        {
            task.RecordReview(reviewer, WorkflowDecision.Approved, "Approved.", at);
        }
    }

    private static void StartExecutiveReview(WorkTask task, IReadOnlyList<string> reviewers, DateTimeOffset at)
    {
        task.StartExecutiveReview(reviewers, at);
    }

    private static void CompleteExecutiveReview(WorkTask task, IReadOnlyList<string> reviewers, DateTimeOffset at)
    {
        task.StartExecutiveReview(reviewers, at);
        foreach (string reviewer in reviewers)
        {
            task.BeginExecutiveReviewStep(reviewer, at);
            task.CompleteExecutiveReviewStep(reviewer, "Reviewed.", at);
        }
    }
}
