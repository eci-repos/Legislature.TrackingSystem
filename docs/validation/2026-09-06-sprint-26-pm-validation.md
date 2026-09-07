# Sprint 26 - Production Hardening: Client-Side Page Authorization Gating - PM Validation Report

Date: 2026-09-06

Sprint: Sprint 26

Status: Complete

## 1. Scope Summary

Sprint 26 gates the Blazor WebAssembly client pages by the role-to-permission matrix so unauthorized users cannot navigate to restricted pages. The authoritative `PermissionMatrix` was moved into the Domain project so the server and the WebAssembly client share the same mapping. The client registers permission-based policies, evaluates them against the user's role claim, uses `AuthorizeRouteView` with an access-denied view, adds `[Authorize]` attributes to the pages, and hides restricted navigation links with `AuthorizeView`.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 26 Evidence |
| --- | --- | --- |
| The client gates pages by the role-to-permission matrix | Production hardening | `AuthorizeRouteView` renders an access-denied view when the user lacks the required permission; pages carry `[Authorize(Policy = ...)]` attributes. |
| The server and client share the same permission matrix | Production hardening | `PermissionMatrix` moved to the Domain project and referenced by both the server handler and the client handler. |
| The client evaluates permission policies against the user's role | Production hardening | Client `AddAuthorizationCore` registers permission policies; client `PermissionAuthorizationHandler` resolves them against the role claim. |
| Restricted navigation is hidden from unauthorized users | Production hardening | `NavMenu` wraps each link in `AuthorizeView` with the matching permission policy. |
| The dev boundary exercises the gating | Production hardening | `DevAuthenticationStateProvider` returns the default SecurityAdministrator user so authorized content renders and gating is exercised when Entra is not configured. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (167 domain + 23 infrastructure + 12 web). |

## 3. Acceptance Evidence

- The authoritative `PermissionMatrix` lives in the Domain project and is shared by the server and the WebAssembly client.
- The client registers permission-based policies and evaluates them against the user's role claim.
- `Routes.razor` uses `AuthorizeRouteView` with a `NotAuthorized` view that renders `AccessDenied`.
- Client pages carry `[Authorize]` attributes mapped to the permission matrix.
- The `NavMenu` hides links the user is not authorized to access.

## 4. Verification Results

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results:
  - `GET /`, `/access-denied`, `/security`, `/migration`, `/historical`, `/executive`, `/work-intake`, `/fiscal`, `/work-items`, `/bills`, `/search`, `/reports` → 200 (client app serves; pages render under the dev SecurityAdministrator user).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.
  - `GET /api/v1/users` without a token → 401; with a valid token → 200 (API authorization still enforced).
  - `GET /health` → 200.

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Domain/WorkItems/PermissionMatrix.cs` — authoritative role-to-permission matrix (moved from Application).
- `src/Legislature.TrackingSystem.Web.Client/Program.cs` — client `AddAuthorizationCore` permission policies and dev auth state provider.
- `src/Legislature.TrackingSystem.Web.Client/Auth/PermissionRequirement.cs` — client permission requirement.
- `src/Legislature.TrackingSystem.Web.Client/Auth/PermissionAuthorizationHandler.cs` — client permission handler.
- `src/Legislature.TrackingSystem.Web.Client/Routes.razor` — `AuthorizeRouteView` with a `NotAuthorized` view.
- `src/Legislature.TrackingSystem.Web.Client/Pages/AccessDenied.razor` — access-denied page.
- `src/Legislature.TrackingSystem.Web.Client/Pages/*.razor` — `[Authorize]` attributes on the restricted pages.
- `src/Legislature.TrackingSystem.Web.Client/Layout/NavMenu.razor` — `AuthorizeView`-gated navigation.
- `src/Legislature.TrackingSystem.Web/Program.cs` — server permission-policy parity for the client page policies.
- `src/Legislature.TrackingSystem.Web/Auth/BlazorAuthorizationMiddlewareResultHandler.cs` — lets Razor component endpoints pass through the server's authorization middleware while API endpoints keep the default challenge.
- `src/Legislature.TrackingSystem.Web/Components/App.razor` — disables prerendering for the interactive `Routes` component.
- `tests/Legislature.TrackingSystem.Web.Tests/Auth/PermissionAuthorizationHandlerTests.cs` — permission-handler tests.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-26-traceability.md`, `docs/validation/2026-09-06-sprint-26-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- Real external endpoints (legislative API, DOR fiscal systems, Microsoft Graph) and real Entra tenant provisioning remain follow-up items.
- The client-side gating is exercised in the dev boundary with the default SecurityAdministrator user; real role-based rendering requires a provisioned Entra tenant with role claims.
- Exact token/cost telemetry is unavailable in this local execution context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 26 is implemented, verified, and documented. The Blazor WebAssembly client now gates pages by the role-to-permission matrix: the authoritative `PermissionMatrix` lives in the Domain project and is shared by the server and the client, the client registers permission-based policies and evaluates them against the user's role claim, `Routes.razor` uses `AuthorizeRouteView` with an access-denied view, the restricted pages carry `[Authorize]` attributes, and the `NavMenu` hides links the user is not authorized to access. The .NET solution builds and tests pass, and the container smoke test confirms the client app serves and the restricted pages render under the dev SecurityAdministrator user.

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | deepseek-v4-flash:0731-cloud |
| Token count | Unavailable (telemetry not exposed in this environment) |
| Cost | Unavailable (no billing source exposed; not estimated) |
| Elapsed time | Unavailable (not captured) |
| Tool/run counts | Unavailable (not captured) |
| Verification runs | `dotnet format` (1), `dotnet build` (final pass recorded), `dotnet test` (final full solution pass), container rebuild/restart smoke (multiple; final token 200, home 200, access-denied 200, restricted pages 200) |

> AI Metrics note: token/cost telemetry is not available in this execution environment. Values are marked unavailable rather than estimated, per the project's AI Metrics policy.
