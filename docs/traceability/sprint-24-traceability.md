# Sprint 24 - Production Hardening: Observability (Logging, Health, Telemetry) Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 24 adds production observability to the LTS web application: standard health-check endpoints (liveness and readiness), structured JSON console logging, and OpenTelemetry tracing/metrics with an OTLP exporter. The health checks are wired into the container healthcheck and telemetry is config-driven so it is off by default and enabled when an OTLP endpoint is configured.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 24 Evidence | Artifact |
| --- | --- | --- | --- |
| The solution exposes health and readiness | Production hardening | `/health` (liveness) and `/health/ready` (readiness) endpoints; the readiness check verifies the database when configured. | `src/Legislature.TrackingSystem.Web/Program.cs`, `Observability/DatabaseHealthCheck.cs` |
| The solution emits structured logs | Production hardening | `AddJsonConsole` configures structured JSON console output; the readiness endpoint logs structured messages. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| The solution emits telemetry | Production hardening | OpenTelemetry tracing and metrics with an OTLP exporter, config-driven (off by default). | `src/Legislature.TrackingSystem.Web/Program.cs`, `Observability/Observability.cs` |
| The container healthcheck reflects app health | Production hardening | The Dockerfile healthcheck uses the `/health` endpoint. | `src/Legislature.TrackingSystem.Web/Dockerfile` |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (167 domain + 23 integration + 1 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 24 Evidence | Artifact |
| --- | --- | --- | --- |
| Liveness vs readiness separation | Production hardening | `/health` excludes readiness-tagged checks; `/health/ready` runs the database check. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| Database readiness check | Production hardening | `DatabaseHealthCheck` verifies the PostgreSQL connection from a scoped provider. | `Observability/DatabaseHealthCheck.cs` |
| Structured logging | Production hardening | `AddJsonConsole` and structured `ILogger` message templates. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| OpenTelemetry wiring | Production hardening | `AddOpenTelemetry` with resource, tracing, metrics, and OTLP exporter; central activity source and meter. | `src/Legislature.TrackingSystem.Web/Program.cs`, `Observability/Observability.cs` |
| Config-driven telemetry | Production hardening | Telemetry is enabled only when `Otlp:Endpoint` is configured. | `appsettings.json`, `Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| `/health` and `/health/ready` endpoints exist; the readiness check verifies the database when configured | Implemented |
| Structured JSON console logging is configured and key services log structured messages | Implemented |
| OpenTelemetry is wired with an OTLP exporter and is config-driven (off by default) | Implemented |
| The Dockerfile healthcheck uses `/health` | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 24 does not change application persistence. It adds observability (health, logging, telemetry).

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests + 1 web test (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy; `/health` and `/health/ready` returned Healthy and the home page rendered the login control.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. OpenTelemetry instrumentation packages (ASP.NET Core/Http) and the OTLP logs exporter are not in the offline NuGet cache, so the app emits its own spans/counters and OTLP logging is deferred; the OTLP exporter is not exercised against a live collector in this offline environment.

## Residual Risks and Deferrals

- OpenTelemetry ASP.NET Core/Http instrumentation packages and the OTLP logs exporter are not available in the offline cache; the app emits its own spans/counters and OTLP log export is deferred.
- The OTLP exporter is not exercised against a live collector in this offline environment; an `Otlp:Endpoint` must be configured to enable telemetry.
- Real external endpoints (legislative API, DOR fiscal systems, Microsoft Graph) and real Entra tenant provisioning remain follow-up items.
- Exact token/cost telemetry is unavailable in this local execution context.
