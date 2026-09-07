# Sprint 18 - Production Hardening: Complete PostgreSQL Persistence - PM Validation Report

Date: 2026-09-05

Sprint: Sprint 18

Status: Complete

## 1. Scope Summary

Sprint 18 completed the PostgreSQL persistence work started after Sprint 17. The sprint repaired the incomplete EF Core model state, added the missing migrations, migrated all current repository-backed aggregates to EF Core when a connection string is present, and expanded PostgreSQL integration tests to cover the full migrated persistence surface.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 18 Evidence |
| --- | --- | --- |
| Complete repository-backed aggregate persistence | Production hardening | EF Core adapters now exist for all current repository contracts, with in-memory adapters retained only as the no-connection fallback. |
| Work-task persistence | Production hardening; Phase 1-6 work-product scope | `WorkTask` persists identifiers, source trace, categorization, workflow, executive review, content metadata, assignments, reviews, steps, attachments, comments, audit entries, and version history. |
| Template/report/document persistence | US-4.3.x, US-6.2.2 | Document templates, custom reports, correspondence, implementation tasks/shared documents, and executive discussions are persisted through EF Core repositories. |
| Fiscal/security/migration persistence | US-7.x, US-8.1.1, US-9.1.2, US-10.1.1, US-14.1.1 | Fiscal data, work papers, demographic data, email dispatches, expense-estimate elements, bill/fiscal-note links, access restrictions, and migration batches/records are persisted through EF Core repositories. |
| TR-601 versioned RESTful API surface | TR-601 | Existing `/api/v1` endpoints continue to use application contracts; persistence changed behind the infrastructure boundary. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | PostgreSQL integration tests expanded to 20 tests and pass against the local Docker Compose database. |

## 3. Acceptance Evidence

- `LtsDbContext` maps all current repository-backed aggregates and their owned collections.
- `LtsDbContextFactory` supports repeatable EF Core migration generation.
- Migrations generated: `AddRemainingAggregatePersistence`, `AddFiscalSecurityMigrationPersistence`, and `AddWorkTaskPersistence`.
- EF repositories added for work tasks, document templates, custom reports, correspondence, implementation tasks, executive discussions, fiscal data, fiscal work papers, demographic data, email dispatches, expense-estimate elements, bill/fiscal-note links, access restrictions, and legacy migration batches.
- Connection-string-aware DI selects EF repositories when PostgreSQL is configured and in-memory adapters when it is not.
- PostgreSQL integration tests prove round-trips for all migrated repositories, including the work-task owned collection graph.

## 4. Verification Results

- `dotnet ef migrations has-pending-model-changes --project src/Legislature.TrackingSystem.Infrastructure/Legislature.TrackingSystem.Infrastructure.csproj --startup-project src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj --context LtsDbContext` passed: no pending model changes.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 147 domain tests + 20 infrastructure integration tests (0 failed, 0 skipped).
- `docker compose up -d --build --force-recreate web` rebuilt and recreated the web container; PostgreSQL remained healthy and the web container started.
- `docker compose restart web` completed; `http://localhost:5088/api/v1/readiness` returned HTTP 200 after restart.
- PostgreSQL table smoke confirmed Sprint 18 tables exist, and migration history includes all five migrations through `AddWorkTaskPersistence`.

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Domain/WorkItems/WorkTask.cs` — EF-compatible materialization support and private reviewer-key persistence bridge.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/LtsDbContext.cs` — complete EF Core model mappings.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/LtsDbContextFactory.cs` — design-time DbContext factory.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/Ef*Repository.cs` — EF Core adapters for all current repository contracts.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/Migrations/20260905203502_AddRemainingAggregatePersistence*`.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/Migrations/20260905203834_AddFiscalSecurityMigrationPersistence*`.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/Migrations/20260905204414_AddWorkTaskPersistence*`.
- `src/Legislature.TrackingSystem.Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs` — PostgreSQL/in-memory registration selection.
- `tests/Legislature.TrackingSystem.Infrastructure.Tests/EfPersistenceTests.cs` — expanded persistence integration coverage.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-18-traceability.md`, `docs/validation/2026-09-05-sprint-18-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- Generated documents remain generated-on-demand DTOs and are not stored as a repository-backed aggregate.
- The in-memory adapters remain as a deliberate no-connection fallback; they are no longer the primary path when PostgreSQL is configured.
- Real Entra/OpenID Connect, external legislative/fiscal connectors, Microsoft 365/SharePoint/Graph integration, registry publishing, image scanning, deployed Terraform infrastructure, GuardDuty/Security Hub, and cross-region DR remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local Codex task context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 18 is implemented, verified, and documented. PostgreSQL persistence now covers all current repository-backed aggregates, the model and migrations are aligned, and the expanded integration suite passes against the local PostgreSQL instance.

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | GPT-5 Codex |
| Token count | Unavailable (telemetry not exposed in this environment) |
| Cost | Unavailable (no billing source exposed; not estimated) |
| Elapsed time | Unavailable (not captured) |
| Tool/run counts | Unavailable (not captured) |
| Verification runs | `dotnet ef migrations has-pending-model-changes` (1), `dotnet format` (1), `dotnet build` (multiple during repair; final pass recorded), `dotnet test` (final full solution pass), container rebuild/restart smoke (1) |

> AI Metrics note: token/cost telemetry is not available in this execution environment. Values are marked unavailable rather than estimated, per the project's AI Metrics policy.
