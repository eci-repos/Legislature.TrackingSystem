# Sprint 8 - Content Authoring and Attachments Traceability

Status: complete

Last updated: 2026-09-04

## Scope

Sprint 8 is the third Phase 2 vertical slice: the first E4 slice (F4.1 - Rich-Text Authoring and F4.2 - Attachments and Work in Progress, per `docs/backlog/phase-02-workflow-review-authoring-and-document-production.md`). It implements BI-US-4.1.1 (US-4.1.1/B.COM.17), BI-US-4.1.2 (US-4.1.2/B.RFA.09), BI-US-4.1.3 (US-4.1.3/B.RFA.10), BI-US-4.1.4 (US-4.1.4/B.LNP.04), BI-US-4.2.1 (US-4.2.1/B.COM.21), and BI-US-4.2.2 (US-4.2.2/B.COM.22). DOR users draft, edit, and review work products inside the solution with a rich-text editor (and a limited editor for fiscal notes) with spell check, attach multiple documents of supported formats, and save incomplete work without requiring completion or approval. F4.3 Templates and F4.4 Reuse are deferred to Sprint 9+.

## Business Story / Requirement Trace

| Business Story / Requirement | Source | Sprint 8 Evidence | Artifact |
| --- | --- | --- | --- |
| US-4.1.1 draft, edit, review designated work products; rich text; spell check | B.COM.17 | `WorkTask.Content` (rich text) + `SetContent`; `/authoring` page rich-text editor (contenteditable) with spell check. | `Domain/WorkItems/WorkTask.cs`, `Web.Client/Pages/Authoring.razor`, `wwwroot/authoring.js` |
| US-4.1.2 draft/review fiscal estimates and data requests; rich text; spell check | B.RFA.09 | Same rich-text editor applies to fiscal estimates and data requests. | `Authoring.razor`, `ContentService` |
| US-4.1.3 draft/review fiscal notes; limited editor; spell check | B.RFA.10 | Fiscal notes use a limited textarea editor with spell check (`IsLimitedEditor`). | `Authoring.razor` |
| US-4.1.4 draft/review L&P products; rich text; spell check | B.LNP.04 | Rich-text editor applies to L&P products. | `Authoring.razor` |
| US-4.2.1 multiple attachments; PDF/email/Excel and other types; remain associated | B.COM.21 | `Attachment` value object; `WorkTask.AddAttachment`/`RemoveAttachment`; `/authoring` attachment add/remove. | `Domain/WorkItems/Attachment.cs`, `WorkTask.cs`, `ContentService` |
| US-4.2.2 save incomplete work; reopen; no completion/approval required; associations retained | B.COM.22 | `SetContent` saves at any time without completion/approval; `LastSavedAt` tracks the save; content/attachments retained on the task. | `Domain/WorkItems/WorkTask.cs`, `ContentServiceTests` |

## Technical Quality Trace

| Requirement | Source | Sprint 8 Evidence | Artifact |
| --- | --- | --- | --- |
| Automated static controls | TR-702 | `dotnet format --verify-no-changes` passes; `Directory.Build.props` applies `TreatWarningsAsErrors`. | `dotnet format`, `Directory.Build.props` |
| Automated tests asserting acceptance criteria | TR-902 | 70 tests pass (0 failed); 6 new content/attachment tests cover content save, save-without-completion, attachment add/remove, and validation. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/ContentServiceTests.cs` |
| Versioned RESTful API surface | TR-601 | New `/api/v1` endpoints: `PUT work-items/{id}/content`, `POST work-items/{id}/attachments`, `DELETE work-items/{id}/attachments/{attachmentId}`. | `src/Legislature.TrackingSystem.Web/Program.cs` |

## Completion Criteria Status

| Criterion | Status |
| --- | --- |
| Designated work products can be drafted, edited, and reviewed with a rich-text editor and spell check; fiscal notes use a limited editor | Implemented |
| Tasks support multiple attachments of supported formats that remain associated with the task | Implemented |
| Incomplete work can be saved and resumed without requiring completion or approval | Implemented |
| `dotnet format`, `dotnet build`, `dotnet test` pass | Verified (see Verification) |
| Container smoke checks and traceability, handoff, and PM Validation documentation updated, including AI Metrics | Verified |

## Persistence Decision

Sprint 8 continues the interim in-memory repository adapter; PostgreSQL persistence and EF Core migrations remain deferred.

## Verification Evidence

- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors.
- `dotnet test tests/Legislature.TrackingSystem.Domain.Tests/... --configuration Release --no-build -m:1` passed with 70 tests (0 failed, 0 skipped).
- Container smoke checks (content/attachment API and UI render) were verified against `http://localhost:5088` after `docker compose up -d --build --force-recreate web`.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, `dotnet format` requires elevated permissions for its MSBuild build host, and Docker daemon access requires elevated permissions (named-pipe). NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 8 defects; the standard AGENTS.md commands work on a normal developer/container host.
