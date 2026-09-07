# Sprint 25 - Production Hardening: API Hardening (Rate Limiting, Validation, Security Headers) Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 25 hardens the LTS REST API transport and validation layer: rate limiting on the versioned API endpoints, DataAnnotations request validation with automatic 400 responses, and a security-headers middleware (CSP, X-Content-Type-Options, X-Frame-Options, Referrer-Policy, Permissions-Policy). It also fixes two API routing defects surfaced by the smoke test: the versioned API group double-prefixing every route (leaving the real `/api/v1` paths unregistered) and the status-code-pages re-execution masking real API error statuses behind an antiforgery 400.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 25 Evidence | Artifact |
| --- | --- | --- | --- |
| The API is rate limited | Production hardening | `AddRateLimiter` fixed-window "api" policy (PermitLimit 100, Window 1 minute, QueueLimit 0, RejectionStatusCode 429) applied to the versioned API group via `RequireRateLimiting("api")`. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| The API validates request bodies | Production hardening | `ValidationFilter` endpoint filter returns 400 with validation details; DataAnnotations added to 8 request DTOs. | `src/Legislature.TrackingSystem.Web/Security/ValidationFilter.cs`, `Api/WorkItems/*Request.cs` |
| The API sets security headers | Production hardening | `SecurityHeadersMiddleware` sets CSP, X-Content-Type-Options, X-Frame-Options, Referrer-Policy, Permissions-Policy; `UseSecurityHeaders()` wired into the pipeline. | `src/Legislature.TrackingSystem.Web/Security/SecurityHeadersMiddleware.cs`, `SecurityHeadersMiddlewareExtensions.cs` |
| The versioned API routes are reachable | Production hardening | Fixed the API group double-prefix so `/api/v1` endpoints register and respond (previously `/api/v1/api/v1/...`). | `src/Legislature.TrackingSystem.Web/Program.cs` |
| API error statuses are preserved | Production hardening | Scoped `UseStatusCodePages` re-execution to non-`/api` requests so real statuses (429, 400) are not masked by an antiforgery 400. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged in shape; all `/api/v1` endpoints now register and respond. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (167 domain + 23 infrastructure + 4 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 25 Evidence | Artifact |
| --- | --- | --- | --- |
| Rate limiting policy | Production hardening | Fixed-window limiter with explicit permit limit, window, queue limit, and 429 rejection status. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| Request-body validation | Production hardening | `ValidationFilter` validates request-body arguments via `Validator.TryValidateObject(validateAllProperties: true)` and returns `Results.ValidationProblem`. | `src/Legislature.TrackingSystem.Web/Security/ValidationFilter.cs` |
| Security headers | Production hardening | Middleware sets CSP, nosniff, frame-deny, referrer-policy, and permissions-policy headers on every response. | `src/Legislature.TrackingSystem.Web/Security/SecurityHeadersMiddleware.cs` |
| Antiforgery boundary | Production hardening | The JSON API group opts out of antiforgery via `DisableAntiforgery()` (JWT-secured); Blazor interactive pages keep antiforgery. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| API route registration | Production hardening | Empty group prefix keeps the full `/api/v1/...` paths while applying group conventions (rate limit, validation, antiforgery opt-out). | `src/Legislature.TrackingSystem.Web/Program.cs` |
| Error-status preservation | Production hardening | `UseStatusCodePages` re-executes to `/not-found` only for non-`/api` requests. | `src/Legislature.TrackingSystem.Web/Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| Rate limiting is configured and applied to the `/api/v1` endpoints | Implemented |
| A security-headers middleware sets the required headers | Implemented |
| Key request DTOs carry DataAnnotations and a `ValidationFilter` returns 400 for invalid bodies | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 25 does not change application persistence. It hardens the API transport and validation layer.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests + 4 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results:
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.
  - `POST /api/v1/auth/token` with empty/missing `userKey` → 400 `application/problem+json` (validation filter).
  - `GET /api/v1/templates` → 200 (group endpoint registered).
  - `GET /health`, `GET /health/ready`, `GET /` → 200.
  - Security headers (CSP, X-Content-Type-Options, X-Frame-Options, Referrer-Policy, Permissions-Policy) present on responses.
  - Rate limiting: 100 requests return 200, subsequent requests return 429.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- Real external endpoints (legislative API, DOR fiscal systems, Microsoft Graph) and real Entra tenant provisioning remain follow-up items.
- Client-side page authorization gating (deferred from Sprint 21) remains a follow-up item.
- Exact token/cost telemetry is unavailable in this local execution context.
