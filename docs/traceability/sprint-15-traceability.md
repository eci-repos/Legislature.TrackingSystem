# Sprint 15 - Production Hardening: Authentication, Docker, and CI/CD Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 15 begins the production-hardening and transition track: JWT-based authentication/authorization enforcing the role-to-permission matrix at the HTTP layer, Docker hardening (non-root, healthcheck), CI/CD, and infrastructure-as-code plus operations (WAF/SIEM/backup/DR). PostgreSQL persistence is deferred to a dedicated follow-up sprint.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 15 Evidence | Artifact |
| --- | --- | --- | --- |
| US-9.1.1 role-to-permission matrix enforced at HTTP layer | B.COM.06 | JWT bearer tokens carry the role claim; `PermissionAuthorizationHandler` enforces the matrix via `PermissionMatrix`; security/administration endpoints are protected by permission policies. | `Web/Auth/JwtTokenService.cs`, `Web/Auth/PermissionAuthorizationHandler.cs`, `Application/WorkItems/PermissionMatrix.cs`, `Web/Program.cs` |
| US-9.1.1 least-privilege permissions | B.COM.06 | `PermissionMatrix` is the single authoritative matrix shared by the application service and the HTTP handler. | `Application/WorkItems/PermissionMatrix.cs` |
| TR-601 versioned RESTful API surface | TR-601 | Token endpoint added under `/api/v1/auth/token`; existing `/api/v1` endpoints unchanged. | `Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `TreatWarningsAsErrors` enforced. | `dotnet format`, `Directory.Build.props` |
| TR-902 automated tests asserting acceptance criteria | TR-902 | 147 tests pass (0 failed); 4 new tests verify the role-to-permission matrix. | `tests/.../PermissionMatrixTests.cs` |

## Technical Quality Trace

| Requirement | Source | Sprint 15 Evidence | Artifact |
| --- | --- | --- | --- |
| Non-root container execution | Production hardening | Final image runs as the non-root `app` user. | `src/Legislature.TrackingSystem.Web/Dockerfile` |
| Container healthcheck | Production hardening | Readiness healthcheck via `curl` on `/api/v1/readiness`. | `src/Legislature.TrackingSystem.Web/Dockerfile` |
| CI/CD enforcement | Production hardening | GitHub Actions workflow runs format, build, and test on push/PR to `main`. | `.github/workflows/ci.yml` |
| Infrastructure-as-code | Production hardening | Terraform declares VPC, ALB + WAF, ECS Fargate, RDS PostgreSQL with backups, CloudWatch, SSM secrets. | `infra/terraform/*.tf` |
| Operations (WAF/SIEM/backup/DR) | Production hardening | Operations README documents WAF rule groups, logging, backup/DR, and secrets. | `infra/README.md` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| JWT token endpoint issues signed bearer tokens with the role claim | Implemented |
| Security/administration endpoints protected by permission policies; 401 without token, 403 without permission | Implemented |
| Web.Client transparently acquires a token so existing pages remain functional | Implemented |
| Docker image runs as non-root with a healthcheck; container builds and starts | Implemented |
| GitHub Actions CI workflow runs format, build, and test | Implemented |
| Terraform and operations documentation declare the target topology and WAF/SIEM/backup/DR posture | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks (incl. JWT auth flow) and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

PostgreSQL persistence with EF Core migrations is deferred to a dedicated follow-up sprint (Sprint 16). Sprint 15 continues the interim in-memory repository adapter; the Terraform topology declares the target RDS PostgreSQL instance but the application does not yet connect to it.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 147 tests (0 failed, 0 skipped).
- `docker compose config --quiet` passed.
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`); the hardened image runs as a non-root user with a healthcheck.
- JWT auth smoke against `http://localhost:5088`: `POST /api/v1/auth/token` issued a token for `admin`; a protected endpoint returned 401 without a token and 200 with a valid token; a token for a role lacking the required permission returned 403.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 15 defects; the standard AGENTS.md commands work on a normal developer/container host.
