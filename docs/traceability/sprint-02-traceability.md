# Sprint 2 Traceability

Status: active

Last updated: 2026-09-04

## Scope

Sprint 2 implements the first POC vertical slice for legislative work intake and explicit identifiers: create work tasks with mandatory attributes, automatically assign a unique identifier to each work item, and support authorized identifier override with duplicate prevention.

## Business Trace Matrix

| Business Story | Source Requirement | Sprint 2 Evidence | Artifact |
| --- | --- | --- | --- |
| US-1.3.1 | B.COM.11 | Work task creation with mandatory attributes (due date, priority, status, owner, source trace). | `WorkTask`, `WorkTaskService.CreateAsync`, `POST /api/v1/work-tasks`, Work Intake page |
| US-2.1.1 | B.COM.03 | Automatic unique identifier assignment for work items. | `WorkItemIdentifier`, `IWorkItemIdentifierGenerator`, `WorkItemIdentifierGenerator` |
| US-2.1.2 | B.RFA.03 | Authorized identifier override with duplicate prevention and retention. | `WorkTask.OverrideIdentifier`, `WorkTaskService.OverrideIdentifierAsync`, `POST /api/v1/work-tasks/{id}/identifier-override` |

## Technical Trace Matrix

| Technical Story | Source Requirement | Sprint 2 Evidence | Artifact |
| --- | --- | --- | --- |
| TS-6.1 | TR-601 | Versioned RESTful API surface for work intake and identifiers. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TS-7.5 | TR-702 | Coding standards and automated static controls applied. | `Directory.Build.props`, `dotnet format` |
| TS-9.1 / TS-9.2 | — | Automated tests asserting promoted acceptance criteria. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/` |

## Persistence Decision

Sprint 2 uses an interim in-memory repository adapter (`InMemoryWorkTaskRepository`) in Infrastructure. This keeps the POC runnable and deterministically verifiable without a database dependency. PostgreSQL persistence and EF Core migrations remain deferred to a later sprint.

## Verification Evidence

- `dotnet restore Legislature.TrackingSystem.sln -m:1` passed.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors (Domain, Application, Infrastructure, Web.Client incl. Blazor WASM output, Web, and Tests all build).
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed with 16 tests.
- API smoke checks (hosted on `http://localhost:5090`) for work task creation (auto-generated `LTS-Task-<hex>` identifier), retrieval, identifier override, and duplicate override prevention (HTTP 409 Conflict) passed.
- Container build (`docker compose build web`), startup (`docker compose up -d web`; `lts-web` on `0.0.0.0:5088->8080`), and container smoke checks against `http://localhost:5088` (readiness HTTP 200, work task create/get/override, duplicate override 409) all passed with Docker access via elevated permissions.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, and Docker daemon access requires elevated permissions; parallel MSBuild nodes, out-of-proc task hosts, and test-host parent-process monitoring are denied under process isolation without elevation. NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 2 defects; the standard AGENTS.md commands work on a normal developer/container host.
