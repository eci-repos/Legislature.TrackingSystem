# Sprint 16 - Production Hardening: PostgreSQL Persistence Foundation Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 16 establishes the PostgreSQL persistence foundation with EF Core migrations for the core bill and user aggregates, proving the full persistence pattern end-to-end. The remaining aggregates stay on the interim in-memory adapter and are migrated in follow-up sprints.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 16 Evidence | Artifact |
| --- | --- | --- | --- |
| Persistence foundation (bill aggregate) | Production hardening | `LtsDbContext` maps `Bill` with owned `BillVersion`/`BillAmendment`; `EfBillRepository` implements `IBillRepository`; `InitialCreate` migration. | `Infrastructure/Persistence/LtsDbContext.cs`, `EfBillRepository.cs`, `Persistence/Migrations/` |
| Persistence foundation (user aggregate) | Production hardening | `LtsDbContext` maps `UserAccount`; `EfUserRepository` implements `IUserRepository`. | `Infrastructure/Persistence/EfUserRepository.cs` |
| TR-601 versioned RESTful API surface | TR-601 | Existing `/api/v1` endpoints unchanged; persistence is an infrastructure concern behind the same contracts. | `Infrastructure/Persistence/*.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `TreatWarningsAsErrors` enforced. | `dotnet format`, `Directory.Build.props` |
| TR-902 automated tests asserting acceptance criteria | TR-902 | 147 domain tests + 2 infrastructure integration tests pass (0 failed). | `tests/.../EfPersistenceTests.cs` |

## Technical Quality Trace

| Requirement | Source | Sprint 16 Evidence | Artifact |
| --- | --- | --- | --- |
| EF Core + Npgsql persistence | Production hardening | `Microsoft.EntityFrameworkCore` 10.0.0, `Microsoft.EntityFrameworkCore.Design` 10.0.0, `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.0. | `Infrastructure/*.csproj`, `Web/*.csproj` |
| Migrations applied at startup | Production hardening | `Program.cs` calls `db.Database.MigrateAsync()` when a connection string is present. | `Web/Program.cs` |
| DI prefers EF Core when a connection string is present | Production hardening | `AddLtsInfrastructure(connectionString)` registers EF Core repositories (scoped) or falls back to in-memory. | `Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs` |
| Restart persistence | Production hardening | A bill ingested through the API persisted across a container restart. | Container smoke (see Verification) |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| EF Core + Npgsql referenced; `LtsDbContext` maps the bill and user aggregates; initial migration generated | Implemented |
| `EfBillRepository` and `EfUserRepository` implement the existing contracts | Implemented |
| DI registers EF Core repositories when a connection string is present and applies migrations at startup; falls back to in-memory otherwise | Implemented |
| A bill ingested through the API persists across a container restart | Verified |
| Integration tests against a real PostgreSQL instance pass (and skip cleanly when unreachable) | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 16 establishes the PostgreSQL persistence foundation for the core bill and user aggregates. The remaining aggregates (work tasks, packages, relationships, notifications, fiscal data, etc.) stay on the interim in-memory adapter and are migrated in follow-up sprints. The application uses EF Core when a `ConnectionStrings:Default` value is present; otherwise it falls back to the in-memory adapter.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test` passed: 147 domain tests + 2 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`); migrations applied at startup.
- Persistence smoke against `http://localhost:5088`: ingested `HB 8001`, restarted the container, and confirmed the bill persisted in PostgreSQL.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 16 defects; the standard AGENTS.md commands work on a normal developer/container host.
