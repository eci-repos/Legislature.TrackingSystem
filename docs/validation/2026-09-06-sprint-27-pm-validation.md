# Sprint 27 - Production Hardening: Real Entra Tenant Provisioning - PM Validation Report

Date: 2026-09-06

Sprint: Sprint 27

Status: Complete

## 1. Scope Summary

Sprint 27 prepares the existing Entra/OpenID Connect authentication boundary for a real Microsoft Entra ID tenant. It adds startup configuration validation that fails fast with actionable errors, completes the client-side OIDC wiring (redirect URI, scopes, authority), and provides the provisioning artifacts (a runbook and a declarative Terraform module) for registering the app, configuring API permissions and redirect URIs, and mapping Entra roles/groups to the LTS role-to-permission matrix.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 27 Evidence |
| --- | --- | --- |
| The Entra configuration is validated at startup | Production hardening | `EntraAuthOptionsValidator` validates the `AzureAd` section; the server throws with actionable errors when it is invalid. |
| The client OIDC wiring is complete | Production hardening | The client sets a `RedirectUri` and requires `AzureAd:Authority` for interactive sign-in. |
| A provisioning runbook documents the tenant setup | Production hardening | `docs/entra-provisioning-runbook.md` documents tenant creation, app registration, client secret, API permissions, redirect URIs, and role/group mappings. |
| A declarative module provisions the app registration | Production hardening | `infra/terraform-entra/` provisions the app registration, redirect URI, app roles, Graph permissions, client secret, and service principal. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (175 domain + 23 infrastructure + 12 web). |

## 3. Acceptance Evidence

- The `AzureAd` configuration is validated at startup with actionable errors.
- The client OIDC wiring is complete (redirect URI, scopes, authority) and validated.
- A provisioning runbook documents the Entra tenant, app registration, client secret, API permissions, redirect URIs, and role/group mappings.
- A declarative Terraform module provisions the Entra app registration.

## 4. Verification Results

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 175 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, no `AzureAd` configured):
  - `GET /` → 200 (client app serves).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.
  - `GET /api/v1/users` without a token → 401; with a valid token → 200 (API authorization still enforced).
  - `GET /health` → 200.

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Application/Authentication/EntraAuthOptionsValidator.cs` — startup configuration validation.
- `src/Legislature.TrackingSystem.Web/Program.cs` — startup enforcement of the Entra configuration validation.
- `src/Legislature.TrackingSystem.Web.Client/Program.cs` — client OIDC redirect URI and authority requirement.
- `docs/entra-provisioning-runbook.md` — Entra tenant provisioning runbook.
- `infra/terraform-entra/main.tf`, `variables.tf`, `outputs.tf`, `README.md` — declarative Entra app-registration module.
- `tests/Legislature.TrackingSystem.Domain.Tests/Authentication/EntraAuthOptionsValidatorTests.cs` — validator tests.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-27-traceability.md`, `docs/validation/2026-09-06-sprint-27-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- A real Entra tenant is not provisioned from this offline workspace; the runbook and Terraform module document and declare the provisioning.
- Client secret rotation and certificate-based authentication are operational follow-ups.
- Conditional access, device compliance, and MFA policies are tenant-level follow-ups.
- Exact token/cost telemetry is unavailable in this local execution context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 27 is implemented, verified, and documented. The Entra/OpenID Connect boundary is now production-ready: the `AzureAd` configuration is validated at startup with actionable errors, the client OIDC wiring is complete (redirect URI and authority), and the provisioning artifacts (runbook and Terraform module) document and declare the tenant, app registration, client secret, API permissions, redirect URIs, and role/group mappings. The .NET solution builds and tests pass, and the container smoke test confirms the dev boundary still serves the app and enforces API authorization.

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | deepseek-v4-flash:0731-cloud |
| Token count | Unavailable (telemetry not exposed in this environment) |
| Cost | Unavailable (no billing source exposed; not estimated) |
| Elapsed time | Unavailable (not captured) |
| Tool/run counts | Unavailable (not captured) |
| Verification runs | `dotnet format` (1), `dotnet build` (final pass recorded), `dotnet test` (final full solution pass), container rebuild/restart smoke (multiple; final token 200, home 200, users 401/200, health 200) |

> AI Metrics note: token/cost telemetry is not available in this execution environment. Values are marked unavailable rather than estimated, per the project's AI Metrics policy.
