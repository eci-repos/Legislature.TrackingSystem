# Sprint 12 - Phase 4: Fiscal Analysis, Financial Inputs, and Productivity Integration Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 12 implements Phase 4: fiscal data integration (F7.1), RFA supporting analysis (F7.2), Microsoft 365 productivity integration (F8.1), and expense estimate management (F14.1), per `docs/backlog/phase-04-fiscal-analysis-financial-inputs-and-productivity-integration.md`.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 12 Evidence | Artifact |
| --- | --- | --- | --- |
| US-7.1.1 retrieve/calculate/update fiscal data | B.COM.36 | `FiscalData` (FTE, cost rule, revenue fund/source) + `FiscalDataService` list/upsert/calculate fiscal note. | `Domain/WorkItems/FiscalData.cs`, `Application/WorkItems/FiscalDataService.cs` |
| US-7.1.2 supporting documentation and historical research | B.COM.37 | `FiscalWorkPaper` stored with work products and retrievable. | `Domain/WorkItems/FiscalWorkPaper.cs`, `FiscalWorkPaperService.cs` |
| US-7.2.1 flag budget bills and compare with fiscal notes | B.RFA.07 | `Bill.IsBudgetBill` + `BudgetBillService` flag/list/link fiscal notes. | `Domain/WorkItems/Bill.cs`, `BillFiscalNoteLink.cs`, `BudgetBillService.cs` |
| US-7.2.2 demographic data by session | B.RFA.08 | `DemographicData` stored/retrieved by legislative session. | `Domain/WorkItems/DemographicData.cs`, `DemographicDataService.cs` |
| US-8.1.1 Microsoft 365 integration boundaries | B.COM.30 | `ProductivityIntegrationService` populates templates and records email dispatches. | `Domain/WorkItems/EmailDispatch.cs`, `ProductivityIntegrationService.cs` |
| US-14.1.1 expense-estimate elements | B.BGT.01 | `ExpenseEstimateElement` (goods/services, salary percentage) + `ExpenseEstimateService` upsert/list/calculate. | `Domain/WorkItems/ExpenseEstimateElement.cs`, `ExpenseEstimateService.cs` |

## Technical Quality Trace

| Requirement | Source | Sprint 12 Evidence | Artifact |
| --- | --- | --- | --- |
| Automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `Directory.Build.props` applies `TreatWarningsAsErrors`. | `dotnet format`, `Directory.Build.props` |
| Automated tests asserting acceptance criteria | TR-902 | 119 tests pass (0 failed); 15 new tests cover fiscal data, work papers, budget bills, demographics, productivity, and expense estimates. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/FiscalDataServiceTests.cs`, `FiscalWorkPaperServiceTests.cs`, `BudgetBillServiceTests.cs`, `DemographicDataServiceTests.cs`, `ProductivityIntegrationServiceTests.cs`, `ExpenseEstimateServiceTests.cs` |
| Versioned RESTful API surface | TR-601 | New `/api/v1` endpoints for fiscal data, work papers, budget bills, demographics, productivity, and expense estimates. | `src/Legislature.TrackingSystem.Web/Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| Fiscal data can be retrieved, calculated, and updated; supporting work papers are stored and retrievable with historical products | Implemented |
| Budget bills can be flagged and compared with associated fiscal notes; demographic data is stored and retrieved by session | Implemented |
| Productivity integration boundaries populate templates and record email dispatches; expense-estimate elements are entered, updated, and incorporated into calculations | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 12 continues the interim in-memory repository adapter; PostgreSQL persistence and EF Core migrations remain deferred.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 119 tests (0 failed, 0 skipped).
- Container smoke checks (fiscal/budget/demographics/productivity API and UI render) were verified against `http://localhost:5088` after `docker compose up -d --build --force-recreate web`.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 12 defects; the standard AGENTS.md commands work on a normal developer/container host.
