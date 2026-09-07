# Deployment & Operations Runbook

Status: active

Last updated: 2026-09-06

## Purpose

This runbook documents the LTS deployment & operations pipeline: the CI/CD workflows, the AWS Terraform topology, the Terraform state backend, GuardDuty/Security Hub, cross-region disaster recovery, and how to run the pipeline. The pipeline is declarative and intended to run in GitHub Actions; it is not run from the offline development workspace.

## 1. Prerequisites

- An AWS account with the target region (default `us-west-2`) and a DR region (default `us-east-1`).
- A GitHub repository hosting this codebase.
- An OIDC identity provider and deploy role in AWS for GitHub Actions (the role ARN is supplied as `AWS_DEPLOY_ROLE_ARN`).
- An ACM certificate for the ALB HTTPS listener (the ARN is supplied as `SSL_CERTIFICATE_ARN`).
- An Entra tenant and app registration for the `terraform-entra` module (see `docs/entra-provisioning-runbook.md`).

## 2. GitHub secrets

The workflows read the following secrets. Configure them in the repository settings:

| Secret | Used by | Purpose |
| --- | --- | --- |
| `AWS_DEPLOY_ROLE_ARN` | deploy.yml | OIDC role to assume for AWS operations. |
| `AWS_ACCOUNT_ID` | deploy.yml | AWS account id for the ECR image URI. |
| `SSL_CERTIFICATE_ARN` | deploy.yml | ACM certificate ARN for the ALB HTTPS listener. |
| `DB_PASSWORD` | deploy.yml | RDS PostgreSQL master password. |
| `JWT_SIGNING_KEY` | deploy.yml | JWT HMAC signing key. |
| `ARM_CLIENT_ID` | deploy.yml | Azure service principal client id for the Entra module. |
| `ARM_CLIENT_SECRET` | deploy.yml | Azure service principal client secret for the Entra module. |
| `ARM_TENANT_ID` | deploy.yml | Azure tenant id for the Entra module. |
| `ARM_SUBSCRIPTION_ID` | deploy.yml | Azure subscription id for the Entra module. |
| `ENTRA_REDIRECT_URI` | deploy.yml | Redirect URI for the Entra app registration. |

## 3. Terraform state backend bootstrap

The AWS topology uses an S3 remote backend with DynamoDB state locking. The backend block cannot create its own backend, so bootstrap the S3 bucket and DynamoDB table once, out-of-band:

```bash
aws s3api create-bucket \
  --bucket lts-terraform-state \
  --region us-west-2 \
  --create-bucket-configuration LocationConstraint=us-west-2

aws s3api put-bucket-versioning \
  --bucket lts-terraform-state \
  --versioning-configuration Status=Enabled

aws dynamodb create-table \
  --table-name lts-terraform-lock \
  --attribute-definitions AttributeName=LockID,AttributeType=S \
  --key-schema AttributeName=LockID,KeyType=HASH \
  --billing-mode PAY_PER_REQUEST \
  --region us-west-2
```

The `state.tf` resource manages the DynamoDB lock table so it is part of the declared topology; Terraform adopts the bootstrapped table on the first apply.

## 4. CI/CD workflows

### CI (`ci.yml`)

Runs on push/PR to `main`:

- `dotnet format --verify-no-changes`.
- `dotnet build` (Release).
- `dotnet test` (Release, no-build).
- `terraform fmt -check` and `terraform validate` for the AWS topology (`infra/terraform`) and the Entra module (`infra/terraform-entra`), using `terraform init -backend=false` so validation does not require the remote backend.

### Deploy (`deploy.yml`)

Runs on push to `main`, version tags, and manual dispatch:

- **build-push-scan** — Builds the web image, pushes it to ECR, and scans it with Trivy (HIGH/CRITICAL, fail on findings), uploading the SARIF report.
- **terraform** — Validates, plans, and applies the AWS topology (`infra/terraform`) with the S3 remote backend. Apply runs only on `main`.
- **terraform-entra** — Validates, plans, and applies the Entra app-registration module (`infra/terraform-entra`). Apply runs only on `main`.

## 5. AWS topology

The `infra/terraform` module declares:

- **Networking** — VPC, public/private subnets, internet gateway, route tables.
- **WAF** — AWS-managed rule sets (Common, SQL injection) on the ALB.
- **ALB** — HTTPS listener with the ACM certificate; health check on `/api/v1/readiness`.
- **ECS Fargate** — the web service (2 tasks) behind the ALB, with CloudWatch log aggregation.
- **RDS PostgreSQL** — encrypted, Multi-AZ, automated backups (30-day retention), deletion protection.
- **Secrets** — the JWT signing key in SSM Parameter Store (SecureString).
- **GuardDuty** — continuous threat detection.
- **Security Hub** — foundational and CIS AWS benchmarks.
- **DR** — a cross-region RDS read replica in the DR region.

## 6. Running the pipeline

1. Configure the GitHub secrets (§2).
2. Bootstrap the Terraform state backend (§3).
3. Push to `main` (or trigger `workflow_dispatch`). The deploy workflow builds, pushes, scans, and applies the topology.
4. Verify the ALB DNS name (`outputs.tf`) serves the app over HTTPS and `/health` returns 200.

## 7. Verification

- `GET https://<alb-dns>/` → 200 (app serves).
- `GET https://<alb-dns>/health` → 200 (liveness).
- `GET https://<alb-dns>/health/ready` → 200 (readiness).
- GuardDuty and Security Hub findings are visible in the AWS console.
- The DR read replica is present in the DR region.

## Residual risks and deferrals

- The pipeline is declarative and not run from this offline workspace; it must be exercised in GitHub Actions against a real AWS account.
- The S3 state bucket and DynamoDB lock table must be bootstrapped before the first apply.
- The Entra module uses local state in the deploy job; a durable remote backend for it is a follow-up.
- Secret rotation and certificate renewal are operational follow-ups.
