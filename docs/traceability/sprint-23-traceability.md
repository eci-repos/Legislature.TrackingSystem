# Sprint 23 - Production Hardening: External Connectors (Legislative, Fiscal, M365) Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 23 adds the external connector layer for the LTS integration boundaries: an external legislative source (F5.1), internal DOR fiscal data sources (F7.1), and Microsoft 365 productivity integration (F8.1). It defines connector contracts in the Application layer, provides HTTP adapters and dev-boundary fakes in Infrastructure, selects the active connector from configuration (mirroring the Entra auth boundary), and wires the connectors into the existing ingestion and productivity services.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 23 Evidence | Artifact |
| --- | --- | --- | --- |
| External legislative updates are ingested | F5.1, US-5.1.1/5.1.2/5.1.3 | `ILegislativeSourceConnector` fetches bill language, status, and amendments; `LegislativeIngestionService` uses it. | `Application/Connectors/ILegislativeSourceConnector.cs`, `LegislativeIngestionService.cs` |
| Fiscal data is retrieved from internal DOR systems | F7.1, US-7.1.1 | `IFiscalDataSourceConnector` fetches fiscal data points; `FiscalDataService` refreshes from it. | `Application/Connectors/IFiscalDataSourceConnector.cs`, `FiscalDataService.cs` |
| Microsoft 365 productivity integration | F8.1, US-8.1.1 | `IM365Connector` sends email and stores documents; `ProductivityIntegrationService` dispatches through it. | `Application/Connectors/IM365Connector.cs`, `ProductivityIntegrationService.cs` |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (167 domain + 23 integration). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 23 Evidence | Artifact |
| --- | --- | --- | --- |
| Connector contracts in the Application layer | Production hardening | Three connector interfaces and snapshot DTOs live in `Application/Connectors`. | `Application/Connectors/*.cs` |
| HTTP adapters in Infrastructure | Production hardening | `HttpLegislativeSourceConnector`, `HttpFiscalDataSourceConnector`, `GraphM365Connector` (client-credentials token acquisition). | `Infrastructure/Connectors/*.cs` |
| Dev-boundary fakes | Production hardening | `FakeLegislativeSourceConnector`, `FakeFiscalDataSourceConnector`, `FakeM365Connector` keep the app runnable offline. | `Infrastructure/Connectors/*.cs` |
| Config-driven selection | Production hardening | `ConnectorOptions` + `AddLtsConnectors` select the HTTP/Graph adapter when configured and the dev fake otherwise. | `Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs` |
| Service wiring | Production hardening | The three integration services use their connectors; dev fakes return no data so repository-backed behavior is preserved offline. | `Application/WorkItems/*.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| Connector contracts and snapshot DTOs exist in the Application layer | Implemented |
| HTTP adapters and dev-boundary fakes exist in Infrastructure | Implemented |
| `AddLtsConnectors` selects the HTTP adapter when configured and the dev fake otherwise | Implemented |
| The three integration services use their connectors | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 23 does not change application persistence. It adds the external connector transport layer; ingested data continues to persist through the existing repositories.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy; readiness returned 200 and the home page rendered the login control (dev boundary intact).

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. Real external endpoints (legislative API, DOR fiscal systems, Microsoft Graph) are not exercised in this offline environment; the connector wiring is verified by build, the config-driven selection tests, and the dev-boundary smoke tests.

## Residual Risks and Deferrals

- Real external endpoints (legislative API, DOR fiscal systems, Microsoft Graph) and their credentials/tenant provisioning remain follow-up items; the HTTP/Graph adapters are not exercised against live systems in this offline environment.
- The `GraphM365Connector` acquires a client-credentials token; interactive/on-behalf-of flows and SharePoint site targeting are not yet configured.
- Real Entra interactive sign-in against a live tenant, Entra app registration/tenant provisioning, and the redirect URI configuration remain follow-up items.
- Exact token/cost telemetry is unavailable in this local execution context.
