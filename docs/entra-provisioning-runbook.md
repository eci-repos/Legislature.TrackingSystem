# Microsoft Entra ID Provisioning Runbook

Status: active

Last updated: 2026-09-06

## Purpose

This runbook provisions a real Microsoft Entra ID tenant and wires the LTS `AzureAd` (Entra/OpenID Connect) boundary to it, replacing the POC symmetric-key JWT dev boundary for production. The application already contains the full Entra boundary (server-side JWT bearer validation via `EntraAuthOptions`/`EntraClaimsMapper`, and client-side interactive sign-in via `AddOidcAuthentication`); this runbook covers the tenant-side provisioning and the configuration values the application expects.

## Prerequisites

- An Azure subscription with permission to create an Entra ID tenant and register applications (Azure AD Application Administrator or equivalent).
- The `AzureAd` configuration section in `appsettings.json` (or environment variables) is empty/absent so the app runs in the dev boundary until provisioning is complete.
- The application is deployed at a known HTTPS base URL (e.g. `https://lts.example.gov`).

## 1. Create or select the Entra tenant

1. Sign in to the Azure portal.
2. Create a new Microsoft Entra ID tenant (or select an existing one) dedicated to the LTS application.
3. Record the **Tenant (directory) ID** — this is `AzureAd:TenantId`.

## 2. Register the application

1. In the tenant, go to **App registrations → New registration**.
2. Name: `Legislature.TrackingSystem`.
3. Supported account types: **Accounts in this organizational directory only** (single tenant).
4. Redirect URI (Web platform): `https://lts.example.gov/authentication/login-callback`.
5. Register and record the **Application (client) ID** — this is `AzureAd:ClientId`.

## 3. Create a client secret

1. In the app registration, go to **Certificates & secrets → Client secrets → New client secret**.
2. Set an expiry (e.g. 12 months) and record the secret value immediately — it is shown only once.
3. Store the secret in the deployment secret store (e.g. AWS Secrets Manager / CI/CD secret), never in source control. The server validates bearer tokens against the tenant authority and does not need the client secret; the secret is used by the interactive sign-in flow.

## 4. Configure API permissions

1. In the app registration, go to **API permissions → Add a permission**.
2. Add **Microsoft Graph → Delegated permissions → `openid`, `profile`, `email`** (and `User.Read` if user profile data is needed).
3. Grant admin consent for the tenant.

## 5. Configure role/group mappings

The application maps Entra `roles`/`groups` claims to LTS `UserRole` values via `AzureAd:RoleMappings`/`AzureAd:GroupMappings`. Choose one of:

- **App roles** — In the app registration, go to **App roles → Create app role** and create one role per LTS `UserRole` (e.g. `LTS.Analyst`, `LTS.Reviewer`, `LTS.Approver`, `LTS.ExecutiveReviewer`, `LTS.FinancialUser`, `LTS.ReadOnly`, `LTS.SecurityAdministrator`). Assign users/groups to the roles. The `roles` claim then carries the role value.
- **Groups** — Create Entra groups and assign users. The `groups` claim carries the group object id; map each group object id to an LTS `UserRole` in `AzureAd:GroupMappings`.

> Note: the `groups` claim is only emitted for groups under the group-size limit unless the app opts into the `groups` claim with the `GroupMembershipClaims` setting. Prefer app roles for a deterministic `roles` claim.

## 6. Configure the application

Set the `AzureAd` section (via environment variables or `appsettings.json`):

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<tenant-id>",
    "ClientId": "<client-id>",
    "Authority": "https://login.microsoftonline.com/<tenant-id>/v2.0",
    "RedirectUri": "https://lts.example.gov/authentication/login-callback",
    "RoleClaimType": "roles",
    "GroupClaimType": "groups",
    "RoleMappings": {
      "LTS.Analyst": "Analyst",
      "LTS.Reviewer": "Reviewer",
      "LTS.Approver": "Approver",
      "LTS.ExecutiveReviewer": "ExecutiveReviewer",
      "LTS.FinancialUser": "FinancialUser",
      "LTS.ReadOnly": "ReadOnly",
      "LTS.SecurityAdministrator": "SecurityAdministrator"
    },
    "GroupMappings": {}
  }
}
```

The server validates this configuration at startup and fails fast with actionable errors if it is incomplete or malformed.

## 7. Verify

1. Start the server with the `AzureAd` section configured; it must start without a configuration error.
2. Open the client base URL; the app must redirect to the Entra tenant sign-in.
3. Sign in as a user assigned an LTS role; the app must render the pages the role's permissions allow and hide the rest.
4. Call a protected API endpoint with the acquired access token; it must return 200, and a token for a role lacking the required permission must return 403.

## Declarative provisioning

A declarative Terraform module under `infra/terraform-entra/` provisions the app registration, redirect URIs, and app roles. Run it in CI or a provisioning pipeline (not from this offline workspace). See `infra/terraform-entra/README.md`.

## Residual risks and deferrals

- The client secret rotation and certificate-based authentication are operational follow-ups.
- Conditional access, device compliance, and MFA policies are tenant-level follow-ups.
- The `groups` claim size limit and group-membership claim opt-in must be confirmed against the tenant configuration.
