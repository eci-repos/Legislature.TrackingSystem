# Sprint 25 - Production Hardening: API Hardening (Rate Limiting, Validation, Security Headers) - PM Validation Report

Date: 2026-09-06

Sprint: Sprint 25

Status: Complete

## 1. Scope Summary

Sprint 25 hardens the LTS REST API transport and validation layer: rate limiting on the versioned API endpoints, DataAnnotations request validation with automatic 400 responses, and a security-headers middleware (CSP, X-Content-Type-Options, X-Frame-Options, Referrer-Policy, Permissions-Policy). The smoke test surfaced and the sprint also fixed two API routing defects: the versioned API group double-prefixing every route (leaving the real `/api/v1` paths unregistered) and the status-code-pages re-execution masking real API error statuses behind an antiforgery 400.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 25 Evidence |
| --- | --- | --- |
| The API is rate limited | Production hardening | `AddRateLimiter` fixed-window "api" policy applied to the versioned API group; excess requests return 429. |
| The API validates request bodies | Production hardening | `ValidationFilter` returns 400 with validation details; DataAnnotations added to 8 request DTOs. |
| The API sets security headers | Production hardening | `SecurityHeadersMiddleware` sets CSP, nosniff, frame-deny, referrer-policy, and permissions-policy headers. |
| The versioned API routes are reachable | Production hardening | Fixed the API group double-prefix so `/api/v1` endpoints register and respond. |
| API error statuses are preserved | Production hardening | Scoped `UseStatusCodePages` re-execution to non-`/api` requests so real statuses (429, 400) are not masked. |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged in shape; all `/api/v1` endpoints now register and respond. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (167 domain + 23 infrastructure + 4 web). |

## 3. Acceptance Evidence

- Rate limiting is configured and applied to the `/api/v1` endpoints; excess requests return 429.
- A security-headers middleware sets the required headers on responses.
- Key request DTOs carry DataAnnotations and a `ValidationFilter` returns 400 for invalid bodies.
- The versioned API group registers its endpoints at the real `/api/v1` paths (previously double-prefixed).
- API error statuses (429, 400) are preserved rather than masked by the `/not-found` re-execution.

## 4. Verification Results

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

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Web/Program.cs` — rate limiting, API group wiring, scoped status-code pages, antiforgery boundary.
- `src/Legislature.TrackingSystem.Web/Security/SecurityHeadersMiddleware.cs` — security-headers middleware.
- `src/Legislature.TrackingSystem.Web/Security/SecurityHeadersMiddlewareExtensions.cs` — `UseSecurityHeaders()` extension.
- `src/Legislature.TrackingSystem.Web/Security/ValidationFilter.cs` — request-body validation endpoint filter.
- `src/Legislature.TrackingSystem.Web/Api/WorkItems/*Request.cs` — DataAnnotations on 8 request DTOs.
- `tests/Legislature.TrackingSystem.Web.Tests/Security/SecurityHeadersMiddlewareTests.cs` — security-headers tests.
- `tests/Legislature.TrackingSystem.Web.Tests/Security/ValidationFilterTests.cs` — validation-filter tests.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-25-traceability.md`, `docs/validation/2026-09-06-sprint-25-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- Real external endpoints (legislative API, DOR fiscal systems, Microsoft Graph) and real Entra tenant provisioning remain follow-up items.
- Client-side page authorization gating (deferred from Sprint 21) remains a follow-up item.
- Exact token/cost telemetry is unavailable in this local execution context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 25 is implemented, verified, and documented. The LTS API is now rate-limited, request bodies are validated with automatic 400 responses, and security headers are applied. The smoke test also surfaced and the sprint fixed two API routing defects (the versioned API group double-prefix and the status-code-pages masking of API error statuses), so the `/api/v1` endpoints now register and respond with their real status codes. The .NET solution builds and tests pass, and the container smoke test confirms the token endpoint issues a valid JWT, validation returns 400, security headers are present, and the rate limiter returns 429.

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | deepseek-v4-flash:0731-cloud |
| Token count | Unavailable (telemetry not exposed in this environment) |
| Cost | Unavailable (no billing source exposed; not estimated) |
| Elapsed time | Unavailable (not captured) |
| Tool/run counts | Unavailable (not captured) |
| Verification runs | `dotnet format` (1), `dotnet build` (final pass recorded), `dotnet test` (final full solution pass), container rebuild/restart smoke (multiple; final token 200, validation 400, templates 200, health 200, security headers present, rate limit 429) |

> AI Metrics note: token/cost telemetry is not available in this execution environment. Values are marked unavailable rather than estimated, per the project's AI Metrics policy.
