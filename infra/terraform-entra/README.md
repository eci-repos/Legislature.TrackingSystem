# Terraform — Microsoft Entra ID App Registration

This module provisions the Microsoft Entra ID app registration that the LTS `AzureAd` (Entra/OpenID Connect) boundary expects: the application, redirect URI, app roles (mapped to LTS `UserRole` values), Microsoft Graph delegated permissions, a client secret, and the service principal.

## Prerequisites

- Terraform >= 1.5.
- The `hashicorp/azuread` provider (downloaded on `terraform init`).
- Azure credentials for the target tenant (via `ARM_CLIENT_ID`, `ARM_CLIENT_SECRET`, `ARM_TENANT_ID`, `ARM_SUBSCRIPTION_ID`, or a managed identity).

## Usage

```bash
cd infra/terraform-entra
terraform init
terraform plan -var="redirect_uri=https://lts.example.gov/authentication/login-callback"
terraform apply -var="redirect_uri=https://lts.example.gov/authentication/login-callback"
```

Capture the outputs (`application_client_id`, `client_secret_value`) and set them as `AzureAd:ClientId` and the client secret in the deployment secret store. See `docs/entra-provisioning-runbook.md` for the full provisioning and configuration steps.

## Notes

- This module is declarative and intended to run in CI or a provisioning pipeline. It is not applied from the offline development workspace (no network or Azure credentials).
- The `client_secret_value` output is sensitive and shown only on apply; store it in a secret store.
- The `groups` claim is not used by this module; role-based authorization uses the `roles` claim from the app roles.
