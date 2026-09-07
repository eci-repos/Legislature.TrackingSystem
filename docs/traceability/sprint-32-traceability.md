# Sprint 32 - Production Hardening: Client Authorization Test Coverage Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 32 adds unit tests for the client-side `PermissionAuthorizationHandler` so the WebAssembly client's role-to-permission enforcement is covered by automated tests, matching the server-side handler coverage.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 32 Evidence | Artifact |
| --- | --- | --- | --- |
| The client-side authorization handler is covered by tests | Production hardening | `ClientPermissionAuthorizationHandlerTests` covers the client-side `PermissionAuthorizationHandler` role-to-permission matrix (US-9.1.1, B.COM.06). | `tests/Legislature.TrackingSystem.Web.Tests/Auth/ClientPermissionAuthorizationHandlerTests.cs` |
| The client handler is testable | Production hardening | The Web.Tests project references the Web.Client project so the client-side handler is directly testable. | `tests/Legislature.TrackingSystem.Web.Tests/Legislature.TrackingSystem.Web.Tests.csproj` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (191 domain + 23 infrastructure + 21 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 32 Evidence | Artifact |
| --- | --- | --- | --- |
| Client handler coverage | Production hardening | 9 tests cover the client-side handler: role grants/denies across the matrix and the no-role-claim case. | `tests/Legislature.TrackingSystem.Web.Tests/Auth/ClientPermissionAuthorizationHandlerTests.cs` |
| Testability | Production hardening | The Web.Tests project references the Web.Client project. | `tests/Legislature.TrackingSystem.Web.Tests/Legislature.TrackingSystem.Web.Tests.csproj` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| The client-side `PermissionAuthorizationHandler` is covered by automated unit tests | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 32 does not change application persistence. It adds test coverage for the client-side authorization handler.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 191 domain tests + 23 infrastructure integration tests + 21 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, Development + PostgreSQL):
  - `GET /` → 200 (client app serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- The client-side handler is now covered by tests; the client handler still mirrors the server handler (both use the shared Domain `PermissionMatrix`).
- Exact token/cost telemetry is unavailable in this local execution context.
