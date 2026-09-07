# Backlog - UX Overhaul Sprints (Forms, Layout, and Theme)

Status: proposed

Last updated: 2026-09-06

## Objective

The current web-app forms share a plain Bootstrap 5 build with a custom light theme override (`src/Legislature.TrackingSystem.Web/wwwroot/app.css`). The layout and theme are functional but not user-friendly or visually polished. This backlog promotes a sprint per form so each form is reworked to a **beautiful, consistent Bootstrap 5 theme** that is easy to read and easy to use, moving away from the existing layout and theme.

## Guiding Approach

1. **Establish a shared design system first (Sprint 36)** so every later form sprint reuses one theme, one set of CSS design tokens, and one set of reusable components. This avoids divergent styling across forms.
2. **Adopt a clean, light, professional Bootstrap 5 theme** (Bootswatch-based, **Flatly**), with readable text in all forms and lists and no high-contrast/funky accents. The exact palette is set in Sprint 36.
3. **Refactor the global layout and navigation** (Sprint 37) onto the new theme before form-by-form work.
4. **One sprint per form** so each form can be reviewed and refined independently.

## UX Design Principles for Every Form

- **Easy to read** — clear hierarchy (page title, lede, section headings), good contrast, scannable density, consistent spacing.
- **Easy to use** — obvious primary actions, clear labels, helpful placeholders, grouped related fields, validation messages that are visible and friendly.
- **Beautiful and consistent** — a single theme, rounded and elevated cards, polished form controls, sensible focus states.

## Sprint Set

| Sprint | Scope / Form | Primary Pages |
| --- | --- | --- |
| 36 | Theme foundation & design system | `app.css`, shared design tokens, component styles, chosen Bootstrap 5 theme, nav icons |
| 37 | Home Dashboard | `Home.razor` |
| 38 | Global layout & navigation | `MainLayout.razor`, `NavMenu.razor` |
| 39 | Sign-in / Authentication | `Authentication.razor`, `AccessDenied.razor` |
| 40 | Work Intake | `WorkIntake.razor` |
| 41 | Work Queue | `WorkQueue.razor` |
| 42 | Work Items | `WorkItems.razor` |
| 43 | Packages | `Packages.razor` |
| 44 | Bills | `Bills.razor`, `Compare.razor` |
| 45 | Search | `Search.razor` |
| 46 | Reports | `Reports.razor` |
| 47 | Fiscal | `Fiscal.razor` |
| 48 | Budget Bills | `Budget.razor` |
| 49 | Demographics | `Demographics.razor` |
| 50 | Productivity | `Productivity.razor` |
| 51 | Templates | `Templates.razor` |
| 52 | Authoring | `Authoring.razor` |
| 53 | Notifications | `Notifications.razor` |
| 54 | Correspondence | `Correspondence.razor` |
| 55 | Implementation | `Implementation.razor` |
| 56 | Executive | `Executive.razor`, `Historical.razor` |
| 57 | Security (admin) | `Security.razor` |
| 58 | Migration | `Migration.razor`, `Maintenance.razor` |
| 59 | Utility & cleanup | `NotFound.razor`, shared utility styles, final consistency pass |

## TR-702 / TR-902 (applies to every UX sprint)

- **TR-702** — Automated static controls continue to pass (`dotnet format`).
- **TR-902** — The full solution test suite continues to pass (`dotnet test`); UI markup changes must not break existing web tests.
- The web app must build, serve, and pass the dev-boundary container smoke test after each sprint.

## Definition of Ready for a Form Sprint

A form sprint is ready when the theme foundation (Sprint 36) and layout/navigation (Sprint 38) are complete, so the form can be reworked against an established design system.
