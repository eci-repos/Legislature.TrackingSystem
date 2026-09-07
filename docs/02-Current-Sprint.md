# Current Sprint

Sprint: Sprint 37 - UX Overhaul: Home Dashboard

Status: complete

Last updated: 2026-09-06

## Sprint Objective

Replace the developer-facing Home page (Sprint-1 traceability evidence) with a user-facing **Dashboard** that surfaces useful legislative tracking details in workflow/lifecycle order, using the established light theme, design system, and icons.

## Authorized Scope

- **Dashboard panels** — Replace `Home.razor` with a dashboard of read-only panels: KPI stat cards, a workflow funnel (Draft → Pending Review → Under Review → Approved → Finalized), items needing attention (pending review / overdue), recent work items, packages in progress, and an executive-review snapshot.
- **Data source** — Reuse existing read endpoints (`GET /api/v1/work-items`, `GET /api/v1/packages`, `GET /api/v1/notifications`) to populate the panels; no new API surface.
- **Navigation** — Each panel links into the existing pages (Work Items, Work Queue, Packages, Executive).
- **TR-702** — Automated static controls continue to pass.
- **TR-902** — Automated tests assert the promoted acceptance criteria; existing web tests continue to pass.

## Persistence Decision

Sprint 37 does not change application persistence or API behavior. It changes only the client presentation layer (the Home page markup and its data-loading logic).

## Completion Criteria

- The Home page is a useful, read-only dashboard showing legislative tracking details in workflow/lifecycle order.
- The dashboard reuses the existing light theme, design system, and icons, and links into the relevant pages.
- The app builds, serves, and passes the dev-boundary container smoke test.
- `dotnet format` passes, `dotnet build` passes with 0 warnings/errors, and `dotnet test` passes.
- Traceability, handoff, and PM Validation documentation are updated, including AI Metrics.

## Completed Work

- **Replaced the Home page with a user-facing Dashboard** — Rewrote `Home.razor` (previously Sprint-1 traceability evidence) as a read-only dashboard of panels that surface legislative tracking details in workflow/life-cycle order:
  - **KPI stat cards** — Total work items, In review, Approved, Finalized, Overdue, Packages in progress.
  - **Workflow funnel** — Draft → Pending Review → Under Review → Approved → Finalized with live counts.
  - **Needs attention** — items pending/under review or overdue, ordered by due date.
  - **Recent work items** — newest items by created date.
  - **Packages in progress** and **Executive review** snapshots.
- **Data source** — Reuses existing read endpoints (`GET /api/v1/work-items?groupBy=None`, `GET /api/v1/packages`); no new API surface. Each panel links into the existing pages (Work Items, Work Queue, Packages, Executive).
- **Theme/design system** — Built on the established light theme, design system, and status badges.

## Verification

- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` — 0 warnings, 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` — 251 tests pass (206 domain + 23 infrastructure + 22 web).
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` — passes.
- Container rebuilt (`docker compose build web`) and recreated; dev boundary (Development + PostgreSQL):
  - `GET /` → 200 (dashboard serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `GET /api/v1/work-items?groupBy=None` → 200 (dashboard data).
  - `GET /api/v1/packages` → 200 (dashboard data).

## PM Validation

- PM Validation report prepared: `docs/validation/2026-09-06-sprint-37-pm-validation.md`.
- Traceability report prepared: `docs/traceability/sprint-37-traceability.md`.

## Seed Data Enhancement (follow-up)

To showcase search, listing, and related features, the idempotent `SeedDataInitializer` was extended to seed a coherent, realistic DOR dataset as a follow-up of the core HB 1200 / SB 88 legislative thread. The dev database was reset so the full seed runs fresh on startup.

- **Bills** — 8 realistic bills (HB 1200, SB 88, HB 1201, SB 89, HB 1300, SB 101, HB 1400, SB 110) with versions, amendments, and budget/implementation flags.
- **Work items** — 14 items across types and workflow states (Draft, PendingReview, UnderReview, Approved, Rejected, Finalized) with assignments, content, comments, and executive review.
- **Supporting data** — fiscal data (8), demographics (2 sessions), templates (4), notifications (6), correspondence (3), implementation tasks (3), executive discussions (2), custom reports (3), access restrictions (2), fiscal work papers (2), expense-estimate elements (3), bill-fiscal-note links (2), email dispatches (2), generated documents (1), migration batch (1), and 11 users with roles.
- **Bills listing fix** — `VersionService.GetBillHistoryAsync` now returns all bills when no bill number is supplied, so the Bills page lists the seeded bills instead of an empty table.
- **Verification** — build 0 warnings/errors, 251 tests pass, `dotnet format` passes; container rebuilt/recreated; `/api/v1/bills/history?billNumber=` returns 8 bills, `/api/v1/notifications?userKey=admin` returns notifications, and all listing/search endpoints return data.

## Results List Width Fix (follow-up)

The Work Items "Results" list (and the identical Work Queue and Packages lists) were capped at the standard page width (`page-shell` 1080px / `queue-wrap` 1120px), so the wide table was cut off on the right. Fixed so the list uses the full available window width, with horizontal scroll on narrow windows:

- Added a `.page-shell-wide` variant (`max-width: 100%`) and applied it to the Work Items, Work Queue, and Packages pages.
- Changed `.queue-wrap` to `max-width: 100%`.
- Gave `.queue-table` a `min-width: 1000px` so on narrow windows the existing `.table-responsive` wrapper scrolls horizontally instead of squeezing/cutting columns; on wide windows the table fills the available width.
- Verified: build 0 warnings/errors, 251 tests pass, `dotnet format` passes; container rebuilt/recreated; the served `app.css` contains the new rules.

## Selected-List-Item Readability Fix (follow-up)

On the Bills tab (and the identical list patterns on Budget, Authoring, Maintenance, and Templates), selecting a list item applied Bootstrap's default `.list-group-item.active` dark primary background, which made the inner `text-muted` description/label unreadable. Fixed globally in `app.css` so a selected list item uses a light, readable highlight (`#e8f1f9` background, normal text color, and a readable muted tone for inner labels). Verified: build 0 warnings/errors, 251 tests pass, `dotnet format` passes; container rebuilt/recreated; the served `app.css` contains the override.

## Dev-Boundary Token Fix (follow-up)

The Historical page showed nothing because its endpoint (`/api/v1/historical`) is the only one that requires authorization (`RequireViewHistorical`), but the dev-boundary Web.Client used a plain `HttpClient` with no token, so the API returned 401. Fixed by adding a `DevTokenMessageHandler` in `Web.Client/Program.cs` that transparently acquires a JWT for the default `admin` user from `/api/v1/auth/token` and attaches it as a Bearer token to API requests. This makes authenticated endpoints (e.g. Historical) work without manual login in the dev boundary. Verified: build 0 warnings/errors, 251 tests pass, `dotnet format` passes; container rebuilt/recreated; `/api/v1/historical?years=10` returns 17 work products.

## End-User Glossary (follow-up)

Created `docs/12-End-User-Glossary.md`, a plain-language table reference of all application terms for end users, grouped by topic (core concepts, legislative objects, work items, workflow, executive review, packages, fiscal data, users/roles/permissions, other features) with every status value listed in tables. Linked from the end-user guide (`docs/11-End-User-Guide.md`) and referenced in the handoff guide.

## Completed Sprints

- Sprint 0 — Resource readiness (complete, PM-validated).
- Sprint 1 — POC foundation and architecture scaffold (complete, PM-validated).
- Sprint 1A — Local container readiness (complete, PM-validated).
- Sprint 2 — POC work intake and identifiers (complete, PM Validation prepared).
- Sprint 3 — POC assignment and work queues (complete, PM Validation prepared).
- Sprint 4 — POC relationships, packages, and categorization (complete, PM Validation prepared).
- Sprint 5 — POC hardening and demonstration (complete, PM Validation prepared).
- Sprint 6 — Configurable Workflow, Phase 2 F3.1 (complete, PM Validation prepared).
- Sprint 7 — RFA Executive Review, Phase 2 F3.2 (complete, PM Validation prepared).
- Sprint 8 — Content Authoring and Attachments, Phase 2 E4 slice (complete, PM Validation prepared).
- Sprint 9 — Templates, Generated Documents, and Reuse, Phase 2 completion (complete, PM Validation prepared).
- Sprint 10 — Phase 1 Completion (collaboration, notifications, customer due date, task maintenance) (complete, PM Validation prepared).
- Sprint 11 — Phase 3: Legislative Data Lifecycle, Search, and Reporting (complete, PM Validation prepared).
- Sprint 12 — Phase 4: Fiscal Analysis, Financial Inputs, and Productivity Integration (complete, PM Validation prepared).
- Sprint 13 — Phase 5: Security, Operations, Migration, and Historical Reference (complete, PM Validation prepared).
- Sprint 14 — Phase 6: Specialized Legislative Programs and Executive Experience (complete, PM Validation prepared).
- Sprint 15 — Production Hardening: Authentication, Docker, and CI/CD (complete, PM Validation prepared).
- Sprint 16 — Production Hardening: PostgreSQL Persistence Foundation (complete, PM Validation prepared).
- Sprint 17 — Production Hardening: PostgreSQL Persistence Expansion (complete, PM Validation prepared).
- Sprint 18 — Production Hardening: Complete PostgreSQL Persistence (complete, PM Validation prepared).
- Sprint 19 — Production Hardening: Persist Generated Documents (complete, PM Validation prepared).
- Sprint 20 — Production Hardening: Entra/OpenID Connect Authentication Boundary (complete, PM Validation prepared).
- Sprint 21 — Production Hardening: Web.Client Interactive Entra Sign-In (complete, PM Validation prepared).
- Sprint 22 — Production Hardening: Deployment Hardening (Registry, Scanning, Terraform, DR) (complete, PM Validation prepared).
- Sprint 23 — Production Hardening: External Connectors (Legislative, Fiscal, M365) (complete, PM Validation prepared).
- Sprint 24 — Production Hardening: Observability (Logging, Health, Telemetry) (complete, PM Validation prepared).
- Sprint 25 — Production Hardening: API Hardening (Rate Limiting, Validation, Security Headers) (complete, PM Validation prepared).
- Sprint 26 — Production Hardening: Client-Side Page Authorization Gating (complete, PM Validation prepared).
- Sprint 27 — Production Hardening: Real Entra Tenant Provisioning (complete, PM Validation prepared).
- Sprint 28 — Production Hardening: External Connector Live Endpoints (complete, PM Validation prepared).
- Sprint 29 — Production Hardening: Deployment & Operations (complete, PM Validation prepared).
- Sprint 30 — Production Hardening: Persistence Policy (complete, PM Validation prepared).
- Sprint 31 — Production Hardening: Acceptance (complete, PM Validation prepared).
- Sprint 32 — Production Hardening: Client Authorization Test Coverage (complete, PM Validation prepared).
- Sprint 33 — Production Hardening: API Hardening Refinement (complete, PM Validation prepared).
- Sprint 34 — Production Hardening: Authoring & Template Hardening (complete, PM Validation prepared).
- Sprint 35 — Production Hardening: Workflow & Review Refinement (complete, PM Validation prepared).
- Sprint 36 — UX Overhaul: Theme Foundation & Design System (complete, PM Validation prepared).
- Sprint 37 — UX Overhaul: Home Dashboard (complete, PM Validation prepared).
