# Sprint 28 - Production Hardening: External Connector Live Endpoints Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 28 prepares the external connector layer (legislative source, DOR fiscal data, Microsoft 365) for live endpoints: it adds startup configuration validation that fails fast with actionable errors, adds a connector connectivity health check integrated into readiness, and provides a provisioning runbook for the live endpoints.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 28 Evidence | Artifact |
| --- | --- | --- | --- |
| The connector configuration is validated at startup | Production hardening | `ConnectorOptionsValidator` validates the `Connectors` section and the server throws with actionable errors when a configured connector is invalid. | `src/Legislature.TrackingSystem.Application/Connectors/ConnectorOptionsValidator.cs`, `src/Legislature.TrackingSystem.Web/Program.cs` |
| Connector connectivity is verified | Production hardening | `ConnectorHealthCheck` probes each configured connector and is registered as a readiness check on `/health/ready`. | `src/Legislature.TrackingSystem.Web/Observability/ConnectorHealthCheck.cs`, `src/Legislature.TrackingSystem.Web/Program.cs` |
| A provisioning runbook documents the live endpoints | Production hardening | `docs/connector-provisioning-runbook.md` documents the legislative, fiscal, and Microsoft 365 endpoint provisioning and configuration. | `docs/connector-provisioning-runbook.md` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (182 domain + 23 infrastructure + 12 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 28 Evidence | Artifact |
| --- | --- | --- | --- |
| Fail-fast configuration validation | Production hardening | `ConnectorOptionsValidator.Validate` returns actionable problems (non-HTTPS base URL, missing API key, incomplete M365 credentials). | `src/Legislature.TrackingSystem.Application/Connectors/ConnectorOptionsValidator.cs` |
| Startup enforcement | Production hardening | The server throws `InvalidOperationException` with the validation problems when a connector is configured and invalid. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| Readiness connectivity check | Production hardening | `ConnectorHealthCheck` reports healthy/degraded/unhealthy based on probes of the configured connectors; reports healthy when none are configured. | `src/Legislature.TrackingSystem.Web/Observability/ConnectorHealthCheck.cs` |
| Declarative provisioning guidance | Production hardening | The runbook documents the endpoint URLs, auth, configuration sections, and verification for each connector. | `docs/connector-provisioning-runbook.md` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| The `Connectors` configuration is validated at startup with actionable errors | Implemented |
| A connector connectivity health check is integrated into `/health/ready` | Implemented |
| A provisioning runbook documents the live legislative, fiscal, and Microsoft 365 endpoints | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 28 does not change application persistence. It hardens the external connector layer and its provisioning artifacts.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 182 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, no `Connectors` configured):
  - `GET /` → 200 (client app serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness; the `connectors` check reports healthy with no connectors configured).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. Live external endpoints cannot be provisioned from this offline workspace; the runbook documents the provisioning, and the startup validation and health check are exercised by unit tests and the dev-boundary smoke test.

## Residual Risks and Deferrals

- Live external endpoints are not provisioned from this offline workspace; the runbook documents the provisioning.
- The exact legislative and fiscal API contracts must be confirmed against the live systems.
- The M365 Graph scopes and client-credentials flow must be confirmed against the tenant's app registration.
- Exact token/cost telemetry is unavailable in this local execution context.
