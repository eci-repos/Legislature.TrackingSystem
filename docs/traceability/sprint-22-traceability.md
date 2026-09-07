# Sprint 22 - Production Hardening: Deployment Hardening (Registry, Scanning, Terraform, DR) Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 22 hardens the deployment path for the LTS web application. It adds a CI/CD deployment pipeline that builds and publishes the container image to a registry, scans the image for vulnerabilities, and validates/plans the Terraform infrastructure. It extends the Terraform topology with an ECR repository, GuardDuty, Security Hub, and cross-region disaster recovery, and hardens the Dockerfile with OCI metadata and a `.dockerignore`.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 22 Evidence | Artifact |
| --- | --- | --- | --- |
| The solution is deployable through a governed pipeline | Production hardening | A `deploy.yml` workflow builds/pushes the image to ECR, scans it, and validates/plans the Terraform. | `.github/workflows/deploy.yml` |
| Container images are published to a registry | Production hardening | An ECR repository is declared and the pipeline pushes the image to it. | `infra/terraform/ecr.tf`, `deploy.yml` |
| Container images are scanned for vulnerabilities | Production hardening | The pipeline runs Trivy with a fail-on-HIGH/CRITICAL gate and uploads SARIF. | `deploy.yml` |
| Infrastructure is declared as code and validated | Production hardening | Terraform `fmt`/`init`/`validate`/`plan` run in CI; `apply` on main. | `deploy.yml`, `infra/terraform/*.tf` |
| Continuous security monitoring | Production hardening | GuardDuty detector and Security Hub account with foundational/CIS standards are declared. | `infra/terraform/security.tf` |
| Disaster recovery across regions | Production hardening | A cross-region RDS read replica of the primary database is declared. | `infra/terraform/dr.tf` |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (161 domain + 21 integration). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 22 Evidence | Artifact |
| --- | --- | --- | --- |
| Least-privilege container runtime | Production hardening | The image runs as the non-root `app` user; OCI labels added for provenance. | `src/Legislature.TrackingSystem.Web/Dockerfile` |
| Build context hygiene | Production hardening | `.dockerignore` excludes build artifacts, local secrets, CI, docs, and infra. | `.dockerignore` |
| Immutable, encrypted registry | Production hardening | ECR repository is immutable, scan-on-push, AES256-encrypted, with a lifecycle policy. | `infra/terraform/ecr.tf` |
| Continuous security monitoring | Production hardening | GuardDuty and Security Hub with foundational/CIS standards. | `infra/terraform/security.tf` |
| Cross-region DR | Production hardening | Cross-region RDS read replica in a declared DR region topology. | `infra/terraform/dr.tf` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| A `deploy.yml` workflow builds/pushes the image to ECR, scans it with Trivy, and validates/plans the Terraform | Implemented |
| Terraform declares an ECR repository, GuardDuty detector, Security Hub account, and cross-region DR | Implemented |
| The Dockerfile carries OCI labels and a `.dockerignore` is present | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| The Docker image still builds | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 22 does not change application persistence. It hardens the deployment and infrastructure-as-code path.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- `docker build -f src/Legislature.TrackingSystem.Web/Dockerfile` succeeded; the image carries the OCI labels.
- `.github/workflows/deploy.yml` validated as well-formed YAML.
- Terraform additions reviewed for HCL correctness (ECR, GuardDuty, Security Hub, cross-region DR).

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions. `terraform validate`/`plan` and Trivy scanning are not run locally because the tooling is not installed in this offline workspace; they run in the CI pipeline.

## Residual Risks and Deferrals

- The `deploy.yml` pipeline requires AWS OIDC role, ECR, and Terraform backend configuration (secrets: `AWS_DEPLOY_ROLE_ARN`, `AWS_ACCOUNT_ID`, `SSL_CERTIFICATE_ARN`, `DB_PASSWORD`, `JWT_SIGNING_KEY`) to be provisioned before it can run end-to-end.
- `terraform validate`/`plan` and Trivy scanning are exercised in CI, not locally (tooling not installed in this offline workspace).
- Real Entra interactive sign-in against a live tenant, Entra app registration/tenant provisioning, and the redirect URI configuration remain follow-up items.
- External legislative/fiscal connectors and Microsoft 365/SharePoint/Graph integration remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local execution context.
