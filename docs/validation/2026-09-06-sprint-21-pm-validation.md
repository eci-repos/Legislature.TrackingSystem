# Sprint 21 - Production Hardening: Web.Client Interactive Entra Sign-In - PM Validation Report

Date: 2026-09-06

Sprint: Sprint 21

Status: Complete

## 1. Scope Summary

Sprint 21 completes the Entra/OpenID Connect authentication boundary from Sprint 20 by adding the interactive sign-in flow to the Blazor WebAssembly client. The Web.Client authenticates against Microsoft Entra ID via the OpenID Connect authorization code flow when `AzureAd` configuration is present, attaches access tokens to API requests, and provides login/logout UI. When Entra is not configured, the client falls back to the existing dev boundary.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 21 Evidence |
| --- | --- | --- |
| Authorized users can access the solution with real identity | Production hardening; US-9.1.x | The Web.Client uses `AddOidcAuthentication` to sign in against the Entra tenant when `AzureAd` is configured. |
| Role-based permissions distinguish preparation, approval, and delivery | US-9.1.1, B.COM.06 | The client attaches access tokens to API requests via `AuthorizationMessageHandler`, and the server enforces the role-to-permission matrix. |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged; the client authenticates against it. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (161 domain + 21 integration). |

## 3. Acceptance Evidence

- The Web.Client references `Microsoft.AspNetCore.Components.WebAssembly.Authentication`.
- `AddOidcAuthentication` is configured from the `AzureAd` section in the Web.Client `Program.cs`.
- `RemoteAuthenticatorView` routes and login/logout UI are present.
- The client `HttpClient` attaches access tokens to API requests via `AuthorizationMessageHandler` in Entra mode.
- The dev boundary (plain `HttpClient` + `DevAuthenticationStateProvider`) remains functional when Entra is not configured.

## 4. Verification Results

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy.
- Client auth UI smoke (hosted on `http://localhost:5088`): the home page renders the "Log in" control linking to `authentication/login`; the `/security` page renders without the not-found regression; the dev auth boundary still issues tokens and protects `/api/v1/users`.

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Web.Client/Program.cs` — configuration-driven `AddOidcAuthentication` and token-attaching `HttpClient`.
- `src/Legislature.TrackingSystem.Web.Client/Pages/Authentication.razor` — `RemoteAuthenticatorView` route.
- `src/Legislature.TrackingSystem.Web.Client/Layout/MainLayout.razor` — `AuthorizeView` login/logout controls.
- `src/Legislature.TrackingSystem.Web.Client/Routes.razor` — `CascadingAuthenticationState` wrapper.
- `src/Legislature.TrackingSystem.Web.Client/wwwroot/appsettings.json` — `AzureAd` client configuration section.
- `src/Legislature.TrackingSystem.Web/Auth/ServerAuthenticationStateProvider.cs` — server-side prerender auth state.
- `src/Legislature.TrackingSystem.Web/Program.cs` — server-side auth state provider registration.
- `src/Legislature.TrackingSystem.Web.Client/Legislature.TrackingSystem.Web.Client.csproj` — auth package reference.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-21-traceability.md`, `docs/validation/2026-09-06-sprint-21-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- Real Entra interactive sign-in against a live tenant, Entra app registration/tenant provisioning, and the redirect URI configuration remain follow-up items.
- Client-side page-level authorization gating (`AuthorizeRouteView`/`[Authorize]`) was deferred because it caused a server-prerender not-found regression; the server-side API still enforces authorization on protected endpoints.
- External legislative/fiscal connectors, Microsoft 365/SharePoint/Graph integration, registry publishing, image scanning, deployed Terraform infrastructure, GuardDuty/Security Hub, and cross-region DR remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local execution context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 21 is implemented, verified, and documented. The Web.Client now supports interactive Entra/OpenID Connect sign-in when configured, attaches access tokens to API requests, and provides login/logout UI, while the dev boundary remains functional when Entra is not configured. The client auth wiring is verified by build and dev-boundary smoke tests.

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | GPT-5 Codex |
| Token count | Unavailable (telemetry not exposed in this environment) |
| Cost | Unavailable (no billing source exposed; not estimated) |
| Elapsed time | Unavailable (not captured) |
| Tool/run counts | Unavailable (not captured) |
| Verification runs | `dotnet format` (1), `dotnet build` (multiple during repair; final pass recorded), `dotnet test` (final full solution pass), container rebuild/restart smoke (1) |

> AI Metrics note: token/cost telemetry is not available in this execution environment. Values are marked unavailable rather than estimated, per the project's AI Metrics policy.
