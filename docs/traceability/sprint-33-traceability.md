# Sprint 33 - Production Hardening: API Hardening Refinement Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 33 refines the API hardening: it adds per-IP/per-user rate-limit partitioning, adds per-route security header customization, and expands request validation beyond the 8 key DTOs.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 33 Evidence | Artifact |
| --- | --- | --- | --- |
| The API rate limiter partitions by client IP and by the authenticated user | Production hardening | The `api` rate-limit policy uses `RateLimitPartition.GetFixedWindowLimiter` keyed by the authenticated user's `sub` claim when present, else the client IP. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| Security headers can be customized per route | Production hardening | `SecurityHeadersOptions` holds the defaults; `SecurityHeadersOverride` is per-route endpoint metadata; the middleware applies overrides. | `src/Legislature.TrackingSystem.Web/Security/SecurityHeadersOptions.cs`, `SecurityHeadersOverride.cs`, `SecurityHeadersMiddleware.cs`, `SecurityHeadersMiddlewareExtensions.cs` |
| Request validation covers a broader API surface | Production hardening | DataAnnotations (`[Required]`, `[MinLength]`) added to 30+ request DTOs so the existing `ValidationFilter` validates them. | `src/Legislature.TrackingSystem.Web/Api/WorkItems/*.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (191 domain + 23 infrastructure + 22 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 33 Evidence | Artifact |
| --- | --- | --- | --- |
| Per-user/per-IP rate-limit partition | Production hardening | The `api` policy partitions by the authenticated user's `sub` claim or the client IP. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| Per-route security header override | Production hardening | `SecurityHeadersOverride` is endpoint metadata; the middleware replaces the default for any non-null override property. | `src/Legislature.TrackingSystem.Web/Security/*.cs` |
| Broader request validation | Production hardening | DataAnnotations added to 30+ request DTOs; the `ValidationFilter` validates all request-body arguments. | `src/Legislature.TrackingSystem.Web/Api/WorkItems/*.cs`, `src/Legislature.TrackingSystem.Web/Security/ValidationFilter.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| The API rate limiter partitions by client IP and by the authenticated user when present | Implemented |
| Security headers can be customized per route | Implemented |
| Request validation covers a broader set of request DTOs | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 33 does not change application persistence. It refines the API hardening (rate limiting, security headers, request validation).

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 191 domain tests + 23 infrastructure integration tests + 22 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, Development + PostgreSQL):
  - `GET /` → 200 (client app serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.
  - Security headers present on responses (CSP, nosniff, frame-deny, referrer-policy, permissions-policy).

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- The rate limiter is a fixed-window policy (no sliding-window refinement); per-user partitioning is keyed on the `sub` claim.
- The security-header overrides are opt-in per route; the default set remains fixed.
- Exact token/cost telemetry is unavailable in this local execution context.
