# Resource Management

Status: active

Last updated: 2026-09-06

## Purpose

This document identifies managed source resources used by Legislature.TrackingSystem and defines how those resources are converted into implementation-planning artifacts.

## Managed Source Folder

Path: `docs/specs/`

The `docs/specs/` folder contains the original Word and Excel resources for the project. These files are source material. Do not edit, rename, or overwrite them unless a sprint explicitly authorizes source-resource maintenance.

Generated Markdown files MAY live beside their source files when they are derivative planning resources. Each generated file MUST identify its source and preserve requirement traceability.

## Source Resource Inventory

| Resource | Type | Current Use |
| --- | --- | --- |
| `docs/04-Recommended-Tech-Stack.md` | Markdown document | Recommended enterprise POC technology stack and source-fit rationale. |
| `docs/05-Pair-WorkPlan-and-Schedule.md` | Markdown document | Senior developer and AI-Coder pairing model, schedule, POC acceptance model, and logical next steps. |
| `docs/06-Companion-Timeline.md` | Markdown document | Relative and example calendar timeline for Sprint 0 through POC acceptance. |
| `docs/07-POC-Architecture-Scaffold.md` | Markdown document | Sprint 1 POC solution structure, foundation decisions, local development posture, and traceability pattern. |
| `docs/08-AI-Coder-Handoff-Guide.md` | Markdown document | Current onboarding, continuation, verification, and next-step guide for another AI-Coder. |
| `docs/database/lts-postgresql-ddl.sql` | Generated SQL script | Single-file idempotent PostgreSQL DDL/migration script generated from the current EF Core migration chain. |
| `docs/handoff/2026-09-04-container-readiness.md` | Markdown document | Sprint 1A local container readiness handoff notes for another AI-Coder. |
| `docs/backlog/README.md` | Markdown document | Active proposed backlog index generated from the business agile backlog. |
| `docs/backlog/archive/README.md` | Markdown document | Archive index for completed backlog files or items. |
| `docs/traceability/sprint-01-traceability.md` | Markdown document | Sprint 1 technical trace matrix and verification evidence. |
| `docs/validation/README.md` | Markdown document | PM Validation report location, naming rules, and closeout expectations. |
| `docs/validation/PM-Validation-Report-Template.md` | Markdown template | Reusable template for PM Validation reports prepared after completed and verified sprints and/or promoted use cases. |
| `docs/validation/2026-09-04-sprint-0-pm-validation.md` | Markdown document | Sprint 0 PM Validation report prepared before Sprint 1 promotion. |
| `docs/validation/2026-09-04-sprint-1-pm-validation.md` | Markdown document | Sprint 1 PM Validation report prepared after foundation verification. |
| `docs/validation/2026-09-04-container-readiness-pm-validation.md` | Markdown document | Sprint 1A PM Validation report prepared after local container readiness verification. |
| `DOR_Agile_Backlog_User_Stories.xlsx` | Excel workbook | Business agile backlog source for story, acceptance criteria, priority, and source requirement traceability. |
| `DOR_Agile_Backlog_User_Stories.md` | Generated Markdown | Sprint-planning-ready conversion of the business backlog, grouped into logical phases. |
| `DOR_Technical_Agile_Backlog_User_Stories.xlsx` | Excel workbook | Technical agile backlog source. Not converted in Sprint 0. |
| `DOR_Technical_Agile_Backlog_User_Stories.md` | Generated Markdown | Sprint-planning-ready conversion of the technical backlog with source trace matrix. |
| `DOR_Business_Rules.xlsx` | Excel workbook | Business rules source. Not converted in Sprint 0. |
| `DOR_High_Level_Architecture_Design.docx` | Word document | Architecture source for modular web application, DOR AWS hosting posture, integration, data, security, and operations direction. |
| `DOR_LTS_Requirements_Gap_Analysis.docx` | Word document | Gap-analysis source for unresolved scope and refinement needs. |
| `Full Project Requirements Specification Template.docx` | Word document | Business requirements source/context for legislation-related DOR work. |
| `Technical Requirements.docx` | Word document | Technical requirements source for LTS architecture, hosting, security, delivery, and standards. |

## Resource Usage Rules

- Read `docs/02-Current-Sprint.md` before using a resource for implementation work.
- Prefer original `.docx` and `.xlsx` files when resolving source meaning or traceability.
- Use generated Markdown files for planning, review, sprint slicing, and quick reference.
- Keep generated Markdown deterministic and regenerable from the source resource where practical.
- Record source file, source sheet or section, generated output, and verification checks in sprint and handoff notes.
- Store PM Validation reports under `docs/validation/` after the related sprint and/or promoted use case is completed and verified.
- Keep `docs/08-AI-Coder-Handoff-Guide.md` current whenever sprint status, implementation state, commands, known gaps, or next-step guidance changes.
- Keep local container commands and readiness evidence current whenever Docker service definitions change.

## Generated Resource Register

| Generated Resource | Source Resource | Generator | Verification |
| --- | --- | --- | --- |
| `docs/specs/DOR_Agile_Backlog_User_Stories.md` | `docs/specs/DOR_Agile_Backlog_User_Stories.xlsx` | `tools/convert_dor_backlog.py` | 69 stories, six logical phases, source trace matrix present. |
| `docs/backlog/README.md` and `docs/backlog/phase-*.md` | `docs/specs/DOR_Agile_Backlog_User_Stories.xlsx` | `tools/generate_business_backlog_items.py` | 69 proposed backlog items, six phase files, 69 phase trace rows. |
| `docs/specs/DOR_Technical_Agile_Backlog_User_Stories.md` | `docs/specs/DOR_Technical_Agile_Backlog_User_Stories.xlsx` | `tools/convert_dor_technical_backlog.py` | 94 technical stories, 17 technical epics, source trace matrix present. |
| `docs/database/lts-postgresql-ddl.sql` | `src/Legislature.TrackingSystem.Infrastructure/Persistence/Migrations/` | `dotnet ef migrations script --idempotent --context LtsDbContext --project src/Legislature.TrackingSystem.Infrastructure/Legislature.TrackingSystem.Infrastructure.csproj --startup-project src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj --configuration Release --output docs/database/lts-postgresql-ddl.sql` | 845-line PostgreSQL script generated after Release build succeeded. |
