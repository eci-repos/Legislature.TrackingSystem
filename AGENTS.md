# AGENTS

> Based on Eduardo Sobrino AI-Coding approach. 
> Operating instructions for AI-Coder agents (and any contributor) working in **Legislature.TrackingSystem**.
> Read this file in full before starting any work. It is the initiation contract between the project and every agent.

---

## 1. Mission & Repository Context

**Legislature.TrackingSystem** is a custom Department of Revenue Legislative Tracking System (LTS) project for tracking legislation, fiscal work products, collaboration, review and approval, document handling, integrations, search, reporting, historical retention, and executive access.

- The authoritative project source corpus lives under `docs/specs/` and includes DOR business requirements, technical requirements, architecture design, gap analysis, business rules, and agile backlog workbooks.
- This repository is expected to implement a governed, modular, API-centric LTS web application once capabilities are promoted into the current sprint.
- The first implementation target should be a proof of concept (POC) vertical slice that demonstrates source-traceable legislative work intake, identifiers, assignments, work queues, relationships, and package organization before broader production hardening.
- The recommended POC technology stack is documented in `docs/04-Recommended-Tech-Stack.md`; use ASP.NET Core / Blazor WebAssembly on the current supported .NET LTS baseline unless the current sprint changes that decision.
- The senior developer and AI-Coder pair work-plan is documented in `docs/05-Pair-WorkPlan-and-Schedule.md`; use it when planning, promoting, or executing POC implementation sprints.
- The companion timeline is documented in `docs/06-Companion-Timeline.md`; use it for POC schedule planning unless the current sprint sets different dates.

Every decision you make must serve a single goal: **deliver a specification-grounded, enterprise-quality LTS implementation whose artifacts are traceable to the DOR source requirements and current sprint authority**. You are not free-authoring an app; you are executing the project's specification and design intent.

---

## 2. Authoritative Source of Truth

The authoritative source corpus is managed under `docs/specs/`. The resource inventory and usage rules live in `docs/03-Resource-Management.md`. Generated Markdown conversions are derivative planning resources and do not replace the original Word or Excel source files.

### Version / release policy

- The live project authority is the current repository state under `docs/`, especially `docs/02-Current-Sprint.md` for authorized work and `docs/specs/` for source requirements.
- Original `.docx` and `.xlsx` files in `docs/specs/` remain the source files. Markdown conversions under `docs/specs/` are generated aids for planning, traceability, and sprint definition.
- If multiple resources disagree, follow this order: `docs/02-Current-Sprint.md`, source requirement/specification files in `docs/specs/`, generated Markdown resource files, then historical handoff notes.

### Reading order & key entry points

Read these before implementing, in order:

1. `docs/00-Project-Charter.md`
2. `docs/01-Project-WorkPlan.md`
3. `docs/02-Current-Sprint.md`
4. `docs/03-Resource-Management.md`
5. `docs/04-Recommended-Tech-Stack.md` when implementation technology or POC scaffolding is relevant
6. `docs/05-Pair-WorkPlan-and-Schedule.md` when implementation sprint planning or pair execution is relevant
7. `docs/06-Companion-Timeline.md` when sprint timing, milestones, or POC schedule planning is relevant
8. `docs/08-AI-Coder-Handoff-Guide.md` before continuing work from a previous AI-Coder session
9. Relevant source documents under `docs/specs/`
10. Relevant generated planning files under `docs/specs/`, including `docs/specs/DOR_Agile_Backlog_User_Stories.md`

### Terminology grounding

Use **current terminology** only. Canonical terms include Department of Revenue (DOR), Legislative Tracking System (LTS), Research and Fiscal Analysis (RFA), Legislation & Policy (L&P), Budget & Fiscal Services (B&FS), Budget Office, bill, amendment, fiscal note, fiscal estimate, bill description, work product, task, package, executive review, implementation task, and source requirement.

---

## 3. Deprecated Terminology — Do Not Use

No retired terminology has been formally identified yet. Do not introduce alternate product names, informal replacements for LTS, or renamed business areas unless a current source requirement or sprint file establishes the term.

- When you encounter stale legacy references in existing files, treat them as defects. Remove or replace them with the correct current term for the concept.
- Frame all work in **Legislature.TrackingSystem** / **[canonical vocabulary]** terms.

---

## 4. Repository Guidance

- This repository follows the documentation order:
  `docs/00-Project-Charter.md` → `docs/01-Project-WorkPlan.md` → `docs/02-Current-Sprint.md`.
- **`docs/02-Current-Sprint.md`** is the active implementation authority. Work on any phase, module, or capability is authorized **only** when it is explicitly described in the current sprint file, or in a sprint file that the current sprint references.
- If this repository has no `docs/` tree yet, the first authorized task is to create it (`docs/00-Project-Charter.md`, `docs/01-Project-WorkPlan.md`, `docs/02-Current-Sprint.md`) so the sprint authority exists.
- Place modules and services in repository locations mandated by the active architecture and sprint files. Until a more specific implementation structure is promoted, keep application source under `src/` and documentation under `docs/`.

---

## 5. Documentation Maintenance (Mandatory)

- Keep `AGENTS.md`, `docs/02-Current-Sprint.md`, and all history/handoff/log files under `docs/` up to date with **every** change that advances the project. This is a standing mandate, not an optional step.
- Whenever work is completed, partially completed, or parked: update the current sprint file, the relevant handoff notes, and the roadmap/log files that record progress. **Never leave documentation describing a stale state.**
- Maintain `docs/08-AI-Coder-Handoff-Guide.md` as the current onboarding and continuation guide for the next AI-Coder. Update it whenever project state, commands, sprint status, known gaps, or next-step guidance changes.
- After each sprint and/or promoted use case is completed and verified, prepare a PM Validation report as a required closeout deliverable. The report MUST summarize completed scope, source requirements and user stories, acceptance evidence, verification results, delivered artifacts, residual risks or deferrals, and the PM validation decision.
- Each PM Validation report MUST include AI Metrics for the completed sprint and/or promoted use case. Record actual token counts, model identifiers, elapsed time, tool/run counts, and cost only when available from the execution environment or billing source. When exact values are unavailable, mark them as unavailable rather than estimating silently. Cost estimates MUST be clearly labeled as estimates and include their pricing basis.
- Backlog / proposed-sprint work lives under `docs/backlog/`. Keep `docs/backlog/*` current as items are promoted (move to a sprint file), dropped (delete/archive), or re-scoped (update). A backlog item is **not** authorized work until the current sprint promotes it.
- Proposed business backlog items are generated from `docs/specs/DOR_Agile_Backlog_User_Stories.xlsx` into `docs/backlog/README.md` and `docs/backlog/phase-*.md` using `tools/generate_business_backlog_items.py`.
- Fully implemented backlog files are moved to `docs/backlog/archive/` (indexed by `docs/backlog/archive/README.md`) so the active backlog reads empty. Keep historical file contents untouched except for the status header.

---

## 6. Build, Test & Verify

- Build: `dotnet build Legislature.TrackingSystem.sln --configuration Release`
- Test: `dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build`
- Static analysis/format verification: `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore`
- Run locally: `dotnet run --project src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj --urls http://localhost:5088`
- Container config check: `docker compose config`
- Container build: `docker compose build web`
- Container run for local testing: `docker compose up -d web`
- Container status: `docker compose ps`
- Container stop: `docker compose down`
- Optional local PostgreSQL readiness only: `docker compose up -d postgres`
- Documentation/resource verification includes deterministic conversion checks for generated Markdown resources and traceability counts against source workbooks.

Sprint 1 established the first implementation scaffold. Sprint 1A added a local Docker container for the ASP.NET Core / Blazor WebAssembly web app. PostgreSQL is available through Docker Compose for local readiness, but the application does not depend on database persistence until a later sprint promotes persistence scope.

> **Verify before you claim done.** A change is not complete if it does not build and the relevant test suite does not pass.

---

## 7. CI/CD

- CI/CD workflow is not established yet. Add workflow location and enforced checks when implementation scaffolding is introduced.
- CI is the enforcement point for build & test success. Treat a failing CI run as a blocking defect, not a suggestion.

---

# Agent AI-Coder Instructions

## 8.1 Working Model — you are part of the development loop

You operate as a component of the project's development loop **`spec -> normalize -> plan -> execute -> validate -> repair -> promote -> trace -> report`**. Honor these non-negotiable principles:

- **Specification-Driven Operation** — Begin from a versioned specification or a governed change to one. Never begin from informal instructions, hidden context, or untracked changes.
- **Validation Before Acceptance** — Generated or modified artifacts must be validated (build + tests + deterministic checks) before promotion to stable. Your own assertion is **not** a substitute for deterministic validation.
- **Reuse Before Generation** — Prefer approved, validated, traceable reusable assets over fresh generation. Do not reinvent what already exists in the codebase.
- **Governed Autonomy** — You may act autonomously only within governance and sprint constraints. Sprint authority defines *what* is authorized; governance rules define *how* it may proceed.
- **Bounded Repair** — Repair until the defect is fixed or the configured limit is reached; on repeated failure, escalate rather than loop infinitely.
- **Targeted Re-Entry** — When a specification changes, use traceability / impact analysis to identify affected tasks and artifacts; avoid full regeneration when targeted construction suffices.
- **State-Controlled Continuation** — Use durable project state (sprint file, traceability records), not logs or memory, as the source of truth for whether work may proceed.

## 8.2 Operating Rules

- **Current Sprint is authoritative.** The active sprint determines what work is authorized, including any phase, module, service, UI, infrastructure, documentation, tests, or handoff updates it names.
- Completed phases are considered **closed** unless explicitly reopened by the current sprint or by a sprint file the current sprint references. Do not rework closed scope.
- **If `AGENTS.md` and `Current-Sprint.md` disagree, `Current-Sprint.md` takes precedence.** Do not request clarification when the current sprint clearly identifies the authorized work.
- **Before coding anything:** read the current sprint file, then the relevant authoritative documents (§2) and schema/contract files. Ground your vocabulary and contracts in the spec, not your own guesses.
- When a task, module name, or term is ambiguous, resolve it from the authoritative source; never silently invent meaning.

## 8.3 SDLC & Quality Standards (Enterprise Level)

- Always apply widely accepted best practices to achieve **Enterprise Quality** code and solutions.
- Build **extensible and replaceable** components, each purpose-designed for one responsibility, with rigorous **separation of concerns**.
- Manage **enterprise-grade dependency injection** to the highest standard: register by interface, respect lifetimes, prefer constructor injection, avoid service locator / anti-patterns.
- **Reuse instead of reinventing.** Before writing a component, check the existing codebase and any reusable-asset guidance.
- Write clean, readable, testable code: honor SOLID, `IAsyncDisposable`/`CancellationToken`/async-await conventions (where applicable), avoid static mutable state, and keep interfaces narrow.
- **Explicit Semantics** — entity types and identity must be explicit and machine-checkable, never inferred from prose.

## 8.4 Conventions to Honor

- Use **normative language** precisely: MUST/SHALL (mandatory), MUST NOT (prohibition), SHOULD (recommended, justify deviation), MAY (permitted).
- Use the project's **identifier conventions** and **shared enums**; do not redefine them.
- Dates/times in ISO 8601; versions in SemVer.
- Enforce the project's **cross-cutting invariants**: no construction without readiness validation; no accepted artifact without deterministic validation; everything traceable; no secrets in logs, telemetry, or specs; a tool timeout is never success.

## 8.5 Definition of Done

A change is **done** only when **all** of the following hold:

1. **Authorized** — explicitly described in the current sprint file.
2. **Implemented** — follows the project's conventions and repository layout (§4), to enterprise quality.
3. **Verified** — the project builds and the relevant test suites pass (§6).
4. **Traceable** — links back to the specifying requirement/entity per traceability rules.
5. **PM Validated** — the required PM Validation report is prepared after completion and verification for the sprint and/or promoted use case.
6. **AI Metrics Recorded** — the PM Validation report records available AI usage, cost, tool, verification, and delivery metrics, with unavailable values explicitly identified.
7. **Documented** — the sprint file, handoff notes, PM Validation report, and logs under `docs/` are updated (§5); no stale documentation.
8. **Free of deprecated terminology** — no legacy references remain in the touched scope (§3).
