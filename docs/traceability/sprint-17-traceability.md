# Sprint 17 - Production Hardening: PostgreSQL Persistence Expansion Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 17 expands the PostgreSQL persistence foundation to the package, work-item relationship, and notification aggregates, proving the EF Core pattern for aggregates with owned collections and mutation flows, and adding an explicit `UpdateAsync` to the package repository contract so mutations persist.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 17 Evidence | Artifact |
| --- | --- | --- | --- |
| Persistence expansion (package aggregate) | Production hardening | `LtsDbContext` maps `Package` with owned `PackageMember`/`PackageRecipient`; `EfPackageRepository` implements `IPackageRepository`; `AddPackageRelationshipNotification` migration. | `Infrastructure/Persistence/LtsDbContext.cs`, `EfPackageRepository.cs`, `Persistence/Migrations/` |
| Persistence expansion (relationship aggregate) | Production hardening | `LtsDbContext` maps `WorkItemRelationship`; `EfWorkItemRelationshipRepository` implements `IWorkItemRelationshipRepository`. | `Infrastructure/Persistence/EfWorkItemRelationshipRepository.cs` |
| Persistence expansion (notification aggregate) | Production hardening | `LtsDbContext` maps `Notification`; `EfNotificationRepository` implements `INotificationRepository`. | `Infrastructure/Persistence/EfNotificationRepository.cs` |
| Mutation persistence | Production hardening | `IPackageRepository.UpdateAsync` added and called after package mutations; implemented in both adapters. | `Application/WorkItems/IPackageRepository.cs`, `PackageService.cs`, `Infrastructure/Persistence/EfPackageRepository.cs` |
| TR-601 versioned RESTful API surface | TR-601 | Existing `/api/v1` endpoints unchanged; persistence is an infrastructure concern behind the same contracts. | `Infrastructure/Persistence/*.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `TreatWarningsAsErrors` enforced. | `dotnet format`, `Directory.Build.props` |
| TR-902 automated tests asserting acceptance criteria | TR-902 | 147 domain tests + 5 infrastructure integration tests pass (0 failed). | `tests/.../EfPersistenceTests.cs` |

## Technical Quality Trace

| Requirement | Source | Sprint 17 Evidence | Artifact |
| --- | --- | --- | --- |
| Owned collections persisted | Production hardening | `PackageMember`/`PackageRecipient` mapped as owned collections of `Package`. | `Infrastructure/Persistence/LtsDbContext.cs` |
| Mutation flows persist | Production hardening | `PackageService` calls `UpdateAsync` after each mutation; EF adapter flushes tracked changes. | `Application/WorkItems/PackageService.cs`, `EfPackageRepository.cs` |
| DI prefers EF Core when a connection string is present | Production hardening | `AddLtsInfrastructure(connectionString)` registers the three EF repositories (scoped) or falls back to in-memory. | `Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs` |
| Restart persistence | Production hardening | A package (with members), a relationship, and a notification persisted across a container restart. | Container smoke (see Verification) |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| `LtsDbContext` maps `Package`, `WorkItemRelationship`, and `Notification`; migration generated | Implemented |
| `EfPackageRepository`, `EfWorkItemRelationshipRepository`, and `EfNotificationRepository` implement the existing contracts | Implemented |
| `IPackageRepository.UpdateAsync` added and called after package mutations; both adapters implement it | Implemented |
| DI registers the EF repositories when a connection string is present; falls back to in-memory otherwise | Implemented |
| A package (with members/recipients), a relationship, and a notification persist across a container restart | Verified |
| Integration tests against a real PostgreSQL instance pass (and skip cleanly when unreachable) | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 17 expands the PostgreSQL persistence foundation to the package, work-item relationship, and notification aggregates. The remaining aggregates (work tasks, document templates, custom reports, fiscal data, correspondence, implementation tasks, executive discussions, etc.) stay on the interim in-memory adapter and are migrated in follow-up sprints. The application uses EF Core when a `ConnectionStrings:Default` value is present; otherwise it falls back to the in-memory adapter.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test` passed: 147 domain tests + 5 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`); migrations applied at startup.
- Persistence smoke against `http://localhost:5088`: created a package with a member, a relationship, and a notification; restarted the container; confirmed the package (with members), relationship, and notification persisted in PostgreSQL.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 17 defects; the standard AGENTS.md commands work on a normal developer/container host.
