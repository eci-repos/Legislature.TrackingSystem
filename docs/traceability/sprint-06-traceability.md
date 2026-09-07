# Sprint 6 - Configurable Workflow Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 6 is the first Phase 2 vertical slice: F3.1 - Configurable Workflow (per `docs/backlog/phase-02-workflow-review-authoring-and-document-production.md`). It implements BI-US-3.1.1 (US-3.1.1/B.COM.15), BI-US-3.1.2 (US-3.1.2/B.COM.19), and BI-US-3.1.3 (US-3.1.3/B.COM.20). A DOR work item now follows a defined review and approval workflow with separated work/approval responsibilities, multiple reviewers, explicit workflow state, and approved-only final packaging with internal/external recipients. F3.2 RFA Executive Review and E4 Content Authoring/Documents are deferred to Sprint 7+.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 6 Evidence | Artifact |
| --- | --- | --- | --- |
| US-3.1.1 tasks follow a defined workflow; separated work/approval; multiple reviewers; determinable status | B.COM.15 | `WorkTask` gains `WorkflowStatus` and `SubmitForReview` / `RecordReview` / `Finalize`; `SubmitForReview` enforces separation of duties (at least one reviewer must not be a preparer); workflow status is exposed on the task DTO and UI. | `Domain/WorkItems/WorkflowStatus.cs`, `WorkTask.cs`, `Application/WorkItems/IWorkflowService.cs` + `WorkflowService.cs`, `WorkItems.razor` |
| US-3.1.2 completed products enter review; multiple SMEs participate; review before submission; determinable approval state | B.COM.19 | Multi-reviewer required-set model (`RequiredReviewerKeys`); per-reviewer `WorkflowReview` records retained; approval reached only after every required reviewer approves (`PendingReview → UnderReview → Approved/Rejected`). | `Domain/WorkItems/WorkflowReview.cs`, `WorkflowDecision.cs`, `WorkTask.RecordReview`, `WorkflowServiceTests` |
| US-3.1.3 only reviewed/approved products finalized; packaged for submission; internal + external recipients | B.COM.20 | Package `Finalize()` plus `PackageStatus.Finalized`; `PackageService.FinalizePackageAsync` refuses finalization unless every member work product is `Approved`; `AddRecipient` supports `Internal`/`External`. | `Domain/WorkItems/Package.cs`, `PackageRecipient.cs`, `PackageRecipientKind.cs`, `Application/WorkItems/PackageService.cs`, `Packages.razor` |

## Technical Quality Trace

| Requirement | Source | Sprint 6 Evidence | Artifact |
| --- | --- | --- | --- |
| Automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `Directory.Build.props` applies `TreatWarningsAsErrors`. | `dotnet format`, `Directory.Build.props` |
| Automated tests asserting acceptance criteria | TR-902 | 52 tests pass (0 failed); 12 new workflow/package workflow tests cover separation of duties, pending/under-review/approved/rejected/finalized transitions, non-assigned reviewer rejection, approved-only package finalize, and internal/external recipients. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/WorkflowServiceTests.cs` |
| Versioned RESTful API surface | TR-601 | New `/api/v1` endpoints: `POST work-items/{id}/submit-for-review`, `POST work-items/{id}/reviews`, `POST work-items/{id}/finalize`, `POST packages/{id}/recipients`, `POST packages/{id}/finalize`. | `src/Legislature.TrackingSystem.Web/Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| Work items have an explicit workflow status (draft → review → approved/rejected → finalized) with separation between preparers and approvers | Implemented |
| Multiple reviewers can participate and the product approval state is determinable | Implemented |
| Only approved products can be finalized/packaged; packages support internal and external recipients | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 6 continues the interim in-memory repository adapter; PostgreSQL persistence and EF Core migrations remain deferred.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 52 tests (0 failed, 0 skipped).
- Container smoke checks (workflow endpoints and UI render) were verified against `http://localhost:5088` after `docker compose up -d --build web`.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 6 defects; the standard AGENTS.md commands work on a normal developer/container host.
