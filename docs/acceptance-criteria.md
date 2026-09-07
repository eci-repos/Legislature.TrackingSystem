# Acceptance Criteria & Definition of Done

Status: active

Last updated: 2026-09-06

## Purpose

This document defines the acceptance criteria and Definition of Done for the LTS. It consolidates the acceptance checks, test expectations, traceability expectations, handoff deliverables, PM Validation report requirements, and AI Metrics requirements that every sprint and promoted use case must satisfy. It is the executable contract for "done" and is enforced by `tools/verify-acceptance.ps1`.

## 1. Definition of Done

A change is **done** only when all of the following hold:

1. **Authorized** — explicitly described in the current sprint file (`docs/02-Current-Sprint.md`).
2. **Implemented** — follows the project's conventions and repository layout, to enterprise quality.
3. **Verified** — the project builds and the relevant test suites pass.
4. **Traceable** — links back to the specifying requirement/entity per the traceability rules.
5. **PM Validated** — the required PM Validation report is prepared after completion and verification.
6. **AI Metrics Recorded** — the PM Validation report records available AI usage, cost, tool, verification, and delivery metrics, with unavailable values explicitly identified.
7. **Documented** — the sprint file, handoff notes, PM Validation report, and logs under `docs/` are updated; no stale documentation.
8. **Free of deprecated terminology** — no legacy references remain in the touched scope.

## 2. Acceptance Checks

The acceptance verification script (`tools/verify-acceptance.ps1`) runs the following checks and reports pass/fail:

| Check | Command | Pass condition |
| --- | --- | --- |
| Format | `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` | Exit 0 (no formatting changes) |
| Build | `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` | 0 warnings, 0 errors |
| Test | `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` | 0 failed, 0 skipped |
| Container config | `docker compose config` | Valid compose file |
| Container build | `docker compose build web` | Image builds |
| Container smoke | `docker compose up -d --force-recreate web` then HTTP probes | `GET /`, `/health`, `/health/ready` → 200; token endpoint → 200 |

## 3. Test Expectations

- The full solution test suite must pass with 0 failed and 0 skipped.
- Tests are organized by project: Domain.Tests (unit), Infrastructure.Tests (integration), Web.Tests (web/authorization).
- Each promoted acceptance criterion is asserted by at least one automated test (TR-902).
- Static controls (TR-702) are enforced by `dotnet format --verify-no-changes`.

## 4. Traceability Expectations

- Each sprint produces a traceability report under `docs/traceability/sprint-<n>-traceability.md`.
- The report maps the promoted scope to source requirements/user stories and to the delivered artifacts.
- The report records the completion criteria status and the verification evidence.
- The handoff guide (`docs/08-AI-Coder-Handoff-Guide.md`) records the current state, detailed state, last-verified state, traceability anchors, known gaps, and next-promotion path.

## 5. Handoff Deliverables

- The current sprint file (`docs/02-Current-Sprint.md`) is updated with the completed work, verification, and PM Validation status.
- The handoff guide (`docs/08-AI-Coder-Handoff-Guide.md`) is updated with the current state, detailed state, last-verified state, traceability anchors, known gaps, and next-promotion path.
- The traceability report and PM Validation report are prepared.

## 6. PM Validation Report Requirements

- A PM Validation report is prepared after each sprint and/or promoted use case is completed and verified.
- The report MUST be traceable to promoted source user stories and requirement IDs.
- The report SHOULD include acceptance evidence, verification results, delivered artifacts, AI Metrics, known gaps, residual risks, deferred items, and the PM validation decision.
- Use `docs/validation/PM-Validation-Report-Template.md` unless the current sprint defines a more specific format.

## 7. AI Metrics Requirements

- Each PM Validation report MUST include AI Metrics for the completed sprint and/or promoted use case.
- Record actual token counts, model identifiers, elapsed time, tool/run counts, and cost only when available from the execution environment or billing source.
- When exact values are unavailable, mark them as unavailable rather than estimating silently.
- Cost estimates MUST be clearly labeled as estimates and include their pricing basis.

## 8. Running the Acceptance Verification

```powershell
# From the repository root:
powershell -ExecutionPolicy Bypass -File tools/verify-acceptance.ps1
```

The script runs the checks in §2 and exits non-zero if any check fails. See `tools/verify-acceptance.ps1` for details.
