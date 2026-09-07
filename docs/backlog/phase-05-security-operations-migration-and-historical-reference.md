# Backlog - Phase 5: Security, Operations, Migration, and Historical Reference

Status: proposed

Last updated: 2026-09-04

## Planning Objective

Harden role restrictions, performance, support availability, migration, and historical reference capabilities.

Backlog size: 6 must-have, 0 nice-to-have, 6 total.

Development path: Enterprise hardening and transition.

## Technical Baseline

- ASP.NET Core / Blazor WebAssembly POC stack
- Modular monolith with Domain, Application, Infrastructure, Web, and Tests boundaries
- Versioned API and UI behavior traceable to source story IDs
- Unit/API/component tests for promoted acceptance criteria
- Documentation updates in sprint, handoff, and traceability records
- PM Validation report after completed and verified sprint and/or promoted use case scope

## Backlog Items

### E9 - Security, Access, Performance, and Operations

#### F9.1 - Role-Based Security

##### BI-US-9.1.1

- Source user story: US-9.1.1
- Source requirement: B.COM.06
- Priority: Must Have
- User role: DOR security administrator
- Planning status: Promoted and implemented in Sprint 13.
- User story: As a DOR security administrator, I want permissions based on user roles, so that users can perform only the activities authorized for their responsibilities.
- Acceptance criteria:
  1. Permissions can distinguish preparation activity.
  2. Permissions can distinguish approval activity.
  3. Permissions can distinguish delivery activity.
  4. Read-only access to final products can be provided.
  5. Users cannot perform restricted functions outside their assigned permissions.
- Open question / clarification: What is the authoritative role and permission matrix?
- Development notes:
  - Enforce least-privilege policies in application services and UI visibility.
  - Add or update tests that assert the promoted acceptance criteria for `US-9.1.1`.
  - Preserve traceability to `B.COM.06` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-9.1.2

- Source user story: US-9.1.2
- Source requirement: B.COM.16
- Priority: Must Have
- User role: DOR security administrator
- Planning status: Promoted and implemented in Sprint 13.
- User story: As a DOR security administrator, I want access restrictions at portions of a work task or product, so that sensitive information can be restricted by data type or user type.
- Acceptance criteria:
  1. Access can be restricted within applicable work tasks or products.
  2. Restrictions can be based on data type.
  3. Restrictions can be based on user type.
  4. Unauthorized users cannot access restricted information.
- Open question / clarification: What level of granularity is required: field, section, document, attachment, comment, or another level?
- Development notes:
  - Enforce least-privilege policies in application services and UI visibility.
  - Add or update tests that assert the promoted acceptance criteria for `US-9.1.2`.
  - Preserve traceability to `B.COM.16` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F9.2 - Concurrent Use and Availability

##### BI-US-9.2.1

- Source user story: US-9.2.1
- Source requirement: B.COM.07
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 13.
- User story: As a DOR user, I want more than 60 users to use the solution concurrently without degradation, so that agency-wide collaboration can continue during legislative peaks.
- Acceptance criteria:
  1. More than 60 users can access the solution concurrently.
  2. Concurrent activity does not cause unacceptable degradation.
  3. Normal collaboration capabilities remain available during the supported concurrency level.
- Open question / clarification: What measurable response-time or performance threshold defines "without degradation"?
- Development notes:
  - Add performance and availability tests when implementation scope exists.
  - Add or update tests that assert the promoted acceptance criteria for `US-9.2.1`.
  - Preserve traceability to `B.COM.07` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-9.2.2

- Source user story: US-9.2.2
- Source requirement: B.COM.42
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 13.
- User story: As a DOR user, I want the solution and support available 24/7 during peak legislative months and as needed during the remainder of the year, so that legislative deadlines can be met regardless of normal business hours.
- Acceptance criteria:
  1. The solution supports 24/7 availability during November through June.
  2. Support is available 24/7 during November through June.
  3. Year-round availability and support are provided as required by DOR.
  4. Support accommodates legislative work outside normal business hours.
- Open question / clarification: What SLA applies? What does "as needed year-round" mean operationally? What incident response and resolution targets apply?
- Development notes:
  - Add performance and availability tests when implementation scope exists.
  - Add or update tests that assert the promoted acceptance criteria for `US-9.2.2`.
  - Preserve traceability to `B.COM.42` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E10 - Data Migration and Historical Records

#### F10.1 - Legacy Migration

##### BI-US-10.1.1

- Source user story: US-10.1.1
- Source requirement: B.COM.41
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 13.
- User story: As a DOR user, I want legacy-system data migrated into the new solution, so that historical legislative and fiscal information remains available after replacement of the existing system.
- Acceptance criteria:
  1. Applicable legacy data can be migrated.
  2. Migrated records remain usable in the new solution.
  3. Required relationships between migrated records are retained or reconstructed.
  4. Migrated information is accessible according to applicable permissions.
- Open question / clarification: Which legacy systems are in scope? What volumes, formats, attachments, historical periods, quality rules, and reconciliation requirements apply?
- Development notes:
  - Treat migration as a repeatable import with validation, reconciliation, and rollback records.
  - Add or update tests that assert the promoted acceptance criteria for `US-10.1.1`.
  - Preserve traceability to `B.COM.41` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F10.2 - Financial Historical Reference

##### BI-US-10.2.1

- Source user story: US-10.2.1
- Source requirement: B.EXP.01
- Priority: Must Have
- User role: B&FS financial user
- Planning status: Promoted and implemented in Sprint 13.
- User story: As a B&FS financial user, I want to view all work products from the prior 10 years, so that my input on current work is consistent with historical analysis.
- Acceptance criteria:
  1. Authorized users can access work products covering the required 10-year period.
  2. Historical products can be viewed.
  3. Historical information can be used to inform current work.
  4. Access restrictions continue to apply.
- Open question / clarification: Is "10 years" based on calendar year, legislative session, biennium, or a rolling 10-year period?
- Development notes:
  - Model historical access around years, sessions, and fiscal work-product references.
  - Add or update tests that assert the promoted acceptance criteria for `US-10.2.1`.
  - Preserve traceability to `B.EXP.01` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase Trace Matrix

| Backlog Item | User Story | Source Requirement | Priority | Feature |
| --- | --- | --- | --- | --- |
| BI-US-9.1.1 | US-9.1.1 | B.COM.06 | Must Have | F9.1 - Role-Based Security |
| BI-US-9.1.2 | US-9.1.2 | B.COM.16 | Must Have | F9.1 - Role-Based Security |
| BI-US-9.2.1 | US-9.2.1 | B.COM.07 | Must Have | F9.2 - Concurrent Use and Availability |
| BI-US-9.2.2 | US-9.2.2 | B.COM.42 | Must Have | F9.2 - Concurrent Use and Availability |
| BI-US-10.1.1 | US-10.1.1 | B.COM.41 | Must Have | F10.1 - Legacy Migration |
| BI-US-10.2.1 | US-10.2.1 | B.EXP.01 | Must Have | F10.2 - Financial Historical Reference |
