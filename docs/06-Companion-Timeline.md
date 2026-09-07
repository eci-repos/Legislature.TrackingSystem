# Companion Timeline

Status: active

Last updated: 2026-09-04

## Purpose

This companion timeline translates the senior developer and AI-Coder pair work-plan into a time-oriented view. It is intended for planning and coordination only. It does not authorize implementation by itself; implementation remains governed by `docs/02-Current-Sprint.md`.

## Timeline Assumptions

- Sprint length: two weeks for implementation sprints.
- Sprint 0 is the current documentation/resource-planning sprint.
- Sprint 1 starts only after it is promoted into `docs/02-Current-Sprint.md`.
- Calendar dates are examples for planning. Final dates should be confirmed by the senior developer when Sprint 1 is promoted.
- The recommended calendar anchor assumes Sprint 1 starts Tuesday, 2026-09-08, because Monday, 2026-09-07 is a U.S. federal holiday.

## Relative Timeline

| Window | Sprint | Focus | Senior Developer Emphasis | AI-Coder Emphasis | Exit Evidence |
| --- | --- | --- | --- | --- | --- |
| Current | Sprint 0 | Resource management, objective, stack, pair plan, timeline | Accept planning baseline and decide whether to promote POC work | Maintain docs, source resources, traceability, and planning artifacts | Current docs updated, verified, and PM Validation report prepared if closed |
| Weeks 1-2 | Sprint 1 | POC foundation and architecture scaffold | Approve stack, project structure, quality gates, and Sprint 1 scope | Scaffold solution, build/test harness, traceability pattern, initial docs, and PM Validation report | Buildable solution with passing starter tests and PM Validation report |
| Weeks 3-4 | Sprint 2 | Work intake and identifiers | Review domain model and identifier semantics | Implement creation workflow, explicit IDs, validation, persistence, tests, and PM Validation report | Work item/task/product/package creation demo and PM Validation report |
| Weeks 5-6 | Sprint 3 | Assignment and work queues | Resolve assignment-history and mandatory-field decisions | Implement assign/reassign/update/cancel/duplicate, user work queues, and PM Validation report | Queue demo with status, priority, due date, ownership, and PM Validation report |
| Weeks 7-8 | Sprint 4 | Relationships, packages, grouping, and traceability | Review relationship/package model and usability | Implement linking, package grouping, filters, sorting, visible trace, and PM Validation report | Related-record and package demo with trace links and PM Validation report |
| Weeks 9-10 | Sprint 5 | POC hardening and demonstration | Accept or reject POC, identify production gaps | Harden tests, docs, seed data, known gaps, walkthrough, and PM Validation report | POC demo package, passing tests, handoff notes, and PM Validation report |
| Weeks 11-12 | Sprint 6 | Phase 2 planning or first expansion sprint | Decide next phase scope based on POC outcome | Prepare technical/business trace matrix for next sprint and PM Validation report for any completed promoted use case | Approved next-sprint scope and PM Validation report when applicable |

## Example Calendar Timeline

This example assumes Sprint 1 is promoted and starts on Tuesday, 2026-09-08.

| Dates | Sprint | Focus | Target Outcome |
| --- | --- | --- | --- |
| 2026-09-03 to 2026-09-04 | Sprint 0 closeout | Planning baseline and readiness | Documentation set ready for Sprint 1 promotion |
| 2026-09-08 to 2026-09-18 | Sprint 1 | POC foundation and architecture scaffold | Buildable ASP.NET Core / Blazor WebAssembly solution |
| 2026-09-21 to 2026-10-02 | Sprint 2 | Work intake and identifiers | Create source-traceable legislative work items with explicit IDs |
| 2026-10-05 to 2026-10-16 | Sprint 3 | Assignment and work queues | Assign/reassign work and show user work queues |
| 2026-10-19 to 2026-10-30 | Sprint 4 | Relationships, packages, grouping, and traceability | Link related work and package/group/filter records |
| 2026-11-02 to 2026-11-13 | Sprint 5 | POC hardening and demonstration | Demonstrable POC with tests, docs, seed data, known gaps |
| 2026-11-16 to 2026-11-25 | Sprint 6 planning/expansion | Phase 2 readiness or expansion | Promote next implementation sprint after POC review |

## Expedited POC Timeline Option

If the POC must be delivered as soon as possible, the pair may compress the POC into a single accelerated delivery window while preserving core AI-Coder governance: source traceability, build/test verification, documented scope, and senior developer review.

This expedited option assumes Sprint 1 is promoted immediately and that the POC focuses only on the Phase 1 vertical slice. Monday, 2026-09-07 is a U.S. federal holiday, so the schedule assumes no work that day.

### Fastest Responsible Enterprise POC

Target date: Friday, 2026-09-18.

This is the recommended accelerated option. It compresses the original five-sprint POC path into approximately 10 business days while preserving enough time for architecture boundaries, tests, documentation, traceability, and review.

| Dates | Focus | Target Outcome |
| --- | --- | --- |
| 2026-09-03 to 2026-09-04 | Sprint 1 promotion, technical backlog extraction, solution scaffold | Authorized POC sprint, ASP.NET Core / Blazor solution skeleton, build/test commands |
| 2026-09-08 to 2026-09-09 | Domain model, work intake, and identifiers | Create legislative work item, task, product, or package with explicit identifiers and source trace |
| 2026-09-10 to 2026-09-11 | Assignment and work queues | Assign/reassign work and show queue with status, priority, due date, and owner |
| 2026-09-14 to 2026-09-15 | Relationships, packages, filtering, and sorting | Link related records, organize packages, and support grouping/filtering |
| 2026-09-16 to 2026-09-17 | Tests, hardening, seed data, traceability, and docs | Unit/API/component tests, seed data, traceability matrix, updated sprint and handoff docs |
| 2026-09-18 | Demo readiness and senior developer review | Runnable POC walkthrough, known gaps, passing checks, and next-sprint recommendation |

### Most Aggressive Demo-Grade POC

Target date: Friday, 2026-09-11.

This option can produce a thinner demonstration, but it should be labeled demo-grade rather than enterprise POC. Use it only when speed matters more than persistence depth, integration realism, and full hardening.

Tradeoffs for the 2026-09-11 target:

- Use in-memory or local fake persistence instead of PostgreSQL.
- Use a deterministic development identity boundary instead of real Entra integration.
- Keep SharePoint, Microsoft 365, and external integration adapters as interfaces/fakes only.
- Focus on one clean vertical path: create work item, generate identifier, assign owner, show queue, and display source trace.
- Defer broader relationship modeling, package management, filtering depth, accessibility review, and end-to-end hardening.

### Expedited Acceptance Boundaries

Even under acceleration, the pair MUST NOT skip:

- Sprint promotion before implementation.
- Traceability to source story IDs and requirement IDs.
- Build and test execution before claiming completion.
- Documentation updates for scope, verification, known gaps, and handoff.
- PM Validation report preparation after completed and verified sprint and/or promoted use case scope.
- Senior developer review before calling the POC accepted.

## Milestone Gates

| Milestone | Gate | Required Evidence |
| --- | --- | --- |
| M0 | Planning baseline accepted | Project docs, resource register, recommended stack, pair work-plan, companion timeline, and PM Validation report expectation exist |
| M1 | POC foundation accepted | Solution builds, starter tests pass, architecture boundaries and commands are documented, and PM Validation report is prepared |
| M2 | Work intake accepted | POC can create a work item/task/product/package with explicit ID and source trace, and PM Validation report is prepared |
| M3 | Work management accepted | POC can assign, reassign, update, cancel, duplicate, and display assigned work, and PM Validation report is prepared |
| M4 | Organization accepted | POC can link related records, organize packages, sort, filter, and group, and PM Validation report is prepared |
| M5 | POC accepted | Senior developer can run the demo, verify tests, review traceability, prepare PM Validation report evidence, and accept or redirect next scope |

## Logical Next Steps

1. Confirm whether to promote Sprint 1 using the example date anchor or a different start date.
2. Convert the technical agile backlog to Markdown for Sprint 1 traceability.
3. Create a Sprint 1 scope document with business stories, technical stories, and acceptance checks.
4. Confirm local .NET 10 SDK availability.
5. Scaffold the POC solution only after Sprint 1 is promoted.
