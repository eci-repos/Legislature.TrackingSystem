# Sprint 34 - Production Hardening: Authoring & Template Hardening Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 34 hardens the authoring and template layer: it adds a template merge-field catalog with validation, adds template version control, and adds a richer notification delivery/trigger model.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 34 Evidence | Artifact |
| --- | --- | --- | --- |
| Template merge fields are explicit data contracts validated against a catalog | US-4.3.1, B.COM.24; US-4.3.3, B.COM.26 | `TemplateMergeFieldCatalog` lists the known fields; `TemplateRenderer` exposes `GetUsedFields` and `GetUnknownFields` so unknown fields are surfaced. | `src/Legislature.TrackingSystem.Application/WorkItems/TemplateMergeFieldCatalog.cs`, `TemplateRenderer.cs` |
| Templates can be maintained separately and retain history | US-4.3.2, B.COM.25 | `DocumentTemplate` tracks a `Version` that increments on each body update and produces a `DocumentTemplateVersion` snapshot. | `src/Legislature.TrackingSystem.Domain/WorkItems/DocumentTemplate.cs`, `DocumentTemplateVersion.cs` |
| Notifications carry a delivery channel and a cause | US-1.2.1, B.COM.02 | `Notification` now carries a `NotificationChannel` and a `NotificationTrigger`; the assignment notification is tagged with the `Assignment` trigger. | `src/Legislature.TrackingSystem.Domain/WorkItems/Notification.cs`, `NotificationChannel.cs`, `NotificationTrigger.cs` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests asserting acceptance criteria | TR-902 | The full solution test suite passes (202 domain + 23 infrastructure + 22 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 34 Evidence | Artifact |
| --- | --- | --- | --- |
| Template merge-field catalog and validation | US-4.3.1, B.COM.24; US-4.3.3, B.COM.26 | Catalog of known fields; renderer exposes used/unknown field extraction. | `src/Legislature.TrackingSystem.Application/WorkItems/TemplateMergeFieldCatalog.cs`, `TemplateRenderer.cs` |
| Template version control | US-4.3.2, B.COM.25 | `DocumentTemplate.Version` increments on update; `CreateVersion` captures a snapshot. | `src/Legislature.TrackingSystem.Domain/WorkItems/DocumentTemplate.cs`, `DocumentTemplateVersion.cs` |
| Notification channel and trigger | US-1.2.1, B.COM.02 | `Notification` carries channel and trigger; assignment notifications tagged with the `Assignment` trigger. | `src/Legislature.TrackingSystem.Domain/WorkItems/Notification.cs`, `NotificationChannel.cs`, `NotificationTrigger.cs` |
| Persistence of the new model | Persistence policy | EF Core migration `20260906160341_AddTemplateVersionAndNotificationChannel` adds the `Channel`, `Trigger`, and `Version` columns; the single-file DDL is regenerated. | `src/Legislature.TrackingSystem.Infrastructure/Persistence/Migrations/`, `docs/database/lts-postgresql-ddl.sql` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| A template merge-field catalog validates template bodies and exposes the used fields | Implemented |
| Template updates record a version | Implemented |
| The notification model carries a channel and a trigger | Implemented |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 34 adds persistence for the hardened authoring model: the `DocumentTemplate.Version`, `Notification.Channel`, and `Notification.Trigger` columns. An EF Core migration (`20260906160341_AddTemplateVersionAndNotificationChannel`) and the regenerated single-file DDL (`docs/database/lts-postgresql-ddl.sql`) capture the change.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release -m:1` passed: 202 domain tests + 23 infrastructure integration tests + 22 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, Development + PostgreSQL):
  - `GET /` → 200 (client app serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- The merge-field catalog is a fixed list; it is not yet configurable per template type.
- Template version snapshots are produced by the domain but are not yet persisted as a separate version table.
- Notification delivery channels beyond in-app (e.g., email) are modeled but not yet dispatched.
- Exact token/cost telemetry is unavailable in this local execution context.
