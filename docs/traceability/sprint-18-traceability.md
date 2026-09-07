# Sprint 18 - Production Hardening: Complete PostgreSQL Persistence Traceability

Status: complete

Last updated: 2026-09-05

## Scope

Sprint 18 completes the PostgreSQL persistence migration for current repository-backed aggregates, repairs the partially started post-Sprint-17 EF Core model, generates missing migrations, and expands PostgreSQL integration coverage.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 18 Evidence | Artifact |
| --- | --- | --- | --- |
| Complete repository-backed aggregate persistence | Production hardening | All repository contracts now have EF Core adapters when a PostgreSQL connection string is configured. | `src/Legislature.TrackingSystem.Infrastructure/Persistence/Ef*Repository.cs`, `InfrastructureServiceCollectionExtensions.cs` |
| Work-task persistence | Production hardening; US-1.x through US-6.x work-product behavior | `WorkTask` maps to PostgreSQL with owned assignments, reviews, executive review data, steps, attachments, comments, audit entries, versions, source trace, and reviewer keys. | `LtsDbContext.cs`, `EfWorkTaskRepository.cs`, `AddWorkTaskPersistence` migration |
| Document/template/report persistence | US-4.3.x, US-6.2.2 | `DocumentTemplate` and `CustomReport` persist through EF Core repositories and generated tables. | `EfDocumentTemplateRepository.cs`, `EfCustomReportRepository.cs`, `AddRemainingAggregatePersistence` migration |
| L&P/executive implementation persistence | US-11.1.1, US-12.1.x, US-13.2.2 | `Correspondence`, `ImplementationTask` with `SharedDocument`, and `ExecutiveDiscussion` persist through EF Core repositories. | `EfCorrespondenceRepository.cs`, `EfImplementationTaskRepository.cs`, `EfExecutiveDiscussionRepository.cs` |
| Fiscal, demographic, and productivity persistence | US-7.1.x, US-7.2.2, US-8.1.1, US-14.1.1 | Fiscal data, fiscal work papers, demographic data, email dispatches, expense-estimate elements, and bill/fiscal-note links persist through EF Core repositories. | `EfFiscalDataRepository.cs`, `EfFiscalWorkPaperRepository.cs`, `EfDemographicDataRepository.cs`, `EfEmailDispatchRepository.cs`, `EfExpenseEstimateRepository.cs`, `EfBillFiscalNoteLinkRepository.cs` |
| Security and migration persistence | US-9.1.2, US-10.1.1 | Access restrictions and legacy migration batches with records persist through EF Core repositories. | `EfAccessRestrictionRepository.cs`, `EfLegacyMigrationRepository.cs`, `AddFiscalSecurityMigrationPersistence` migration |
| TR-601 versioned RESTful API surface | TR-601 | Existing `/api/v1` endpoints remain behind the same application/repository contracts while infrastructure changes underneath. | `src/Legislature.TrackingSystem.Web/Program.cs`, repository registrations |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `TreatWarningsAsErrors` remains enforced. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | PostgreSQL integration coverage expanded to 20 tests with round-trips for every migrated repository and the work-task owned graph. | `tests/Legislature.TrackingSystem.Infrastructure.Tests/EfPersistenceTests.cs` |

## Technical Quality Trace

| Requirement | Source | Sprint 18 Evidence | Artifact |
| --- | --- | --- | --- |
| EF model matches migrations | Production hardening | `dotnet ef migrations has-pending-model-changes` reports no model changes after generated migrations. | `Persistence/Migrations/*`, `LtsDbContextModelSnapshot.cs` |
| Owned collections persisted | Production hardening | `WorkTask`, `Bill`, `Package`, `ImplementationTask`, and `LegacyMigrationBatch` owned collections are mapped to child tables. | `LtsDbContext.cs` |
| Design-time migration generation | Production hardening | `LtsDbContextFactory` provides a repeatable design-time DbContext for EF tooling. | `LtsDbContextFactory.cs` |
| Connection-string-aware DI | Production hardening | PostgreSQL repositories are registered when `ConnectionStrings:Default` is present; in-memory adapters remain the no-connection fallback. | `InfrastructureServiceCollectionExtensions.cs` |
| Container startup applies migrations | Production hardening | Rebuilt/recreated container starts healthy and migration history includes Sprint 18 migrations. | Docker Compose smoke, PostgreSQL migration-history query |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| `LtsDbContext` maps all current repository-backed aggregates and owned collections | Implemented |
| EF Core repository adapters exist for all repository contracts | Implemented |
| DI selects EF Core with a connection string and in-memory without one | Implemented |
| Migrations generated and no pending model changes remain | Verified |
| PostgreSQL integration tests cover all migrated repositories and `WorkTask` owned graph | Verified |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Container startup applies migrations and app remains healthy after restart | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 18 completes the PostgreSQL migration for all current repository-backed aggregates. Generated documents remain generated-on-demand DTOs. The in-memory adapters remain only as the fallback path when no connection string is configured.

## Verification Evidence

- `dotnet ef migrations has-pending-model-changes --project src/Legislature.TrackingSystem.Infrastructure/Legislature.TrackingSystem.Infrastructure.csproj --startup-project src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj --context LtsDbContext` passed: no pending model changes.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 147 domain tests + 20 infrastructure integration tests (0 failed, 0 skipped).
- `docker compose up -d --build --force-recreate web` rebuilt and recreated the web container; `lts-postgres` remained healthy and `lts-web` started.
- `http://localhost:5088/api/v1/readiness` and `http://localhost:5088/_framework/blazor.web.js` returned HTTP 200.
- PostgreSQL migration history contains `AddRemainingAggregatePersistence`, `AddFiscalSecurityMigrationPersistence`, and `AddWorkTaskPersistence`.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) for reliable local verification. PostgreSQL integration tests run when the local Docker Compose database is reachable and skip cleanly when it is not.
