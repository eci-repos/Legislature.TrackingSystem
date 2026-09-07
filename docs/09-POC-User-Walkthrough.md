# POC User Walkthrough

Sprint 5 - POC Hardening and Demonstration

Last updated: 2026-09-04

This walkthrough demonstrates the Legislative Tracking System (LTS) POC end to end. It assumes the web app is running locally at `http://localhost:5088` (see `docs/08-AI-Coder-Handoff-Guide.md` for build/run commands) and that the in-memory seed data has loaded at startup.

## Before You Start

- The POC uses an **interim in-memory repository adapter**. All data lives in memory and resets when the container restarts. Seed data is reloaded automatically at startup.
- Seed data includes six work items, assignments, one package, and relationships (see `src/Legislature.TrackingSystem.Infrastructure/WorkItems/SeedDataInitializer.cs`).
- If a page shows "Couldn't connect to the server. Refresh the page to retry.", the browser could not reach the API. Refresh the page (Ctrl+F5) or open `http://127.0.0.1:5088` explicitly.

## Walkthrough Path

### 1. Work Intake — create a work item

- Open **Work Intake** from the navigation menu.
- Enter a title (e.g., "Analyze SB 200 fiscal impact"), select a type (Fiscal Note), priority, status, and an optional due date and owner.
- Enter the source trace fields (Story ID, Requirement ID, Requirement Type, Source Document).
- Submit. The new work item is created with an explicit identifier (e.g., `LTS-FN-...`) and appears in the confirmation.

### 2. Assignment — assign work to a user

- Open **Work Queue** and enter an assignee key (e.g., `jdoe`).
- Load the queue to see the seeded assignments for that user, including status, priority, role, per-assignment due date, and rework badge.
- Assign a new work item to a user via the assignment API or the seeded data to see it appear in that user's queue.

### 3. Packages — group work products into a deliverable

- Open **Packages**.
- The seeded package "FY2026 HB 1200 Fiscal Package" is listed with status **In Progress** and its work products.
- Create a new package, add work products, and deliver it. Delivered packages show status **Delivered** and retain their members.

### 4. Categorization — mark confidential / executive review

- Open **Work Items**.
- The seeded fiscal estimate is marked **Confidential** and the data request is marked **Executive Review**.
- Filter by Confidential or Executive Review to see only those items.

### 5. Work Items — identify, sort, filter, and group

- Open **Work Items**.
- Sort by Title, Type, Priority, Status, Due Date, or Created At (ascending/descending).
- Filter by Confidential, Executive Review, On Hold status, Work Type, or Package.
- Group by Type, Status, Package, Confidential, or Executive Review to organize the list.

### 6. Relationships — link related work

- Via the relationships API, link two work items by Legislative Identifier, Topic, Document Type, or Package.
- Query relationships from either side of a link to see the complete context of a work item.

## Expected Seed Data

| Work item | Type | Status | Owner | Notes |
| --- | --- | --- | --- | --- |
| Analyze HB 1200 fiscal impact | Fiscal Note | Assigned | jdoe | In the FY2026 HB 1200 package |
| Prepare fiscal note for SB 88 | Fiscal Note | In Progress | asmith | In the FY2026 HB 1200 package |
| Draft bill analysis for HB 1200 | Bill Analysis | Proposed | bchen | Linked to HB 1200 fiscal note |
| Data request: prior-year revenue | Data Request | On Hold | cduke | Executive Review |
| Fiscal estimate for HB 1200 | Fiscal Estimate | Assigned | asmith | Confidential; in the FY2026 HB 1200 package |
| Hearing report for SB 88 | Work Product | Submitted | jdoe | Linked to SB 88 fiscal note |

## Known Gaps Observed in the Walkthrough

- Data is in-memory only; it resets on restart (PostgreSQL persistence is deferred).
- There is no real authentication/authorization; assignee keys are free-form strings.
- Search and reporting are not yet implemented (Phase 3).
- The package definition open question (page 1 vs RFA description) remains open; the POC models a package as a named deliverable grouping any work products.
