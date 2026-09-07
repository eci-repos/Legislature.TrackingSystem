# Backlog - Phase 3: Legislative Data Lifecycle, Search, and Reporting

Status: proposed

Last updated: 2026-09-04

## Planning Objective

Add external legislative update boundaries, version history, comparison, search, reports, custom queries, and extracts.

Backlog size: 12 must-have, 0 nice-to-have, 12 total.

Development path: Post-POC implementation expansion.

## Technical Baseline

- ASP.NET Core / Blazor WebAssembly POC stack
- Modular monolith with Domain, Application, Infrastructure, Web, and Tests boundaries
- Versioned API and UI behavior traceable to source story IDs
- Unit/API/component tests for promoted acceptance criteria
- Documentation updates in sprint, handoff, and traceability records
- PM Validation report after completed and verified sprint and/or promoted use case scope

## Backlog Items

### E5 - Legislative Data, Bill Lifecycle, and Version Management

#### F5.1 - External Legislative Updates

##### BI-US-5.1.1

- Source user story: US-5.1.1
- Source requirement: B.COM.31
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want external bill language and legislative changes automatically detected and brought into the solution in real time, so that DOR always works from current legislative information.
- Acceptance criteria:
  1. Applicable external agency systems can be interfaced with.
  2. New bill language can be identified.
  3. New or changed bill information can be retrieved.
  4. Corresponding bills in the DOR solution are updated.
  5. Legislative lifecycle changes such as HB to SHB can be represented.
  6. Relevant hearing schedule changes can be received and updates occur according to the agreed meaning of real time.
- Open question / clarification: What maximum latency satisfies "real-time"?
- Development notes:
  - Keep legislative-source ingestion behind adapters until current external contracts are confirmed.
  - Add or update tests that assert the promoted acceptance criteria for `US-5.1.1`.
  - Preserve traceability to `B.COM.31` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-5.1.2

- Source user story: US-5.1.2
- Source requirement: B.COM.32
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want bill status changes from external agency systems automatically reflected in the DOR solution, so that I do not have to manually maintain legislative status.
- Acceptance criteria:
  1. Applicable external bill-status sources can be interfaced with.
  2. New bill statuses can be detected.
  3. Status information can be retrieved.
  4. Corresponding DOR records are updated in real time.
- Open question / clarification: Which system is authoritative when externally sourced status data conflicts with DOR-entered information?
- Development notes:
  - Keep legislative-source ingestion behind adapters until current external contracts are confirmed.
  - Add or update tests that assert the promoted acceptance criteria for `US-5.1.2`.
  - Preserve traceability to `B.COM.32` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-5.1.3

- Source user story: US-5.1.3
- Source requirement: B.LNP.01
- Priority: Must Have
- User role: L&P analyst
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a L&P analyst, I want proposed and unadopted amendments imported and tracked from external legislative systems, so that I can analyze amendatory language before or during legislative action.
- Acceptance criteria:
  1. Proposed or unadopted amendment information can be retrieved from supported external sources.
  2. Imported amendment data can be tracked in the solution.
  3. Changes to amendatory language can update applicable system data.
  4. The solution can interface with applicable sources identified by the RFP, including leg.wa.gov, Electronic Bill Book, and the Legislative Service Center API.
- Open question / clarification: None recorded.
- Development notes:
  - Keep legislative-source ingestion behind adapters until current external contracts are confirmed.
  - Add or update tests that assert the promoted acceptance criteria for `US-5.1.3`.
  - Preserve traceability to `B.LNP.01` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F5.2 - Version History and Comparison

##### BI-US-5.2.1

- Source user story: US-5.2.1
- Source requirement: B.COM.33
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want prior versions of bills and work products retained when updates occur, so that historical states remain available.
- Acceptance criteria:
  1. Prior bill versions are retained.
  2. Prior work-product versions are retained.
  3. An update does not overwrite the only copy of the previous version.
  4. Users with appropriate access can retrieve retained versions.
- Open question / clarification: B.COM.33 says versions are retained before each update as described in B.COM.26 and B.COM.27, but those requirements concern template population and sharing. Is that cross-reference incorrect?
- Development notes:
  - Preserve immutable version snapshots and comparison boundaries.
  - Add or update tests that assert the promoted acceptance criteria for `US-5.2.1`.
  - Preserve traceability to `B.COM.33` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-5.2.2

- Source user story: US-5.2.2
- Source requirement: B.COM.35
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want to view and retain every version of work papers, tasks, and documents as they progress, so that the full evolution of a work product is available.
- Acceptance criteria:
  1. Versions are retained through completion.
  2. Authorized users can view retained versions.
  3. Version history applies to applicable work papers, tasks, and documents.
- Open question / clarification: None recorded.
- Development notes:
  - Preserve immutable version snapshots and comparison boundaries.
  - Add or update tests that assert the promoted acceptance criteria for `US-5.2.2`.
  - Preserve traceability to `B.COM.35` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-5.2.3

- Source user story: US-5.2.3
- Source requirement: B.LNP.02
- Priority: Must Have
- User role: L&P analyst
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a L&P analyst, I want to compare a draft bill with another bill version, including a version from a prior session, so that I can identify differences between the documents.
- Acceptance criteria:
  1. A user can select two applicable bill documents for comparison.
  2. Draft bills can be compared with prior versions.
  3. Prior-session versions can participate in comparison.
  4. Differences between the two documents are presented to the user.
- Open question / clarification: What comparison presentation is required: redline, side-by-side, semantic comparison, or another method?
- Development notes:
  - Preserve immutable version snapshots and comparison boundaries.
  - Add or update tests that assert the promoted acceptance criteria for `US-5.2.3`.
  - Preserve traceability to `B.LNP.02` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-5.2.4

- Source user story: US-5.2.4
- Source requirement: B.COM.43
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want to view a bill complete history across years within a biennium, so that prior work can be reused and unnecessary rework reduced.
- Acceptance criteria:
  1. Bills can be followed across applicable years of a biennium.
  2. Associated work products remain connected to that history.
  3. Authorized users can view historical bill information.
  4. Historical records can support current-year work.
- Open question / clarification: None recorded.
- Development notes:
  - Preserve immutable version snapshots and comparison boundaries.
  - Add or update tests that assert the promoted acceptance criteria for `US-5.2.4`.
  - Preserve traceability to `B.COM.43` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E6 - Search, Reporting, and Decision Support

#### F6.1 - Enterprise Search

##### BI-US-6.1.1

- Source user story: US-6.1.1
- Source requirement: B.COM.09
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want robust search across system functionality, so that I can quickly locate tasks, documents, bills, and other information.
- Acceptance criteria:
  1. Users can search for tasks.
  2. Users can search for documents.
  3. Users can search for bills.
  4. Users can search other supported system information.
  5. Search respects security restrictions.
- Open question / clarification: Is full-text search of document contents and attachments required, or only structured metadata search?
- Development notes:
  - Design search around authorized visibility and indexed record metadata.
  - Add or update tests that assert the promoted acceptance criteria for `US-6.1.1`.
  - Preserve traceability to `B.COM.09` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F6.2 - Standard and Ad Hoc Reporting

##### BI-US-6.2.1

- Source user story: US-6.2.1
- Source requirement: B.COM.38
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want predetermined reports for commonly required data, so that recurring operational and performance information can be generated efficiently.
- Acceptance criteria:
  1. Predetermined reports can be executed.
  2. Reports can include performance measures.
  3. Reports can include outstanding fiscal tasks.
  4. The capability supports other agreed recurring report sets.
- Open question / clarification: Which predefined reports must be delivered at go-live?
- Development notes:
  - Separate standard reports, saved queries, governed database analysis, and external extracts.
  - Add or update tests that assert the promoted acceptance criteria for `US-6.2.1`.
  - Preserve traceability to `B.COM.38` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-6.2.2

- Source user story: US-6.2.2
- Source requirement: B.COM.39
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want to create and save custom queries and reports, so that recurring analytical needs do not have to be recreated each time.
- Acceptance criteria:
  1. Authorized users can create custom queries.
  2. Authorized users can create custom reports.
  3. Custom queries can be saved.
  4. Custom reports can be saved and subsequently reused.
- Open question / clarification: None recorded.
- Development notes:
  - Separate standard reports, saved queries, governed database analysis, and external extracts.
  - Add or update tests that assert the promoted acceptance criteria for `US-6.2.2`.
  - Preserve traceability to `B.COM.39` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-6.2.3

- Source user story: US-6.2.3
- Source requirement: B.COM.40
- Priority: Must Have
- User role: authorized DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a authorized DOR user, I want access to the database for queries and display of data, so that information can be analyzed as needed.
- Acceptance criteria:
  1. Authorized users can query system data.
  2. Query results can be displayed.
  3. Applicable security controls remain enforced.
  4. Access covers the data required to satisfy the RFP entire-database requirement.
- Open question / clarification: Does "access the entire database" mean direct database access, a governed query layer, reporting semantic model, API access, or equivalent functional access?
- Development notes:
  - Separate standard reports, saved queries, governed database analysis, and external extracts.
  - Add or update tests that assert the promoted acceptance criteria for `US-6.2.3`.
  - Preserve traceability to `B.COM.40` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-6.2.4

- Source user story: US-6.2.4
- Source requirement: B.COM.34
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 11.
- User story: As a DOR user, I want to extract work products in the forms required for internal or external delivery, so that work can be distributed when needed.
- Acceptance criteria:
  1. Applicable work products can be extracted from the solution.
  2. Extracted outputs can be used internally.
  3. Extracted outputs can be used externally.
  4. Required output formats can be supported once defined.
- Open question / clarification: What exact export formats are mandatory?
- Development notes:
  - Separate standard reports, saved queries, governed database analysis, and external extracts.
  - Add or update tests that assert the promoted acceptance criteria for `US-6.2.4`.
  - Preserve traceability to `B.COM.34` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase Trace Matrix

| Backlog Item | User Story | Source Requirement | Priority | Feature |
| --- | --- | --- | --- | --- |
| BI-US-5.1.1 | US-5.1.1 | B.COM.31 | Must Have | F5.1 - External Legislative Updates |
| BI-US-5.1.2 | US-5.1.2 | B.COM.32 | Must Have | F5.1 - External Legislative Updates |
| BI-US-5.1.3 | US-5.1.3 | B.LNP.01 | Must Have | F5.1 - External Legislative Updates |
| BI-US-5.2.1 | US-5.2.1 | B.COM.33 | Must Have | F5.2 - Version History and Comparison |
| BI-US-5.2.2 | US-5.2.2 | B.COM.35 | Must Have | F5.2 - Version History and Comparison |
| BI-US-5.2.3 | US-5.2.3 | B.LNP.02 | Must Have | F5.2 - Version History and Comparison |
| BI-US-5.2.4 | US-5.2.4 | B.COM.43 | Must Have | F5.2 - Version History and Comparison |
| BI-US-6.1.1 | US-6.1.1 | B.COM.09 | Must Have | F6.1 - Enterprise Search |
| BI-US-6.2.1 | US-6.2.1 | B.COM.38 | Must Have | F6.2 - Standard and Ad Hoc Reporting |
| BI-US-6.2.2 | US-6.2.2 | B.COM.39 | Must Have | F6.2 - Standard and Ad Hoc Reporting |
| BI-US-6.2.3 | US-6.2.3 | B.COM.40 | Must Have | F6.2 - Standard and Ad Hoc Reporting |
| BI-US-6.2.4 | US-6.2.4 | B.COM.34 | Must Have | F6.2 - Standard and Ad Hoc Reporting |
