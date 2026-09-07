# Sprint 23 - Production Hardening: External Connectors (Legislative, Fiscal, M365) - PM Validation Report

Date: 2026-09-06

Sprint: Sprint 23

Status: Complete

## 1. Scope Summary

Sprint 23 adds the external connector layer for the LTS integration boundaries: an external legislative source (F5.1), internal DOR fiscal data sources (F7.1), and Microsoft 365 productivity integration (F8.1). It defines connector contracts in the Application layer, provides HTTP adapters and dev-boundary fakes in Infrastructure, selects the active connector from configuration (mirroring the Entra auth boundary), and wires the connectors into the existing ingestion and productivity services.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 23 Evidence |
| --- | --- | --- |
| External legislative updates are ingested | F5.1, US-5.1.1/5.1.2/5.1.3 | `ILegislativeSourceConnector` fetches bill language, status, and amendments; `LegislativeIngestionService` uses it. |
| Fiscal data is retrieved from internal DOR systems | F7.1, US-7.1.1 | `IFiscalDataSourceConnector` fetches fiscal data points; `FiscalDataService` refreshes from it. |
| Microsoft 365 productivity integration | F8.1, US-8.1.1 | `IM365Connector` sends email and stores documents; `ProductivityIntegrationService` dispatches through it. |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (167 domain + 23 integration). |

## 3. Acceptance Evidence

- Connector contracts and snapshot DTOs exist in the Application layer.
- HTTP adapters and dev-boundary fakes exist in Infrastructure.
- `AddLtsConnectors` selects the HTTP adapter when configured and the dev fake otherwise.
- The three integration services use their connectors.

## 4. Verification Results

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy; readiness returned 200 and the home page rendered the login control (dev boundary intact).

## 5. Delivered Artifacts

- `src/Legislature.TrackingSystem.Application/Connectors/` — connector contracts, snapshot DTOs, and `ConnectorOptions`.
- `src/Legislature.TrackingSystem.Infrastructure/Connectors/` — HTTP/Graph adapters and dev-boundary fakes.
- `src/Legislature.TrackingSystem.Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs` — `AddLtsConnectors`.
- `src/Legislature.TrackingSystem.Application/WorkItems/LegislativeIngestionService.cs`, `FiscalDataService.cs`, `ProductivityIntegrationService.cs` — connector wiring.
- `src/Legislature.TrackingSystem.Web/Program.cs` and `appsettings.json` — connector registration and `Connectors` section.
- `tests/.../Connectors/ConnectorOptionsTests.cs` and `tests/.../ConnectorRegistrationTests.cs` — new tests.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-23-traceability.md`, `docs/validation/2026-09-06-sprint-23-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- Real external endpoints (legislative API, DOR fiscal systems, Microsoft Graph) and their credentials/tenant provisioning remain follow-up items; the HTTP/Graph adapters are not exercised against live systems in this offline environment.
- The `GraphM365Connector` acquires a client-credentials token; interactive/on-behalf-of flows and SharePoint site targeting are not yet configured.
- Real Entra interactive sign-in against a live tenant, Entra app registration/tenant provisioning, and the redirect URI configuration remain follow-up items.
- Exact token/cost telemetry is unavailable in this local execution context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 23 is implemented, verified, and documented. The external connector layer is in place with contracts, HTTP/Graph adapters, dev-boundary fakes, config-driven selection, and service wiring. The .NET solution builds and tests pass, and the dev boundary remains functional offline.

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
