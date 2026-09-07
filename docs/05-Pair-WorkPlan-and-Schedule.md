# Senior Developer and AI-Coder Pair WorkPlan and Schedule

Status: active

Last updated: 2026-09-04

## Purpose

This document defines how a senior human developer and AI-Coder developer should work as a pair to move Legislature.TrackingSystem from governed source resources into a proof of concept and later sprint-based implementation.

This is a planning document. It does not authorize implementation by itself. Implementation remains authorized only when scope is promoted into `docs/02-Current-Sprint.md`.

## Pairing Model

The human senior developer owns engineering judgment, final technical decisions, stakeholder interpretation, review, and acceptance. The AI-Coder developer accelerates source analysis, code generation, tests, documentation, traceability, and repeatable verification.

Both roles work from the same durable project state:

- `docs/00-Project-Charter.md`
- `docs/01-Project-WorkPlan.md`
- `docs/02-Current-Sprint.md`
- `docs/03-Resource-Management.md`
- `docs/04-Recommended-Tech-Stack.md`
- `docs/specs/`

## Role Responsibilities

| Responsibility | Senior Developer | AI-Coder Developer |
| --- | --- | --- |
| Sprint scope | Approves promoted scope and tradeoffs | Drafts sprint slices from source stories and technical requirements |
| Requirements interpretation | Resolves ambiguity and stakeholder-facing decisions | Extracts source evidence, traceability, open questions, and acceptance criteria |
| Architecture | Owns final architecture decisions | Proposes options aligned to docs and implements selected design |
| Implementation | Reviews design and code, pairs on critical paths | Scaffolds, implements, refactors, and keeps work scoped |
| Testing | Defines risk-based coverage expectations | Writes unit, integration, component, and workflow tests |
| Security and compliance | Confirms policy posture and sensitive boundaries | Adds RBAC boundaries, no-secret checks, audit hooks, and documentation |
| Documentation | Accepts durable documentation updates | Updates sprint, handoff, resource, and traceability files |
| Verification | Reviews build/test results and residual risk | Runs deterministic checks and reports exact outcomes |

## Pairing Cadence

Use a two-week sprint cadence for implementation work unless the current sprint states otherwise.

Daily pairing rhythm:

- 15 minutes: inspect current sprint objective, blockers, and verification status.
- 60 to 120 minutes: focused pairing block on the highest-risk story or cross-cutting concern.
- 15 minutes: update traceability, docs, and next-action notes.

End-of-sprint rhythm:

- Senior developer reviews source traceability, acceptance criteria, code, and test evidence.
- AI-Coder regenerates or updates docs, verifies build/test commands, and writes handoff notes.
- AI-Coder prepares the PM Validation report after the completed sprint and/or promoted use case has been verified.
- The pair decides whether work is complete, needs repair, or must be parked with explicit residual risk.

## Planned Schedule

The schedule below converts the current logical backlog phases into a practical pair-delivery plan. Calendar dates should be assigned only when a sprint is formally promoted.

| Sequence | Planning Sprint | Primary Focus | Source Scope | Pair Outcome |
| --- | --- | --- | --- | --- |
| 0 | Sprint 0 | Resource management and planning baseline | Project docs, specs folder, business backlog conversion, tech stack, pair plan | Durable project authority and planning resources |
| 1 | Sprint 1 | POC foundation and architecture scaffold | Recommended tech stack, Phase 1 technical prerequisites | Buildable ASP.NET Core / Blazor solution with test harness and traceability structure |
| 2 | Sprint 2 | POC work intake and identifiers | Phase 1, E1/E2, especially US-1.3.1, US-2.1.1, US-2.1.2 | Create work items/tasks/products/packages with explicit identifiers and required attributes |
| 3 | Sprint 3 | POC assignment and work queues | Phase 1, US-1.3.2 through US-1.3.5, US-1.4.1, US-1.4.2 | Assign/reassign/update/cancel/duplicate work and display user work queues |
| 4 | Sprint 4 | POC relationships, packages, grouping, and traceability | Phase 1, US-2.2.1, US-2.2.2, US-2.3.1 | Link related data/tasks/products/documents, organize packages, filter/group work, show source trace |
| 5 | Sprint 5 | POC hardening and demonstration | Phase 1 acceptance criteria plus technical quality requirements | Demonstrable POC with tests, seed data, user walkthrough, known gaps, and next-sprint recommendation |
| 6 | Sprint 6+ | Workflow, review, and authoring expansion | Phase 2 | Begin workflow/review and content authoring after POC acceptance |
| 7 | Future | Legislative data lifecycle, search, reporting | Phase 3 | Add external legislative data boundaries, version history, search, and reports |
| 8 | Future | Fiscal, productivity, and document integrations | Phase 4 | Add fiscal analysis boundaries and Microsoft 365/SharePoint workflows |
| 9 | Future | Enterprise hardening | Phase 5 | Add security hardening, operations, migration, and historical reference capabilities |
| 10 | Future | Specialized programs and executive experience | Phase 6 | Add L&P, implementation management, and executive user experience capabilities |

## Sprint 1 Readiness Checklist

Before scaffolding the POC, the pair should complete these items:

- Promote Sprint 1 into `docs/02-Current-Sprint.md`.
- Convert the technical agile backlog workbook or extract the technical requirements needed for the POC foundation.
- Confirm the .NET SDK baseline and local development prerequisites.
- Define solution/project structure and naming.
- Define build, test, lint/static-analysis, and run commands in `AGENTS.md`.
- Define the initial traceability pattern from source story IDs to code, tests, seed data, and UI.
- Decide whether PostgreSQL is required in Sprint 1 or whether an in-memory/local adapter is acceptable until Sprint 2.

## POC Acceptance Model

The POC is acceptable when the senior developer can run the solution locally and demonstrate:

- A DOR user can create a legislative work item, task, work product, or package.
- The created item receives an explicit, inspectable identifier.
- Work can be assigned, reassigned, updated, canceled, duplicated, or corrected according to promoted scope.
- A user work queue shows assigned work with status, priority, due date, and ownership.
- Related tasks, products, assignments, documents, topics, identifiers, or packages can be linked.
- Work can be sorted, filtered, or grouped by designated criteria.
- UI/API behavior is traceable to the relevant user story IDs and source requirement IDs.
- Build and tests pass.
- Sprint, handoff, and traceability documentation is current.
- The PM Validation report is prepared and records the validation decision, evidence, delivered artifacts, known gaps, risks, and deferred items.

## PM Validation Report Deliverable

After each sprint and/or promoted use case is completed and verified, the AI-Coder developer MUST prepare a PM Validation report for senior developer and PM review.

The report MUST be traceable to promoted source user stories and requirement IDs, and SHOULD include acceptance evidence, verification results, delivered artifacts, AI Metrics, known gaps, residual risks, deferred items, and the PM validation decision. Use `docs/validation/PM-Validation-Report-Template.md` unless the current sprint defines a more specific format.

AI Metrics SHOULD record actual token, cost, model, elapsed-time, tool/run, verification, and delivery metrics when they are available. Unknown values MUST be marked unavailable rather than inferred.

## AI-Coder Guardrails

The AI-Coder developer MUST:

- Read `docs/02-Current-Sprint.md` before implementation.
- Keep implementation strictly within promoted scope.
- Use the recommended stack unless the current sprint changes it.
- Prefer existing source resources and generated planning artifacts over invention.
- Update documentation in the same change set as code.
- Run build/tests before claiming implementation work is complete.
- Prepare the PM Validation report after completed sprint and/or promoted use case verification.
- Include available AI Metrics in each PM Validation report and clearly mark unavailable token or cost telemetry.
- Escalate unresolved requirement ambiguity rather than encoding guesses as business rules.

## Senior Developer Guardrails

The senior developer SHOULD:

- Promote one small implementation slice at a time.
- Review source-story traceability before accepting generated code.
- Keep POC scope focused on proving the delivery model and core workflow.
- Defer production integrations, infrastructure, and security-hardening details until the POC demonstrates the business flow.
- Record decisions in durable docs rather than relying on chat history.

## Logical Next Steps

1. Promote Sprint 1 as a POC foundation sprint in `docs/02-Current-Sprint.md`.
2. Convert `docs/specs/DOR_Technical_Agile_Backlog_User_Stories.xlsx` into Markdown so technical acceptance criteria can be traced like the business backlog.
3. Create a POC traceability matrix mapping Phase 1 business stories to technical stories, tests, and planned modules.
4. Confirm the local .NET 10 SDK and tooling availability.
5. Scaffold the ASP.NET Core / Blazor WebAssembly solution with modular project boundaries.
6. Add build, test, lint/static-analysis, and run commands to `AGENTS.md`.
7. Implement the first thin vertical slice: create a work item/task with a generated identifier and visible source trace.
8. Add tests for domain invariants, API behavior, and the first Blazor workflow.
9. Expand the POC through assignment, work queues, relationships, packages, filtering, grouping, and handoff documentation.
10. Review the POC with the senior developer and decide whether to harden, expand into Phase 2, or revise scope.
