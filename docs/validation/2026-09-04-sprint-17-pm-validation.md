# Sprint 17 - Production Hardening: PostgreSQL Persistence Expansion - PM Validation Report

Date: 2026-09-04

Sprint: Sprint 17

Status: Complete

## 1. Scope Summary

Sprint 17 expanded the PostgreSQL persistence foundation to the package, work-item relationship, and notification aggregates, proving the EF Core pattern for aggregates with owned collections and mutation flows, and adding an explicit `UpdateAsync` to the package repository contract so mutations persist.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 17 Evidence |
| --- | --- | --- |
| Persistence expansion (package aggregate) | Production hardening | `LtsDbContext` maps `Package` with owned `PackageMember`/`PackageRecipient`; `EfPackageRepository` implements `IPackageRepository`; `AddPackageRelationshipNotification` and `AddPackageMemberShadowKey` migrations. |
| Persistence expansion (relationship aggregate) | Production hardening | `LtsDbContext` maps `WorkItemRelationship`; `EfWorkItemRelationshipRepository` implements `IWorkItemRelationshipRepository`. |
| Persistence expansion (notification aggregate) | Production hardening | `LtsDbContext` maps `Notification`; `EfNotificationRepository` implements `INotificationRepository`. |
| Mutation persistence | Production hardening | `IPackageRepository.UpdateAsync` added and called after package mutations; implemented in both adapters. |
| TR-601 versioned RESTful API surface | TR-601 | Existing `/api/v1` endpoints unchanged; persistence is an infrastructure concern behind the same contracts. |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `TreatWarningsAsErrors` enforced. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | 147 domain tests + 6 infrastructure integration tests pass (0 failed). |

## 3. Acceptance Evidence

- `LtsDbContext` maps `Package` (with owned `PackageMember`/`PackageRecipient`), `WorkItemRelationship`, and `Notification`; migrations generated.
- `EfPackageRepository`, `EfWorkItemRelationshipRepository`, and `EfNotificationRepository` implement the existing contracts.
- `IPackageRepository.UpdateAsync` added and called after package mutations; both the in-memory and EF adapters implement it.
- DI registers the EF repositories when a connection string is present; otherwise it falls back to the in-memory adapter.
- A package (with members/recipients), a relationship, and a notification persist across a container restart.
- Integration tests against a real PostgreSQL instance pass (and skip cleanly when the database is unreachable).

## 4. Verification Results

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test` passed: 147 domain tests + 6 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`); migrations applied at startup.
- Persistence smoke against `http://localhost:5088`: created a package with a member, a relationship, and a notification; restarted the container; confirmed the package (with members), relationship, and notification persisted in PostgreSQL.

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Infrastructure/Persistence/LtsDbContext.cs` — entity configurations for `Package`, `WorkItemRelationship`, and `Notification`.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/EfPackageRepository.cs`, `EfWorkItemRelationshipRepository.cs`, `EfNotificationRepository.cs`.
- `src/Legislature.TrackingSystem.Infrastructure/Persistence/Migrations/AddPackageRelationshipNotification*`, `AddPackageMemberShadowKey*`.
- `src/Legislature.TrackingSystem.Application/WorkItems/IPackageRepository.cs` — added `UpdateAsync`.
- `src/Legislature.TrackingSystem.Application/WorkItems/PackageService.cs` — calls `UpdateAsync` after each mutation.
- `src/Legislature.TrackingSystem.Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs` — DI wiring.
- `tests/Legislature.TrackingSystem.Infrastructure.Tests/EfPersistenceTests.cs` — package, relationship, notification round-trip tests.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-17-traceability.md`, `docs/validation/2026-09-04-sprint-17-pm-validation.md`, `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- The remaining aggregates (work tasks, document templates, custom reports, fiscal data, correspondence, implementation tasks, executive discussions, etc.) stay on the interim in-memory adapter and are migrated in follow-up sprints. The in-memory adapter remains the fallback when no connection string is present.
- Real Entra/OpenID Connect (replacing the POC symmetric-key JWT) and real external connectors remain deferred follow-up items.

## 7. PM Validation Decision

**Decision: Completed**

The Sprint 17 scope is implemented, verified, and documented. The package, work-item relationship, and notification aggregates now persist in PostgreSQL with EF Core migrations, and the mutation-persistence flow is proven. The remaining aggregates are a documented follow-up (Sprint 18+).

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | deepseek-v4-flash:0731-cloud |
| Token count | Unavailable (telemetry not exposed in this environment) |
| Cost | Unavailable (no billing source exposed; not estimated) |
| Elapsed time | Unavailable (not captured) |
| Tool/run counts | Unavailable (not captured) |
| Verification runs | `dotnet format` (1), `dotnet build` (1), `dotnet test` (2 projects), container rebuild + smoke (1) |

> AI Metrics note: token/cost telemetry is not available in this execution environment. Values are marked unavailable rather than estimated, per the project's AI Metrics policy.
