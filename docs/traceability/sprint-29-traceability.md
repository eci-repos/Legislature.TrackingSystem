# Sprint 29 - Production Hardening: Deployment & Operations Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 29 completes the deployment & operations pipeline: it adds a Terraform state backend (S3 + DynamoDB lock), a deployment runbook, CI hardening (Terraform validation on pull requests), and wires the Entra app-registration module into the deploy pipeline.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 29 Evidence | Artifact |
| --- | --- | --- | --- |
| The AWS topology has a durable, locked remote state | Production hardening | The S3 backend references a DynamoDB lock table; `state.tf` manages the lock table; the runbook documents the bootstrap. | `infra/terraform/main.tf`, `infra/terraform/state.tf`, `docs/deployment-runbook.md` |
| The deployment pipeline is documented | Production hardening | `docs/deployment-runbook.md` documents prerequisites, GitHub secrets, CI/CD workflows, the AWS topology, GuardDuty/Security Hub, DR, and how to run the pipeline. | `docs/deployment-runbook.md` |
| CI validates the Terraform modules | Production hardening | The CI workflow runs `terraform fmt -check` and `terraform validate` for the AWS and Entra modules on push/PR. | `.github/workflows/ci.yml` |
| The deploy pipeline wires the Entra module | Production hardening | The deploy workflow adds a `terraform-entra` job that validates, plans, and applies the Entra app-registration module. | `.github/workflows/deploy.yml` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (182 domain + 23 infrastructure + 12 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 29 Evidence | Artifact |
| --- | --- | --- | --- |
| Remote state with locking | Production hardening | The S3 backend block references `lts-terraform-lock` with `encrypt = true`; `state.tf` manages the DynamoDB lock table. | `infra/terraform/main.tf`, `infra/terraform/state.tf` |
| State bootstrap guidance | Production hardening | The runbook documents the out-of-band S3 bucket and DynamoDB table bootstrap. | `docs/deployment-runbook.md` |
| CI Terraform validation | Production hardening | The CI workflow validates both Terraform modules with `terraform init -backend=false`. | `.github/workflows/ci.yml` |
| Deploy pipeline Entra wiring | Production hardening | The deploy workflow adds a `terraform-entra` job with Azure credentials and the redirect URI. | `.github/workflows/deploy.yml` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| A Terraform state backend (S3 + DynamoDB lock) is declared and documented | Implemented |
| A deployment runbook documents the full pipeline | Implemented |
| CI validates the Terraform modules on pull requests | Implemented |
| The deploy pipeline wires the Entra app-registration module | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 29 does not change application persistence. It hardens the deployment & operations pipeline and its provisioning artifacts.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 182 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary):
  - `GET /` → 200 (client app serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. Terraform is not installed locally and the pipeline is not run from this offline workspace; the CI workflow validates the Terraform modules, and the runbook documents the pipeline.

## Residual Risks and Deferrals

- The pipeline is declarative and not run from this offline workspace; it must be exercised in GitHub Actions against a real AWS account.
- The S3 state bucket and DynamoDB lock table must be bootstrapped before the first apply.
- The Entra module uses local state in the deploy job; a durable remote backend for it is a follow-up.
- Exact token/cost telemetry is unavailable in this local execution context.
