# AI-Coder Handoff Guide

Status: active

Last updated: 2026-09-06

## Purpose

This guide lets another AI-Coder resume Legislature.TrackingSystem work without relying on chat history. Treat durable repository files as the source of truth.

## Start Here

Read these files before changing code or documentation:

1. `AGENTS.md`
2. `docs/00-Project-Charter.md`
3. `docs/01-Project-WorkPlan.md`
4. `docs/02-Current-Sprint.md`
5. `docs/03-Resource-Management.md`
6. `docs/04-Recommended-Tech-Stack.md`
7. `docs/05-Pair-WorkPlan-and-Schedule.md`
8. `docs/06-Companion-Timeline.md`
9. `docs/07-POC-Architecture-Scaffold.md`
10. `docs/traceability/sprint-01-traceability.md`
11. Latest relevant file under `docs/handoff/`
12. Latest relevant file under `docs/validation/`

## Current State

- Sprint 0 is complete and has a PM Validation report.
- Sprint 1 is complete and has a PM Validation report with AI Metrics.
- Sprint 1A local container readiness is complete and has a PM Validation report with AI Metrics.
- Sprint 2 (POC work intake and identifiers) is complete and has a PM Validation report.
- Sprint 3 (POC assignment and work queues) is complete and has a PM Validation report.
- Sprint 4 (POC relationships, packages, and categorization) is complete and has a PM Validation report.
- Sprint 5 (POC hardening and demonstration) is complete and has a PM Validation report; it added idempotent seed data, a user walkthrough, and hardened error handling (40 tests pass, 0 warnings/errors).
- Sprint 6 (Configurable Workflow, Phase 2 F3.1) is complete and has a PM Validation report; it added an explicit review/approval workflow with separated work/approval responsibilities, multiple reviewers, and approved-only final packaging with internal/external recipients (52 tests pass, 0 warnings/errors).
- Sprint 7 (RFA Executive Review, Phase 2 F3.2) is complete and has a PM Validation report; it added a governed Executive Review path restricted to designated users with a sequential reviewer handoff, step-level due dates, and a settable priority (64 tests pass, 0 warnings/errors).
- Sprint 8 (Content Authoring and Attachments, Phase 2 E4 slice: F4.1 + F4.2) is complete and has a PM Validation report; it added a rich-text authoring editor (and a limited editor for fiscal notes) with spell check, attachments, and save-incomplete-work (70 tests pass, 0 warnings/errors).
- Sprint 9 (Templates, Generated Documents, and Reuse, Phase 2 completion: F4.3 + F4.4) is complete and has a PM Validation report; it added maintainable document templates with merge-field document generation, template sharing, and reuse of existing/prior work without copy-and-paste (79 tests pass, 0 warnings/errors).
- Sprint 10 (Phase 1 Completion: collaboration, notifications, customer due date, task maintenance) is complete and has a PM Validation report; it adds collaboration comments, simultaneous viewing, in-app notifications, customer due date tracking, and task maintenance with an audit trail (89 tests pass, 0 warnings/errors). This completes Phase 1.
- Sprint 11 (Phase 3: Legislative Data Lifecycle, Search, and Reporting) is complete and has a PM Validation report; it adds external legislative ingestion (bill language/status/amendments), bill and work-product version history with comparison, enterprise search, and standard/custom reporting with extracts (104 tests pass, 0 warnings/errors). This completes Phase 3.
- Sprint 12 (Phase 4: Fiscal Analysis, Financial Inputs, and Productivity Integration) is complete and has a PM Validation report; it adds fiscal data integration (FTE, cost rules, revenue funds/sources), fiscal-note/expense-estimate calculation, supporting work papers, budget-bill flagging with fiscal-note comparison, demographic data by session, Microsoft 365 productivity integration boundaries (template population and email dispatches), and expense-estimate element management (119 tests pass, 0 warnings/errors). This completes Phase 4.
- Sprint 13 (Phase 5: Security, Operations, Migration, and Historical Reference) is complete and has a PM Validation report; it adds role-based permissions (preparation/approval/delivery/read-only), access restrictions by data type and user type, concurrent-use and availability coverage, repeatable legacy migration with rollback, and 10-year historical reference (132 tests pass, 0 warnings/errors). This completes Phase 5.
- Sprint 14 (Phase 6: Specialized Legislative Programs and Executive Experience) is complete and has a PM Validation report; it adds L&P correspondence tracking, legislative implementation task management (assign/reassign/complete/share/status-report), bill implementation flagging with L&P manager notification, a consolidated executive bill view, and executive bill discussion (143 tests pass, 0 warnings/errors). This completes Phase 6.
- Sprint 15 (Production Hardening: Authentication, Docker, and CI/CD) is complete and has a PM Validation report; it adds JWT-based authentication/authorization enforcing the role-to-permission matrix at the HTTP layer, Docker hardening (non-root, healthcheck), a GitHub Actions CI workflow, and Terraform plus operations documentation (WAF/SIEM/backup/DR) (147 tests pass, 0 warnings/errors). This begins the production-hardening and transition track.
- Sprint 16 (Production Hardening: PostgreSQL Persistence Foundation) is complete and has a PM Validation report; it adds EF Core + Npgsql, a `LtsDbContext` mapping the bill and user aggregates, an `InitialCreate` migration, `EfBillRepository`/`EfUserRepository`, connection-string-aware DI, and integration tests.
- Sprint 17 (Production Hardening: PostgreSQL Persistence Expansion) is complete and has a PM Validation report; it adds package, work-item relationship, and notification persistence, package mutation persistence, and PostgreSQL integration tests.
- Sprint 18 (Production Hardening: Complete PostgreSQL Persistence) is complete and has a PM Validation report; it repairs the partially started Sprint 18 EF model state, migrates all current repository-backed aggregates to PostgreSQL, adds migrations through `AddWorkTaskPersistence`, and expands integration coverage to 20 PostgreSQL tests (167 tests pass, 0 warnings/errors).
- Sprint 19 (Production Hardening: Persist Generated Documents) is complete and has a PM Validation report; it makes generated documents a repository-backed aggregate, persists them through EF Core (with the in-memory adapter as the no-connection fallback), adds a `GET /api/v1/work-items/{id}/documents` endpoint, and expands integration coverage to 21 PostgreSQL tests (170 tests pass, 0 warnings/errors).
- Sprint 20 (Production Hardening: Entra/OpenID Connect Authentication Boundary) is complete and has a PM Validation report; it adds a configuration-driven Entra/OpenID Connect boundary (`EntraAuthOptions` + `EntraClaimsMapper`) that validates bearer tokens against the Entra tenant when `AzureAd` is configured and falls back to the symmetric-key JWT dev boundary otherwise (182 tests pass, 0 warnings/errors).
- Sprint 21 (Production Hardening: Web.Client Interactive Entra Sign-In) is complete and has a PM Validation report; it adds interactive Entra/OpenID Connect sign-in to the Blazor WebAssembly client (`AddOidcAuthentication`, `RemoteAuthenticatorView`, token-attaching `HttpClient`) when `AzureAd` is configured, with the dev boundary preserved when it is not (182 tests pass, 0 warnings/errors).
- Sprint 22 (Production Hardening: Deployment Hardening — Registry, Scanning, Terraform, DR) is complete and has a PM Validation report; it adds a `deploy.yml` CI/CD pipeline (ECR build/push, Trivy scan, Terraform validate/plan/apply), an ECR repository, GuardDuty + Security Hub, cross-region RDS DR, and Dockerfile OCI labels + `.dockerignore` (182 tests pass, 0 warnings/errors).
- Sprint 23 (Production Hardening: External Connectors — Legislative, Fiscal, M365) is complete and has a PM Validation report; it adds the external connector layer (`ILegislativeSourceConnector`, `IFiscalDataSourceConnector`, `IM365Connector`) with HTTP/Graph adapters and dev-boundary fakes, config-driven selection via `AddLtsConnectors`, and wiring into the ingestion and productivity services (190 tests pass, 0 warnings/errors).
- Sprint 24 (Production Hardening: Observability — Logging, Health, Telemetry) is complete and has a PM Validation report; it adds `/health` (liveness) and `/health/ready` (readiness with a database check) endpoints, structured JSON console logging, and config-driven OpenTelemetry tracing/metrics with an OTLP exporter, and updates the Dockerfile healthcheck to `/health` (191 tests pass, 0 warnings/errors).
- Sprint 25 (Production Hardening: API Hardening — Rate Limiting, Validation, Security Headers) is complete and has a PM Validation report; it adds a fixed-window rate limiter (100/min, 429) on the `/api/v1` endpoints, DataAnnotations request validation with a `ValidationFilter` returning 400, and a security-headers middleware (CSP, nosniff, frame-deny, referrer-policy, permissions-policy). The smoke test surfaced and the sprint fixed two API routing defects: the versioned API group double-prefixing every route (leaving the real `/api/v1` paths unregistered) and the status-code-pages re-execution masking real API error statuses behind an antiforgery 400 (194 tests pass, 0 warnings/errors).
- Sprint 26 (Production Hardening: Client-Side Page Authorization Gating) is complete and has a PM Validation report; it moves the authoritative `PermissionMatrix` into the Domain project so the server and the WebAssembly client share the same role-to-permission mapping, registers permission-based policies in the client's `AddAuthorizationCore`, adds a client-side `PermissionAuthorizationHandler`, uses `AuthorizeRouteView` with an access-denied view, adds `[Authorize]` attributes to the restricted pages, and hides restricted navigation links with `AuthorizeView` (202 tests pass, 0 warnings/errors).
- Sprint 27 (Production Hardening: Real Entra Tenant Provisioning) is complete and has a PM Validation report; it adds startup validation of the `AzureAd` configuration (`EntraAuthOptionsValidator`), completes the client OIDC wiring (redirect URI and authority), and provides the provisioning artifacts (a runbook and a declarative Terraform module) for a real Microsoft Entra ID tenant (210 tests pass, 0 warnings/errors).
- Sprint 28 (Production Hardening: External Connector Live Endpoints) is complete and has a PM Validation report; it adds startup validation of the `Connectors` configuration (`ConnectorOptionsValidator`), adds a connector connectivity health check (`ConnectorHealthCheck`) on `/health/ready`, and provides a provisioning runbook for the live legislative, fiscal, and Microsoft 365 endpoints (217 tests pass, 0 warnings/errors).
- Sprint 29 (Production Hardening: Deployment & Operations) is complete and has a PM Validation report; it adds a Terraform state backend (S3 + DynamoDB lock), a deployment runbook, CI hardening (Terraform validation on pull requests), and wires the Entra app-registration module into the deploy pipeline (217 tests pass, 0 warnings/errors).
- Sprint 30 (Production Hardening: Persistence Policy) is complete and has a PM Validation report; it decides that the in-memory fallback remains a supported local/offline development mode but production requires PostgreSQL, adds a `PersistencePolicy` that enforces this at startup (fail fast in Production, warn in non-Production), and documents the policy (226 tests pass, 0 warnings/errors).
- Sprint 31 (Production Hardening: Acceptance) is complete and has a PM Validation report; it defines the acceptance criteria and Definition of Done in `docs/acceptance-criteria.md` and adds a repeatable acceptance verification script `tools/verify-acceptance.ps1` (226 tests pass, 0 warnings/errors). This completes the production-hardening backlog.
- Sprint 32 (Production Hardening: Client Authorization Test Coverage) is complete and has a PM Validation report; it adds unit tests for the client-side `PermissionAuthorizationHandler` (9 tests) so the WebAssembly client's role-to-permission enforcement is covered, matching the server-side handler coverage (235 tests pass, 0 warnings/errors).
- Sprint 33 (Production Hardening: API Hardening Refinement) is complete and has a PM Validation report; it adds per-IP/per-user rate-limit partitioning, per-route security header customization, and expands request validation to 30+ DTOs (236 tests pass, 0 warnings/errors).
- Sprint 34 (Production Hardening: Authoring & Template Hardening) is complete and has a PM Validation report; it adds a template merge-field catalog with validation, template version control, and a notification channel/trigger model (247 tests pass, 0 warnings/errors).
- Sprint 35 (Production Hardening: Workflow & Review Refinement) is complete and has a PM Validation report; it adds a configurable workflow definition per work item type and reviewer notification delivery (251 tests pass, 0 warnings/errors).
- Sprint 36 (UX Overhaul: Theme Foundation & Design System) is complete and has a PM Validation report; it adopts the light, professional Bootswatch Flatly Bootstrap 5 theme (navy primary), adds Bootstrap Icons with a topic-appropriate icon on every left-side nav option, and builds a shared `--lts-*` design system in `app.css`. It also fixes the container never shipping `lib/bootstrap` or icons (the `.dockerignore` excluded `**/wwwroot/lib/`) (251 tests pass, 0 warnings/errors).
- Sprint 37 (UX Overhaul: Home Dashboard) is complete and has a PM Validation report; it replaces the Home page with a read-only dashboard (KPI cards, workflow funnel, needs attention, recent items, packages in progress, executive review) reusing existing read endpoints (251 tests pass, 0 warnings/errors).
- The consolidated closeout and handoff document is `docs/10-Project-Closeout.md`; it summarizes the delivered scope, verification status, remaining live-infrastructure gaps, and the offline-to-online transition path for the PM.
- The end-user draft guide is `docs/11-End-User-Guide.md`; it describes how to use the web application by area (work intake, work queue, work items, review/approval, authoring, templates, search, reporting, fiscal, notifications) and notes POC limitations.
- The end-user glossary is `docs/12-End-User-Glossary.md`; it is a plain-language table reference of all application terms (bills, work items, workflow, packages, roles, permissions, and every status value) and is linked from the end-user guide.
- The current sprint file is `docs/02-Current-Sprint.md`.
- The ASP.NET Core / Blazor WebAssembly foundation exists and builds.
- The web app can run locally through Docker Compose as `lts-web` at `http://localhost:5088`.
- The local app exposes a readiness page at `/` and readiness API at `/api/v1/readiness`.
- Sprint 2 adds work task creation, unique identifier auto-assignment, and identifier override with duplicate prevention.
- Sprint 3 adds task assignment/reassignment, multiple assignees with per-role due dates, and a user work queue with rework identification.
- Sprint 4 adds work item relationships, deliverable packages, and work-item queries that identify, sort, filter, and group by Confidential, Executive Review, On Hold, Work Type, and Package.
- The stores are seeded at startup with a coherent, realistic DOR dataset as a follow-up of the core HB 1200 / SB 88 thread: 8 bills (with versions/amendments/flags), 14 work items across workflow states, 2 packages, fiscal data, demographics, templates, notifications, correspondence, implementation tasks, executive discussions, custom reports, access restrictions, fiscal work papers, expense-estimate elements, bill-fiscal-note links, email dispatches, generated documents, a migration batch, and 11 users with roles. `VersionService.GetBillHistoryAsync` returns all bills when no bill number is supplied so the Bills page lists them. The dev database was reset so the full seed runs fresh on startup.
- List-heavy pages (Work Items, Work Queue, Packages) use a `.page-shell-wide` variant (`max-width: 100%`) so their results tables fill the available window width; `.queue-table` has a `min-width: 1000px` so the `.table-responsive` wrapper scrolls horizontally on narrow windows instead of cutting off the right side.
- Selected list items (Bills, Budget, Authoring, Maintenance, Templates) use a light, readable highlight: `app.css` overrides `.list-group-item.active` (light `#e8f1f9` background, normal text, readable muted labels) instead of Bootstrap's dark primary background that made inner text unreadable.
- In the dev boundary, `Web.Client/Program.cs` registers a `DevTokenMessageHandler` that transparently acquires a JWT for the default `admin` user from `/api/v1/auth/token` and attaches it to API requests, so authenticated endpoints (e.g. Historical, which requires `RequireViewHistorical`) work without manual login.
- Sprint 6 adds a configurable review/approval workflow: work items carry an explicit `WorkflowStatus` (Draft → PendingReview → UnderReview → Approved/Rejected → Finalized), `SubmitForReview` enforces separation of duties (at least one reviewer must not be a preparer), multiple required reviewers each record a retained `WorkflowReview`, and only approved products can be finalized and packaged for submission with internal/external recipients.
- Sprint 7 adds the RFA Executive Review path: a governed path restricted to designated users (`AssignmentRole.ExecutiveReviewer`), a sequential reviewer handoff (`ExecutiveReviewer` steps with begin/adjust/complete) that continues until `ExecutiveReviewStatus.Completed`, per-step due dates (`WorkflowStep`), and a settable `TaskPriority`.
- Sprint 8 adds content authoring and attachments: `WorkTask.Content` (rich text) with `SetContent` (saves incomplete work without completion/approval), `LastSavedAt`, and `Attachment` add/remove; the `/authoring` page provides a rich-text editor (contenteditable) with spell check for most product types and a limited textarea editor for fiscal notes.
- Sprint 9 adds templates, generated documents, and reuse: `DocumentTemplate` (name, applicable work type, body with merge fields, shared flag) and `GeneratedDocument`; `TemplateRenderer` renders `{{Field}}` merge fields from work-product data; `WorkTask.ReuseContentFrom` transfers content without copy-and-paste. The `/templates` page manages templates and generates documents; `/authoring` has a reuse action.
- Sprint 10 completes Phase 1: `WorkTaskComment` collaboration, `Notification` in-app notifications (generated on assignment), `WorkTask.CustomerDueDate`, and task maintenance (`UpdateTask`/`CancelTask`/duplicate) with a `WorkTaskAuditEntry` trail. New pages: `/maintenance`, `/compare` (simultaneous viewing), and `/notifications`.
- Sprint 11 completes Phase 3: `Bill` (with `BillVersion` snapshots and `BillAmendment` tracking), `WorkTaskVersion` (captured on each save), `CustomReport`, and the ingestion/version/search/reporting services. New pages: `/bills`, `/search`, and `/reports`.
- Sprint 12 completes Phase 4: `FiscalData` (FTE/cost rule/revenue fund/source), `FiscalWorkPaper`, `DemographicData`, `EmailDispatch`, `ExpenseEstimateElement`, `BillFiscalNoteLink`, and the `Bill.IsBudgetBill` flag, plus the fiscal data, work paper, budget bill, demographic, productivity integration, and expense estimate services. New pages: `/fiscal`, `/budget`, `/demographics`, and `/productivity`.
- Sprint 13 completes Phase 5: `UserRole`, `Permission`, `UserAccount`, `AccessRestriction`, `LegacyMigrationBatch`, `MigrationRecord`, and the `WorkTask.Year` property, plus the authorization, access control, migration, and historical reference services. New pages: `/security`, `/migration`, and `/historical`.
- Sprint 14 completes Phase 6: `Correspondence`, `ImplementationTask`, `SharedDocument`, `ExecutiveDiscussion`, and the `Bill.RequiresImplementation` flag, plus the correspondence, implementation task, executive bill view, and executive discussion services. New pages: `/correspondence`, `/implementation`, and `/executive`.
- Sprint 15 begins production hardening: `JwtTokenService`, `PermissionRequirement`, `PermissionAuthorizationHandler`, `JwtAuthorizationMessageHandler`, and the public `PermissionMatrix`; a hardened `Dockerfile` (non-root, healthcheck); `.github/workflows/ci.yml`; and `infra/terraform/*.tf` plus `infra/README.md`.
- Sprint 16 establishes the PostgreSQL persistence foundation: `Persistence/LtsDbContext.cs`, `Persistence/EfBillRepository.cs`, `Persistence/EfUserRepository.cs`, `Persistence/Migrations/InitialCreate`, and connection-string-aware DI in `InfrastructureServiceCollectionExtensions.cs`.
- Sprint 17 expands PostgreSQL persistence to packages, work-item relationships, and notifications.
- Sprint 18 completes PostgreSQL persistence for all current repository-backed aggregates, including `WorkTask` and its owned collection graph.
- Sprint 19 makes generated documents a persisted, repository-backed aggregate, closing the last repository-backed persistence gap from the Sprint 18 PM Validation report.
- Sprint 20 adds a configuration-driven Entra/OpenID Connect authentication boundary: when `AzureAd` is configured, JWT bearer validates against the Entra tenant authority and `EntraClaimsMapper` maps `roles`/`groups` claims to the LTS role-to-permission matrix; otherwise the symmetric-key JWT dev boundary remains active.
- Sprint 21 adds interactive Entra/OpenID Connect sign-in to the Web.Client: when `AzureAd` is configured, the client uses `AddOidcAuthentication` and attaches access tokens to API requests; otherwise the dev boundary (plain `HttpClient` + `DevAuthenticationStateProvider`) remains active.
- Sprint 22 hardens the deployment path: `.github/workflows/deploy.yml` builds/pushes the image to ECR, scans it with Trivy (fail on HIGH/CRITICAL), and runs `terraform fmt`/`init`/`validate`/`plan` (and `apply` on main); `infra/terraform/` adds an ECR repository, GuardDuty + Security Hub, and cross-region RDS DR; the Dockerfile carries OCI labels and `.dockerignore` trims the build context.
- Sprint 23 adds the external connector layer: `ILegislativeSourceConnector` (F5.1), `IFiscalDataSourceConnector` (F7.1), and `IM365Connector` (F8.1) with HTTP/Graph adapters and dev-boundary fakes; `AddLtsConnectors` selects the HTTP/Graph adapter when its `Connectors` config section is present and the dev fake otherwise; the ingestion and productivity services use the connectors.
- Sprint 24 adds observability: `/health` (liveness) and `/health/ready` (readiness with a `DatabaseHealthCheck`), structured JSON console logging via `AddJsonConsole`, and config-driven OpenTelemetry tracing/metrics with an OTLP exporter (enabled when `Otlp:Endpoint` is configured); the Dockerfile healthcheck uses `/health`.
- Sprint 25 adds API hardening: `AddRateLimiter` with a fixed-window "api" policy (PermitLimit 100, Window 1 minute, QueueLimit 0, RejectionStatusCode 429) applied to the versioned API group; `SecurityHeadersMiddleware` (CSP, X-Content-Type-Options, X-Frame-Options, Referrer-Policy, Permissions-Policy) via `UseSecurityHeaders()`; and a `ValidationFilter` endpoint filter plus DataAnnotations on 8 request DTOs. The versioned API group uses an empty `MapGroup("")` prefix so the full `/api/v1/...` paths register (the prior `MapGroup("/api/v1")` double-prefixed them to `/api/v1/api/v1/...`), and `UseStatusCodePages` re-executes to `/not-found` only for non-`/api` requests so real API statuses (429, 400) are preserved.
- Sprint 26 adds client-side page authorization gating: the authoritative `PermissionMatrix` moved to `src/Legislature.TrackingSystem.Domain/WorkItems/PermissionMatrix.cs` (shared by the server and the WebAssembly client); the client registers permission policies (`RequirePrepare`, `RequireApprove`, `RequireDeliver`, `RequireReadOnly`, `RequireAdminister`, `RequireManageAccess`, `RequireMigrate`, `RequireViewHistorical`) in `AddAuthorizationCore` and adds a client-side `PermissionAuthorizationHandler`; `Routes.razor` uses `AuthorizeRouteView` with a `NotAuthorized` view rendering `Pages/AccessDenied.razor`; the restricted pages carry `@attribute [Authorize(Policy = ...)]`; and `NavMenu.razor` wraps each link in `AuthorizeView`. The dev `DevAuthenticationStateProvider` returns the default SecurityAdministrator user so gating is exercised when Entra is not configured. The server's `AddAuthorization` block registers the same permission policies (parity for prerendering), prerendering is disabled for the interactive `Routes` component (`InteractiveWebAssemblyRenderMode(prerender: false)`), and a `BlazorAuthorizationMiddlewareResultHandler` lets Razor component endpoints pass through the server's authorization middleware (so the client gates pages) while API endpoints keep the default 401/403 challenge.
- Sprint 27 adds real Entra tenant provisioning readiness: `EntraAuthOptionsValidator` (`src/Legislature.TrackingSystem.Application/Authentication/EntraAuthOptionsValidator.cs`) validates the `AzureAd` section and the server throws with actionable errors when it is configured and invalid; the client sets a `RedirectUri` (defaulting to `{base}/authentication/login-callback`) and requires `AzureAd:Authority` for interactive sign-in; `docs/entra-provisioning-runbook.md` documents the tenant, app registration, client secret, API permissions, redirect URIs, and role/group mappings; and `infra/terraform-entra/` declaratively provisions the app registration, app roles, Graph permissions, client secret, and service principal.
- Sprint 28 adds external connector live-endpoint readiness: `ConnectorOptionsValidator` (`src/Legislature.TrackingSystem.Application/Connectors/ConnectorOptionsValidator.cs`) validates the `Connectors` section and the server throws with actionable errors when a configured connector is invalid; `ConnectorHealthCheck` (`src/Legislature.TrackingSystem.Web/Observability/ConnectorHealthCheck.cs`) probes each configured connector and is registered as a readiness check on `/health/ready`; and `docs/connector-provisioning-runbook.md` documents the legislative, fiscal, and Microsoft 365 endpoint provisioning and configuration.
- Sprint 29 completes the deployment & operations pipeline: the S3 backend in `infra/terraform/main.tf` references the `lts-terraform-lock` DynamoDB table with encryption, `infra/terraform/state.tf` manages the lock table, `docs/deployment-runbook.md` documents the full pipeline, `.github/workflows/ci.yml` validates the Terraform modules on push/PR, and `.github/workflows/deploy.yml` wires the `terraform-entra` module into the deploy pipeline.
- Sprint 30 decides and enforces the persistence policy: `PersistencePolicy` (`src/Legislature.TrackingSystem.Application/Persistence/PersistencePolicy.cs`) resolves the mode from the connection string and the server fails fast in Production (and warns in non-Production) when running in-memory; `docs/persistence-policy.md` documents the decision, boundary, parity guarantees, and migration path.
- Sprint 31 defines the acceptance criteria and Definition of Done: `docs/acceptance-criteria.md` consolidates the acceptance checks, test expectations, traceability expectations, handoff deliverables, PM Validation report, and AI Metrics requirements, and `tools/verify-acceptance.ps1` runs the checks (format, build, test, container config/build/smoke) and reports pass/fail.
- Sprint 32 adds client authorization test coverage: the Web.Tests project references the Web.Client project, and `ClientPermissionAuthorizationHandlerTests` (9 tests) covers the client-side `PermissionAuthorizationHandler` role-to-permission matrix (US-9.1.1, B.COM.06), including the no-role-claim case.
- Sprint 33 refines the API hardening: the `api` rate-limit policy partitions by the authenticated user's `sub` claim or the client IP; `SecurityHeadersOptions`/`SecurityHeadersOverride` enable per-route security header customization; and DataAnnotations were added to 30+ request DTOs so the `ValidationFilter` validates a broader API surface.
- Sprint 34 hardens the authoring and template layer: `TemplateMergeFieldCatalog` validates template bodies and `TemplateRenderer` exposes used/unknown fields; `DocumentTemplate` tracks a `Version` and produces `DocumentTemplateVersion` snapshots; and `Notification` carries a `NotificationChannel` and `NotificationTrigger`. EF Core migration `20260906160341_AddTemplateVersionAndNotificationChannel` adds the `Channel`, `Trigger`, and `Version` columns.
- Sprint 35 refines the workflow and review layer: `WorkflowDefinitionCatalog` specifies the required reviewer count per `WorkItemType` and `WorkflowService.SubmitForReviewAsync` validates submission against it and notifies each reviewer with the `ReviewRequested` trigger.
- `docs/database/lts-postgresql-ddl.sql` is the current single-file idempotent PostgreSQL DDL/migration script generated from the EF Core migration chain.
- Phase 1, Phase 2, Phase 3, Phase 4, Phase 5, and Phase 6 are complete. The production-hardening and transition track is underway (Sprints 15 through 35 complete).

## Repository Shape

| Path | Purpose |
| --- | --- |
| `Legislature.TrackingSystem.sln` | Root .NET solution. |
| `src/Legislature.TrackingSystem.Domain` | Domain model, value objects, source trace primitives, and future domain invariants. |
| `src/Legislature.TrackingSystem.Domain/WorkItems` | Work task aggregate, work item identifier, work item type, priority, and status. |
| `src/Legislature.TrackingSystem.Application` | Application service contracts, use case boundaries, and DI registration. |
| `src/Legislature.TrackingSystem.Application/WorkItems` | Work task service, assignment service, relationship service, package service, workflow service, executive review service, content service, template service, reuse service, task maintenance service, notification service, legislative ingestion service, version service, search service, reporting service, work-item query service, fiscal data service, fiscal work paper service, budget bill service, demographic data service, productivity integration service, expense estimate service, authorization service, access control service, migration service, historical reference service, correspondence service, implementation task service, executive bill view service, executive discussion service, repository/identifier contracts, commands, and DTOs. |
| `src/Legislature.TrackingSystem.Infrastructure` | Persistence/integration/identity/telemetry adapters and DI registration. |
| `src/Legislature.TrackingSystem.Infrastructure/WorkItems` | In-memory work task repository (with assignment state and queue queries), relationship store, package store, work item identifier generator, and `SeedDataInitializer` (idempotent POC demo data). |
| `src/Legislature.TrackingSystem.Web` | ASP.NET Core host, API endpoints, BFF boundary, authorization policies, and composition root. |
| `src/Legislature.TrackingSystem.Web/Dockerfile` | Local container image definition for the ASP.NET Core-hosted Blazor WebAssembly app. |
| `src/Legislature.TrackingSystem.Web.Client` | Blazor WebAssembly client UI. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/WorkIntake.razor` | Work intake UI for creating a work task and viewing its identifier; redesigned with the bundled Bootstrap 5 + `app.css` as a grouped, responsive two-column card form with inline validation, status banners, and an optional Assignment section. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/WorkQueue.razor` | Work queue UI showing a DOR user's assigned work with status, priority, role, per-assignment due date, rework badge, and workload/rework counts. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Packages.razor` | Packages UI to create a package, add/remove work products, add internal/external recipients, finalize, deliver, and view package status. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/WorkItems.razor` | Work items UI to identify, sort, filter, and group work by Confidential, Executive Review, On Hold, Work Type, and Package, to drive each item through review (submit/approve/reject/finalize), and to run the RFA Executive Review path (start/begin/adjust/complete), set priority, and view workflow steps. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Authoring.razor` | Authoring UI to draft, edit, and review work products with a rich-text editor (contenteditable) or a limited textarea editor for fiscal notes, with spell check, to add/remove attachments, and to reuse content from a prior work product. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Templates.razor` | Templates UI to create, edit, and share document templates and to generate customized documentation from a selected work product. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Maintenance.razor` | Task maintenance UI to update, cancel, or duplicate work tasks, set the customer due date, and collaborate via comments with an audit trail. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Compare.razor` | Simultaneous-viewing UI to view multiple work products side by side. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Notifications.razor` | In-app notifications UI to list and mark notifications read. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Bills.razor` | Legislative bills UI to ingest external updates, view versions and history, compare versions, and track amendments. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Search.razor` | Enterprise search UI across work products and bills. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Reports.razor` | Reporting UI to run standard and custom reports and extract work products. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Fiscal.razor` | Fiscal analysis UI to manage fiscal data and expense-estimate elements, add work papers, and calculate fiscal notes and expense estimates. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Budget.razor` | Budget bills UI to flag bills as budget bills and compare them with associated fiscal notes. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Demographics.razor` | Demographics UI to store and retrieve demographic data by legislative session. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Productivity.razor` | Productivity integration UI to populate templates with system data and record email dispatches. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Security.razor` | Security &amp; access UI to register users with roles, view permissions, and restrict access by data type and user type. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Migration.razor` | Legacy migration UI to import legacy data as batches and roll back completed batches. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Historical.razor` | Historical reference UI to view work products across a year window. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Correspondence.razor` | Correspondence UI to record sent correspondence and mark responses received. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Implementation.razor` | Implementation UI to assign/complete implementation tasks, flag bills for implementation, and view the status report. |
| `src/Legislature.TrackingSystem.Web.Client/Pages/Executive.razor` | Executive UI to view a consolidated bill view and discuss Executive work products. |
| `src/Legislature.TrackingSystem.Web.Client/ApiErrorFormatter.cs` | Friendly formatting of API failures (network vs server-error) across the data pages. |
| `tests/Legislature.TrackingSystem.Domain.Tests` | Automated tests for domain and application behavior. |
| `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems` | Tests for work item identifier, work task, work task service, work task assignment service, relationship service, package service, workflow service, executive review service, content service, template service, reuse service, task maintenance service, notification service, legislative ingestion service, version service, search service, reporting service, and work-item query service. |
| `docs/specs/` | Original source workbooks/documents and generated Markdown planning resources. |
| `docs/database/lts-postgresql-ddl.sql` | Single-file idempotent PostgreSQL DDL/migration script generated from the current EF Core migrations. |
| `docs/backlog/` | Proposed backlog items, not implementation authority by themselves. |
| `docs/handoff/` | Durable handoff notes. |
| `docs/validation/` | PM Validation reports and template, including AI Metrics. |

## Commands

Use these from the repository root:

```powershell
dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore
dotnet build Legislature.TrackingSystem.sln --configuration Release
dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build
dotnet run --project src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj --urls http://localhost:5088
docker compose config
docker compose build web
docker compose up -d web
docker compose ps
docker compose down
```

Optional local PostgreSQL readiness:

```powershell
docker compose up -d postgres
```

The Sprint 1A containerized application starts with PostgreSQL as a healthy dependency, but the application does not require database persistence yet.

## Last Verified State

Sprint 1 verification completed on 2026-09-04:

- Technical backlog conversion produced 94 technical stories.
- Technical backlog Markdown contains 94 `TS-*` sections.
- Technical backlog Markdown contains 17 technical epic sections.
- Technical backlog trace matrix contains 94 trace rows.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed.
- `dotnet build Legislature.TrackingSystem.sln --configuration Release` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build` passed with 4 tests.
- `http://localhost:5088/` returned HTTP 200 and rendered the LTS POC foundation page.
- `http://localhost:5088/api/v1/readiness` returned Sprint 1 traceability evidence.

Sprint 1A container readiness verification completed on 2026-09-04:

- `docker compose config` passed.
- `docker compose build web` passed and produced `legislature-tracking-system-web:local`.
- Docker image ID/digest: `sha256:e85e478ae2c7133c7e53e1a382e5aedd9b09da054a508c8b5f9be305474e87cc`.
- `docker compose up -d web` started `lts-postgres` and `lts-web`.
- `docker compose ps` showed `lts-postgres` healthy and `lts-web` mapped from host `5088` to container `8080`.
- `http://localhost:5088/` returned HTTP 200 with Legislature.TrackingSystem readiness markers.
- `http://localhost:5088/api/v1/readiness` returned HTTP 200 JSON with Sprint 1 traceability evidence.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed.
- `dotnet build Legislature.TrackingSystem.sln --configuration Release` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build` passed with 4 tests.

Sprint 2 verification completed on 2026-09-04:

- Sprint 2 is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet restore Legislature.TrackingSystem.sln -m:1` passed.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors (Domain, Application, Infrastructure, Web.Client incl. Blazor WASM output, Web, and Tests all build).
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed with 16 tests.
- Work task API smoke checks passed (hosted on `http://localhost:5090`): create auto-generated `LTS-Task-<hex>` identifier, get matched created ID, identifier override persisted, duplicate override returned HTTP 409 Conflict.
- Container build (`docker compose build web` → `legislature-tracking-system-web:local`), startup (`docker compose up -d web`, `lts-web` on `0.0.0.0:5088->8080`), and smoke checks against `http://localhost:5088` (readiness HTTP 200, work task create/get/override, duplicate override 409) all passed with Docker access via elevated permissions.

> Environment note: .NET SDK 10.0.400 was repaired (missing workload locator SDKs restored). This sandbox restricts process isolation, so solution restore/build/test run with `-m:1` (single-node MSBuild) and elevated file/process permissions, and Docker daemon access requires elevated permissions (named-pipe); with elevation, the container builds and runs. NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (offline `NU1900` would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 2 defects — the standard AGENTS.md commands work on a normal host.

Sprint 3 verification completed on 2026-09-04:

- Sprint 3 is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet restore Legislature.TrackingSystem.sln -m:1` passed.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors (Domain, Application, Infrastructure, Web.Client incl. Blazor WASM output, Web, and Tests all build).
- `dotnet test Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 23 tests (16 prior + 7 new assignment/queue tests).
- Assignment/queue API smoke checks passed (hosted on `http://localhost:5088`): create → assign (Analyst) → reassign (Reviewer) → reassign-back (rework flagged) → work queue shows status/priority/role/due/rework/count; superseded user's queue empty.
- Container build (`docker compose build web` → `legislature-tracking-system-web:local`), startup (`docker compose up -d web`, `lts-web` on `0.0.0.0:5088->8080`), and UI render checks (`/work-intake` and `/work-queue` HTTP 200) all passed with Docker access via elevated permissions.

Sprint 4 verification completed on 2026-09-04:

- Sprint 4 is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet restore Legislature.TrackingSystem.sln -m:1` passed.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors (Domain, Application, Infrastructure, Web.Client incl. Blazor WASM output, Web, and Tests all build).
- `dotnet test Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 40 tests (23 prior + 17 new relationship/package/query tests).
- Relationships/package/query API smoke checks passed (hosted on `http://localhost:5088`): link by LegislativeIdentifier (queryable from target side), package create/add/deliver (Delivered, members retained), categorization (confidential), and work-item query filter by confidential, group by Type/Package, sort by Title.
- Container build (`docker compose build web` → `legislature-tracking-system-web:local`), startup (`docker compose up -d web`, `lts-web` on `0.0.0.0:5088->8080`), and UI render checks (`/packages` and `/work-items` HTTP 200) all passed with Docker access via elevated permissions.

Sprint 5 verification completed on 2026-09-04:

- Sprint 5 (POC hardening and demonstration) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed with 40 tests (0 failed).
- Seed data smoke checks passed (hosted on `http://localhost:5088`): `GET /api/v1/work-items` returns 6 items; `confidential=true` returns exactly the fiscal estimate; `GET /api/v1/packages` returns 1 package ("FY2026 HB 1200 Fiscal Package", In Progress, 3 members); `GET /api/v1/work-queue/jdoe` returns 3 entries (Owner x2, Reviewer x1); the fiscal-note task returns 2 LegislativeIdentifier relationships.
- UI pages `/work-intake`, `/work-queue`, `/packages`, `/work-items` all render HTTP 200.

Container WASM boot fix (2026-09-04, post-Sprint-5-verification): the container image was serving the page HTML referencing `_framework/blazor.web.js`, but the Docker publish output omitted that boot script (and `blazor.server.js`), so `_framework/blazor.web.js` returned 404 and the interactive WebAssembly client never booted in the browser — symptom: interactive pages (e.g., Work Queue) did nothing and auto-loading pages reported "couldn't connect to the server." Root cause: the Dockerfile ran `dotnet restore` with only the `.csproj` files present (before `COPY . .`) and then published with `--no-restore`, which reused that partial restore and skipped regenerating the `Microsoft.AspNetCore.App.Internal.Assets` static web assets. Fix: removed `--no-restore` from the Dockerfile `dotnet publish` step (with a comment explaining why). After `docker compose build web && docker compose up -d web`, `/_framework/blazor.web.js` returns HTTP 200. This is an environment/container-build fix, not a code change; browser users should hard-refresh or clear site data to load the corrected image.

Sprint 6 verification completed on 2026-09-04:

- Sprint 6 (Configurable Workflow, Phase 2 F3.1) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 52 tests (0 failed, 0 skipped; 12 new workflow/package workflow tests).
- Container rebuilt and recreated (`docker compose up -d --build web` then `docker compose up -d --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js` and `/work-items` page both return HTTP 200.
- Workflow API smoke (hosted on `http://localhost:5088`): `POST /api/v1/work-items/{id}/submit-for-review` → `reviews` (approve) → `finalize` transitioned a seeded work item `PendingReview → Approved → Finalized`; `POST /api/v1/packages/{id}/recipients` added an Internal recipient; `POST /api/v1/packages/{id}/finalize` was correctly blocked for a package whose member work product was not approved.
- Note: `docker compose up -d --build web` builds the new image but does not recreate an already-running `lts-web`; use `docker compose up -d --force-recreate web` to pick up the new image.

Sprint 7 verification completed on 2026-09-04:

- Sprint 7 (RFA Executive Review, Phase 2 F3.2) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 64 tests (0 failed, 0 skipped; 12 new executive review tests).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js` and `/work-items` page both return HTTP 200.
- Executive review API smoke (hosted on `http://localhost:5088`): designated a user as `ExecutiveReviewer`, `POST /api/v1/work-items/{id}/executive-review/start` → `.../adjust` → `.../complete` transitioned a seeded fiscal estimate `InProgress → Completed` with an adjustment recorded; `POST /api/v1/work-items/{id}/priority` set `Critical`; `POST /api/v1/work-items/{id}/steps` added a step with a due date.

Sprint 8 verification completed on 2026-09-04:

- Sprint 8 (Content Authoring and Attachments, Phase 2 E4 slice: F4.1 + F4.2) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 70 tests (0 failed, 0 skipped; 6 new content/attachment tests).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js`, `/work-items`, and `/authoring` pages all return HTTP 200.
- Content/attachment API smoke (hosted on `http://localhost:5088`): `PUT /api/v1/work-items/{id}/content` saved rich-text content with a `lastSavedAt` timestamp; `POST /api/v1/work-items/{id}/attachments` added an attachment; `DELETE /api/v1/work-items/{id}/attachments/{attachmentId}` removed it.

Sprint 9 verification completed on 2026-09-04:

- Sprint 9 (Templates, Generated Documents, and Reuse, Phase 2 completion: F4.3 + F4.4) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 79 tests (0 failed, 0 skipped; 9 new template/reuse tests).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js`, `/work-items`, `/authoring`, and `/templates` pages all return HTTP 200.
- Template/document/reuse API smoke (hosted on `http://localhost:5088`): `POST /api/v1/templates` created a shared template; `POST /api/v1/work-items/{id}/documents/generate` rendered merge fields from system data; `POST /api/v1/work-items/{id}/reuse` copied content from a source product into a distinct target product.

Sprint 10 verification completed on 2026-09-04:

- Sprint 10 (Phase 1 Completion: collaboration, notifications, customer due date, task maintenance) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 89 tests (0 failed, 0 skipped; 10 new maintenance/notification tests).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js`, `/work-items`, `/maintenance`, `/compare`, and `/notifications` pages all return HTTP 200.
- Maintenance/notification API smoke (hosted on `http://localhost:5088`): `PUT /api/v1/work-items/{id}` updated a task; `PUT .../customer-due-date` set a customer due date; `POST .../comments` added a comment; assignment generated an in-app notification; `POST .../duplicate` created a distinct task; `POST .../cancel` canceled the task with an audit trail.

Sprint 11 verification completed on 2026-09-04:

- Sprint 11 (Phase 3: Legislative Data Lifecycle, Search, and Reporting) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 104 tests (0 failed, 0 skipped; 16 new Phase 3 tests).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js`, `/bills`, `/search`, and `/reports` pages all return HTTP 200.
- Phase 3 API smoke (hosted on `http://localhost:5088`): `POST /api/v1/bills/ingest` ingested a bill; `POST .../status` set its status; `POST .../amendments` tracked an amendment; `GET .../versions` listed 2 versions; `POST .../versions/compare` reported differences; `POST /api/v1/search` returned hits; `POST /api/v1/reports/standard` ran a standard report; `POST /api/v1/reports/custom` created a custom report and `POST .../custom/run` ran it; `POST /api/v1/work-items/{id}/extract` extracted a work product.

Sprint 12 verification completed on 2026-09-04:

- Sprint 12 (Phase 4: Fiscal Analysis, Financial Inputs, and Productivity Integration) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 119 tests (0 failed, 0 skipped; 15 new Phase 4 tests).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js`, `/fiscal`, `/budget`, `/demographics`, and `/productivity` pages all return HTTP 200.
- Phase 4 API smoke (hosted on `http://localhost:5088`): `POST /api/v1/fiscal-data` upserted fiscal data; `POST /api/v1/work-items/{id}/fiscal-note/calculate` calculated a fiscal note; `POST /api/v1/work-items/{id}/work-papers` added a work paper; `POST /api/v1/bills/{id}/budget-flag` flagged a budget bill; `POST /api/v1/bills/{id}/fiscal-notes` linked a fiscal note and `GET .../fiscal-notes` returned it; `POST /api/v1/demographics` stored demographic data; `POST /api/v1/work-items/{id}/templates/populate` populated a template; `POST /api/v1/email` recorded an email dispatch; `POST /api/v1/expense-estimates` upserted an expense-estimate element and `POST /api/v1/work-items/{id}/expense-estimate/calculate` calculated an expense estimate.

Sprint 13 verification completed on 2026-09-04:

- Sprint 13 (Phase 5: Security, Operations, Migration, and Historical Reference) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 132 tests (0 failed, 0 skipped; 13 new Phase 5 tests).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js`, `/security`, `/migration`, and `/historical` pages all return HTTP 200.
- Phase 5 API smoke (hosted on `http://localhost:5088`): `POST /api/v1/users` registered users with roles and permissions; `POST /api/v1/work-items/{id}/access-restrictions` restricted access and `GET .../access-restrictions` returned it; `POST /api/v1/migrations` imported a legacy batch (2 imported, 0 failed) and `POST /api/v1/migrations/{id}/rollback` rolled it back; `GET /api/v1/historical?years=10` listed historical work products.

Sprint 14 verification completed on 2026-09-04:

- Sprint 14 (Phase 6: Specialized Legislative Programs and Executive Experience) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 143 tests (0 failed, 0 skipped; 11 new Phase 6 tests).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`; `lts-web` on `0.0.0.0:5088->8080`).
- WASM boot asset `/_framework/blazor.web.js`, `/correspondence`, `/implementation`, and `/executive` pages all return HTTP 200.
- Phase 6 API smoke (hosted on `http://localhost:5088`): `POST /api/v1/correspondence` recorded correspondence and `POST /api/v1/correspondence/{id}/response` marked it received; `POST /api/v1/implementation-tasks` assigned an implementation task and `POST .../complete` completed it; `GET /api/v1/implementation-tasks/status-report` returned the report; `POST /api/v1/bills/{id}/implementation-flag` flagged a bill for implementation; `GET /api/v1/bills/{id}/executive-view` loaded the consolidated view; `POST /api/v1/bills/{id}/discussions` posted a question and `POST /api/v1/discussions/{id}/answer` answered it.

Sprint 15 verification completed on 2026-09-04:

- Sprint 15 (Production Hardening: Authentication, Docker, and CI/CD) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 147 tests (0 failed, 0 skipped; 4 new `PermissionMatrixTests`).
- `docker compose config --quiet` passed.
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`); the hardened image runs as a non-root user with a readiness healthcheck.
- JWT auth smoke (hosted on `http://localhost:5088`): `POST /api/v1/auth/token` issued a token for `admin`; a protected endpoint returned 401 without a token and 200 with a valid token; a token for a role lacking the required permission returned 403.

Sprint 16 verification completed on 2026-09-04:

- Sprint 16 (Production Hardening: PostgreSQL Persistence Foundation) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test` passed: 147 domain tests + 2 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`); migrations applied at startup.
- Persistence smoke (hosted on `http://localhost:5088`): ingested `HB 8001` and `HB 8002`, restarted the container, and confirmed both bills persisted in PostgreSQL.
- JWT auth regression passed: `POST /api/v1/auth/token` issued a token for `admin`; `GET /api/v1/users` returned 200.

Sprint 17 verification completed on 2026-09-04:

- Sprint 17 (Production Hardening: PostgreSQL Persistence Expansion) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test` passed: 147 domain tests + 6 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --build --force-recreate web`); migrations applied at startup.
- Persistence smoke (hosted on `http://localhost:5088`): created a package with a member, a relationship, and a notification; restarted the container; confirmed the package (with members), relationship, and notification persisted in PostgreSQL.

Sprint 19 verification completed on 2026-09-06:

- Sprint 19 (Production Hardening: Persist Generated Documents) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet ef migrations add AddGeneratedDocumentPersistence` generated the migration; `dotnet ef migrations has-pending-model-changes` passed (no pending model changes).
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 149 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- PostgreSQL integration tests ran against the local Docker Compose database and passed, including the new generated-document round-trip.

Sprint 20 verification completed on 2026-09-06:

- Sprint 20 (Production Hardening: Entra/OpenID Connect Authentication Boundary) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy.
- Dev auth boundary smoke (hosted on `http://localhost:5088`): `POST /api/v1/auth/token` issued a token for `admin` (SecurityAdministrator); `GET /api/v1/users` returned 401 without a token and 200 with a valid token.

Sprint 21 verification completed on 2026-09-06:

- Sprint 21 (Production Hardening: Web.Client Interactive Entra Sign-In) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy.
- Client auth UI smoke (hosted on `http://localhost:5088`): the home page renders the "Log in" control linking to `authentication/login`; the `/security` page renders without the not-found regression; the dev auth boundary still issues tokens and protects `/api/v1/users`.

Sprint 22 verification completed on 2026-09-06:

- Sprint 22 (Production Hardening: Deployment Hardening — Registry, Scanning, Terraform, DR) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 161 domain tests + 21 infrastructure integration tests (0 failed, 0 skipped).
- `docker build -f src/Legislature.TrackingSystem.Web/Dockerfile` succeeded; the image carries the OCI labels.
- `.github/workflows/deploy.yml` validated as well-formed YAML.
- Terraform additions reviewed for HCL correctness (ECR, GuardDuty, Security Hub, cross-region DR). `terraform validate`/`plan` and Trivy scanning run in CI (not locally; the tooling is not installed in this offline workspace).

Sprint 23 verification completed on 2026-09-06:

- Sprint 23 (Production Hardening: External Connectors — Legislative, Fiscal, M365) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy; readiness returned 200 and the home page rendered the login control (dev boundary intact).

Sprint 24 verification completed on 2026-09-06:

- Sprint 24 (Production Hardening: Observability — Logging, Health, Telemetry) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests + 1 web test (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy; `/health` and `/health/ready` returned Healthy and the home page rendered the login control.

Sprint 25 verification completed on 2026-09-06:

- Sprint 25 (Production Hardening: API Hardening — Rate Limiting, Validation, Security Headers) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests + 4 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- API hardening smoke (hosted on `http://localhost:5088`): `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT; empty/missing `userKey` → 400 `application/problem+json` (validation filter); `GET /api/v1/templates` → 200 (group endpoint registered); `GET /health`, `GET /health/ready`, `GET /` → 200; security headers (CSP, X-Content-Type-Options, X-Frame-Options, Referrer-Policy, Permissions-Policy) present; rate limiting returns 429 after 100 requests.
- Root-cause note: the versioned API group was created with `MapGroup("/api/v1")` while the endpoint mappings still used full `/api/v1/...` paths, double-prefixing every route to `/api/v1/api/v1/...` and leaving the real `/api/v1` paths unregistered (POSTs fell through to the Blazor catch-all and returned 405/400). Fixed with an empty group prefix. Separately, `UseStatusCodePagesWithReExecute("/not-found")` masked real API error statuses (e.g. the rate limiter's 429) behind a 400 antiforgery rejection from the re-executed POST to `/not-found`; replaced with a scoped `UseStatusCodePages` handler that re-executes only for non-`/api` requests.

Sprint 26 verification completed on 2026-09-06:

- Sprint 26 (Production Hardening: Client-Side Page Authorization Gating) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 167 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- Client authorization smoke (hosted on `http://localhost:5088`): `GET /`, `/access-denied`, `/security`, `/migration`, `/historical`, `/executive`, `/work-intake`, `/fiscal`, `/work-items`, `/bills`, `/search`, `/reports` → 200 (client app serves; pages render under the dev SecurityAdministrator user); `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT; `GET /api/v1/users` without a token → 401 and with a valid token → 200 (API authorization still enforced); `GET /health` → 200.

Sprint 27 verification completed on 2026-09-06:

- Sprint 27 (Production Hardening: Real Entra Tenant Provisioning) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 175 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- Entra provisioning smoke (hosted on `http://localhost:5088`, dev boundary with no `AzureAd` configured): `GET /` → 200; `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT; `GET /api/v1/users` without a token → 401 and with a valid token → 200; `GET /health` → 200. The `EntraAuthOptionsValidator` startup validation is exercised by unit tests (a real tenant cannot be provisioned from this offline workspace).

Sprint 28 verification completed on 2026-09-06:

- Sprint 28 (Production Hardening: External Connector Live Endpoints) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 182 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- Connector provisioning smoke (hosted on `http://localhost:5088`, dev boundary with no `Connectors` configured): `GET /` → 200; `GET /health` → 200; `GET /health/ready` → 200 (the `connectors` check reports healthy with no connectors configured); `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT. The `ConnectorOptionsValidator` startup validation is exercised by unit tests (live endpoints cannot be provisioned from this offline workspace).

Sprint 29 verification completed on 2026-09-06:

- Sprint 29 (Production Hardening: Deployment & Operations) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 182 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- Deployment smoke (hosted on `http://localhost:5088`, dev boundary): `GET /` → 200; `GET /health` → 200; `GET /health/ready` → 200; `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT. Terraform is not installed locally and the pipeline is not run from this offline workspace; the CI workflow validates the Terraform modules, and the runbook documents the pipeline.

Sprint 30 verification completed on 2026-09-06:

- Sprint 30 (Production Hardening: Persistence Policy) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 191 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- Persistence policy smoke (hosted on `http://localhost:5088`, dev boundary, Development + PostgreSQL): `GET /` → 200; `GET /health` → 200; `GET /health/ready` → 200; `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT. The `PersistencePolicy` enforcement is exercised by unit tests; the container runs in Development with PostgreSQL, so the in-memory warning path is not triggered there.

Sprint 31 verification completed on 2026-09-06:

- Sprint 31 (Production Hardening: Acceptance) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 191 domain tests + 23 infrastructure integration tests + 12 web tests (0 failed, 0 skipped).
- `tools/verify-acceptance.ps1` ran the format, build, test, container config, container build, and container smoke checks and reported pass.
- Container smoke test (dev boundary, Development + PostgreSQL): `GET /` → 200, `GET /health` → 200, `GET /health/ready` → 200, token endpoint → 200.

Sprint 32 verification completed on 2026-09-06:

- Sprint 32 (Production Hardening: Client Authorization Test Coverage) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 191 domain tests + 23 infrastructure integration tests + 21 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- Client authorization smoke (hosted on `http://localhost:5088`, dev boundary, Development + PostgreSQL): `GET /` → 200; `GET /health` → 200; `GET /health/ready` → 200; `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

Sprint 33 verification completed on 2026-09-06:

- Sprint 33 (Production Hardening: API Hardening Refinement) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 191 domain tests + 23 infrastructure integration tests + 22 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- API hardening smoke (hosted on `http://localhost:5088`, dev boundary, Development + PostgreSQL): `GET /` → 200; `GET /health` → 200; `GET /health/ready` → 200; `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT; security headers present on responses (CSP, nosniff, frame-deny, referrer-policy, permissions-policy).

Sprint 34 verification completed on 2026-09-06:

- Sprint 34 (Production Hardening: Authoring & Template Hardening) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release -m:1` passed: 202 domain tests + 23 infrastructure integration tests + 22 web tests (0 failed, 0 skipped).
- EF Core migration `20260906160341_AddTemplateVersionAndNotificationChannel` added; single-file DDL regenerated.
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- Authoring/template smoke (hosted on `http://localhost:5088`, dev boundary, Development + PostgreSQL): `GET /` → 200; `GET /health` → 200; `GET /health/ready` → 200; `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

Sprint 35 verification completed on 2026-09-06:

- Sprint 35 (Production Hardening: Workflow & Review Refinement) is promoted and implemented in `docs/02-Current-Sprint.md`.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release -m:1` passed: 206 domain tests + 23 infrastructure integration tests + 22 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated (`docker compose up -d --force-recreate web`); `lts-web` healthy.
- Workflow/review smoke (hosted on `http://localhost:5088`, dev boundary, Development + PostgreSQL): `GET /` → 200; `GET /health` → 200; `GET /health/ready` → 200; `POST /api/v1/auth/token` with `{"userKey":"admin"}` → 200 with a valid SecurityAdministrator JWT.

## Traceability Anchors

Sprint 1 is tied to these technical stories:

| Technical Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| TS-1.1 | TR-101 | New custom build scaffolded in this repository. |
| TS-1.2 | TR-104 | Local ASP.NET Core host established; deployment remains deferred. |
| TS-1.5 | TR-107 | .NET 10 baseline confirmed and pinned. |
| TS-6.1 | TR-601 | Versioned readiness API created. |
| TS-7.2 | xx-xxx | ASP.NET Core / Blazor WebAssembly stack documented and scaffolded. |
| TS-7.3 | xx-xxx | Modular monolith boundaries established. |

Sprint 2 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-1.3.1 | B.COM.11 | Work task creation with mandatory attributes. |
| US-2.1.1 | B.COM.03 | Automatic unique identifier assignment. |
| US-2.1.2 | B.RFA.03 | Identifier override with duplicate prevention. |

Sprint 3 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-1.3.2 | B.COM.12 | Assign and reassign tasks to DOR users; assigned user identifiable; reassignment retains work product and history. |
| US-1.3.3 | B.COM.13 | Multiple assignees per task with per-role due dates. |
| US-1.3.5 | B.COM.10 | User work queue with status, priority, role, due date, rework, and count. |

Sprint 4 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-2.2.1 | B.COM.05 | Work item relationships by topic, document type, legislative identifier, and named package. |
| US-2.2.2 | B.RFA.06 | Deliverable package grouping work products with an evaluable status. |
| US-2.3.1 | B.COM.04 | Identify, sort, filter, and group work by Confidential, Executive Review, On Hold, Work Type, and Package. |

Sprint 6 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-3.1.1 | B.COM.15 | Tasks follow a defined workflow; work and approval responsibilities separated; multiple reviewers; determinable workflow status. |
| US-3.1.2 | B.COM.19 | Completed products enter a review workflow; multiple subject matter experts participate; review before submission; determinable approval state. |
| US-3.1.3 | B.COM.20 | Only approved products finalized; finalized products packaged for submission; packages support internal and external recipients. |

Sprint 7 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-3.2.1 | B.RFA.01 | Executive Review restricted to designated users; fiscal notes and fiscal estimates can undergo Executive Review. |
| US-3.2.2 | B.RFA.02 | Entire Executive Review inside the solution; reviewer access, adjust/correct, completion, and automatic handoff until complete. |
| US-3.2.3 | B.RFA.04 | Step/subtask due dates; multiple step-level due dates within one product. |
| US-3.2.4 | B.RFA.05 | Settable priority per work product, viewable and usable in workload management. |

Sprint 8 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-4.1.1 | B.COM.17 | Designated work products drafted, edited, and reviewed with a rich-text editor and spell check. |
| US-4.1.2 | B.RFA.09 | Fiscal estimates and data requests drafted and reviewed with rich text and spell check. |
| US-4.1.3 | B.RFA.10 | Fiscal notes drafted and reviewed with a limited editor and spell check. |
| US-4.1.4 | B.LNP.04 | L&P products drafted and reviewed with rich text and spell check. |
| US-4.2.1 | B.COM.21 | Multiple attachments of supported formats remain associated with the task. |
| US-4.2.2 | B.COM.22 | Incomplete work saved and reopened without requiring completion or approval. |

Sprint 9 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-4.3.1 | B.COM.24 | Customized documentation generated from system data; usable internally and externally. |
| US-4.3.2 | B.COM.25 | Authorized users modify/update custom templates; maintained separately per work type. |
| US-4.3.3 | B.COM.26 | Templates/work papers auto-populated from system data. |
| US-4.3.4 | B.COM.27 | Documentation and templates shared with authorized users. |
| US-4.4.1 | B.COM.28 | Applicable work transferred to a new product without copy-and-paste. |
| US-4.4.2 | B.COM.29 | Prior-year product data transferred into similar-bill products without copy-and-paste. |

Sprint 10 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-1.1.1 | B.COM.01 | Multiple experts collaborate on a task from a single reference location. |
| US-1.1.2 | B.COM.08 | Multiple work products viewed simultaneously without discarding unsaved work. |
| US-1.2.1 | B.COM.02 | In-app notifications generated on assignment and directed to applicable users. |
| US-1.3.4 | B.COM.14 | Customer due date stored, viewable, and retained per work product. |
| US-1.4.1 | B.COM.18 | Tasks updated, canceled, changed, duplicated, or corrected. |
| US-1.4.2 | B.COM.23 | Work changed after submission/approval with prior state preserved in the audit trail. |

Sprint 11 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-5.1.1 | B.COM.31 | External bill language updates ingested and applied to corresponding bills. |
| US-5.1.2 | B.COM.32 | Bill status changes automatically reflected in the DOR solution. |
| US-5.1.3 | B.LNP.01 | Proposed and unadopted amendments imported and tracked. |
| US-5.2.1 | B.COM.33 | Bill version history retained through completion. |
| US-5.2.2 | B.COM.35 | Work-product version history retained through completion. |
| US-5.2.3 | B.LNP.02 | Users can compare versions of a bill. |
| US-5.2.4 | B.COM.43 | Bills followed across years within a biennium. |
| US-6.1.1 | B.COM.09 | Enterprise search across work products, bills, and documents. |
| US-6.2.1 | B.COM.38 | Standard reports (performance measures, outstanding fiscal tasks). |
| US-6.2.2 | B.COM.39 | Custom queries and reports created, saved, and reused. |
| US-6.2.3 | B.COM.40 | Governed database access for reporting. |
| US-6.2.4 | B.COM.34 | Work products extracted in required formats. |

Sprint 12 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-7.1.1 | B.COM.36 | Fiscal data (FTE, cost rules, revenue funds/sources) retrieved, calculated, and updated; fiscal-note calculation. |
| US-7.1.2 | B.COM.37 | Supporting documentation for fiscal work stored and retrievable with historical products. |
| US-7.2.1 | B.RFA.07 | Budget bills flagged and compared with associated fiscal notes. |
| US-7.2.2 | B.RFA.08 | Demographic data stored and retrieved by legislative session. |
| US-8.1.1 | B.COM.30 | Microsoft 365 integration boundaries: template population and recorded email dispatches. |
| US-14.1.1 | B.BGT.01 | Expense-estimate elements (goods/services, salary percentage) entered, updated, and incorporated into calculations. |

Sprint 13 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-9.1.1 | B.COM.06 | Role-based permissions distinguish preparation, approval, and delivery; read-only access; no restricted functions outside assigned permissions. |
| US-9.1.2 | B.COM.16 | Access restricted within work tasks/products by data type and user type; unauthorized users cannot access restricted information. |
| US-9.2.1 | B.COM.07 | More than 60 users can create work concurrently. |
| US-9.2.2 | B.COM.42 | Readiness/availability endpoint reports available status. |
| US-10.1.1 | B.COM.41 | Legacy data migrated as repeatable, validated, reconcilable, and rollback-able batches. |
| US-10.2.1 | B.EXP.01 | Authorized users can view work products across the required 10-year period. |

Sprint 14 is tied to these business stories:

| Business Story | Source Requirement | Current Evidence |
| --- | --- | --- |
| US-11.1.1 | B.LNP.03 | Correspondence recorded with recipient, sent state, response state, and linked bill/work context. |
| US-12.1.1 | B.LNP.04 | Implementation tasks assigned and reassigned across DOR with division, required work, due date, and completion. |
| US-12.1.2 | B.LNP.05 | Implementation-plan collaborators share related documents. |
| US-12.1.3 | B.LNP.06 | Implementation-task status report. |
| US-12.1.4 | B.LNP.07 | Bill flagged as requiring implementation with L&P manager notification. |
| US-12.1.5 | B.LNP.08 | L&P Manager assigns implementation tasks. |
| US-12.1.6 | B.LNP.09 | Authorized L&P users can review entire fiscal notes. |
| US-13.1.1 | B.EXEC.01 | Authorized Executive users can access remotely without the DOR VPN. |
| US-13.1.2 | B.EXEC.02 | Solution usable from supported DOR cell phones. |
| US-13.2.1 | B.EXEC.03 | Consolidated executive bill view with analysis and fiscal notes/estimates. |
| US-13.2.2 | B.EXEC.04 | Executive discussion on the bill page with participant notification. |

Use `docs/traceability/sprint-01-traceability.md`, `docs/traceability/sprint-02-traceability.md`, `docs/traceability/sprint-03-traceability.md`, `docs/traceability/sprint-04-traceability.md`, `docs/traceability/sprint-05-traceability.md`, `docs/traceability/sprint-06-traceability.md`, `docs/traceability/sprint-07-traceability.md`, `docs/traceability/sprint-08-traceability.md`, `docs/traceability/sprint-09-traceability.md`, `docs/traceability/sprint-10-traceability.md`, `docs/traceability/sprint-11-traceability.md`, `docs/traceability/sprint-12-traceability.md`, `docs/traceability/sprint-13-traceability.md`, `docs/traceability/sprint-14-traceability.md`, `docs/traceability/sprint-15-traceability.md`, `docs/traceability/sprint-16-traceability.md`, `docs/traceability/sprint-17-traceability.md`, `docs/traceability/sprint-18-traceability.md`, `docs/traceability/sprint-26-traceability.md`, `docs/traceability/sprint-27-traceability.md`, `docs/traceability/sprint-28-traceability.md`, `docs/traceability/sprint-29-traceability.md`, `docs/traceability/sprint-30-traceability.md`, `docs/traceability/sprint-31-traceability.md`, `docs/traceability/sprint-32-traceability.md`, `docs/traceability/sprint-33-traceability.md`, `docs/traceability/sprint-34-traceability.md`, and `docs/traceability/sprint-35-traceability.md` for the durable matrices.

Sprint 5 (POC hardening and demonstration) traces the Phase 1 acceptance criteria and technical quality requirements (seed data, walkthrough, format/build/test verification) as documented in `docs/traceability/sprint-05-traceability.md`.

Sprint 6 (Configurable Workflow, Phase 2 F3.1) traces US-3.1.1/B.COM.15, US-3.1.2/B.COM.19, and US-3.1.3/B.COM.20 as documented in `docs/traceability/sprint-06-traceability.md`.

Sprint 7 (RFA Executive Review, Phase 2 F3.2) traces US-3.2.1/B.RFA.01, US-3.2.2/B.RFA.02, US-3.2.3/B.RFA.04, and US-3.2.4/B.RFA.05 as documented in `docs/traceability/sprint-07-traceability.md`.

Sprint 8 (Content Authoring and Attachments, Phase 2 E4 slice) traces US-4.1.1/B.COM.17, US-4.1.2/B.RFA.09, US-4.1.3/B.RFA.10, US-4.1.4/B.LNP.04, US-4.2.1/B.COM.21, and US-4.2.2/B.COM.22 as documented in `docs/traceability/sprint-08-traceability.md`.

Sprint 9 (Templates, Generated Documents, and Reuse, Phase 2 completion) traces US-4.3.1/B.COM.24, US-4.3.2/B.COM.25, US-4.3.3/B.COM.26, US-4.3.4/B.COM.27, US-4.4.1/B.COM.28, and US-4.4.2/B.COM.29 as documented in `docs/traceability/sprint-09-traceability.md`.

Sprint 10 (Phase 1 Completion) traces US-1.1.1/B.COM.01, US-1.1.2/B.COM.08, US-1.2.1/B.COM.02, US-1.3.4/B.COM.14, US-1.4.1/B.COM.18, and US-1.4.2/B.COM.23 as documented in `docs/traceability/sprint-10-traceability.md`.

Sprint 11 (Phase 3) traces US-5.1.1/B.COM.31, US-5.1.2/B.COM.32, US-5.1.3/B.LNP.01, US-5.2.1/B.COM.33, US-5.2.2/B.COM.35, US-5.2.3/B.LNP.02, US-5.2.4/B.COM.43, US-6.1.1/B.COM.09, US-6.2.1/B.COM.38, US-6.2.2/B.COM.39, US-6.2.3/B.COM.40, and US-6.2.4/B.COM.34 as documented in `docs/traceability/sprint-11-traceability.md`.

Sprint 12 (Phase 4) traces US-7.1.1/B.COM.36, US-7.1.2/B.COM.37, US-7.2.1/B.RFA.07, US-7.2.2/B.RFA.08, US-8.1.1/B.COM.30, and US-14.1.1/B.BGT.01 as documented in `docs/traceability/sprint-12-traceability.md`.

Sprint 13 (Phase 5) traces US-9.1.1/B.COM.06, US-9.1.2/B.COM.16, US-9.2.1/B.COM.07, US-9.2.2/B.COM.42, US-10.1.1/B.COM.41, and US-10.2.1/B.EXP.01 as documented in `docs/traceability/sprint-13-traceability.md`.

Sprint 14 (Phase 6) traces US-11.1.1/B.LNP.03, US-12.1.1/B.LNP.04, US-12.1.2/B.LNP.05, US-12.1.3/B.LNP.06, US-12.1.4/B.LNP.07, US-12.1.5/B.LNP.08, US-12.1.6/B.LNP.09, US-13.1.1/B.EXEC.01, US-13.1.2/B.EXEC.02, US-13.2.1/B.EXEC.03, and US-13.2.2/B.EXEC.04 as documented in `docs/traceability/sprint-14-traceability.md`.

Sprint 15 (Production Hardening) traces US-9.1.1/B.COM.06 (role-to-permission matrix enforced at the HTTP layer), TR-601, TR-702, and TR-902, plus the production-hardening scope (Docker, CI/CD, infrastructure-as-code, operations) as documented in `docs/traceability/sprint-15-traceability.md`.

Sprint 16 (Production Hardening: PostgreSQL Persistence Foundation) traces the production-hardening persistence scope, TR-601, TR-702, and TR-902 as documented in `docs/traceability/sprint-16-traceability.md`.

Sprint 17 (Production Hardening: PostgreSQL Persistence Expansion) traces the production-hardening persistence scope, TR-601, TR-702, and TR-902 as documented in `docs/traceability/sprint-17-traceability.md`.

Sprint 18 (Production Hardening: Complete PostgreSQL Persistence) traces the production-hardening persistence scope, TR-601, TR-702, and TR-902 as documented in `docs/traceability/sprint-18-traceability.md`.

Sprint 25 (Production Hardening: API Hardening) traces the production-hardening API scope (rate limiting, request validation, security headers), TR-601, TR-702, and TR-902 as documented in `docs/traceability/sprint-25-traceability.md`.

Sprint 26 (Production Hardening: Client-Side Page Authorization Gating) traces the production-hardening client-authorization scope (shared permission matrix, client permission policies, route/page/navigation gating), US-9.1.1/B.COM.06, TR-702, and TR-902 as documented in `docs/traceability/sprint-26-traceability.md`.

Sprint 27 (Production Hardening: Real Entra Tenant Provisioning) traces the production-hardening Entra scope (startup configuration validation, client OIDC wiring, provisioning runbook, Terraform module), TR-702, and TR-902 as documented in `docs/traceability/sprint-27-traceability.md`.

Sprint 28 (Production Hardening: External Connector Live Endpoints) traces the production-hardening connector scope (startup configuration validation, connector connectivity health check, provisioning runbook), TR-702, and TR-902 as documented in `docs/traceability/sprint-28-traceability.md`.

Sprint 29 (Production Hardening: Deployment & Operations) traces the production-hardening deployment scope (Terraform state backend, deployment runbook, CI hardening, deploy pipeline Entra wiring), TR-702, and TR-902 as documented in `docs/traceability/sprint-29-traceability.md`.

Sprint 30 (Production Hardening: Persistence Policy) traces the production-hardening persistence scope (policy decision, mode resolution, startup enforcement), TR-702, and TR-902 as documented in `docs/traceability/sprint-30-traceability.md`.

Sprint 31 (Production Hardening: Acceptance) traces the production-hardening acceptance scope (acceptance criteria & Definition of Done, acceptance verification script), TR-702, and TR-902 as documented in `docs/traceability/sprint-31-traceability.md`.

Sprint 32 (Production Hardening: Client Authorization Test Coverage) traces the production-hardening client-authorization scope (client handler testability, client handler tests), TR-702, and TR-902 as documented in `docs/traceability/sprint-32-traceability.md`.

Sprint 33 (Production Hardening: API Hardening Refinement) traces the production-hardening API scope (rate-limit partitioning, per-route security headers, expanded request validation), TR-702, and TR-902 as documented in `docs/traceability/sprint-33-traceability.md`.

Sprint 34 (Production Hardening: Authoring & Template Hardening) traces the production-hardening authoring scope (merge-field catalog, template version control, notification channel/trigger), TR-702, and TR-902 as documented in `docs/traceability/sprint-34-traceability.md`.

Sprint 35 (Production Hardening: Workflow & Review Refinement) traces the production-hardening workflow scope (configurable workflow per type, reviewer notification delivery), TR-702, and TR-902 as documented in `docs/traceability/sprint-35-traceability.md`.

## Known Gaps and Deferrals

- Sprints 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, and 19 are promoted, implemented, and verified, and each has a completed PM Validation report (PM decision: Completed). Sprints 9, 10, 11, 12, 13, and 14 complete Phase 2, Phase 1, Phase 3, Phase 4, Phase 5, and Phase 6 respectively; Sprints 15, 16, 17, 18, and 19 advance the production-hardening and transition track.
- Sprint 19 migrates all current repository-backed aggregates, including generated documents, to PostgreSQL when a connection string is configured.
- The Sprint 6 workflow is a fixed state machine (not yet user-configurable per workflow type); "configurable workflow" is interpreted as the defined, explicit workflow states and reviewer requirements in that slice.
- The Sprint 7 Executive Review path is sequential (the open question "sequential, parallel, or configurable by product" is resolved as sequential for the POC); real reviewer notification (email) is deferred and represented by the automatic handoff.
- The Sprint 8 rich-text editor is a self-contained contenteditable editor with browser spell check and basic formatting; attachments store metadata only (binary storage, size limits, and malware scanning are deferred).
- The Sprint 9 template merge fields are a fixed, documented set (no user-defined field catalog or template version control yet); reuse copies content only (not attachments or full product structure).
- The Sprint 10 notifications are in-app events only (delivery channels, trigger catalog, and escalation are deferred); the customer due date is a single value per product; changing an approved product records the prior state in the audit trail but does not auto-reopen the workflow.
- The Sprint 11 external legislative ingestion is behind the application service boundary but has no real external agency connector, polling/scheduling, or confirmed "real-time" latency contract; version comparison is a simple line-set diff; custom reports are keyword-filter queries over work products (no full query language or governed SQL access layer).
- The Sprint 12 fiscal-system calls are behind the application service boundary but there is no real internal DOR system connector; fiscal-note and expense-estimate calculations are simple POC formulas; the Microsoft 365 integration is a boundary only (template population and recorded email dispatches) with no real Teams/Outlook/Excel/Word tenant integration; the budget-bill flag and fiscal-note comparison are POC-level; persisted data uses PostgreSQL when configured and the in-memory fallback remains available when no connection string is present.
- The Sprint 13 role-to-permission matrix is a POC model with no real authentication/authorization (Entra/OpenID Connect) at the HTTP layer; access-restriction granularity is modeled by data type string and user type; concurrency/availability are validated by unit tests and the readiness endpoint (no load benchmark or SLA/incident-response contract); legacy migration is a repeatable import that creates work tasks (no real legacy connector or reconciliation report beyond batch counts); the 10-year historical window is modeled on the work-product year.
- The Sprint 14 correspondence is manually logged (automatic Outlook capture deferred); implementation tasks are distinct from pre-enactment work tasks but there is no real legacy connector or reconciliation report beyond the status report; the executive bill view is read-optimized and source-linked but the "most common, important bill information" open question remains open; executive discussion is not treated as an official record subject to retention/audit requirements; remote/mobile access is a responsive web boundary with no native mobile application, conditional-access, device-management, or network-security standard established.
- The Sprint 15 JWT authentication is a POC symmetric-key implementation; real Entra/OpenID Connect (with the OpenID Connect package and an identity provider) is deferred. The token endpoint issues tokens by user key without a password, and the Web.Client transparently acquires a token for a default user (no interactive login). The Terraform topology and operations README are declarative code and are not deployed from this workspace; the Docker image is not pushed to a registry or scanned; GuardDuty/Security Hub and cross-region DR are follow-up items.
- The Sprint 25 rate limiter is a fixed-window policy applied globally to the `/api/v1` group; Sprint 33 added per-user/per-IP partitioning (keyed on the `sub` claim or client IP) but no sliding-window refinement. The security-headers set is fixed by default with per-route overrides available (Sprint 33). Request validation now covers 30+ request DTOs (Sprint 33), not every DTO in the API surface.
- The template merge-field catalog (Sprint 34) is a fixed list, not yet configurable per template type; template version snapshots are produced by the domain but not yet persisted as a separate version table; notification delivery channels beyond in-app (e.g., email) are modeled but not yet dispatched.
- The workflow definition (Sprint 35) is a fixed catalog, not yet editable at runtime; reviewer notifications are in-app only, with email delivery modeled but not yet dispatched.
- The UX overhaul (Sprints 36-37) establishes the theme foundation, design system (Bootswatch Flatly + Bootstrap Icons + `app.css`), and a Home dashboard. Individual forms are reworked onto the design system in dedicated sprints per `docs/backlog/ux-overhaul-sprints.md` (Sprints 38-59: one sprint per form). The `.dockerignore` previously excluded `**/wwwroot/lib/` so Bootstrap and icons were never served; that exclusion was removed so the theme ships in the container image.
- The Sprint 26 client-side gating is exercised in the dev boundary with the default SecurityAdministrator user; real role-based rendering requires a provisioned Entra tenant with role claims. The client-side `PermissionAuthorizationHandler` mirrors the server-side handler (both use the shared Domain `PermissionMatrix`); both handlers are now covered by unit tests (Sprint 32 added the client handler tests).
- The Sprint 27 Entra boundary is production-ready in code and provisioning artifacts, but a real Entra tenant is not provisioned from this offline workspace; the runbook and Terraform module document and declare the provisioning. Client secret rotation, certificate-based authentication, and tenant-level conditional access/MFA policies are follow-ups.
- The Sprint 28 connector layer is production-ready in code and provisioning artifacts, but live external endpoints are not provisioned from this offline workspace; the runbook documents the provisioning. The exact legislative and fiscal API contracts and the M365 Graph scopes must be confirmed against the live systems.
- The Sprint 29 deployment pipeline is declarative and not run from this offline workspace; it must be exercised in GitHub Actions against a real AWS account. The S3 state bucket and DynamoDB lock table must be bootstrapped before the first apply, and the Entra module uses local state in the deploy job (a durable remote backend for it is a follow-up).
- The Sprint 30 persistence policy keeps the in-memory fallback as a supported local/offline development mode; it is not a production persistence target. The in-memory adapters are not guaranteed to match EF Core transactional/query semantics exactly.
- The Sprint 31 acceptance verification script is a local tool; CI enforces the same checks in the CI workflow.
- The Sprint 16-18 PostgreSQL persistence work covers all current repository-backed aggregates. The in-memory adapter remains the fallback when no connection string is present.
- Rework is identified by a POC interpretation (reassignment to a user who previously held an assignment on the task); the source requirement does not define rework, so this interpretation is documented and open to refinement.
- The package definition open question (page 1 defines a package as a set of fiscal estimates, while the RFA description refers to packages containing estimates, fiscal notes, and data requests) remains open; the POC models a package as a named deliverable that can group any work products.
- Real authentication/authorization (Entra/OpenID Connect) is deferred; the identifier-override endpoint is exposed for the POC with the authorization boundary documented.
- Entra/OpenID Connect, SharePoint, Microsoft Graph, AWS deployment, WAF/SIEM integration, backup implementation, and disaster-recovery implementation are deferred.
- The Docker image is local only and has not been scanned, pushed to a registry, or deployed through CI/CD.
- TS-7.2 and TS-7.3 retain `xx-xxx` placeholder source requirement IDs from the source workbook.
- The repository is not initialized as a Git repository in this workspace, so use file-system and deterministic command checks rather than Git status for change accounting.
- Exact token and cost telemetry is not exposed in this local Codex task context. PM Validation reports must mark unavailable values explicitly.

## Next Promotion Path

Phase 6 is the final phase in the pair work plan (`docs/05-Pair-WorkPlan-and-Schedule.md`). All planned phases (Phase 1 through Phase 6) are complete, and the production-hardening and transition track is underway (Sprints 15 through 35 complete). The production-hardening backlog is now complete (Entra provisioning, external connector live endpoints, deployment & operations, persistence policy, acceptance, client authorization test coverage, API hardening refinement, authoring & template hardening, and workflow & review refinement). All offline-feasible known gaps are now closed; the remaining gaps require live infrastructure (real Entra tenant, live external endpoints, real AWS deployment, secret rotation, certificate auth, conditional access/MFA).

The business backlog under `docs/backlog/` is fully promoted through Phase 6; no proposed phase backlog remains.

A new UI/UX overhaul track is underway per `docs/backlog/ux-overhaul-sprints.md`: Sprint 36 (theme foundation & design system, Bootswatch Flatly + Bootstrap Icons + nav icons) and Sprint 37 (Home Dashboard) are complete, and Sprints 38-59 rework one form per sprint onto the design system (global layout & navigation, sign-in, work intake, work queue, work items, packages, bills, search, reports, fiscal, budget, demographics, productivity, templates, authoring, notifications, correspondence, implementation, executive/historical, security, migration/maintenance, utility cleanup). The next promotion is **Sprint 38 (Global Layout & Navigation)**.

## Non-Negotiables for the Next AI-Coder

- Do not implement scope that is not promoted in `docs/02-Current-Sprint.md`.
- Do not treat generated Markdown as replacing original `.docx` or `.xlsx` source files.
- Preserve source traceability from requirements to implementation, tests, docs, and PM Validation evidence.
- Use enterprise-quality patterns: DI by interface, constructor injection, narrow responsibilities, async/cancellation patterns where relevant, and no service locator.
- Run format, build, tests, and relevant smoke checks before claiming done.
- Update current sprint, traceability, handoff, PM Validation report, AI Metrics, and this handoff guide before closing work.
