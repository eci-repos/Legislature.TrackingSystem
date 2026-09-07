# Sprint 20 - Production Hardening: Entra/OpenID Connect Authentication Boundary Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 20 replaces the JWT-only authentication boundary with a real Microsoft Entra ID / OpenID Connect identity integration, while keeping the local/offline development boundary functional. The app validates bearer tokens against the Entra tenant authority when `AzureAd` configuration is present, maps Entra claims to the LTS user model and role-to-permission matrix, and falls back to the existing symmetric-key JWT dev boundary when Entra is not configured.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 20 Evidence | Artifact |
| --- | --- | --- | --- |
| Role-based permissions distinguish preparation, approval, and delivery; no restricted functions outside assigned permissions | US-9.1.1, B.COM.06 | Entra `roles`/`groups` claims map to LTS `UserRole` via configurable mappings, and a `ClaimTypes.Role` claim is applied so the existing role-to-permission matrix handler enforces the matrix. | `EntraClaimsMapper.cs`, `PermissionAuthorizationHandler.cs` |
| Authorized users can access the solution with real identity | Production hardening; US-9.1.x | JWT bearer validates against the Entra tenant authority (issuer, audience, OIDC-metadata signing keys) when `AzureAd` is configured. | `Program.cs`, `EntraAuthOptions.cs` |
| TR-601 versioned RESTful API surface | TR-601 | The `/api/v1/auth/token` dev endpoint remains functional for local/offline use; the versioned API surface is unchanged. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `TreatWarningsAsErrors` remains enforced. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | 12 unit tests cover the Entra options and claim mapping. | `tests/Legislature.TrackingSystem.Domain.Tests/Authentication/EntraAuthTests.cs` |

## Technical Quality Trace

| Requirement | Source | Sprint 20 Evidence | Artifact |
| --- | --- | --- | --- |
| Configuration-driven auth selection | Production hardening | Program.cs selects the Entra boundary when `AzureAd` is configured and the symmetric-key JWT dev boundary otherwise. | `Program.cs` |
| Claim-to-user-model mapping | Production hardening | `EntraClaimsMapper` extracts the user key from `oid`/`upn`/`preferred_username` and maps `roles`/`groups` to `UserRole`. | `EntraClaimsMapper.cs` |
| Role-to-permission matrix preserved | US-9.1.1, B.COM.06 | The existing `PermissionAuthorizationHandler` continues to enforce the matrix via the applied `ClaimTypes.Role` claim. | `PermissionAuthorizationHandler.cs`, `PermissionMatrix.cs` |
| Dev boundary preserved | Production hardening | The symmetric-key JWT dev boundary and `/api/v1/auth/token` endpoint remain the no-configuration fallback. | `Program.cs`, `JwtTokenService.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| `EntraAuthOptions` binds from the `AzureAd` section and exposes `IsConfigured` and `Issuer` | Implemented |
| `EntraClaimsMapper` extracts the user key and maps `roles`/`groups` claims to `UserRole`, and applies a `ClaimTypes.Role` claim | Implemented |
| Program.cs selects Entra validation when `AzureAd` is configured and the symmetric-key JWT dev boundary otherwise | Implemented |
| The `/api/v1/auth/token` dev endpoint remains functional for local/offline use | Verified |
| Unit tests cover the Entra options and claim mapping | Verified |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 20 does not change persistence. It changes the authentication boundary: real Entra/OpenID Connect validation when configured, with the existing JWT dev boundary as the no-configuration fallback.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy.
- Dev auth boundary smoke (hosted on `http://localhost:5088`): `POST /api/v1/auth/token` issued a token for `admin` (SecurityAdministrator); `GET /api/v1/users` returned 401 without a token and 200 with a valid token.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. Real Entra token validation requires a live tenant and is not exercised in this offline environment; the claim-mapping and options logic is covered by unit tests.

## Residual Risks and Deferrals

- Real Entra token validation against a live tenant, the Web.Client interactive sign-in flow (MSAL/redirect), and Entra app registration/tenant provisioning remain follow-up items.
- External legislative/fiscal connectors, Microsoft 365/SharePoint/Graph integration, registry publishing, image scanning, deployed Terraform infrastructure, GuardDuty/Security Hub, and cross-region DR remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local execution context.
