# Sprint 3 Traceability

Status: active

Last updated: 2026-09-04

## Scope

Sprint 3 implements the POC assignment and work-queue vertical slice on top of Sprint 2 work intake: assign and reassign tasks to DOR users, support multiple assignees on a task with per-role due dates, and expose a user work queue showing assigned work with status, priority, role, due dates, and rework identification.

## Business Trace Matrix

| Business Story | Source Requirement | Sprint 3 Evidence | Artifact |
| --- | --- | --- | --- |
| US-1.3.2 | B.COM.12 | Assign and reassign tasks to DOR users; currently assigned user identifiable; reassignment retains work product and prior assignment history. | `WorkTask.AssignUser` / `WorkTask.ReassignUser`, `WorkTaskAssignmentService.AssignAsync` / `ReassignAsync`, `POST /api/v1/work-tasks/{id}/assignments` and `/reassign` |
| US-1.3.3 | B.COM.13 | Multiple assignees per task with per-role due dates that coexist on the same task. | `TaskAssignment` (role + due date), `WorkTask.ActiveAssignments`, `WorkTaskAssignmentService` |
| US-1.3.5 | B.COM.10 | User work queue with status, priority, role, per-assignment due date, rework identification, and workload count. | `WorkQueueDto`, `WorkTaskAssignmentService.GetWorkQueueAsync`, `GET /api/v1/work-queue/{assigneeKey}`, Work Queue page |

## Technical Trace Matrix

| Technical Story | Source Requirement | Sprint 3 Evidence | Artifact |
| --- | --- | --- | --- |
| TS-6.1 | TR-601 | Versioned RESTful API surface for assignment, reassignment, and work-queue queries. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TS-7.5 | TR-702 | Coding standards and automated static controls applied. | `Directory.Build.props`, `dotnet format` |
| TS-9.2 | TR-902 | Automated tests asserting the promoted acceptance criteria. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/WorkTaskAssignmentServiceTests.cs` |

## POC Interpretation Notes

- A "DOR user" is represented by a stable user key (e.g., username) because authentication/authorization is deferred; the assigner is captured via an assigned-by key when provided.
- Rework is identified when a task is reassigned to a user who previously held an assignment on the same task (from the retained assignment history).
- Reassignment supersedes the prior assignment record (kept for history) rather than deleting it, so work product and supporting information are never removed.

## Persistence Decision

Sprint 3 extends the interim in-memory repository adapter (`InMemoryWorkTaskRepository`) with assignment state and queue queries. PostgreSQL persistence and EF Core migrations remain deferred to a later sprint.

## Verification Evidence

- `dotnet restore Legislature.TrackingSystem.sln -m:1` passed.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors (Domain, Application, Infrastructure, Web.Client incl. Blazor WASM output, Web, and Tests all build).
- `dotnet test Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 23 tests (16 prior + 7 new assignment/queue tests).
- Container build (`docker compose build web`), startup (`docker compose up -d web`; `lts-web` on `0.0.0.0:5088->8080`), and container smoke checks against `http://localhost:5088` passed: work task create, assign (Analyst), reassign (Reviewer), reassign-back (rework flagged), and work-queue queries (status/priority/role/due/rework/count; superseded user's queue empty).
- UI pages `/work-intake` (with Assignment section) and `/work-queue` (with Load queue) render HTTP 200; nav includes the Work Queue link.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, and Docker daemon access requires elevated permissions; parallel MSBuild nodes, out-of-proc task hosts, and test-host parent-process monitoring are denied under process isolation without elevation. NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 3 defects; the standard AGENTS.md commands work on a normal developer/container host.
