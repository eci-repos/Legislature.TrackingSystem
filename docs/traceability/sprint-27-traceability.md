# Sprint 27 - Production Hardening: Real Entra Tenant Provisioning Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 27 prepares the existing Entra/OpenID Connect authentication boundary for a real Microsoft Entra ID tenant: it adds startup configuration validation that fails fast with actionable errors, completes the client-side OIDC wiring (redirect URI, scopes, authority), and provides the provisioning artifacts (a runbook and a declarative Terraform module) for registering the app, configuring API permissions and redirect URIs, and mapping Entra roles/groups to the LTS role-to-permission matrix.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 27 Evidence | Artifact |
| --- | --- | --- | --- |
| The Entra configuration is validated at startup | Production hardening | `EntraAuthOptionsValidator` validates the `AzureAd` section and the server throws with actionable errors when it is invalid. | `src/Legislature.TrackingSystem.Application/Authentication/EntraAuthOptionsValidator.cs`, `src/Legislature.TrackingSystem.Web/Program.cs` |
| The client OIDC wiring is complete | Production hardening | The client sets a `RedirectUri` (defaulting to `/authentication/login-callback`) and requires `AzureAd:Authority` for interactive sign-in. | `src/Legislature.TrackingSystem.Web.Client/Program.cs` |
| A provisioning runbook documents the tenant setup | Production hardening | `docs/entra-provisioning-runbook.md` documents tenant creation, app registration, client secret, API permissions, redirect URIs, and role/group mappings. | `docs/entra-provisioning-runbook.md` |
| A declarative module provisions the app registration | Production hardening | `infra/terraform-entra/` provisions the app registration, redirect URI, app roles, Graph permissions, client secret, and service principal. | `infra/terraform-entra/*.tf`, `README.md` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (175 domain + 23 infrastructure + 12 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 27 Evidence | Artifact |
| --- | --- | --- | --- |
| Fail-fast configuration validation | Production hardening | `EntraAuthOptionsValidator.Validate` returns a list of actionable problems (missing instance/tenant/client id, non-HTTPS instance, invalid role/group mapping values, no role/group mappings). | `src/Legislature.TrackingSystem.Application/Authentication/EntraAuthOptionsValidator.cs` |
| Startup enforcement | Production hardening | The server throws `InvalidOperationException` with the validation problems when `AzureAd` is configured and invalid. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| Client redirect URI | Production hardening | `RedirectUri` defaults to `{base}/authentication/login-callback` and is set on the OIDC provider options. | `src/Legislature.TrackingSystem.Web.Client/Program.cs` |
| Client authority requirement | Production hardening | The client throws if `AzureAd:Authority` is missing for interactive sign-in. | `src/Legislature.TrackingSystem.Web.Client/Program.cs` |
| Declarative provisioning | Production hardening | Terraform module provisions the app registration, app roles, Graph delegated permissions, client secret, and service principal. | `infra/terraform-entra/` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| The `AzureAd` configuration is validated at startup with actionable errors | Implemented |
| The client OIDC wiring is complete (redirect URI, scopes, authority) and validated | Implemented |
| A provisioning runbook documents the Entra tenant, app registration, client secret, API permissions, redirect URIs, and role/group mappings | Implemented |
| A declarative Terraform module provisions the Entra app registration | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 27 does not change application persistence. It hardens the Entra/OpenID Connect authentication boundary and its provisioning artifacts.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 175 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, no `AzureAd` configured):
  - `GET /` → 200 (client app serves).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.
  - `GET /api/v1/users` without a token → 401; with a valid token → 200 (API authorization still enforced).
  - `GET /health` → 200.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. A real Entra tenant cannot be provisioned from this offline workspace; the runbook and Terraform module are the provisioning artifacts, and the startup validation is exercised by unit tests.

## Residual Risks and Deferrals

- A real Entra tenant is not provisioned from this offline workspace; the runbook and Terraform module document and declare the provisioning.
- Client secret rotation and certificate-based authentication are operational follow-ups.
- Conditional access, device compliance, and MFA policies are tenant-level follow-ups.
- Exact token/cost telemetry is unavailable in this local execution context.
