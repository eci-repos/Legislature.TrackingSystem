# Sprint 36 - UX Overhaul: Theme Foundation & Design System Traceability

Status: complete

Last updated: 2026-09-06

## Scope

Sprint 36 establishes the visual foundation for the UX overhaul: it adopts a clean, light, professional Bootstrap 5 theme (**Flatly**), adds a topic-appropriate icon to every left-side navigation option (Bootstrap Icons), and reworks the shared stylesheet into a cohesive light design system that every later form sprint reuses.

## Source / Requirement Trace

| Item | Source | Sprint 36 Evidence | Artifact |
| --- | --- | --- | --- |
| Adopt a beautiful, light, professional Bootstrap 5 theme across the app | PM UX directive (forms/layout/theme overhaul: readable, professional, light, no funky colors) | Replaced the plain Bootstrap 5 build with the Bootswatch "Flatly" themed `bootstrap.min.css` and overrode link color to a deep navy for legibility. | `src/Legislature.TrackingSystem.Web/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css` |
| Topic-appropriate icon on every left-menu option | PM UX directive (icon per option reflecting its topic) | Added Bootstrap Icons css/fonts and rewrote `NavMenu.razor` so every nav option uses a real icon glyph; restyled the nav for icon/text spacing, hover, and active states. | `src/Legislature.TrackingSystem.Web/wwwroot/lib/bootstrap-icons/`, `NavMenu.razor`, `NavMenu.razor.css`, `MainLayout.razor.css`, `App.razor` |
| Establish a shared light design system (tokens, type, controls, cards) | PM UX directive | Reworked `app.css` into a light design-system stylesheet with CSS custom properties for color, spacing, radius, shadow, and typography; updated button, form, card, alert, badge, table, page-header, and focus styles. | `src/Legislature.TrackingSystem.Web/wwwroot/app.css` |
| TR-702 automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; warning-as-error posture remains. | `Directory.Build.props`, verification run |
| TR-902 automated tests | TR-902 | The full solution test suite passes (206 domain + 23 infrastructure + 22 web). | `tests/` |

## Technical Quality Trace

| Requirement | Source | Sprint 36 Evidence | Artifact |
| --- | --- | --- | --- |
| Consistent, light, readable theme | PM UX directive | Bootswatch "Flatly" applied; navy primary (`#2c3e50`) and deep-navy links for readability on light surfaces. | `wwwroot/lib/bootstrap/dist/css/bootstrap.min.css` |
| Reusable design tokens | PM UX directive | `:root` CSS custom properties (`--lts-*`) centralize color, radius, shadow, spacing, and font for reuse across form sprints. | `wwwroot/app.css` |
| Cohesive shared components | PM UX directive | Page shell, form controls, cards, alerts, badges, tables, and focus states unified under the light design system. | `wwwroot/app.css` |
| Icons render on every nav option | PM UX directive | Real Bootstrap Icons glyphs replace the template's blank `-nav-menu` placeholders; icon font css/fonts served. | `wwwroot/lib/bootstrap-icons/`, `NavMenu.razor` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| A premium, consistent, light and professional Bootstrap 5 theme is applied across the app, with readable text in all forms and lists | Implemented |
| The design system (CSS custom properties + shared component styles) is in place | Implemented |
| The left-side navigation shows a topic-appropriate icon on every option | Implemented |
| The app builds, serves, and passes the dev-boundary container smoke test | Verified |
| `dotnet format`, `dotnet build`, and `dotnet test` pass | Verified |
| Traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 36 does not change persistence or API behavior. It changes only the client presentation layer (theme, shared component styles, nav icons). The `.dockerignore` change (removing `**/wwwroot/lib/`) only affects what static assets ship into the container image.

## Verification Evidence

- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1` passed: 206 domain + 23 infrastructure + 22 web tests (0 failed, 0 skipped).
- Container rebuilt and recreated; `lts-web` healthy. Smoke test results (dev boundary, Development + PostgreSQL):
  - `GET /` → 200 (client app serves with the new theme).
  - `GET /health` → 200 (liveness).
  - `GET /health/ready` → 200 (readiness).
  - `GET /lib/bootstrap/dist/css/bootstrap.min.css` → 200 (Bootswatch "Flatly", navy primary).
  - `GET /lib/bootstrap-icons/bootstrap-icons.css` → 200; `GET /lib/bootstrap-icons/fonts/bootstrap-icons.woff2` → 200.
  - All rendered CSS/font asset links resolve to 200.

> Environment note: This build/verification environment uses single-node MSBuild (`-m:1`) and elevated file/process permissions for reliable local verification. The WebAssembly client build and `dotnet format` require the ability to spawn MSBuild/Roslyn task-host processes, which the sandbox's process isolation blocks without elevated permissions.

## Residual Risks and Deferrals

- The theme foundation is CSS/markup-only; individual forms are reworked onto the design system in later sprints (Sprints 37-59 per `docs/backlog/ux-overhaul-sprints.md`).
- Per-form layout refinements and markup changes are deferred to each form's dedicated sprint.
- Exact token/cost telemetry is unavailable in this local execution context.
