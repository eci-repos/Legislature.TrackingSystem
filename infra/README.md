# Legislature.TrackingSystem — Production Operations

This directory holds the production-hardening infrastructure-as-code and operations guidance for
the LTS web application. The Terraform under `infra/terraform/` declares the target AWS topology;
it is **not** deployed from this workspace and must be applied through a governed pipeline with
secrets supplied from a secret store.

## Topology

- **Compute** — ECS Fargate service (2 tasks) running the ASP.NET Core / Blazor WebAssembly app on
  port 8080, in private subnets.
- **Ingress** — Application Load Balancer (ALB) with TLS 1.2+ and a WAF web ACL in front.
- **Data** — RDS PostgreSQL 17, Multi-AZ, storage-encrypted, with automated backups.
- **Observability** — CloudWatch log group (90-day retention) for application logs.

## Security (WAF)

The WAF web ACL (`aws_wafv2_web_acl.main`) applies AWS managed rule groups:

- `AWSManagedRulesCommonRuleSet` — core protection (SQLi, XSS, bad inputs, etc.).
- `AWSManagedRulesSQLiRuleSet` — SQL-injection-specific rules.

All rules log to CloudWatch metrics with sampled requests enabled. The ALB only accepts HTTPS
(443) and forwards to the ECS security group; the ECS security group only accepts traffic from the
ALB; the RDS security group only accepts traffic from the ECS security group.

## SIEM / Logging

- Application logs stream to CloudWatch Logs (`/ecs/lts-<env>`, 90-day retention).
- WAF and ALB metrics are published to CloudWatch.
- For a full SIEM pipeline, export CloudWatch Logs to a central log store (e.g. AWS Security Hub
  + GuardDuty, or a third-party SIEM) and enable GuardDuty for threat detection. This is a
  follow-up integration, not yet wired.

## Backup & Disaster Recovery

- **RDS** — automated backups with 30-day retention, Multi-AZ for high availability, and
  `deletion_protection` enabled. A final snapshot is taken on deletion.
- **State** — Terraform state is stored in S3 (see `backend` block) with locking.
- **DR** — The Terraform topology is declarative and can be re-applied in a secondary region;
  RDS cross-region read replicas and a runbook are follow-up items.

## Secrets

- `Jwt__Key` is stored in AWS SSM Parameter Store (`/lts/<env>/jwt-key`, `SecureString`) and
  injected into the ECS task as a secret. Never commit the signing key.
- `db_password` and `jwt_signing_key` are Terraform variables marked `sensitive` and must be
  supplied from a secret store, never in code.

## Applying

```bash
cd infra/terraform
terraform init
terraform plan -var-file=production.tfvars
terraform apply -var-file=production.tfvars
```

> The `production.tfvars` file is not committed. Supply `ssl_certificate_arn`, `web_image`,
> `db_password`, and `jwt_signing_key` from your secret store.
