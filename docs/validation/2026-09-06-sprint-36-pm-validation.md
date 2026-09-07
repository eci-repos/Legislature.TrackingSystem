# Sprint 36 - PM Validation Report

Sprint 36 - UX Overhaul: Theme Foundation & Design System

Status: complete

Date: 2026-09-06

## 1. PM Validation Decision

**Approved.** The theme foundation and design system are implemented, verified, and traceable.

## 2. Completed Scope

- Root-caused and fixed why Bootstrap never reached the browser: the container `.dockerignore` excluded `**/wwwroot/lib/`, so `lib/bootstrap/...` was omitted from the published image and every Bootstrap component rendered unstyled. Removed the exclusion so the theme ships into the container.
- Adopted a clean, light, professional Bootstrap 5 theme (**Bootswatch Flatly**) with a navy primary (`#2c3e50`) and deep-navy links for readable text in all forms and lists; removed the initial Quartz theme after PM feedback that it was too colorful and hard to read.
- Added **Bootstrap Icons** and a topic-specific icon on every left-side navigation option (fixing the template's blank `-nav-menu` placeholder icons), with a restyled light sidebar.
- Reworked `src/Legislature.TrackingSystem.Web/wwwroot/app.css` into a cohesive light design system with `--lts-*` CSS custom properties for color, spacing, radius, shadow, and typography, plus unified page header, form, card, alert, badge, table, and focus styles.
- Readability fixes: added readable muted-tone status badges for all workflow/exec-review/package/priority statuses (previously undefined `workflow-*`/`package-*` classes rendered white-on-white/invisible), set a friendly color-coded medium-blue primary for action buttons (green for approve/complete, red for reject).

## 3. Source Requirements / User Stories

This sprint addresses the PM-directed UX directive to overhaul forms, layout, and theme ("beautiful Bootstrap 5 themes" that are easy to read and easy to use). It is the foundation (Sprint 36) that the individual form sprints (Sprints 37-59) build upon per `docs/backlog/ux-overhaul-sprints.md`. TR-702 and TR-902 are asserted in this sprint.

## 4. Acceptance Evidence

| Criterion | Evidence |
| --- | --- |
| A light, professional, readable Bootstrap 5 theme is applied across the app | Bootswatch Flatly served at `GET /lib/bootstrap/dist/css/bootstrap.min.css` → 200, contains "Theme: flatly" and the navy primary (`44,62,80`). |
| The design system (CSS custom properties + shared component styles) is in place | `app.css` defines `--lts-*` tokens and shared light component styles; served at 200. |
| A topic-appropriate icon appears on every left-menu option | Bootstrap Icons css/fonts served (200); `NavMenu.razor` uses real icon glyphs for all 18 options. |
| The app builds, serves, and passes the dev-boundary container smoke test | Build 0 warnings/0 errors; container rebuilt and recreated; `/`, `/health`, `/health/ready` all 200; all rendered CSS/font assets resolve 200. |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | This report + `docs/traceability/sprint-36-traceability.md` + handoff guide update. |

## 5. Verification Results

- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` — 0 warnings, 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` — 251 tests pass (206 domain + 23 infrastructure + 22 web).
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` — passes.
- Container (dev boundary, Development + PostgreSQL): `docker compose build --no-cache web`, `up -d --force-recreate web`. Smoke test: `/`, `/health`, `/health/ready`, `/lib/bootstrap/dist/css/bootstrap.min.css` all 200.
- Container static-asset manifest now contains 45 `bootstrap.min.css` references (was 0), confirming the library ships once the `.dockerignore` exclusion is removed.

## 6. Delivered Artifacts

- `src/Legislature.TrackingSystem.Web/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css` (Bootswatch Flatly theme).
- `src/Legislature.TrackingSystem.Web/wwwroot/lib/bootstrap-icons/` (Bootstrap Icons css + woff/woff2 fonts).
- `src/Legislature.TrackingSystem.Web/Components/App.razor` (references the icon stylesheet).
- `src/Legislature.TrackingSystem.Web/wwwroot/app.css` (light design system).
- `src/Legislature.TrackingSystem.Web.Client/Layout/NavMenu.razor` + `NavMenu.razor.css` + `MainLayout.razor.css` (real nav icons, light sidebar).
- `.dockerignore` (removed `**/wwwroot/lib/` exclusion).
- `docs/backlog/ux-overhaul-sprints.md` (sprint-per-form roadmap).
- `docs/traceability/sprint-36-traceability.md`.
- `docs/validation/2026-09-06-sprint-36-pm-validation.md` (this report).

## 7. Residual Risks / Deferrals

- This sprint establishes the theme foundation (CSS-only). Individual forms are reworked onto the design system in their own sprints (Sprints 37-59). Per-form layout/markup refinements are deferred to those sprints.
- The Flatly theme is served from the bundled static asset tree; any future theme swap replaces the same file and the manifest regenerates on rebuild.
- Exact token/cost telemetry is unavailable in this local execution context (see AI Metrics below).

## 8. AI Metrics

| Metric | Value |
| --- | --- |
| Model | deepseek-v4-flash:0731-cloud |
| Provider | DeepSeek (cloud) |
| Environment | Local agent harness (sandboxed PowerShell + file access) |
| Token counts | Unavailable - no telemetry exported by the local harness |
| Cost | Unavailable - no billing/telemetry source available |

> Per project policy, exact token/cost values are marked unavailable rather than estimated because the local execution environment does not export token or billing telemetry.
