# Sprint 31 - Production Hardening: Acceptance Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 31 defines and makes executable the acceptance criteria for the LTS: acceptance checks, test expectations, traceability expectations, handoff deliverables, PM Validation report, and AI Metrics requirements. It adds a consolidated acceptance & Definition of Done document and a repeatable acceptance verification script.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 31 Evidence | Artifact |
| --- | --- | --- | --- |
| The acceptance criteria and Definition of Done are defined | Production hardening | `docs/acceptance-criteria.md` defines the acceptance checks, test expectations, traceability expectations, handoff deliverables, PM Validation report, and AI Metrics requirements. | `docs/acceptance-criteria.md` |
| The acceptance checks are executable | Production hardening | `tools/verify-acceptance.ps1` runs format, build, test, container config/build/smoke and reports pass/fail. | `tools/verify-acceptance.ps1` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (191 domain + 23 infrastructure + 12 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 31 Evidence | Artifact |
| --- | --- | --- | --- |
| Consolidated acceptance contract | Production hardening | The document consolidates the Definition of Done, acceptance checks, test expectations, traceability expectations, handoff deliverables, PM Validation report, and AI Metrics requirements. | `docs/acceptance-criteria.md` |
| Repeatable verification | Production hardening | The script runs the acceptance checks and exits non-zero on failure. | `tools/verify-acceptance.ps1` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| An acceptance & Definition of Done document defines the acceptance checks, test expectations, traceability expectations, handoff deliverables, PM Validation report, and AI Metrics requirements | Implemented |
| A repeatable acceptance verification script runs the acceptance checks and reports pass/fail | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 31 does not change application persistence. It defines the acceptance criteria and verification for the LTS.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 191 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- `tools/verify-acceptance.ps1` ran the format, build, test, container config, container build, and container smoke checks and reported pass.
- Container smoke test (dev boundary, Development + PostgreSQL): `GET /` → 200, `GET /health` → 200, `GET /health/ready` → 200, token endpoint → 200.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- The acceptance verification script is a local tool; CI enforces the same checks in the CI workflow.
- Exact token/cost telemetry is unavailable in this local execution context.
