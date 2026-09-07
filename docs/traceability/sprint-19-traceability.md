# Sprint 19 - Production Hardening: Persist Generated Documents Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 19 closes the last remaining persistence gap from the production-hardening track by making generated documents a repository-backed aggregate. Generated documents are now persisted through EF Core when a connection string is present (with the in-memory adapter as the no-connection fallback), a query surface retrieves them, and the round-trip is proven with a PostgreSQL integration test.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 19 Evidence | Artifact |
| --- | --- | --- | --- |
| Generated documentation usable internally and externally | US-4.3.1, B.COM.24 | `GeneratedDocument` is now a persisted, repository-backed aggregate with an explicit `Create` factory and read-only identity. | `src/Legislature.TrackingSystem.Domain/WorkItems/GeneratedDocument.cs` |
| Templates/work papers auto-populated from system data | US-4.3.3, B.COM.26 | `TemplateService.GenerateDocumentAsync` persists the generated document before returning it. | `src/Legislature.TrackingSystem.Application/WorkItems/TemplateService.cs` |
| Generated documents retained and retrievable with the work product | Production hardening; US-4.3.x | `IGeneratedDocumentRepository` supports add, find-by-id, and get-for-work-item; `ListGeneratedDocumentsAsync` returns them for a work item. | `IGeneratedDocumentRepository.cs`, `ITemplateService.cs`, `TemplateService.cs` |
| TR-601 versioned RESTful API surface | TR-601 | Added `GET /api/v1/work-items/{id}/documents`; existing `/api/v1` endpoints remain behind the same application/repository contracts. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `TreatWarningsAsErrors` remains enforced. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | 2 domain tests and 1 PostgreSQL integration test cover generated-document persistence and listing. | `TemplateServiceTests.cs`, `EfPersistenceTests.cs` |

## Technical Quality Trace

| Requirement | Source | Sprint 19 Evidence | Artifact |
| --- | --- | --- | --- |
| EF model matches migrations | Production hardening | `dotnet ef migrations has-pending-model-changes` reports no model changes after the generated migration. | `Persistence/Migrations/20260906095500_AddGeneratedDocumentPersistence*`, `LtsDbContextModelSnapshot.cs` |
| Generated documents persisted | Production hardening | `GeneratedDocument` maps to the `generated_documents` table with an index on `WorkItemId`. | `LtsDbContext.cs`, `AddGeneratedDocumentPersistence` migration |
| Connection-string-aware DI | Production hardening | EF repository registered when a connection string is present; in-memory repository registered otherwise. | `InfrastructureServiceCollectionExtensions.cs` |
| UI lists generated documents | Production hardening | Templates page lists generated documents for the selected work product and refreshes after generation. | `src/Legislature.TrackingSystem.Web.Client/Pages/Templates.razor` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| `GeneratedDocument` is a repository-backed aggregate with a `Create` factory and explicit identity | Implemented |
| `IGeneratedDocumentRepository` exists with add and query operations | Implemented |
| `TemplateService.GenerateDocumentAsync` persists the generated document; a query method lists generated documents for a work item | Implemented |
| `LtsDbContext` maps `GeneratedDocument`; an EF Core migration is generated and no pending model changes remain | Verified |
| DI selects the EF repository with a connection string and the in-memory repository without one | Implemented |
| A `GET /api/v1/work-items/{id}/documents` endpoint lists generated documents for a work item | Implemented |
| PostgreSQL integration tests cover the generated-document round-trip | Verified |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 19 makes generated documents a persisted aggregate, closing the last repository-backed persistence gap identified in the Sprint 18 PM Validation report. The application uses EF Core when a `ConnectionStrings:Default` value is present; otherwise it falls back to the in-memory adapter for local/offline development.

## Verification Evidence

- `dotnet ef migrations has-pending-model-changes --project src/Legislature.TrackingSystem.Infrastructure/Legislature.TrackingSystem.Infrastructure.csproj --startup-project src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj --context LtsDbContext` passed: no pending model changes.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 149 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- PostgreSQL integration tests ran against the local Docker Compose database and passed, including the new generated-document round-trip.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. PostgreSQL integration tests run when the local Docker Compose database is reachable and skip cleanly when it is not.

## Residual Risks and Deferrals

- Real Entra/OpenID Connect, external legislative/fiscal connectors, Microsoft 365/SharePoint/Graph integration, registry publishing, image scanning, deployed Terraform infrastructure, GuardDuty/Security Hub, and cross-region DR remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local execution context.
