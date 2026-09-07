# Sprint 26 - Production Hardening: Client-Side Page Authorization Gating Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 26 gates the Blazor WebAssembly client pages by the role-to-permission matrix so unauthorized users cannot navigate to restricted pages. The authoritative `PermissionMatrix` was moved into the Domain project so the server and the WebAssembly client share the same mapping. The client registers permission-based policies, evaluates them against the user's role claim, uses `AuthorizeRouteView` with an access-denied view, adds `[Authorize]` attributes to the pages, and hides restricted navigation links with `AuthorizeView`.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 26 Evidence | Artifact |
| --- | --- | --- | --- |
| The client gates pages by the role-to-permission matrix | Production hardening | `AuthorizeRouteView` in `Routes.razor` renders an access-denied view when the user lacks the required permission; pages carry `[Authorize(Policy = ...)]` attributes. | `src/Legislature.TrackingSystem.Web.Client/Routes.razor`, `Pages/*.razor` |
| The server and client share the same permission matrix | Production hardening | `PermissionMatrix` moved to the Domain project and referenced by both the server handler and the client handler. | `src/Legislature.TrackingSystem.Domain/WorkItems/PermissionMatrix.cs` |
| The client evaluates permission policies against the user's role | Production hardening | Client `AddAuthorizationCore` registers permission policies; client `PermissionAuthorizationHandler` resolves them against the role claim. | `src/Legislature.TrackingSystem.Web.Client/Program.cs`, `Auth/PermissionAuthorizationHandler.cs` |
| The server and client policies stay in parity | Production hardening | The server's `AddAuthorization` block registers the same permission policies as the client so server-side prerendering can resolve the client page policies. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| The server does not mask client-side gating | Production hardening | `BlazorAuthorizationMiddlewareResultHandler` lets Razor component endpoints pass through the server's authorization middleware while API endpoints keep the default 401/403 challenge; prerendering is disabled for the interactive `Routes` component. | `src/Legislature.TrackingSystem.Web/Auth/BlazorAuthorizationMiddlewareResultHandler.cs`, `Components/App.razor` |
| Restricted navigation is hidden from unauthorized users | Production hardening | `NavMenu` wraps each link in `AuthorizeView` with the matching permission policy. | `src/Legislature.TrackingSystem.Web.Client/Layout/NavMenu.razor` |
| The dev boundary exercises the gating | Production hardening | `DevAuthenticationStateProvider` returns the default SecurityAdministrator user so authorized content renders and gating is exercised when Entra is not configured. | `src/Legislature.TrackingSystem.Web.Client/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (167 domain + 23 infrastructure + 12 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 26 Evidence | Artifact |
| --- | --- | --- | --- |
| Shared authoritative matrix | Production hardening | `PermissionMatrix.GetPermissionsForRole` lives in Domain and is the single source of truth for both server and client authorization. | `src/Legislature.TrackingSystem.Domain/WorkItems/PermissionMatrix.cs` |
| Client permission policies | Production hardening | `AddAuthorizationCore` registers `RequirePrepare`, `RequireApprove`, `RequireDeliver`, `RequireReadOnly`, `RequireAdminister`, `RequireManageAccess`, `RequireMigrate`, `RequireViewHistorical`. | `src/Legislature.TrackingSystem.Web.Client/Program.cs` |
| Client permission handler | Production hardening | `PermissionAuthorizationHandler` succeeds a `PermissionRequirement` when the role claim grants the permission per the matrix. | `src/Legislature.TrackingSystem.Web.Client/Auth/PermissionAuthorizationHandler.cs` |
| Route-level gating | Production hardening | `AuthorizeRouteView` with a `NotAuthorized` fragment renders `AccessDenied` inside the main layout. | `src/Legislature.TrackingSystem.Web.Client/Routes.razor`, `Pages/AccessDenied.razor` |
| Page-level gating | Production hardening | Each restricted page carries `@attribute [Authorize(Policy = ...)]` mapped to the permission matrix. | `src/Legislature.TrackingSystem.Web.Client/Pages/*.razor` |
| Navigation gating | Production hardening | `NavMenu` hides links the user is not authorized to access via `AuthorizeView`. | `src/Legislature.TrackingSystem.Web.Client/Layout/NavMenu.razor` |
| Dev boundary | Production hardening | `DevAuthenticationStateProvider` returns a SecurityAdministrator user (matching the dev token endpoint's default "admin" user). | `src/Legislature.TrackingSystem.Web.Client/Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| The authoritative `PermissionMatrix` lives in the Domain project and is shared by the server and the WebAssembly client | Implemented |
| The client registers permission-based policies and evaluates them against the user's role claim | Implemented |
| `Routes.razor` uses `AuthorizeRouteView` with a `NotAuthorized` view | Implemented |
| Client pages carry `[Authorize]` attributes mapped to the permission matrix | Implemented |
| The `NavMenu` hides links the user is not authorized to access | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 26 does not change application persistence. It adds client-side authorization gating on top of the existing server-side authorization.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results:
  - `GET /`, `/access-denied`, `/security`, `/migration`, `/historical`, `/executive`, `/work-intake`, `/fiscal`, `/work-items`, `/bills`, `/search`, `/reports` → 200 (client app serves; pages render under the dev SecurityAdministrator user).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.
  - `GET /api/v1/users` without a token → 401; with a valid token → 200 (API authorization still enforced).
  - `GET /health` → 200.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- Real external endpoints (legislative API, DOR fiscal systems, Microsoft Graph) and real Entra tenant provisioning remain follow-up items.
- The client-side gating is exercised in the dev boundary with the default SecurityAdministrator user; real role-based rendering requires a provisioned Entra tenant with role claims.
- Exact token/cost telemetry is unavailable in this local execution context.
