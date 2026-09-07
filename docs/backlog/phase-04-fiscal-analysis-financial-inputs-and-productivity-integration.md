# Backlog - Phase 4: Fiscal Analysis, Financial Inputs, and Productivity Integration

Status: proposed

Last updated: 2026-09-04

## Planning Objective

Add fiscal-data boundaries, supporting fiscal analysis, budget-office inputs, and Microsoft 365 productivity integration.

Backlog size: 6 must-have, 0 nice-to-have, 6 total.

Development path: Post-POC implementation expansion.

## Technical Baseline

- ASP.NET Core / Blazor WebAssembly POC stack
- Modular monolith with Domain, Application, Infrastructure, Web, and Tests boundaries
- Versioned API and UI behavior traceable to source story IDs
- Unit/API/component tests for promoted acceptance criteria
- Documentation updates in sprint, handoff, and traceability records
- PM Validation report after completed and verified sprint and/or promoted use case scope

## Backlog Items

### E7 - Fiscal Analysis and Financial Data

#### F7.1 - Fiscal Data Integration

##### BI-US-7.1.1

- Source user story: US-7.1.1
- Source requirement: B.COM.36
- Priority: Must Have
- User role: fiscal analyst
- Planning status: Promoted and implemented in Sprint 12.
- User story: As a fiscal analyst, I want the solution to retrieve, calculate, and update fiscal data from internal DOR systems, so that fiscal notes and estimates use current agency financial information.
- Acceptance criteria:
  1. Applicable internal DOR systems can be interfaced with.
  2. FTE information can be retrieved or used.
  3. Applicable cost rules can be incorporated.
  4. Fiscal-note and fiscal-estimate calculations and editing are supported.
  5. Revenue funds and source information can be incorporated.
  6. Updated fiscal data can be reflected in applicable products.
- Open question / clarification: Which DOR systems are sources? Which calculations occur in the LTS versus the source systems? Who owns and maintains cost rules?
- Development notes:
  - Keep fiscal-system calls behind adapters and use synthetic data for development.
  - Add or update tests that assert the promoted acceptance criteria for `US-7.1.1`.
  - Preserve traceability to `B.COM.36` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-7.1.2

- Source user story: US-7.1.2
- Source requirement: B.COM.37
- Priority: Must Have
- User role: fiscal analyst
- Planning status: Promoted and implemented in Sprint 12.
- User story: As a fiscal analyst, I want documentation showing how fiscal work papers and tasks were completed, so that historical work can improve consistency, accuracy, and efficiency in current fiscal analysis.
- Acceptance criteria:
  1. Supporting documentation for fiscal work can be stored.
  2. Historical fiscal notes and estimates can be researched.
  3. Supporting documentation can be retrieved with historical products.
  4. Authorized users can use historical material when preparing current work.
- Open question / clarification: None recorded.
- Development notes:
  - Keep fiscal-system calls behind adapters and use synthetic data for development.
  - Add or update tests that assert the promoted acceptance criteria for `US-7.1.2`.
  - Preserve traceability to `B.COM.37` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F7.2 - RFA Supporting Analysis

##### BI-US-7.2.1

- Source user story: US-7.2.1
- Source requirement: B.RFA.07
- Priority: Must Have
- User role: authorized DOR user
- Planning status: Promoted and implemented in Sprint 12.
- User story: As a authorized DOR user, I want to flag and view bills included in DOR budget and compare them with associated fiscal notes, so that budget assumptions and fiscal analyses can be reconciled.
- Acceptance criteria:
  1. Authorized users can flag applicable budget bills.
  2. Users can view flagged bills.
  3. Associated fiscal notes can be viewed for comparison.
  4. The comparison relationship remains associated with the bill.
- Open question / clarification: How is a bill identified as part of the DOR budget, who can set or remove the flag, and what constitutes a valid comparison?
- Development notes:
  - Model budget and demographic data as versioned session-specific references.
  - Add or update tests that assert the promoted acceptance criteria for `US-7.2.1`.
  - Preserve traceability to `B.RFA.07` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-7.2.2

- Source user story: US-7.2.2
- Source requirement: B.RFA.08
- Priority: Must Have
- User role: RFA analyst
- Planning status: Promoted and implemented in Sprint 12.
- User story: As a RFA analyst, I want to store and retrieve current and previous demographic data by legislative session, so that I can explain expense differences and demographic trends between bills and years.
- Acceptance criteria:
  1. Current demographic data can be stored.
  2. Historical demographic data can be retained.
  3. Data can be associated with relevant legislative sessions.
  4. Users can retrieve information needed to compare expenses between bills or years.
  5. Users can access information needed to identify demographic changes or trends.
- Open question / clarification: What demographic datasets, sources, dimensions, and update schedules are required?
- Development notes:
  - Model budget and demographic data as versioned session-specific references.
  - Add or update tests that assert the promoted acceptance criteria for `US-7.2.2`.
  - Preserve traceability to `B.RFA.08` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E8 - Microsoft 365 and External System Integration

#### F8.1 - Productivity Suite Integration

##### BI-US-8.1.1

- Source user story: US-8.1.1
- Source requirement: B.COM.30
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 12.
- User story: As a DOR user, I want the solution integrated with Teams, Outlook, Excel, and Word, so that system data can populate working documents and be distributed without redundant manual processing.
- Acceptance criteria:
  1. Applicable integration with Teams is supported.
  2. Applicable integration with Outlook is supported.
  3. Applicable integration with Excel is supported.
  4. Applicable integration with Word is supported.
  5. System data can populate applicable templates.
  6. Applicable outputs can be emailed to internal and external stakeholders.
- Open question / clarification: What specific use cases and direction of integration are required for each Microsoft application?
- Development notes:
  - Use Microsoft 365 integration boundaries first; real tenant integration requires later configuration.
  - Add or update tests that assert the promoted acceptance criteria for `US-8.1.1`.
  - Preserve traceability to `B.COM.30` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E14 - Budget Office Fiscal Inputs

#### F14.1 - Expense Estimate Management

##### BI-US-14.1.1

- Source user story: US-14.1.1
- Source requirement: B.BGT.01
- Priority: Must Have
- User role: Budget Office user
- Planning status: Promoted and implemented in Sprint 12.
- User story: As a Budget Office user, I want to enter and update the elements used to calculate expense estimates, so that fiscal work products reflect current agency cost assumptions.
- Acceptance criteria:
  1. Authorized users can enter applicable expense-estimate elements.
  2. Authorized users can update those elements.
  3. Supported elements include costs of goods and services.
  4. Supported elements include percentages of salary where applicable.
  5. Applicable values can be incorporated into fiscal work products.
- Open question / clarification: Are the Budget Office calculations expected to be performed automatically by the solution? What formulas, rounding rules, effective dates, and approval processes govern the calculations?
- Development notes:
  - Model expense estimate inputs as auditable fiscal assumptions.
  - Add or update tests that assert the promoted acceptance criteria for `US-14.1.1`.
  - Preserve traceability to `B.BGT.01` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase Trace Matrix

| Backlog Item | User Story | Source Requirement | Priority | Feature |
| --- | --- | --- | --- | --- |
| BI-US-7.1.1 | US-7.1.1 | B.COM.36 | Must Have | F7.1 - Fiscal Data Integration |
| BI-US-7.1.2 | US-7.1.2 | B.COM.37 | Must Have | F7.1 - Fiscal Data Integration |
| BI-US-7.2.1 | US-7.2.1 | B.RFA.07 | Must Have | F7.2 - RFA Supporting Analysis |
| BI-US-7.2.2 | US-7.2.2 | B.RFA.08 | Must Have | F7.2 - RFA Supporting Analysis |
| BI-US-8.1.1 | US-8.1.1 | B.COM.30 | Must Have | F8.1 - Productivity Suite Integration |
| BI-US-14.1.1 | US-14.1.1 | B.BGT.01 | Must Have | F14.1 - Expense Estimate Management |
