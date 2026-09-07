# Sprint 35 - Production Hardening: Workflow & Review Refinement Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 35 refines the workflow and review layer: it makes the workflow configurable per work item type and adds reviewer notification delivery.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 35 Evidence | Artifact |
| --- | --- | --- | --- |
| The workflow is configurable per work item type | F3.1 - Configurable Workflow, US-3.1.1, US-3.1.2 | `WorkflowDefinitionCatalog` specifies the required reviewer count per `WorkItemType`; `WorkflowService.SubmitForReviewAsync` validates the submitted reviewer count against it. | `src/Legislature.TrackingSystem.Application/WorkItems/WorkflowDefinition.cs`, `WorkflowService.cs` |
| Reviewers are notified when a work item is submitted for review | US-1.2.1, B.COM.02 | `WorkflowService.SubmitForReviewAsync` notifies each reviewer with the `ReviewRequested` trigger. | `src/Legislature.TrackingSystem.Application/WorkItems/WorkflowService.cs`, `NotificationService.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (206 domain + 23 infrastructure + 22 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 35 Evidence | Artifact |
| --- | --- | --- | --- |
| Configurable workflow per type | F3.1, US-3.1.1, US-3.1.2 | `WorkflowDefinitionCatalog` maps each `WorkItemType` to a required reviewer count; submission is validated against it. | `src/Legislature.TrackingSystem.Application/WorkItems/WorkflowDefinition.cs`, `WorkflowService.cs` |
| Reviewer notification delivery | US-1.2.1, B.COM.02 | `WorkflowService` injects `INotificationService` and notifies each reviewer with the `ReviewRequested` trigger on submit. | `src/Legislature.TrackingSystem.Application/WorkItems/WorkflowService.cs`, `NotificationService.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| A workflow definition specifies the required review policy per work item type | Implemented |
| Review submission is validated against the workflow definition | Implemented |
| Reviewers are notified when a work item is submitted for review | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 35 does not change application persistence. It refines the workflow and review layer (configurable workflow, reviewer notifications).

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release -m:1` passed: 206 domain tests + 23 infrastructure integration tests + 22 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, Development + PostgreSQL):
  - `GET /` → 200 (client app serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- The workflow definition is a fixed catalog; it is not yet editable at runtime.
- Reviewer notifications are in-app only; email delivery is modeled but not yet dispatched.
- Exact token/cost telemetry is unavailable in this local execution context.
