# Sprint 30 - Production Hardening: Persistence Policy Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 30 decides and enforces the persistence policy: the no-connection in-memory fallback remains a supported local/offline development mode, but production requires PostgreSQL. It adds a persistence policy document and a startup enforcement that fails fast in Production and warns in non-Production when running in-memory.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 30 Evidence | Artifact |
| --- | --- | --- | --- |
| The persistence policy is decided and documented | Production hardening | `docs/persistence-policy.md` decides the in-memory fallback status, the production requirement, the boundary, parity guarantees, and the migration path. | `docs/persistence-policy.md` |
| The persistence policy is enforced at startup | Production hardening | `PersistencePolicy` resolves the mode from the connection string and the server fails fast in Production (and warns in non-Production) when running in-memory. | `src/Legislature.TrackingSystem.Application/Persistence/PersistencePolicy.cs`, `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (191 domain + 23 infrastructure + 12 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 30 Evidence | Artifact |
| --- | --- | --- | --- |
| Mode resolution | Production hardening | `PersistencePolicy.ResolveMode` returns `Postgres` when a connection string is configured and `InMemory` otherwise. | `src/Legislature.TrackingSystem.Application/Persistence/PersistencePolicy.cs` |
| Policy enforcement | Production hardening | `PersistencePolicy.IsAllowed` returns false for in-memory in Production; the server throws at startup and logs a warning in non-Production. | `src/Legislature.TrackingSystem.Application/Persistence/PersistencePolicy.cs`, `src/Legislature.TrackingSystem.Web/Program.cs` |
| Documented decision | Production hardening | The policy document records the decision, rationale, parity guarantees, and migration path. | `docs/persistence-policy.md` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| A persistence policy document decides the in-memory fallback status and the production requirement | Implemented |
| A `PersistencePolicy` resolves the mode and enforces the policy at startup (fail fast in Production, warn in non-Production) | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 30 decides the persistence policy: the in-memory fallback remains a supported local/offline development mode, but production requires PostgreSQL. It does not change the persistence implementation.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 191 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, Development + PostgreSQL):
  - `GET /` → 200 (client app serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. The persistence policy enforcement is exercised by unit tests; the container smoke test runs in Development with PostgreSQL, so the in-memory warning path is not triggered there.

## Residual Risks and Deferrals

- The in-memory fallback is not a production persistence target; it is retained for local/offline development.
- A future sprint may add a runtime mode indicator if observability requires it.
- Exact token/cost telemetry is unavailable in this local execution context.
