# Sprint 20 - Production Hardening: Entra/OpenID Connect Authentication Boundary - PM Validation Report

Date: 2026-09-06

Sprint: Sprint 20

Status: Complete

## 1. Scope Summary

Sprint 20 replaces the JWT-only authentication boundary with a real Microsoft Entra ID / OpenID Connect identity integration, while keeping the local/offline development boundary functional. The app validates bearer tokens against the Entra tenant authority when `AzureAd` configuration is present, maps Entra claims to the LTS user model and role-to-permission matrix, and falls back to the existing symmetric-key JWT dev boundary when Entra is not configured.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 20 Evidence |
| --- | --- | --- |
| Role-based permissions distinguish preparation, approval, and delivery; no restricted functions outside assigned permissions | US-9.1.1, B.COM.06 | Entra `roles`/`groups` claims map to LTS `UserRole` via configurable mappings, and a `ClaimTypes.Role` claim is applied so the existing role-to-permission matrix handler enforces the matrix. |
| Authorized users can access the solution with real identity | Production hardening; US-9.1.x | JWT bearer validates against the Entra tenant authority (issuer, audience, OIDC-metadata signing keys) when `AzureAd` is configured. |
| TR-601 versioned RESTful API surface | TR-601 | The `/api/v1/auth/token` dev endpoint remains functional for local/offline use; the versioned API surface is unchanged. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | 12 unit tests cover the Entra options and claim mapping. |

## 3. Acceptance Evidence

- `EntraAuthOptions` binds from the `AzureAd` section and exposes `IsConfigured` and `Issuer`/`Authority`.
- `EntraClaimsMapper` extracts the user key from `oid`/`upn`/`preferred_username`, maps `roles`/`groups` claims to `UserRole`, and applies a `ClaimTypes.Role` claim for the existing authorization handler.
- Program.cs selects the Entra boundary when `AzureAd` is configured and the symmetric-key JWT dev boundary otherwise.
- The `/api/v1/auth/token` dev endpoint remains functional for local/offline use.
- 12 unit tests cover the Entra options and claim mapping.

## 4. Verification Results

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy.
- Dev auth boundary smoke (hosted on `http://localhost:5088`): `POST /api/v1/auth/token` issued a token for `admin` (SecurityAdministrator); `GET /api/v1/users` returned 401 without a token and 200 with a valid token.

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Application/Authentication/EntraAuthOptions.cs` — Entra configuration options.
- `src/Legislature.TrackingSystem.Application/Authentication/EntraClaimsMapper.cs` — Entra claim-to-user-model mapping.
- `src/Legislature.TrackingSystem.Web/Program.cs` — configuration-driven auth selection (Entra vs dev JWT).
- `src/Legislature.TrackingSystem.Web/appsettings.json` — `AzureAd` configuration section.
- `tests/Legislature.TrackingSystem.Domain.Tests/Authentication/EntraAuthTests.cs` — unit tests.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-20-traceability.md`, `docs/validation/2026-09-06-sprint-20-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- Real Entra token validation against a live tenant, the Web.Client interactive sign-in flow (MSAL/redirect), and Entra app registration/tenant provisioning remain follow-up items.
- External legislative/fiscal connectors, Microsoft 365/SharePoint/Graph integration, registry publishing, image scanning, deployed Terraform infrastructure, GuardDuty/Security Hub, and cross-region DR remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local execution context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 20 is implemented, verified, and documented. The authentication boundary now supports real Microsoft Entra ID / OpenID Connect validation when configured, with the existing JWT dev boundary preserved as the no-configuration fallback. The claim-mapping logic is covered by unit tests, and the dev auth boundary is smoke-verified against the running container.

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
