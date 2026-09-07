# External Connector Provisioning Runbook

Status: active

Last updated: 2026-09-06

## Purpose

This runbook provisions the live endpoints for the LTS external connector layer and wires the `Connectors` configuration to them, replacing the dev-boundary fakes for production. The application already contains the full connector layer (HTTP adapters for the legislative and fiscal sources, a Microsoft Graph adapter for M365, and config-driven selection in `AddLtsConnectors`); this runbook covers the endpoint provisioning and the configuration values the application expects.

## Prerequisites

- Access to the external systems (legislative API, DOR fiscal systems, Microsoft 365 tenant).
- The `Connectors` configuration section in `appsettings.json` (or environment variables) is empty/absent so the app runs with the dev-boundary fakes until provisioning is complete.
- The application is deployed at a known HTTPS base URL.

## 1. Legislative source connector (F5.1 - External Legislative Updates)

The `HttpLegislativeSourceConnector` fetches bill language, status, and amendments from a REST endpoint.

1. Provision or obtain the legislative API base URL (e.g. `https://legislature.example.gov/api/`).
2. Obtain an API key for the service account.
3. Configure:

```json
{
  "Connectors": {
    "LegislativeSource": {
      "BaseUrl": "https://legislature.example.gov/api/",
      "ApiKey": "<api-key>"
    }
  }
}
```

The adapter calls `GET {BaseUrl}bills/{billNumber}?biennium={biennium}` and `GET {BaseUrl}bills/{billNumber}/amendments?biennium={biennium}`, sending the API key in the `X-Api-Key` header.

## 2. DOR fiscal data connector (F7.1 - Fiscal Data Integration)

The `HttpFiscalDataSourceConnector` fetches fiscal data points from an internal DOR REST endpoint.

1. Provision or obtain the fiscal data API base URL (e.g. `https://fiscal.example.gov/api/`).
2. Obtain an API key for the service account.
3. Configure:

```json
{
  "Connectors": {
    "FiscalDataSource": {
      "BaseUrl": "https://fiscal.example.gov/api/",
      "ApiKey": "<api-key>"
    }
  }
}
```

The adapter calls `GET {BaseUrl}fiscal-data` (optionally filtered by category), sending the API key in the `X-Api-Key` header.

## 3. Microsoft 365 connector (F8.1 - Productivity Suite Integration)

The `GraphM365Connector` sends email and stores documents via Microsoft Graph.

1. Register an app in the Microsoft 365 / Entra tenant (see `docs/entra-provisioning-runbook.md`).
2. Grant the app the Microsoft Graph delegated permissions for `Mail.Send` and `Files.ReadWrite` (or the scopes the integration requires).
3. Configure:

```json
{
  "Connectors": {
    "M365": {
      "TenantId": "<tenant-id>",
      "ClientId": "<client-id>",
      "ClientSecret": "<client-secret>"
    }
  }
}
```

The adapter calls `https://graph.microsoft.com/v1.0/` using the client-credentials flow.

## 4. Configure the application

Set the `Connectors` section (via environment variables or `appsettings.json`). The server validates this configuration at startup and fails fast with actionable errors if a configured connector is incomplete or malformed.

## 5. Verify

1. Start the server with the `Connectors` section configured; it must start without a configuration error.
2. `GET /health/ready` must report the `connectors` check healthy when all configured endpoints respond, degraded when some fail, and unhealthy when all fail.
3. Exercise the ingestion, fiscal, and M365 features; the HTTP/Graph adapters must call the live endpoints.

## Residual risks and deferrals

- The exact legislative and fiscal API contracts (paths, payloads, auth) must be confirmed against the live systems; the adapters assume the documented shapes.
- The M365 Graph scopes and client-credentials flow must be confirmed against the tenant's app registration.
- API key rotation and secret management are operational follow-ups.
