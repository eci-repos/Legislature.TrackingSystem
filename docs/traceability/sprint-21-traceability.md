# Sprint 21 - Production Hardening: Web.Client Interactive Entra Sign-In Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 21 completes the Entra/OpenID Connect authentication boundary from Sprint 20 by adding the interactive sign-in flow to the Blazor WebAssembly client. The Web.Client authenticates against Microsoft Entra ID via the OpenID Connect authorization code flow when `AzureAd` configuration is present, attaches access tokens to API requests, and provides login/logout UI. When Entra is not configured, the client falls back to the existing dev boundary.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 21 Evidence | Artifact |
| --- | --- | --- | --- |
| Authorized users can access the solution with real identity | Production hardening; US-9.1.x | The Web.Client uses `AddOidcAuthentication` to sign in against the Entra tenant when `AzureAd` is configured. | `src/Legislature.TrackingSystem.Web.Client/Program.cs` |
| Role-based permissions distinguish preparation, approval, and delivery | US-9.1.1, B.COM.06 | The client attaches access tokens to API requests via `AuthorizationMessageHandler`, and the server enforces the role-to-permission matrix. | `Web.Client/Program.cs`, `PermissionAuthorizationHandler.cs` |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged; the client authenticates against it. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `TreatWarningsAsErrors` remains enforced. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (161 domain + 21 integration). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 21 Evidence | Artifact |
| --- | --- | --- | --- |
| Configuration-driven client auth | Production hardening | `AddOidcAuthentication` is configured from the `AzureAd` section; the dev boundary (plain `HttpClient`) is used when Entra is not configured. | `Web.Client/Program.cs`, `wwwroot/appsettings.json` |
| Token attachment | Production hardening | `AuthorizationMessageHandler` attaches access tokens to API requests in Entra mode. | `Web.Client/Program.cs` |
| Interactive sign-in UI | Production hardening | `RemoteAuthenticatorView` route and `AuthorizeView` login/logout controls are present. | `Pages/Authentication.razor`, `Layout/MainLayout.razor` |
| Dev boundary preserved | Production hardening | `DevAuthenticationStateProvider` keeps `AuthorizeView`/`CascadingAuthenticationState` functional without Entra; the plain `HttpClient` dev boundary remains active. | `Web.Client/Program.cs` |
| Server prerender auth state | Production hardening | `ServerAuthenticationStateProvider` renders the layout's `AuthorizeView` correctly during server prerendering. | `src/Legislature.TrackingSystem.Web/Auth/ServerAuthenticationStateProvider.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| The Web.Client references `Microsoft.AspNetCore.Components.WebAssembly.Authentication` | Implemented |
| `AddOidcAuthentication` is configured from the `AzureAd` section in the Web.Client `Program.cs` | Implemented |
| `RemoteAuthenticatorView` routes and login/logout UI are present | Implemented |
| The client `HttpClient` attaches access tokens to API requests | Implemented |
| The dev boundary remains functional when Entra is not configured | Verified |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 21 does not change persistence. It completes the client-side authentication boundary started in Sprint 20.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy.
- Client auth UI smoke (hosted on `http://localhost:5088`): the home page renders the "Log in" control linking to `authentication/login`; the `/security` page renders without the not-found regression; the dev auth boundary still issues tokens and protects `/api/v1/users`.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. Real Entra interactive sign-in requires a live tenant and is not exercised in this offline environment; the client auth wiring is verified by build and the dev-boundary smoke tests.

## Residual Risks and Deferrals

- Real Entra interactive sign-in against a live tenant, Entra app registration/tenant provisioning, and the redirect URI configuration remain follow-up items.
- Client-side page-level authorization gating (`AuthorizeRouteView`/`[Authorize]`) was deferred because it caused a server-prerender not-found regression; the server-side API still enforces authorization on protected endpoints.
- External legislative/fiscal connectors, Microsoft 365/SharePoint/Graph integration, registry publishing, image scanning, deployed Terraform infrastructure, GuardDuty/Security Hub, and cross-region DR remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local execution context.
