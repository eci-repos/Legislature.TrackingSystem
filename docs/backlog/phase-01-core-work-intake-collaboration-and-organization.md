# Backlog - Phase 1: Core Work Intake, Collaboration, and Organization

Status: proposed

Last updated: 2026-09-04

## Planning Objective

Create the smallest useful LTS vertical slice: intake, identifiers, assignments, work queues, relationships, packages, filtering, grouping, and traceability.

Backlog size: 15 must-have, 0 nice-to-have, 15 total.

Development path: POC foundation through POC acceptance.

## Technical Baseline

- ASP.NET Core / Blazor WebAssembly POC stack
- Modular monolith with Domain, Application, Infrastructure, Web, and Tests boundaries
- Versioned API and UI behavior traceable to source story IDs
- Unit/API/component tests for promoted acceptance criteria
- Documentation updates in sprint, handoff, and traceability records
- PM Validation report after completed and verified sprint and/or promoted use case scope

## Backlog Items

### E1 - Collaborative Legislative Work Management

#### F1.1 - Centralized Collaboration

##### BI-US-1.1.1

- Source user story: US-1.1.1
- Source requirement: B.COM.01
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 10.
- User story: As a DOR user, I want multiple subject matter experts to collaborate on specified topics, documents, tasks, and assignments in a single reference location, so that all contributors can work from a common source of information.
- Acceptance criteria:
  1. Multiple authorized users can collaborate around the same topic, work task, assignment, or document.
  2. Collaboration information is accessible from a single reference location associated with the applicable work.
  3. Multiple subject matter experts can contribute to the same body of work.
  4. Access is subject to applicable role and data restrictions.
- Open question / clarification: None recorded.
- Development notes:
  - Model a collaboration workspace or common record context before adding comments, notifications, or documents.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.1.1`.
  - Preserve traceability to `B.COM.01` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-1.1.2

- Source user story: US-1.1.2
- Source requirement: B.COM.08
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 10.
- User story: As a DOR user, I want to view multiple documents and work products simultaneously, so that I can compare and analyze related information efficiently.
- Acceptance criteria:
  1. A user can have more than one document or work product available for viewing at the same time.
  2. Simultaneous viewing does not require the user to discard unsaved work.
  3. Access restrictions continue to apply to every displayed item.
- Open question / clarification: Does "simultaneously" require split-screen viewing, multiple browser tabs/windows, an in-application document viewer, or another user experience?
- Development notes:
  - Model a collaboration workspace or common record context before adding comments, notifications, or documents.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.1.2`.
  - Preserve traceability to `B.COM.08` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F1.2 - Notifications

##### BI-US-1.2.1

- Source user story: US-1.2.1
- Source requirement: B.COM.02
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 10.
- User story: As a DOR user, I want to receive notifications based on designated criteria, so that I am alerted when assigned work or legislative changes require my attention.
- Acceptance criteria:
  1. Notifications can be generated when work tasks are assigned.
  2. Notifications can be generated when external bill changes affect related work products.
  3. Notification triggers are based on designated criteria.
  4. Notifications are directed to the applicable users.
- Open question / clarification: What notification channels, trigger catalog, subscription options, and escalation rules are required?
- Development notes:
  - Start with in-app notification events in the POC; defer delivery channels until requirements are clarified.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.2.1`.
  - Preserve traceability to `B.COM.02` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F1.3 - Task Creation and Assignment

##### BI-US-1.3.1

- Source user story: US-1.3.1
- Source requirement: B.COM.11
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 2.
- User story: As a DOR user, I want to create work tasks, so that legislative work can be formally initiated and tracked.
- Acceptance criteria:
  1. An authorized user can create a task.
  2. The task is stored in the solution.
  3. The task receives the attributes required to support assignment, workflow, search, and reporting.
- Open question / clarification: What fields are mandatory when a task is created?
- Development notes:
  - Define mandatory task fields before implementation; include due date, priority, status, owner, and trace references.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.3.1`.
  - Preserve traceability to `B.COM.11` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-1.3.2

- Source user story: US-1.3.2
- Source requirement: B.COM.12
- Priority: Must Have
- User role: authorized DOR user
- Planning status: Promoted and implemented in Sprint 3.
- User story: As a authorized DOR user, I want to assign and reassign tasks to users within DOR, so that work can be routed to the appropriate resource.
- Acceptance criteria:
  1. An authorized user can assign a task to a DOR user.
  2. An authorized user can subsequently reassign the task.
  3. The currently assigned user is identifiable.
  4. Reassignment does not remove the work product or supporting information.
- Open question / clarification: Must assignment history, including prior assignees and dates, be retained?
- Development notes:
  - Define mandatory task fields before implementation; include due date, priority, status, owner, and trace references.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.3.2`.
  - Preserve traceability to `B.COM.12` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-1.3.3

- Source user story: US-1.3.3
- Source requirement: B.COM.13
- Priority: Must Have
- User role: work coordinator
- Planning status: Promoted and implemented in Sprint 3.
- User story: As a work coordinator, I want to assign multiple users to the same task with due dates based on each user role or required work, so that collaborative activities can be scheduled independently.
- Acceptance criteria:
  1. More than one user can be assigned to a task or assignment.
  2. A due date can be associated with each applicable user role or work assignment.
  3. Users can identify their individual responsibilities and applicable due dates.
  4. Multiple due dates can coexist on the same overall task.
- Open question / clarification: None recorded.
- Development notes:
  - Define mandatory task fields before implementation; include due date, priority, status, owner, and trace references.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.3.3`.
  - Preserve traceability to `B.COM.13` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-1.3.4

- Source user story: US-1.3.4
- Source requirement: B.COM.14
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 10.
- User story: As a DOR user, I want to track the customer due date for each work product, so that DOR can meet external or internal delivery commitments.
- Acceptance criteria:
  1. Each applicable work product can store a customer due date.
  2. The customer due date can be viewed with the applicable work product.
  3. The customer due date remains associated with the product throughout its workflow.
- Open question / clarification: Who is considered a "customer," and can a work product have more than one customer due date?
- Development notes:
  - Define mandatory task fields before implementation; include due date, priority, status, owner, and trace references.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.3.4`.
  - Preserve traceability to `B.COM.14` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-1.3.5

- Source user story: US-1.3.5
- Source requirement: B.COM.10
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 3.
- User story: As a DOR user, I want a work queue showing all work assigned to me, so that I can manage my workload, rework, status, priority, and volume.
- Acceptance criteria:
  1. A user can view work assigned to that user.
  2. The queue identifies rework.
  3. The queue provides status information.
  4. The queue provides priority information.
  5. The queue supports visibility into workload quantity.
- Open question / clarification: None recorded.
- Development notes:
  - Define mandatory task fields before implementation; include due date, priority, status, owner, and trace references.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.3.5`.
  - Preserve traceability to `B.COM.10` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F1.4 - Task Maintenance

##### BI-US-1.4.1

- Source user story: US-1.4.1
- Source requirement: B.COM.18
- Priority: Must Have
- User role: authorized DOR user
- Planning status: Promoted and implemented in Sprint 10.
- User story: As a authorized DOR user, I want to update, cancel, change, duplicate, or correct tasks, so that the system reflects changing legislative circumstances.
- Acceptance criteria:
  1. Authorized users can update tasks.
  2. Authorized users can cancel tasks.
  3. Authorized users can change applicable task information.
  4. Authorized users can duplicate a task.
  5. Authorized users can make corrections to task information.
- Open question / clarification: None recorded.
- Development notes:
  - Preserve change intent and audit fields when tasks are updated, canceled, duplicated, corrected, or reopened.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.4.1`.
  - Preserve traceability to `B.COM.18` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-1.4.2

- Source user story: US-1.4.2
- Source requirement: B.COM.23
- Priority: Must Have
- User role: authorized DOR user
- Planning status: Promoted and implemented in Sprint 10.
- User story: As a authorized DOR user, I want to reassign, update, cancel, or change work after submission or approval, so that DOR can respond when circumstances change.
- Acceptance criteria:
  1. The capability is limited to authorized users.
  2. Submitted work can be reassigned where permitted.
  3. Submitted or approved work can be corrected, updated, changed, or canceled where permitted.
  4. The prior approved or submitted state is not silently lost.
- Open question / clarification: Does changing an approved product automatically reopen its review and approval workflow?
- Development notes:
  - Preserve change intent and audit fields when tasks are updated, canceled, duplicated, corrected, or reopened.
  - Add or update tests that assert the promoted acceptance criteria for `US-1.4.2`.
  - Preserve traceability to `B.COM.23` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E2 - Identification, Relationships, and Legislative Work Organization

#### F2.1 - Unique Identification

##### BI-US-2.1.1

- Source user story: US-2.1.1
- Source requirement: B.COM.03
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 2.
- User story: As a DOR user, I want every work task, product, and document type to receive a unique identifier, so that individual items can be reliably tracked and referenced.
- Acceptance criteria:
  1. The solution automatically assigns an identifier to applicable items.
  2. Identifiers uniquely distinguish individual records.
  3. The capability applies to bill analyses, fiscal notes, fiscal estimates, data requests, and other applicable work types.
- Open question / clarification: What identifier format is required? Must identifiers remain unique across work types, years, and biennia?
- Development notes:
  - Make identifier semantics explicit and testable; do not infer item type from display text.
  - Add or update tests that assert the promoted acceptance criteria for `US-2.1.1`.
  - Preserve traceability to `B.COM.03` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-2.1.2

- Source user story: US-2.1.2
- Source requirement: B.RFA.03
- Priority: Must Have
- User role: authorized RFA user
- Planning status: Promoted and implemented in Sprint 2.
- User story: As a authorized RFA user, I want to override an automatically assigned identifier, so that exceptional fiscal work can use the appropriate business identifier.
- Acceptance criteria:
  1. Authorized users can replace or modify an automatically generated identifier.
  2. The system prevents an override from creating an impermissible duplicate identifier.
  3. The override is retained with the applicable work product.
- Open question / clarification: Which roles may override identifiers and what audit trail is required?
- Development notes:
  - Make identifier semantics explicit and testable; do not infer item type from display text.
  - Add or update tests that assert the promoted acceptance criteria for `US-2.1.2`.
  - Preserve traceability to `B.RFA.03` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F2.2 - Work Relationships and Packages

##### BI-US-2.2.1

- Source user story: US-2.2.1
- Source requirement: B.COM.05
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 4.
- User story: As a DOR user, I want related data, tasks, products, assignments, and documents linked together, so that the complete history and context of legislative work can be accessed from related records.
- Acceptance criteria:
  1. Work can be linked by topic.
  2. Work can be linked by document type.
  3. Work can be linked by unique legislative identifier such as bill number.
  4. Work can be linked by named package.
  5. Bill proposals, drafts, and bill versions can remain related as the bill progresses through stages.
  6. Fiscal notes, fiscal estimates, sponsor letters, staff emails, data requests, and hearing reports can be linked to applicable legislative work or packages.
- Open question / clarification: None recorded.
- Development notes:
  - Represent relationships as first-class links with type, source item, target item, and audit metadata.
  - Add or update tests that assert the promoted acceptance criteria for `US-2.2.1`.
  - Preserve traceability to `B.COM.05` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-2.2.2

- Source user story: US-2.2.2
- Source requirement: B.RFA.06
- Priority: Must Have
- User role: RFA user
- Planning status: Promoted and implemented in Sprint 4.
- User story: As a RFA user, I want to combine several work products into one deliverable and evaluate the package status, so that related fiscal work can be managed and delivered together.
- Acceptance criteria:
  1. Multiple work products can be associated with one package.
  2. The package can be delivered as one product.
  3. Users can determine the status of the package.
  4. The underlying work products remain identifiable.
- Open question / clarification: Page 1 defines a package as a set of fiscal estimates, while the RFA description refers to packages containing estimates, fiscal notes, and data requests. Which definition controls?
- Development notes:
  - Represent relationships as first-class links with type, source item, target item, and audit metadata.
  - Add or update tests that assert the promoted acceptance criteria for `US-2.2.2`.
  - Preserve traceability to `B.RFA.06` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F2.3 - Categorization

##### BI-US-2.3.1

- Source user story: US-2.3.1
- Source requirement: B.COM.04
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 4.
- User story: As a DOR user, I want to identify, sort, filter, and group information using designated criteria, so that I can organize legislative work for my business needs.
- Acceptance criteria:
  1. Users can identify records using designated criteria.
  2. Users can sort information.
  3. Users can filter information.
  4. Users can group information.
  5. Supported criteria include, at minimum, Confidential, Executive Review, On Hold, Work Type, and Package where applicable.
- Open question / clarification: None recorded.
- Development notes:
  - Use controlled criteria for sorting, filtering, and grouping; avoid free-form categories where enums are required.
  - Add or update tests that assert the promoted acceptance criteria for `US-2.3.1`.
  - Preserve traceability to `B.COM.04` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase Trace Matrix

| Backlog Item | User Story | Source Requirement | Priority | Feature |
| --- | --- | --- | --- | --- |
| BI-US-1.1.1 | US-1.1.1 | B.COM.01 | Must Have | F1.1 - Centralized Collaboration |
| BI-US-1.1.2 | US-1.1.2 | B.COM.08 | Must Have | F1.1 - Centralized Collaboration |
| BI-US-1.2.1 | US-1.2.1 | B.COM.02 | Must Have | F1.2 - Notifications |
| BI-US-1.3.1 | US-1.3.1 | B.COM.11 | Must Have | F1.3 - Task Creation and Assignment |
| BI-US-1.3.2 | US-1.3.2 | B.COM.12 | Must Have | F1.3 - Task Creation and Assignment |
| BI-US-1.3.3 | US-1.3.3 | B.COM.13 | Must Have | F1.3 - Task Creation and Assignment |
| BI-US-1.3.4 | US-1.3.4 | B.COM.14 | Must Have | F1.3 - Task Creation and Assignment |
| BI-US-1.3.5 | US-1.3.5 | B.COM.10 | Must Have | F1.3 - Task Creation and Assignment |
| BI-US-1.4.1 | US-1.4.1 | B.COM.18 | Must Have | F1.4 - Task Maintenance |
| BI-US-1.4.2 | US-1.4.2 | B.COM.23 | Must Have | F1.4 - Task Maintenance |
| BI-US-2.1.1 | US-2.1.1 | B.COM.03 | Must Have | F2.1 - Unique Identification |
| BI-US-2.1.2 | US-2.1.2 | B.RFA.03 | Must Have | F2.1 - Unique Identification |
| BI-US-2.2.1 | US-2.2.1 | B.COM.05 | Must Have | F2.2 - Work Relationships and Packages |
| BI-US-2.2.2 | US-2.2.2 | B.RFA.06 | Must Have | F2.2 - Work Relationships and Packages |
| BI-US-2.3.1 | US-2.3.1 | B.COM.04 | Must Have | F2.3 - Categorization |
