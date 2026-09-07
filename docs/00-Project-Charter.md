# Legislature.TrackingSystem Project Charter

Status: active

Last updated: 2026-09-04

## Mission

Legislature.TrackingSystem exists to deliver a Department of Revenue Legislative Tracking System (LTS) that supports legislative tracking, fiscal work products, collaboration, review and approval, document handling, integrations, search, reporting, historical retention, and executive access.

The project is governed by the source corpus in `docs/specs/` and by the active sprint authority in `docs/02-Current-Sprint.md`.

## Overall Product Objective

The overall objective is to build a specification-driven, enterprise-quality LTS application that gives DOR staff and executives a single governed workspace for the legislative lifecycle. The system should help users track bills and amendments, create and assign legislative work tasks, produce and review fiscal notes, fiscal estimates, bill descriptions, analyses, packages, implementation tasks, correspondence, reports, and supporting documents, and retain the history needed for future analysis.

The target solution is a modular, API-centric web application aligned to the DOR-operated AWS architecture described in the source documents. It should preserve source traceability from requirements through sprint scope, implementation, tests, and handoff records.

## Proof-of-Concept Direction

The first implementation milestone should be a POC vertical slice, not a full production build. The POC should prove that the AI-Coder workflow can turn the source backlog into working software while preserving traceability and verification. The preferred POC scope is Phase 1 from the business backlog: legislative work intake, unique identifiers, task assignment, work queue visibility, related-record linking, package organization, priority, status, due dates, and source-story traceability.

The recommended POC implementation stack is documented in `docs/04-Recommended-Tech-Stack.md`. The stack uses ASP.NET Core and Blazor WebAssembly on the current supported .NET LTS release because the source requirements permit a custom .NET build and do not prescribe a conflicting framework.

## Source Authority

The original Word and Excel files in `docs/specs/` are the source resources for project requirements, architecture, business rules, gap analysis, and backlog planning.

Generated Markdown resources are planning aids. They MUST preserve traceability to the original source files and MUST NOT be treated as replacements for the original `.docx` or `.xlsx` files.

## Operating Principles

- Current sprint authority controls what work may be implemented.
- Source requirements control vocabulary, scope, and acceptance criteria.
- Generated implementation artifacts MUST remain traceable to source requirements.
- Documentation MUST be updated when work is completed, partially completed, or parked.
- Verification is required before work is represented as done.
- A PM Validation report MUST be prepared after each sprint and/or promoted use case is completed and verified.

## Current Resource Baseline

The managed source resource folder is `docs/specs/`. The active resource inventory is maintained in `docs/03-Resource-Management.md`.

The first converted planning resource is `docs/specs/DOR_Agile_Backlog_User_Stories.md`, generated from `docs/specs/DOR_Agile_Backlog_User_Stories.xlsx`.
