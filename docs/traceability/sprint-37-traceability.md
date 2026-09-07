# Sprint 37 - UX Overhaul: Home Dashboard Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 37 replaces the developer-facing Home page (Sprint-1 traceability evidence) with a user-facing **Dashboard** that surfaces useful legislative tracking details in workflow/life-cycle order, built on the established light theme, design system, and icons.

## Source / Requirement Trace

| Item | Source | Sprint 37 Evidence | Artifact |
| --- | --- | --- | --- |
| Replace the Home tab with a useful Dashboard | PM UX directive (Home tab not useful; show tracking details in workflow/life-cycle order) | Rewrote `Home.razor` as a read-only dashboard with KPI cards, a workflow funnel, needs-attention, recent items, packages in progress, and executive-review panels. | `src/Legislature.TrackingSystem.Web.Client/Pages/Home.razor` |
| Display tracking details in workflow/life-cycle order | PM UX directive | Workflow funnel renders Draft → Pending Review → Under Review → Approved → Finalized with live counts; panels link into the relevant pages. | `Home.razor` |
| Reuse existing data/API (no new surface) | Reuse-before-generation | Dashboard reads `GET /api/v1/work-items?groupBy=None` and `GET /api/v1/packages`; no new endpoints. | `Home.razor` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes. | verification run |
| TR-902 automated tests | TR-902 | The full solution test suite passes (206 domain + 23 infrastructure + 22 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 37 Evidence | Artifact |
| --- | --- | --- | --- |
| Read-only, user-facing dashboard | PM UX directive | Panels are display-only; navigation links go to the existing pages for actions. | `Home.razor` |
| Consistent theme/design system | PM UX directive | Uses the light theme, `card`/`badge`/`workflow-*` design-system classes. | `Home.razor`, `app.css` |
| Robust loading/error handling | Enterprise quality | `OnInitializedAsync` loads data with busy/error states; `ApiErrorFormatter` surfaces failures. | `Home.razor` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| The Home page is a useful, read-only dashboard showing legislative tracking details in workflow/life-cycle order | Implemented |
| The dashboard reuses the existing light theme, design system, and icons, and links into the relevant pages | Implemented |
| The app builds, serves, and passes the dev-boundary container smoke test | Verified |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 37 does not change application persistence or API behavior. It changes only the client presentation layer (the Home page markup and its data-loading logic).

## Verification Evidence

- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 206 domain + 23 infrastructure + 22 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, Development + PostgreSQL):
  - `GET /` → 200 (dashboard serves).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `GET /api/v1/work-items?groupBy=None` → 200 (dashboard data).
  - `GET /api/v1/packages` → 200 (dashboard data).

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- The dashboard is read-only and reflects the current data; it does not yet include per-user "my queue" or notification panels (deferred to the Work Queue and Notifications form sprints).
- Exact token/cost telemetry is unavailable in this local execution context.

## Seed Data Enhancement (follow-up)

To showcase search, listing, and related features, the idempotent `SeedDataInitializer` was extended to seed a coherent, realistic DOR dataset as a follow-up of the core HB 1200 / SB 88 thread, and the dev database was reset so the full seed runs fresh on startup. This populates every previously-empty or auto-generated table with realistic data (8 bills, 14 work items, 2 packages, fiscal data, demographics, templates, notifications, correspondence, implementation tasks, executive discussions, custom reports, access restrictions, fiscal work papers, expense-estimate elements, bill-fiscal-note links, email dispatches, generated documents, a migration batch, and 11 users). `VersionService.GetBillHistoryAsync` now returns all bills when no bill number is supplied so the Bills page lists them. Verified: build 0 warnings/errors, 251 tests pass, `dotnet format` passes, container rebuilt/recreated, and all listing/search endpoints return data.
