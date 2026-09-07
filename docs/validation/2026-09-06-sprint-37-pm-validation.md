# Sprint 37 - PM Validation Report

Sprint 37 - UX Overhaul: Home Dashboard

Status: complete

Date: 2026-09-06

## 1. PM Validation Decision

**Approved.** The Home page is now a useful, read-only dashboard showing legislative tracking details in workflow/life-cycle order, verified and traceable.

## 2. Completed Scope

- Replaced the developer-facing Home page (Sprint-1 traceability evidence) with a user-facing **Dashboard** in `Home.razor`.
- Panels: KPI stat cards (total, in review, approved, finalized, overdue, packages in progress), a **workflow funnel** (Draft → Pending Review → Under Review → Approved → Finalized), **needs attention** (pending/under review or overdue), **recent work items**, **packages in progress**, and an **executive review** snapshot.
- Reuses existing read endpoints (`GET /api/v1/work-items?groupBy=None`, `GET /api/v1/packages`); no new API surface. Each panel links into the existing pages.

## 3. Source Requirements / User Stories

This sprint addresses the PM-directed UX directive to replace the non-useful Home tab with a dashboard of useful legislature tracking details in workflow/life-cycle order. TR-702 and TR-902 are asserted in this sprint.

## 4. Acceptance Evidence

| Criterion | Evidence |
| --- | --- |
| The Home page is a useful, read-only dashboard showing tracking details in workflow/life-cycle order | `Home.razor` renders KPI cards, workflow funnel, needs-attention, recent items, packages in progress, and executive-review panels. |
| The dashboard reuses the existing light theme, design system, and icons, and links into the relevant pages | Uses `card`/`badge`/`workflow-*` design-system classes; panels link to Work Items, Work Queue, Packages, Executive. |
| The app builds, serves, and passes the dev-boundary container smoke test | Build 0 warnings/0 errors; container rebuilt and recreated; `/`, `/health`, `/health/ready` all 200; dashboard data endpoints 200. |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | This report + `docs/traceability/sprint-37-traceability.md` + handoff guide update. |

## 5. Verification Results

- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` — 0 warnings, 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` — 251 tests pass (206 domain + 23 infrastructure + 22 web).
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` — passes.
- Container (dev boundary, Development + PostgreSQL): `docker compose build web`, `up -d --force-recreate web`. Smoke test: `/`, `/health`, `/health/ready`, `/api/v1/work-items?groupBy=None`, `/api/v1/packages` all 200.

## 6. Delivered Artifacts

- `src/Legislature.TrackingSystem.Web.Client/Pages/Home.razor` (dashboard).
- `docs/traceability/sprint-37-traceability.md`.
- `docs/validation/2026-09-06-sprint-37-pm-validation.md` (this report).
- `docs/02-Current-Sprint.md` (Sprint 37 complete + listed).
- `docs/backlog/ux-overhaul-sprints.md` (Home Dashboard promoted as Sprint 37; layout/navigation moved to Sprint 38).

## 7. Residual Risks / Deferrals

- The dashboard is read-only and reflects current data; per-user "my queue" and notification panels are deferred to the Work Queue and Notifications form sprints.
- Exact token/cost telemetry is unavailable in this local execution context (see AI Metrics below).

## 7a. Seed Data Enhancement (follow-up)

To showcase search, listing, and related features, the idempotent `SeedDataInitializer` was extended to seed a coherent, realistic DOR dataset as a follow-up of the core HB 1200 / SB 88 thread, and the dev database was reset so the full seed runs fresh on startup. This populates every previously-empty or auto-generated table with realistic data: 8 bills (with versions/amendments/flags), 14 work items across workflow states, 2 packages, fiscal data, demographics, templates, notifications, correspondence, implementation tasks, executive discussions, custom reports, access restrictions, fiscal work papers, expense-estimate elements, bill-fiscal-note links, email dispatches, generated documents, a migration batch, and 11 users with roles. `VersionService.GetBillHistoryAsync` now returns all bills when no bill number is supplied so the Bills page lists them. Verified: build 0 warnings/errors, 251 tests pass, `dotnet format` passes, container rebuilt/recreated, and all listing/search endpoints return data.

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | deepseek-v4-flash:0731-cloud |
| Provider | DeepSeek (cloud) |
| Environment | Local agent harness (sandboxed PowerShell + file access) |
| Token counts | Unavailable - no telemetry exported by the local harness |
| Cost | Unavailable - no billing/telemetry source available |

> Per project policy, exact token/cost values are marked unavailable rather than estimated because the local execution environment does not export token or billing telemetry.
