# Sprint 19 - Production Hardening: Persist Generated Documents - PM Validation Report

Date: 2026-09-06

Sprint: Sprint 19

Status: Complete

## 1. Scope Summary

Sprint 19 closes the last remaining persistence gap from the production-hardening track by making generated documents a repository-backed aggregate. Generated documents are now persisted through EF Core when a connection string is present (with the in-memory adapter as the no-connection fallback), a query surface retrieves them, and the round-trip is proven with a PostgreSQL integration test.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 19 Evidence |
| --- | --- | --- |
| Generated documentation usable internally and externally | US-4.3.1, B.COM.24 | `GeneratedDocument` is now a persisted, repository-backed aggregate with an explicit `Create` factory and read-only identity. |
| Templates/work papers auto-populated from system data | US-4.3.3, B.COM.26 | `TemplateService.GenerateDocumentAsync` persists the generated document before returning it. |
| Generated documents retained and retrievable with the work product | Production hardening; US-4.3.x | `IGeneratedDocumentRepository` supports add, find-by-id, and get-for-work-item; `ListGeneratedDocumentsAsync` returns them for a work item. |
| TR-601 versioned RESTful API surface | TR-601 | Added `GET /api/v1/work-items/{id}/documents`; existing `/api/v1` endpoints remain behind the same application/repository contracts. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | 2 domain tests and 1 PostgreSQL integration test cover generated-document persistence and listing. |

## 3. Acceptance Evidence

- `GeneratedDocument` is a repository-backed aggregate with a `Create` factory and read-only identity.
- `IGeneratedDocumentRepository` exists with add, find-by-id, and get-for-work-item operations.
- `TemplateService.GenerateDocumentAsync` persists the generated document; `ListGeneratedDocumentsAsync` lists generated documents for a work item.
- `LtsDbContext` maps `GeneratedDocument` to the `generated_documents` table; the `AddGeneratedDocumentPersistence` migration is generated and no pending model changes remain.
- Connection-string-aware DI selects the EF repository when PostgreSQL is configured and the in-memory repository when it is not.
- `GET /api/v1/work-items/{id}/documents` lists generated documents for a work item.
- The Templates page lists generated documents for the selected work product and refreshes after generation.

## 4. Verification Results

- `dotnet ef migrations has-pending-model-changes --project src/Legislature.TrackingSystem.Infrastructure/Legislature.TrackingSystem.Infrastructure.csproj --startup-project src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj --context LtsDbContext` passed: no pending model changes.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 149 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- PostgreSQL integration tests ran against the local Docker Compose database and passed, including the new generated-document round-trip.

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Domain/WorkItems/GeneratedDocument.cs` — repository-backed aggregate with a `Create` factory.
- `src/Legislature.TrackingSystem.Application/WorkItems/IGeneratedDocumentRepository.cs` — persistence boundary.
- `src/Legislature.TrackingSystem.Application/WorkItems/ITemplateService.cs` and `TemplateService.cs` — persist-on-generate and list behavior.
- `src/Legislature.TrackingSystem.Infrastructure/WorkItems/InMemoryGeneratedDocumentRepository.cs` — no-connection fallback adapter.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/EfGeneratedDocumentRepository.cs` — EF Core (PostgreSQL) adapter.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/LtsDbContext.cs` — `GeneratedDocuments` DbSet and mapping.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/Migrations/20260906095500_AddGeneratedDocumentPersistence*` — migration.
- `src/Legislature.TrackingSystem.Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs` — DI registration.
- `src/Legislature.TrackingSystem.Web/Program.cs` — `GET /api/v1/work-items/{id}/documents` endpoint.
- `src/Legislature.TrackingSystem.Web.Client/Pages/Templates.razor` — generated-document listing UI.
- `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/TemplateServiceTests.cs` and `FakeGeneratedDocumentRepository.cs` — domain tests.
- `tests/Legislature.TrackingSystem.Infrastructure.Tests/EfPersistenceTests.cs` — generated-document round-trip integration test.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-19-traceability.md`, `docs/validation/2026-09-06-sprint-19-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- Real Entra/OpenID Connect, external legislative/fiscal connectors, Microsoft 365/SharePoint/Graph integration, registry publishing, image scanning, deployed Terraform infrastructure, GuardDuty/Security Hub, and cross-region DR remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local execution context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 19 is implemented, verified, and documented. Generated documents are now a persisted, repository-backed aggregate, closing the last repository-backed persistence gap from the Sprint 18 PM Validation report. The model and migration are aligned, and the expanded integration suite passes against the local PostgreSQL instance.

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | GPT-5 Codex |
| Token count | Unavailable (telemetry not exposed in this environment) |
| Cost | Unavailable (no billing source exposed; not estimated) |
| Elapsed time | Unavailable (not captured) |
| Tool/run counts | Unavailable (not captured) |
| Verification runs | `dotnet ef migrations add` (1), `dotnet ef migrations has-pending-model-changes` (1), `dotnet format` (1), `dotnet build` (multiple during repair; final pass recorded), `dotnet test` (final full solution pass), PostgreSQL integration run (1) |

> AI Metrics note: token/cost telemetry is not available in this execution environment. Values are marked unavailable rather than estimated, per the project's AI Metrics policy.
