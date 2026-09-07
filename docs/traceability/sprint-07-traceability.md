# Sprint 7 - RFA Executive Review Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 7 is the second Phase 2 vertical slice: F3.2 - RFA Executive Review (per `docs/backlog/phase-02-workflow-review-authoring-and-document-production.md`). It implements BI-US-3.2.1 (US-3.2.1/B.RFA.01), BI-US-3.2.2 (US-3.2.2/B.RFA.02), BI-US-3.2.3 (US-3.2.3/B.RFA.04), and BI-US-3.2.4 (US-3.2.4/B.RFA.05). Authorized executive reviewers conduct Executive Reviews of fiscal products inside the solution with a sequential reviewer handoff, and each work product supports step-level due dates and a settable priority. E4 Content Authoring, Documents, and Templates is deferred to Sprint 8+.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 7 Evidence | Artifact |
| --- | --- | --- | --- |
| US-3.2.1 Executive Review restricted to designated users; fiscal notes/estimates can undergo Executive Review; review before external release | B.RFA.01 | `AssignmentRole.ExecutiveReviewer` designates reviewers; `ExecutiveReviewService.StartExecutiveReviewAsync` rejects any reviewer without an active ExecutiveReviewer assignment; `WorkTask.StartExecutiveReview` starts the governed path. | `Domain/WorkItems/AssignmentRole.cs`, `WorkTask.cs`, `Application/WorkItems/ExecutiveReviewService.cs` |
| US-3.2.2 entire Executive Review inside the solution; reviewer access, adjust/correct, completion, handoff, continues until complete | B.RFA.02 | Sequential ordered reviewers (`ExecutiveReviewer`); `BeginExecutiveReviewStep`/`AdjustExecutiveReview`/`CompleteExecutiveReviewStep`; completing a step hands off to the next reviewer automatically; last completion sets `ExecutiveReviewStatus.Completed`. | `Domain/WorkItems/ExecutiveReviewer.cs`, `ExecutiveReviewAdjustment.cs`, `WorkTask.cs`, `ExecutiveReviewServiceTests` |
| US-3.2.3 step/subtask due dates; multiple step-level due dates; determine which due date applies | B.RFA.04 | `WorkflowStep` with per-step `DueDate`; `AddStep`/`SetStepDueDate`/`CompleteStep`; multiple steps coexist within one product. | `Domain/WorkItems/WorkflowStep.cs`, `WorkflowStepStatus.cs`, `WorkTask.cs` |
| US-3.2.4 set priority per work product; view; use in workload management | B.RFA.05 | `WorkTask.SetPriority`; priority exposed on the DTO and editable in the Work Items UI. | `Domain/WorkItems/WorkTask.cs`, `WorkItems.razor` |

## Technical Quality Trace

| Requirement | Source | Sprint 7 Evidence | Artifact |
| --- | --- | --- | --- |
| Automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `Directory.Build.props` applies `TreatWarningsAsErrors`. | `dotnet format`, `Directory.Build.props` |
| Automated tests asserting acceptance criteria | TR-902 | 64 tests pass (0 failed); 12 new executive review tests cover designated-reviewer enforcement, sequential handoff, adjustment recording, completion, priority, and step due dates. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/ExecutiveReviewServiceTests.cs` |
| Versioned RESTful API surface | TR-601 | New `/api/v1` endpoints: `POST work-items/{id}/executive-review/start`, `.../begin`, `.../adjust`, `.../complete`, `POST work-items/{id}/priority`, `POST work-items/{id}/steps`, `PUT work-items/{id}/steps/{stepId}/due-date`, `POST work-items/{id}/steps/{stepId}/complete`. | `src/Legislature.TrackingSystem.Web/Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| Executive Review is a governed path restricted to designated users; fiscal notes and fiscal estimates can undergo Executive Review | Implemented |
| Executive Review proceeds sequentially with reviewer handoff and continues until complete; reviewers can adjust/correct and indicate completion | Implemented |
| Work products support multiple step-level due dates and a settable priority | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 7 continues the interim in-memory repository adapter; PostgreSQL persistence and EF Core migrations remain deferred.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 64 tests (0 failed, 0 skipped).
- Container smoke checks (executive review workflow and UI render) were verified against `http://localhost:5088` after `docker compose up -d --build --force-recreate web`.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 7 defects; the standard AGENTS.md commands work on a normal developer/container host.
