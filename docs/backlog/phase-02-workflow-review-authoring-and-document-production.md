# Backlog - Phase 2: Workflow, Review, Authoring, and Document Production

Status: proposed

Last updated: 2026-09-04

## Planning Objective

Add configurable workflow, review, approval, rich-text authoring, attachments, templates, generated documents, and reuse.

Backlog size: 19 must-have, 0 nice-to-have, 19 total.

Development path: Post-POC implementation expansion.

## Technical Baseline

- ASP.NET Core / Blazor WebAssembly POC stack
- Modular monolith with Domain, Application, Infrastructure, Web, and Tests boundaries
- Versioned API and UI behavior traceable to source story IDs
- Unit/API/component tests for promoted acceptance criteria
- Documentation updates in sprint, handoff, and traceability records
- PM Validation report after completed and verified sprint and/or promoted use case scope

## Backlog Items

### E3 - Workflow, Review, Approval, and Submission

#### F3.1 - Configurable Workflow

##### BI-US-3.1.1

- Source user story: US-3.1.1
- Source requirement: B.COM.15
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 6.
- User story: As a DOR user, I want tasks to follow a defined workflow in which work and approval responsibilities can be separated, so that legislative products receive required review before completion.
- Acceptance criteria:
  1. A task can proceed through a defined workflow.
  2. One or more users can perform the work.
  3. One or more different users can review and approve the work.
  4. Workflow status can be determined.
- Open question / clarification: None recorded.
- Development notes:
  - Keep workflow states explicit; separate preparation and approval responsibilities.
  - Add or update tests that assert the promoted acceptance criteria for `US-3.1.1`.
  - Preserve traceability to `B.COM.15` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-3.1.2

- Source user story: US-3.1.2
- Source requirement: B.COM.19
- Priority: Must Have
- User role: DOR reviewer
- Planning status: Promoted and implemented in Sprint 6.
- User story: As a DOR reviewer, I want completed products and documentation routed through multiple subject matter experts when required, so that appropriate review occurs before submission.
- Acceptance criteria:
  1. A completed product can enter a review workflow.
  2. Multiple subject matter experts can participate in review and approval.
  3. Review occurs before applicable internal or external submission.
  4. The product approval state can be determined.
- Open question / clarification: None recorded.
- Development notes:
  - Keep workflow states explicit; separate preparation and approval responsibilities.
  - Add or update tests that assert the promoted acceptance criteria for `US-3.1.2`.
  - Preserve traceability to `B.COM.19` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-3.1.3

- Source user story: US-3.1.3
- Source requirement: B.COM.20
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 6.
- User story: As a DOR user, I want finalized products packaged and prepared for submission, so that approved documentation can be delivered to internal and external stakeholders.
- Acceptance criteria:
  1. Only the appropriate reviewed or approved products can be prepared as finalized products.
  2. Finalized documentation can be packaged for submission.
  3. Packages can support applicable internal recipients.
  4. Packages can support applicable external recipients.
- Open question / clarification: None recorded.
- Development notes:
  - Keep workflow states explicit; separate preparation and approval responsibilities.
  - Add or update tests that assert the promoted acceptance criteria for `US-3.1.3`.
  - Preserve traceability to `B.COM.20` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F3.2 - RFA Executive Review

##### BI-US-3.2.1

- Source user story: US-3.2.1
- Source requirement: B.RFA.01
- Priority: Must Have
- User role: authorized executive reviewer
- Planning status: Promoted and implemented in Sprint 7.
- User story: As a authorized executive reviewer, I want permission to conduct Executive Reviews of fiscal notes and fiscal estimates, so that cross-division agreement can be obtained before external release.
- Acceptance criteria:
  1. Executive Review capability is restricted to designated users.
  2. Fiscal notes can undergo Executive Review.
  3. Fiscal estimates can undergo Executive Review.
  4. Review occurs prior to applicable release to OFM or another external stakeholder.
- Open question / clarification: None recorded.
- Development notes:
  - Treat Executive Review as a governed workflow path with role-specific permissions.
  - Add or update tests that assert the promoted acceptance criteria for `US-3.2.1`.
  - Preserve traceability to `B.RFA.01` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-3.2.2

- Source user story: US-3.2.2
- Source requirement: B.RFA.02
- Priority: Must Have
- User role: executive reviewer
- Planning status: Promoted and implemented in Sprint 7.
- User story: As a executive reviewer, I want to perform the entire fiscal-product Executive Review inside the solution, so that review, correction, completion, and reviewer handoffs can occur without a separate process.
- Acceptance criteria:
  1. Each applicable reviewer can access the fiscal product.
  2. A reviewer can adjust or correct the product as permitted.
  3. A reviewer can indicate completion of that review.
  4. Prior reviewer or reviewers can be notified as required.
  5. The next reviewer can be notified automatically.
  6. The workflow continues until the required Executive Review is complete.
- Open question / clarification: Is Executive Review sequential, parallel, or configurable by product?
- Development notes:
  - Treat Executive Review as a governed workflow path with role-specific permissions.
  - Add or update tests that assert the promoted acceptance criteria for `US-3.2.2`.
  - Preserve traceability to `B.RFA.02` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-3.2.3

- Source user story: US-3.2.3
- Source requirement: B.RFA.04
- Priority: Must Have
- User role: RFA coordinator
- Planning status: Promoted and implemented in Sprint 7.
- User story: As a RFA coordinator, I want a due date for each step or subtask within a work product, so that time-sensitive fiscal work can be actively managed.
- Acceptance criteria:
  1. Individual workflow steps or subtasks can have due dates.
  2. Multiple step-level due dates can exist within one product.
  3. Users can determine which due date applies to each step.
- Open question / clarification: None recorded.
- Development notes:
  - Treat Executive Review as a governed workflow path with role-specific permissions.
  - Add or update tests that assert the promoted acceptance criteria for `US-3.2.3`.
  - Preserve traceability to `B.RFA.04` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-3.2.4

- Source user story: US-3.2.4
- Source requirement: B.RFA.05
- Priority: Must Have
- User role: RFA user
- Planning status: Promoted and implemented in Sprint 7.
- User story: As a RFA user, I want to set a priority for each work product, so that limited resources can be focused on the most urgent legislative work.
- Acceptance criteria:
  1. Each applicable product can be assigned a priority.
  2. Priority can be viewed by users managing the work.
  3. Priority can be used in workload management.
- Open question / clarification: What priority values and escalation rules are required?
- Development notes:
  - Treat Executive Review as a governed workflow path with role-specific permissions.
  - Add or update tests that assert the promoted acceptance criteria for `US-3.2.4`.
  - Preserve traceability to `B.RFA.05` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E4 - Content Authoring, Documents, and Templates

#### F4.1 - Rich-Text Authoring

##### BI-US-4.1.1

- Source user story: US-4.1.1
- Source requirement: B.COM.17
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 8.
- User story: As a DOR user, I want to draft, edit, and review designated work products using a self-contained rich-text editor with spell check, so that I can prepare professional work products inside the solution.
- Acceptance criteria:
  1. Designated work products can be drafted in the solution.
  2. Designated work products can be edited.
  3. Designated work products can be reviewed.
  4. The editor supports rich-text functionality.
  5. The editor includes spell check.
- Open question / clarification: Which work-product types are "designated"?
- Development notes:
  - Defer full editor selection until requirements are promoted; define content contracts first.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.1.1`.
  - Preserve traceability to `B.COM.17` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-4.1.2

- Source user story: US-4.1.2
- Source requirement: B.RFA.09
- Priority: Must Have
- User role: RFA user
- Planning status: Promoted and implemented in Sprint 8.
- User story: As a RFA user, I want to draft and review fiscal estimates and data requests in a self-contained rich-text editor with spell check, so that these products can be completed entirely within the solution.
- Acceptance criteria:
  1. Fiscal estimates can be drafted and reviewed in the solution.
  2. Data requests can be drafted and reviewed.
  3. Rich-text formatting is available.
  4. Spell check is available.
- Open question / clarification: None recorded.
- Development notes:
  - Defer full editor selection until requirements are promoted; define content contracts first.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.1.2`.
  - Preserve traceability to `B.RFA.09` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-4.1.3

- Source user story: US-4.1.3
- Source requirement: B.RFA.10
- Priority: Must Have
- User role: RFA user
- Planning status: Promoted and implemented in Sprint 8.
- User story: As a RFA user, I want to draft and review fiscal notes using a limited editor with spell check, so that fiscal-note content conforms to the required authoring model.
- Acceptance criteria:
  1. Fiscal notes can be drafted in the solution.
  2. Fiscal notes can be reviewed in the solution.
  3. Spell check is provided.
  4. Editing capabilities are limited as required by DOR.
- Open question / clarification: What functionality distinguishes the "limited editor" from the rich-text editor?
- Development notes:
  - Defer full editor selection until requirements are promoted; define content contracts first.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.1.3`.
  - Preserve traceability to `B.RFA.10` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-4.1.4

- Source user story: US-4.1.4
- Source requirement: B.LNP.04 (Must-Have occurrence)
- Priority: Must Have
- User role: L&P user
- Planning status: Promoted and implemented in Sprint 8.
- User story: As a L&P user, I want to draft and review work products in a self-contained rich-text editor with spell check, so that legislative analysis can be prepared within the solution.
- Acceptance criteria:
  1. Applicable L&P products can be drafted.
  2. Applicable L&P products can be reviewed.
  3. The editor supports rich text.
  4. Spell check is available.
- Open question / clarification: None recorded.
- Development notes:
  - Defer full editor selection until requirements are promoted; define content contracts first.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.1.4`.
  - Preserve traceability to `B.LNP.04 (Must-Have occurrence)` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F4.2 - Attachments and Work in Progress

##### BI-US-4.2.1

- Source user story: US-4.2.1
- Source requirement: B.COM.21
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 8.
- User story: As a DOR user, I want to attach multiple documents and file formats to work tasks, so that supporting evidence and correspondence remain with the legislative record.
- Acceptance criteria:
  1. A task can contain multiple attachments.
  2. Supported formats include PDF, email, and Excel.
  3. Other permitted document types can be attached.
  4. Attachments remain associated with the applicable task.
- Open question / clarification: What file types, file-size limits, malware scanning rules, and storage limits apply?
- Development notes:
  - Store document metadata separately from binary/document repository concerns.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.2.1`.
  - Preserve traceability to `B.COM.21` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-4.2.2

- Source user story: US-4.2.2
- Source requirement: B.COM.22
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 8.
- User story: As a DOR user, I want to save incomplete work products and tasks, so that I can resume work when needed without losing progress.
- Acceptance criteria:
  1. Incomplete work can be saved.
  2. Saved work can be reopened.
  3. Saving work does not require completion or approval.
  4. Existing associations and task information are retained.
- Open question / clarification: Is autosave required in addition to explicit save, and what versioning behavior is expected for work in progress?
- Development notes:
  - Store document metadata separately from binary/document repository concerns.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.2.2`.
  - Preserve traceability to `B.COM.22` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F4.3 - Templates and Generated Documents

##### BI-US-4.3.1

- Source user story: US-4.3.1
- Source requirement: B.COM.24
- Priority: Must Have
- User role: DOR business-area user
- Planning status: Promoted and implemented in Sprint 9.
- User story: As a DOR business-area user, I want to generate customized documentation using system data, so that outputs appropriate to different business areas can be shared inside and outside DOR.
- Acceptance criteria:
  1. Documentation can be generated from system data.
  2. Different business areas can use applicable customized documentation.
  3. Generated documentation can be used internally.
  4. Generated documentation can be used externally.
- Open question / clarification: None recorded.
- Development notes:
  - Treat template merge fields as explicit data contracts.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.3.1`.
  - Preserve traceability to `B.COM.24` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-4.3.2

- Source user story: US-4.3.2
- Source requirement: B.COM.25
- Priority: Must Have
- User role: authorized DOR user
- Planning status: Promoted and implemented in Sprint 9.
- User story: As a authorized DOR user, I want to modify and update custom templates, so that documentation can adapt when business needs change.
- Acceptance criteria:
  1. Authorized users can modify custom templates.
  2. Authorized users can update templates.
  3. Templates can be maintained separately for applicable work-product or document types.
  4. Template changes can accommodate changing business needs.
- Open question / clarification: Does template modification require an administrator, business owner, approval workflow, or version control?
- Development notes:
  - Treat template merge fields as explicit data contracts.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.3.2`.
  - Preserve traceability to `B.COM.25` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-4.3.3

- Source user story: US-4.3.3
- Source requirement: B.COM.26
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 9.
- User story: As a DOR user, I want templates and work papers automatically populated with applicable system data, so that documentation can be produced consistently without redundant data entry.
- Acceptance criteria:
  1. System data can populate applicable templates or work papers.
  2. Population can be based on work type.
  3. Population can be based on indicators.
  4. Population can be based on flags.
- Open question / clarification: None recorded.
- Development notes:
  - Treat template merge fields as explicit data contracts.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.3.3`.
  - Preserve traceability to `B.COM.26` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-4.3.4

- Source user story: US-4.3.4
- Source requirement: B.COM.27
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 9.
- User story: As a DOR user, I want to share documentation and document templates stored in the solution, so that authorized users can reuse common materials.
- Acceptance criteria:
  1. Stored documentation can be shared with authorized users.
  2. Stored templates can be shared with authorized users.
  3. Existing security restrictions remain enforced.
- Open question / clarification: None recorded.
- Development notes:
  - Treat template merge fields as explicit data contracts.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.3.4`.
  - Preserve traceability to `B.COM.27` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F4.4 - Reuse Existing Work

##### BI-US-4.4.1

- Source user story: US-4.4.1
- Source requirement: B.COM.28
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 9.
- User story: As a DOR user, I want to transfer applicable work from an existing product to a new product without copy-and-paste, so that prior work can be reused efficiently and accurately.
- Acceptance criteria:
  1. Applicable existing work can be transferred to a new product.
  2. Manual copy-and-paste is not required.
  3. The destination product remains independently identifiable.
- Open question / clarification: Which fields or content are transferable and which must remain product-specific?
- Development notes:
  - Copy or transfer prior work through structured reuse operations, not manual paste assumptions.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.4.1`.
  - Preserve traceability to `B.COM.28` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-4.4.2

- Source user story: US-4.4.2
- Source requirement: B.COM.29
- Priority: Must Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 9.
- User story: As a DOR user, I want to transfer data from prior work products into products for similar bills in a subsequent year without copy-and-paste, so that prior analysis can be reused and rework reduced.
- Acceptance criteria:
  1. Prior-year product data can be located.
  2. Applicable data can be transferred into a new product.
  3. Manual copy-and-paste is not required.
  4. The prior product remains separately available.
- Open question / clarification: Which prior-year fields may be reused automatically versus explicitly selected by a user?
- Development notes:
  - Copy or transfer prior work through structured reuse operations, not manual paste assumptions.
  - Add or update tests that assert the promoted acceptance criteria for `US-4.4.2`.
  - Preserve traceability to `B.COM.29` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase Trace Matrix

| Backlog Item | User Story | Source Requirement | Priority | Feature |
| --- | --- | --- | --- | --- |
| BI-US-3.1.1 | US-3.1.1 | B.COM.15 | Must Have | F3.1 - Configurable Workflow |
| BI-US-3.1.2 | US-3.1.2 | B.COM.19 | Must Have | F3.1 - Configurable Workflow |
| BI-US-3.1.3 | US-3.1.3 | B.COM.20 | Must Have | F3.1 - Configurable Workflow |
| BI-US-3.2.1 | US-3.2.1 | B.RFA.01 | Must Have | F3.2 - RFA Executive Review |
| BI-US-3.2.2 | US-3.2.2 | B.RFA.02 | Must Have | F3.2 - RFA Executive Review |
| BI-US-3.2.3 | US-3.2.3 | B.RFA.04 | Must Have | F3.2 - RFA Executive Review |
| BI-US-3.2.4 | US-3.2.4 | B.RFA.05 | Must Have | F3.2 - RFA Executive Review |
| BI-US-4.1.1 | US-4.1.1 | B.COM.17 | Must Have | F4.1 - Rich-Text Authoring |
| BI-US-4.1.2 | US-4.1.2 | B.RFA.09 | Must Have | F4.1 - Rich-Text Authoring |
| BI-US-4.1.3 | US-4.1.3 | B.RFA.10 | Must Have | F4.1 - Rich-Text Authoring |
| BI-US-4.1.4 | US-4.1.4 | B.LNP.04 (Must-Have occurrence) | Must Have | F4.1 - Rich-Text Authoring |
| BI-US-4.2.1 | US-4.2.1 | B.COM.21 | Must Have | F4.2 - Attachments and Work in Progress |
| BI-US-4.2.2 | US-4.2.2 | B.COM.22 | Must Have | F4.2 - Attachments and Work in Progress |
| BI-US-4.3.1 | US-4.3.1 | B.COM.24 | Must Have | F4.3 - Templates and Generated Documents |
| BI-US-4.3.2 | US-4.3.2 | B.COM.25 | Must Have | F4.3 - Templates and Generated Documents |
| BI-US-4.3.3 | US-4.3.3 | B.COM.26 | Must Have | F4.3 - Templates and Generated Documents |
| BI-US-4.3.4 | US-4.3.4 | B.COM.27 | Must Have | F4.3 - Templates and Generated Documents |
| BI-US-4.4.1 | US-4.4.1 | B.COM.28 | Must Have | F4.4 - Reuse Existing Work |
| BI-US-4.4.2 | US-4.4.2 | B.COM.29 | Must Have | F4.4 - Reuse Existing Work |
