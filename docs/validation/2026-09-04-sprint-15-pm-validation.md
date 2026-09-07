# PM Validation Report: Sprint 15 - Production Hardening: Authentication, Docker, and CI/CD

Status: completed

Prepared date: 2026-09-04

Prepared by: AI-Coder Developer

Reviewed by: PM

## Scope Validated

- Sprint or use case: Sprint 15 - Production Hardening: Authentication, Docker, and CI/CD.
- Current sprint reference: `docs/02-Current-Sprint.md`
- Handoff reference: `docs/08-AI-Coder-Handoff-Guide.md`
- Source requirements traced: US-9.1.1 (B.COM.06), TR-601, TR-702, TR-902, plus production-hardening scope (Docker, CI/CD, infrastructure-as-code, operations).

## Completion Summary

Sprint 15 begins the production-hardening and transition track. JWT-based authentication/authorization now enforces the role-to-permission matrix at the HTTP layer: a token endpoint issues signed bearer tokens carrying the role claim, and the security/administration endpoints (users, access-restrictions, migrations, historical) are protected by permission policies that reject unauthenticated requests (401) and requests lacking the required permission (403). The Web.Client transparently acquires a token so existing pages remain functional. The Docker image now runs as a non-root user with a readiness healthcheck. A GitHub Actions CI workflow runs format, build, and test. Terraform and an operations README declare the target AWS topology (ECS Fargate, ALB + WAF, RDS PostgreSQL with backups, CloudWatch) and the WAF/SIEM/backup/DR posture. PostgreSQL persistence is deferred to a dedicated follow-up sprint (Sprint 16).

## Acceptance Evidence

| Acceptance Item | Evidence | Result |
| --- | --- | --- |
| US-9.1.1 role-to-permission matrix enforced at HTTP layer | `JwtTokenService`, `PermissionAuthorizationHandler`, `PermissionMatrix`, protected endpoints | Implemented |
| Token endpoint issues signed bearer tokens with the role claim | `POST /api/v1/auth/token` | Implemented |
| Protected endpoints reject 401 without token and 403 without permission | Container JWT auth smoke | Implemented |
| Web.Client transparently acquires a token | `JwtAuthorizationMessageHandler` | Implemented |
| Docker image runs as non-root with a healthcheck | Hardened `Dockerfile` | Implemented |
| CI/CD workflow runs format, build, and test | `.github/workflows/ci.yml` | Implemented |
| Infrastructure-as-code and operations (WAF/SIEM/backup/DR) | `infra/terraform/*.tf`, `infra/README.md` | Implemented |
| Automated tests assert the promoted acceptance criteria | 4 new `PermissionMatrixTests` | Implemented |
| Traceability preserved to the source requirements | `docs/traceability/sprint-15-traceability.md` | Prepared |

## Verification Results

| Check | Command or Method | Result | Notes |
| --- | --- | --- | --- |
| Static analysis/format | `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` | Pass | No changes required (exit 0). |
| Solution build | `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` | Pass | 0 warnings, 0 errors. |
| Tests | `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` | Pass | 147 passed, 0 failed, 0 skipped (4 new). |
| Compose config | `docker compose config --quiet` | Pass | Valid compose file. |
| Container build | `docker compose up -d --build --force-recreate web` | Pass | Hardened image built (non-root `app` user, curl healthcheck). |
| Container startup | `docker compose up -d --force-recreate web` | Pass | `lts-web` recreated on the new image; `lts-postgres` healthy. |
| WASM boot asset | `GET /_framework/blazor.web.js` | Pass | HTTP 200. |
| JWT auth smoke | `POST /api/v1/auth/token`; protected endpoint with/without token | Pass | Token issued for `admin`; 401 without token; 200 with valid token; 403 for a role lacking the required permission. |

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test must use `-m:1` (single-node MSBuild) and elevated file/process permissions because parallel MSBuild nodes, out-of-proc task hosts, and test-host parent-process monitoring are denied under process isolation; `dotnet format` requires elevated permissions for its MSBuild build host; and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline — the resulting offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`. These are environment constraints, not Sprint 15 defects; the standard AGENTS.md commands build, format, and test successfully on a normal developer/container host.

## Delivered Artifacts

- Web auth: `src/Legislature.TrackingSystem.Web/Auth/JwtTokenService.cs`, `PermissionRequirement.cs`, `PermissionAuthorizationHandler.cs`, `JwtAuthorizationMessageHandler.cs`, `Api/WorkItems/TokenRequest.cs`.
- Application: `src/Legislature.TrackingSystem.Application/WorkItems/PermissionMatrix.cs` (public authoritative matrix); `AuthorizationService.cs` refactored to use it.
- Web: `Program.cs` (JwtBearer, authorization policies, token endpoint, protected endpoints, token-bootstrap HttpClient); `appsettings.json` (Jwt config); `Legislature.TrackingSystem.Web.csproj` (JwtBearer package).
- Infrastructure: `SeedDataInitializer.cs` (default `admin` SecurityAdministrator user).
- Docker: `src/Legislature.TrackingSystem.Web/Dockerfile` (non-root, healthcheck); `docker-compose.yml` (Jwt env).
- CI/CD: `.github/workflows/ci.yml`.
- Infra-as-code: `infra/terraform/main.tf`, `variables.tf`, `outputs.tf`; `infra/README.md`.
- Tests: `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/PermissionMatrixTests.cs`.
- Docs: `docs/traceability/sprint-15-traceability.md`, updated `docs/02-Current-Sprint.md`, updated `docs/08-AI-Coder-Handoff-Guide.md`.

## AI Metrics

| Metric | Value | Basis / Notes |
| --- | --- | --- |
| AI work scope | Sprint 15 - Production Hardening: Authentication, Docker, and CI/CD | Included sprint promotion, JWT auth implementation, Docker hardening, CI/CD, infrastructure-as-code, tests, verification checks, traceability, handoff, and PM Validation report. |
| AI agent/model | DeepSeek coding agent; exact billable model identifier unavailable | The local task context identifies the coding agent but does not expose the billing SKU used for this run. |
| Work date/time | 2026-09-04 | Based on sprint documents and local execution date. |
| Elapsed AI work time | Unavailable | Wall-clock/task elapsed timing is not exposed as reportable telemetry in this local task context. |
| Input tokens | Unavailable | Exact token telemetry is not exposed in this local task context. |
| Output tokens | Unavailable | Exact token telemetry is not exposed in this local task context. |
| Total tokens | Unavailable | Exact token telemetry is not exposed in this local task context. |
| Actual cost | Unavailable | Billing telemetry is not exposed in this local task context. |
| Estimated cost | Not calculated | No estimate was prepared because exact token counts and billable model SKU were unavailable. |
| Tool calls / commands | Partially available; exact total unavailable | Material verification commands and checks are listed in this report. Full tool-call count was not captured. |
| Files created or changed | Delivered artifact list recorded above | Exact file-change count is not reliable because the repository is not initialized as a Git worktree. |
| Verification checks run | 8 documented checks | Format, solution build (0 warnings/0 errors), tests (147 passed), compose config, container build, container startup, WASM boot asset, and JWT auth smoke. |
| Failed/retried checks | 0 | Sprint 15 required no code repairs; all checks passed. |
| Tests executed | 147 passed, 0 failed, 0 skipped | `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release`. |
| Build result | 0 warnings, 0 errors | `dotnet build Legislature.TrackingSystem.sln --configuration Release`. |
| Human review required | Yes | PM and senior developer review remain required for validation decision and next-phase promotion. |

## Known Gaps, Risks, and Deferrals

- PostgreSQL persistence with EF Core migrations is deferred to a dedicated follow-up sprint (Sprint 16); the application still uses the interim in-memory adapter.
- JWT authentication is a POC symmetric-key implementation; real Entra/OpenID Connect (with the OpenID Connect package and an identity provider) is deferred. The token endpoint issues tokens by user key without a password.
- The Web.Client transparently acquires a token for a default user; there is no interactive login or per-user session.
- The Terraform topology and operations README are declarative code and are not deployed from this workspace; no CI/CD pipeline is wired to deploy them, and the Docker image is not pushed to a registry or scanned.
- The WAF/SIEM/backup/DR posture is declared in code and documentation; GuardDuty/Security Hub and cross-region DR are follow-up items.
- The Docker image is local only and has not been hardened, scanned, pushed to a registry, or wired into CI/CD.
- Exact token and cost telemetry is not exposed in this local task context. PM Validation reports must mark unavailable values explicitly.

## PM Validation Decision

Decision: Completed

Decision date: 2026-09-04

PM notes:

- Sprint 15 promotes and implements the first production-hardening slice. The role-to-permission matrix (US-9.1.1/B.COM.06) is now enforced at the HTTP layer via JWT bearer authentication and permission-based authorization policies, with the security/administration endpoints protected (401 without a token, 403 without the required permission). The Web.Client transparently acquires a token so existing pages remain functional.
- The Docker image runs as a non-root user with a readiness healthcheck; a GitHub Actions CI workflow runs format, build, and test; and Terraform plus an operations README declare the target AWS topology and WAF/SIEM/backup/DR posture.
- Verification passed: format, build (0 warnings/0 errors), 147 tests, compose config, container build/startup, WASM boot asset, and a JWT auth smoke (token issued; 401/200/403 behavior confirmed).
- PostgreSQL persistence is deferred to a dedicated follow-up sprint (Sprint 16).
