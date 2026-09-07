# Backlog

Status: active planning backlog

Last updated: 2026-09-04

## Purpose

This folder contains proposed backlog items prepared from the DOR business agile backlog and related project planning documents. These files help define future sprints, but they do not authorize implementation by themselves. Implementation remains governed by `docs/02-Current-Sprint.md`.

## Source

- Source workbook: `docs/specs/DOR_Agile_Backlog_User_Stories.xlsx`
- Source worksheet: `Agile Backlog`
- Generated planning reference: `docs/specs/DOR_Agile_Backlog_User_Stories.md`
- Recommended stack reference: `docs/04-Recommended-Tech-Stack.md`
- Pair schedule reference: `docs/05-Pair-WorkPlan-and-Schedule.md`
- Timeline reference: `docs/06-Companion-Timeline.md`

## Backlog Index

| Phase | Backlog File | Focus | Story Count | Development Path |
| --- | --- | --- | ---: | --- |
| Phase 1 | `phase-01-core-work-intake-collaboration-and-organization.md` | Core Work Intake, Collaboration, and Organization | 15 | POC foundation through POC acceptance |
| Phase 2 | `phase-02-workflow-review-authoring-and-document-production.md` | Workflow, Review, Authoring, and Document Production | 19 | Post-POC implementation expansion |
| Phase 3 | `phase-03-legislative-data-lifecycle-search-and-reporting.md` | Legislative Data Lifecycle, Search, and Reporting | 12 | Post-POC implementation expansion |
| Phase 4 | `phase-04-fiscal-analysis-financial-inputs-and-productivity-integration.md` | Fiscal Analysis, Financial Inputs, and Productivity Integration | 6 | Post-POC implementation expansion |
| Phase 5 | `phase-05-security-operations-migration-and-historical-reference.md` | Security, Operations, Migration, and Historical Reference | 6 | Enterprise hardening and transition |
| Phase 6 | `phase-06-specialized-legislative-programs-and-executive-experience.md` | Specialized Legislative Programs and Executive Experience | 11 | Specialized capability expansion |

## Planning Rules

- Backlog items are proposed until promoted into `docs/02-Current-Sprint.md`.
- Each item keeps its source user story ID and source requirement ID.
- Sprint planning should pull the smallest coherent vertical slice that can be built, tested, reviewed, and documented.
- Open questions must be resolved by the senior developer or stakeholder before being encoded as business rules.
- A PM Validation report must be prepared after each sprint and/or promoted use case is completed and verified.
- Fully implemented backlog files or items should be moved to `docs/backlog/archive/` only after the current sprint records completion and the PM Validation report is prepared.

## Logical Next Steps

1. Convert the technical agile backlog into Markdown for technical traceability.
2. Create a Phase 1 POC sprint scope from `phase-01-core-work-intake-collaboration-and-organization.md`.
3. Build a traceability matrix linking business backlog items to technical requirements, modules, and tests.
4. Promote the first implementation slice into `docs/02-Current-Sprint.md` before scaffolding code.
5. Prepare the PM Validation report after completed and verified sprint and/or promoted use case scope.
6. Archive completed backlog items only after implementation, verification, documentation, and PM Validation reporting are complete.
