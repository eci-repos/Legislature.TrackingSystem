# Sprint 9 - Templates, Generated Documents, and Reuse Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 9 is the final Phase 2 vertical slice: F4.3 - Templates and Generated Documents and F4.4 - Reuse Existing Work (per `docs/backlog/phase-02-workflow-review-authoring-and-document-production.md`). It implements BI-US-4.3.1 (US-4.3.1/B.COM.24), BI-US-4.3.2 (US-4.3.2/B.COM.25), BI-US-4.3.3 (US-4.3.3/B.COM.26), BI-US-4.3.4 (US-4.3.4/B.COM.27), BI-US-4.4.1 (US-4.4.1/B.COM.28), and BI-US-4.4.2 (US-4.4.2/B.COM.29). DOR users generate customized documentation from system data using maintainable templates, share templates with authorized users, and transfer applicable work from existing or prior-year products into new products without copy-and-paste. This completes Phase 2.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 9 Evidence | Artifact |
| --- | --- | --- | --- |
| US-4.3.1 generate customized documentation from system data; internal/external use | B.COM.24 | `TemplateRenderer` renders `{{Field}}` merge fields from work-product data; `GenerateDocumentAsync` returns a rendered `GeneratedDocument`. | `Application/WorkItems/TemplateRenderer.cs`, `TemplateService.cs` |
| US-4.3.2 modify/update custom templates; maintained separately per type | B.COM.25 | `DocumentTemplate` with `ApplicableWorkType`; `UpdateTemplateAsync`; `/templates` page edit. | `Domain/WorkItems/DocumentTemplate.cs`, `TemplateService.cs`, `Web.Client/Pages/Templates.razor` |
| US-4.3.3 templates/work papers auto-populated from system data | B.COM.26 | Merge-field rendering populates templates from work-product data (type, indicators, flags represented by work attributes). | `TemplateRenderer.cs`, `TemplateService.cs` |
| US-4.3.4 share documentation and templates with authorized users | B.COM.27 | `DocumentTemplate.IsShared` + `SetTemplateSharedAsync`; shared flag surfaced in the `/templates` UI. | `DocumentTemplate.cs`, `TemplateService.cs`, `Templates.razor` |
| US-4.4.1 transfer applicable work to a new product without copy-and-paste | B.COM.28 | `WorkTask.ReuseContentFrom` copies content; destination remains a distinct, independently identifiable task. | `Domain/WorkItems/WorkTask.cs`, `ReuseService.cs` |
| US-4.4.2 transfer prior-year product data into similar-bill products | B.COM.29 | Reuse copies content from a source (prior) product into a target; the prior product remains separately available. | `WorkTask.cs`, `ReuseService.cs`, `Authoring.razor` |

## Technical Quality Trace

| Requirement | Source | Sprint 9 Evidence | Artifact |
| --- | --- | --- | --- |
| Automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `Directory.Build.props` applies `TreatWarningsAsErrors`. | `dotnet format`, `Directory.Build.props` |
| Automated tests asserting acceptance criteria | TR-902 | 79 tests pass (0 failed); 9 new tests cover template create/update/share/generate and reuse. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/TemplateServiceTests.cs`, `ReuseServiceTests.cs` |
| Versioned RESTful API surface | TR-601 | New `/api/v1` endpoints: `GET/POST templates`, `PUT templates/{id}`, `PUT templates/{id}/shared`, `POST work-items/{id}/documents/generate`, `POST work-items/{id}/reuse`. | `src/Legislature.TrackingSystem.Web/Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| Documentation can be generated from system data using templates; templates can be created, edited, and shared | Implemented |
| Applicable work can be transferred from an existing or prior product into a new product without copy-and-paste; destination remains independently identifiable | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 9 continues the interim in-memory repository adapter; PostgreSQL persistence and EF Core migrations remain deferred.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 79 tests (0 failed, 0 skipped).
- Container smoke checks (template/document/reuse API and UI render) were verified against `http://localhost:5088` after `docker compose up -d --build --force-recreate web`.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 9 defects; the standard AGENTS.md commands work on a normal developer/container host.
