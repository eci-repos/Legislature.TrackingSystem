# Legislature.TrackingSystem Project WorkPlan

Status: active

Last updated: 2026-09-04

## Purpose

This work plan organizes the project around governed resource intake, backlog conversion, sprint definition, and later implementation. It does not authorize implementation by itself; implementation scope MUST be promoted into `docs/02-Current-Sprint.md`.

## Overall Delivery Objective

The project is being developed as a custom DOR Legislative Tracking System: a source-traceable web application for legislative work intake, collaboration, task routing, fiscal work products, bill/version history, document generation, search, reporting, security, operations, historical records, and executive access.

The delivery approach should move from source resources to a small proof of concept, then to incrementally promoted implementation sprints. Each sprint should keep requirements, code, tests, handoff notes, and PM Validation reports tied back to source requirement IDs and user stories.

## Current Planning Sequence

1. Establish project documentation authority and resource management.
2. Convert source backlog workbooks into traceable Markdown planning resources.
3. Generate proposed backlog items under `docs/backlog/` from converted source resources.
4. Use prepared backlog items to define logical sprint scope.
5. Define a POC vertical slice from Phase 1 business backlog stories and corresponding technical requirements.
6. Promote a sprint-sized implementation slice into `docs/02-Current-Sprint.md`.
7. Implement only the promoted scope.
8. Verify, document, trace, and hand off completed work.
9. Prepare a PM Validation report after the sprint and/or promoted use case is completed and verified.

## Logical Business Backlog Phases

The initial business backlog conversion organizes `DOR_Agile_Backlog_User_Stories.xlsx` into these planning phases:

| Phase | Focus | Source Epics |
| --- | --- | --- |
| Phase 1 | Core Work Intake, Collaboration, and Organization | E1, E2 |
| Phase 2 | Workflow, Review, Authoring, and Document Production | E3, E4 |
| Phase 3 | Legislative Data Lifecycle, Search, and Reporting | E5, E6 |
| Phase 4 | Fiscal Analysis, Financial Inputs, and Productivity Integration | E7, E8, E14 |
| Phase 5 | Security, Operations, Migration, and Historical Reference | E9, E10 |
| Phase 6 | Specialized Legislative Programs and Executive Experience | E11, E12, E13 |

These phases are planning containers only. They are intended to support future sprint definition while preserving the source workbook's epics, features, user stories, acceptance criteria, and requirement IDs.

## Near-Term Work

- Maintain `docs/03-Resource-Management.md` as the source resource inventory.
- Convert remaining source workbooks when needed for sprint planning.
- Extract or summarize source Word documents only when a sprint requires that source context.
- Maintain proposed backlog items under `docs/backlog/` as sprint-planning inputs.
- Define the first implementation sprint as a POC from Phase 1 business backlog stories and corresponding technical requirements.
- Establish build/test commands when the POC technology scaffold is promoted.

## POC Candidate Scope

The preferred POC demonstrates the smallest useful LTS workflow that proves the architecture and AI-Coder delivery model:

- Create and identify a legislative work item, task, work product, or package.
- Assign and reassign work to DOR users with due date, priority, status, and role-aware ownership.
- Show a user's work queue.
- Link related data, tasks, products, assignments, and documents.
- Group and filter work by designated criteria.
- Preserve visible traceability to source user stories and requirement IDs.

The POC should start with Phase 1 business stories from `docs/specs/DOR_Agile_Backlog_User_Stories.md` and should not attempt document generation, legislative-source integrations, fiscal calculations, security hardening, or production deployment until later sprints authorize those capabilities.

## Recommended POC Technology Stack

The recommended stack is documented in `docs/04-Recommended-Tech-Stack.md`.

In summary, the POC should use ASP.NET Core with Blazor WebAssembly interactivity on .NET 10 LTS, a modular monolith architecture, versioned Web APIs, EF Core, PostgreSQL for local relational persistence, Entra/OpenID Connect integration boundaries, SharePoint integration boundaries, structured logging, OpenTelemetry, xUnit, bUnit, Playwright, and Azure Pipelines-ready build/test automation.

This stack is a recommended baseline, not a production deployment decision. Final AWS compute, database, infrastructure-as-code, identity registration, SharePoint provisioning, and external integration details remain deferred until promoted by a sprint.

## Pair Delivery Plan

The senior developer and AI-Coder developer work-plan and schedule is documented in `docs/05-Pair-WorkPlan-and-Schedule.md`.

The companion timeline is documented in `docs/06-Companion-Timeline.md`.

The plan recommends moving from Sprint 0 resource readiness into a five-sprint POC path:

- Sprint 1: POC foundation and architecture scaffold.
- Sprint 2: work intake and identifiers.
- Sprint 3: assignment and work queues.
- Sprint 4: relationships, packages, grouping, and traceability.
- Sprint 5: POC hardening and demonstration.

## Prepared Backlog

Proposed business backlog items are prepared under `docs/backlog/` from `docs/specs/DOR_Agile_Backlog_User_Stories.xlsx`.

Backlog files:

- `docs/backlog/README.md`
- `docs/backlog/phase-01-core-work-intake-collaboration-and-organization.md`
- `docs/backlog/phase-02-workflow-review-authoring-and-document-production.md`
- `docs/backlog/phase-03-legislative-data-lifecycle-search-and-reporting.md`
- `docs/backlog/phase-04-fiscal-analysis-financial-inputs-and-productivity-integration.md`
- `docs/backlog/phase-05-security-operations-migration-and-historical-reference.md`
- `docs/backlog/phase-06-specialized-legislative-programs-and-executive-experience.md`

Each backlog item keeps its source user story ID, source requirement ID, priority, acceptance criteria, open questions, source document, and development notes. These items remain proposed until promoted into `docs/02-Current-Sprint.md`.

## PM Validation Report Deliverable

After each sprint and/or promoted use case is completed and verified, a PM Validation report MUST be prepared as a required closeout deliverable.

Each PM Validation report SHOULD identify the sprint or use case, completed scope, source user stories and requirement IDs, acceptance criteria, verification evidence, build/test results, traceability records, delivered artifacts, AI Metrics, known gaps, residual risks, deferred items, and the PM validation decision.

PM Validation reports SHOULD be stored under `docs/validation/` using an ISO date and scope-specific file name. The current sprint file and related handoff notes MUST reference the applicable PM Validation report when work is closed.

AI Metrics SHOULD include actual token counts, model identifiers, elapsed time, tool/run counts, and cost when available from the execution environment or billing source. When exact token or cost values are unavailable, the report MUST say they are unavailable. Estimated costs MUST be clearly labeled and include their pricing basis.

## Verification Expectations

Documentation and resource conversion work MUST include deterministic checks such as row counts, story counts, phase counts, and trace matrix completeness. Application implementation work MUST add build and test commands to `AGENTS.md` when the project structure is established.
