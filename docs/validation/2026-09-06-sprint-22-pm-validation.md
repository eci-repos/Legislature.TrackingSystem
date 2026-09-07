# Sprint 22 - Production Hardening: Deployment Hardening (Registry, Scanning, Terraform, DR) - PM Validation Report

Date: 2026-09-06

Sprint: Sprint 22

Status: Complete

## 1. Scope Summary

Sprint 22 hardens the deployment path for the LTS web application. It adds a CI/CD deployment pipeline that builds and publishes the container image to a registry, scans the image for vulnerabilities, and validates/plans the Terraform infrastructure. It extends the Terraform topology with an ECR repository, GuardDuty, Security Hub, and cross-region disaster recovery, and hardens the Dockerfile with OCI metadata and a `.dockerignore`.

## 2. Source Requirements and User Stories

| Requirement / Story | Source | Sprint 22 Evidence |
| --- | --- | --- |
| The solution is deployable through a governed pipeline | Production hardening | A `deploy.yml` workflow builds/pushes the image to ECR, scans it, and validates/plans the Terraform. |
| Container images are published to a registry | Production hardening | An ECR repository is declared and the pipeline pushes the image to it. |
| Container images are scanned for vulnerabilities | Production hardening | The pipeline runs Trivy with a fail-on-HIGH/CRITICAL gate and uploads SARIF. |
| Infrastructure is declared as code and validated | Production hardening | Terraform `fmt`/`init`/`validate`/`plan` run in CI; `apply` on main. |
| Continuous security monitoring | Production hardening | GuardDuty detector and Security Hub account with foundational/CIS standards are declared. |
| Disaster recovery across regions | Production hardening | A cross-region RDS read replica of the primary database is declared. |
| TR-601 versioned RESTful API surface | TR-601 | The versioned API surface is unchanged. |
| TR-702 automated static controls | TR-702 | Format verification passes and warning-as-error posture remains. |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (161 domain + 21 integration). |

## 3. Acceptance Evidence

- A `deploy.yml` workflow builds/pushes the image to ECR, scans it with Trivy, and validates/plans the Terraform.
- Terraform declares an ECR repository, GuardDuty detector, Security Hub account, and cross-region DR.
- The Dockerfile carries OCI labels and a `.dockerignore` is present.

## 4. Verification Results

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- `docker build -f src/Legislature.TrackingSystem.Web/Dockerfile` succeeded; the image carries the OCI labels.
- `.github/workflows/deploy.yml` validated as well-formed YAML.
- Terraform additions reviewed for HCL correctness (ECR, GuardDuty, Security Hub, cross-region DR).

## 5. Delivered Artifacts

- `.github/workflows/deploy.yml` — CI/CD deployment pipeline (ECR build/push, Trivy scan, Terraform validate/plan/apply).
- `infra/terraform/ecr.tf` — ECR repository with lifecycle policy.
- `infra/terraform/security.tf` — GuardDuty detector and Security Hub account with standards.
- `infra/terraform/dr.tf` — cross-region DR topology and RDS read replica.
- `src/Legislature.TrackingSystem.Web/Dockerfile` — OCI image metadata labels.
- `.dockerignore` — build context hygiene.
- `docs/02-Current-Sprint.md`, `docs/traceability/sprint-22-traceability.md`, `docs/validation/2026-09-06-sprint-22-pm-validation.md`, and `docs/08-AI-Coder-Handoff-Guide.md`.

## 6. Residual Risks and Deferrals

- The `deploy.yml` pipeline requires AWS OIDC role, ECR, and Terraform backend configuration (secrets: `AWS_DEPLOY_ROLE_ARN`, `AWS_ACCOUNT_ID`, `SSL_CERTIFICATE_ARN`, `DB_PASSWORD`, `JWT_SIGNING_KEY`) to be provisioned before it can run end-to-end.
- `terraform validate`/`plan` and Trivy scanning are exercised in CI, not locally (tooling not installed in this offline workspace).
- Real Entra interactive sign-in against a live tenant, Entra app registration/tenant provisioning, and the redirect URI configuration remain follow-up items.
- External legislative/fiscal connectors and Microsoft 365/SharePoint/Graph integration remain follow-up production-hardening items.
- Exact token/cost telemetry is unavailable in this local execution context.

## 7. PM Validation Decision

**Decision: Completed**

Sprint 22 is implemented, verified, and documented. The deployment path is hardened with a CI/CD pipeline (registry publish, image scanning, Terraform validation), an ECR repository, GuardDuty and Security Hub monitoring, cross-region DR, and Dockerfile/build-context hygiene. The .NET solution builds and tests pass, and the container image builds with OCI metadata.

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | GPT-5 Codex |
| Token count | Unavailable (telemetry not exposed in this environment) |
| Cost | Unavailable (no billing source exposed; not estimated) |
| Elapsed time | Unavailable (not captured) |
| Tool/run counts | Unavailable (not captured) |
| Verification runs | `dotnet format` (1), `dotnet build` (1), `dotnet test` (1), `docker build` (2 — first failed on `.dockerignore` excluding required files, then succeeded), YAML validation (1) |

> AI Metrics note: token/cost telemetry is not available in this execution environment. Values are marked unavailable rather than estimated, per the project's AI Metrics policy.
