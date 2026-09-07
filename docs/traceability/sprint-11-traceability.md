# Sprint 11 - Phase 3: Legislative Data Lifecycle, Search, and Reporting Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 11 implements Phase 3: external legislative updates (F5.1), version history and comparison (F5.2), enterprise search (F6.1), and standard and ad hoc reporting (F6.2), per `docs/backlog/phase-03-legislative-data-lifecycle-search-and-reporting.md`.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 11 Evidence | Artifact |
| --- | --- | --- | --- |
| US-5.1.1 external bill language updates | B.COM.31 | `Bill` + `LegislativeIngestionService.IngestBillUpdateAsync`; lifecycle changes (HB to SHB) represented via version labels. | `Domain/WorkItems/Bill.cs`, `Application/WorkItems/LegislativeIngestionService.cs` |
| US-5.1.2 bill status changes | B.COM.32 | `BillStatus` + `IngestBillStatusAsync` updates corresponding DOR records. | `Domain/WorkItems/BillStatus.cs`, `LegislativeIngestionService.cs` |
| US-5.1.3 amendments imported and tracked | B.LNP.01 | `BillAmendment` + `IngestAmendmentAsync`. | `Domain/WorkItems/BillAmendment.cs`, `LegislativeIngestionService.cs` |
| US-5.2.1 bill version history | B.COM.33 | `BillVersion` snapshots retained on each update. | `Domain/WorkItems/BillVersion.cs`, `Bill.cs` |
| US-5.2.2 work-product version history | B.COM.35 | `WorkTaskVersion` captured on each `SetContent`. | `Domain/WorkItems/WorkTaskVersion.cs`, `WorkTask.cs` |
| US-5.2.3 compare bill versions | B.LNP.02 | `VersionService.CompareBillVersionsAsync` reports added/removed lines. | `Application/WorkItems/VersionService.cs` |
| US-5.2.4 follow bills across years within a biennium | B.COM.43 | Bills keyed by (number, biennium); `GetBillHistoryAsync` returns across biennia. | `IBillRepository.FindByNumberAndBienniumAsync`, `VersionService.cs` |
| US-6.1.1 enterprise search | B.COM.09 | `SearchService.SearchAsync` across work products and bills. | `Application/WorkItems/SearchService.cs` |
| US-6.2.1 standard reports | B.COM.38 | `ReportingService.RunStandardReportAsync` (outstanding fiscal tasks, work products by type). | `Application/WorkItems/ReportingService.cs` |
| US-6.2.2 custom queries and reports | B.COM.39 | `CustomReport` + create/list/run custom reports. | `Domain/WorkItems/CustomReport.cs`, `ReportingService.cs` |
| US-6.2.3 governed database access | B.COM.40 | Reporting reads through the governed application service/repository boundary (no direct data access). | `IReportingService`, `IWorkTaskRepository` |
| US-6.2.4 extract work products | B.COM.34 | `ReportingService.ExtractWorkProductAsync` (text/html). | `Application/WorkItems/ReportingService.cs` |

## Technical Quality Trace

| Requirement | Source | Sprint 11 Evidence | Artifact |
| --- | --- | --- | --- |
| Automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `Directory.Build.props` applies `TreatWarningsAsErrors`. | `dotnet format`, `Directory.Build.props` |
| Automated tests asserting acceptance criteria | TR-902 | 104 tests pass (0 failed); 16 new tests cover ingestion, versions, search, and reporting. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/LegislativeIngestionServiceTests.cs`, `VersionServiceTests.cs`, `SearchServiceTests.cs`, `ReportingServiceTests.cs` |
| Versioned RESTful API surface | TR-601 | New `/api/v1` endpoints for bills, versions, search, reports, and extracts. | `src/Legislature.TrackingSystem.Web/Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| External bill language, status, and amendments can be ingested and applied to corresponding bills; prior versions are retained and comparable | Implemented |
| Enterprise search returns work products and bills; standard and custom reports run; work products can be extracted | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 11 continues the interim in-memory repository adapter; PostgreSQL persistence and EF Core migrations remain deferred.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 104 tests (0 failed, 0 skipped).
- Container smoke checks (bills/versions/search/reports/extract API and UI render) were verified against `http://localhost:5088` after `docker compose up -d --build --force-recreate web`.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 11 defects; the standard AGENTS.md commands work on a normal developer/container host.
