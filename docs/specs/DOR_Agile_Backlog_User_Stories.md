# DOR Agile Backlog User Stories

Generated: 2026-09-03

Source workbook: `docs/specs/DOR_Agile_Backlog_User_Stories.xlsx`

Source worksheet: `Agile Backlog`

Purpose: This Markdown file converts the business agile backlog into a sprint-planning-ready reference. It preserves story-level traceability, acceptance criteria, priority, open questions, and source-document references from the source workbook.

Planning status: This document is a derivative planning resource. It does not authorize implementation by itself; implementation remains governed by `docs/02-Current-Sprint.md`.

Regeneration: Run `python tools/convert_dor_backlog.py` from the repository root after the source workbook changes.

## Conversion Summary

- Total user stories: 69
- Priority mix: 62 must-have, 7 nice-to-have, 69 total
- Organization: stories are grouped into logical phases, then epics, features, and user stories.
- Sprint use: each phase includes candidate sprint planning units, but final sprint scope should be promoted into `docs/02-Current-Sprint.md` before implementation.

## Logical Phase Map

| Phase | Focus | Epics | Candidate Sprint Planning Units | Story Count |
| --- | --- | --- | --- | ---: |
| Phase 1 | Core Work Intake, Collaboration, and Organization | E1, E2 | Collaboration workspace, notifications, task creation, assignment, and work queues<br>Identifiers, cross-record relationships, packages, sorting, filtering, and grouping | 15 |
| Phase 2 | Workflow, Review, Authoring, and Document Production | E3, E4 | Configurable workflow, multi-reviewer routing, executive review, priorities, and due dates<br>Rich-text authoring, attachments, work-in-progress save, templates, generated documents, and reuse | 19 |
| Phase 3 | Legislative Data Lifecycle, Search, and Reporting | E5, E6 | External legislative updates, bill status updates, amendments, version retention, and comparison<br>Enterprise search, predetermined reports, saved queries, database query access, and extracts | 12 |
| Phase 4 | Fiscal Analysis, Financial Inputs, and Productivity Integration | E7, E8, E14 | Fiscal data retrieval, fiscal-work documentation, budget reconciliation, demographics, and expense estimates<br>Microsoft 365 integration for Teams, Outlook, Excel, and Word productivity workflows | 6 |
| Phase 5 | Security, Operations, Migration, and Historical Reference | E9, E10 | Role-based security, partial work-product restrictions, concurrency, and availability<br>Legacy migration and 10-year historical fiscal reference access | 6 |
| Phase 6 | Specialized Legislative Programs and Executive Experience | E11, E12, E13 | L&P correspondence and legislative implementation management<br>Remote/mobile executive access, executive bill view, and executive bill-page discussion | 11 |

## Phase 1 - Core Work Intake, Collaboration, and Organization

Builds the core operating model that later workflow, documents, reporting, and integrations depend on.

Phase backlog: 15 must-have, 0 nice-to-have, 15 total.

Candidate sprint planning units:
- Collaboration workspace, notifications, task creation, assignment, and work queues
- Identifiers, cross-record relationships, packages, sorting, filtering, and grouping

### E1 - Collaborative Legislative Work Management

Epic backlog: 10 must-have, 0 nice-to-have, 10 total.

#### F1.1 - Centralized Collaboration

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-1.1.1 - B.COM.01

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want multiple subject matter experts to collaborate on specified topics, documents, tasks, and assignments in a single reference location, so that all contributors can work from a common source of information.
- Acceptance criteria:
  1. Multiple authorized users can collaborate around the same topic, work task, assignment, or document.
  2. Collaboration information is accessible from a single reference location associated with the applicable work.
  3. Multiple subject matter experts can contribute to the same body of work.
  4. Access is subject to applicable role and data restrictions.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-1.1.2 - B.COM.08

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to view multiple documents and work products simultaneously, so that I can compare and analyze related information efficiently.
- Acceptance criteria:
  1. A user can have more than one document or work product available for viewing at the same time.
  2. Simultaneous viewing does not require the user to discard unsaved work.
  3. Access restrictions continue to apply to every displayed item.
- Open question / clarification: Does "simultaneously" require split-screen viewing, multiple browser tabs/windows, an in-application document viewer, or another user experience?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F1.2 - Notifications

Feature backlog: 1 must-have, 0 nice-to-have, 1 total.

##### US-1.2.1 - B.COM.02

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to receive notifications based on designated criteria, so that I am alerted when assigned work or legislative changes require my attention.
- Acceptance criteria:
  1. Notifications can be generated when work tasks are assigned.
  2. Notifications can be generated when external bill changes affect related work products.
  3. Notification triggers are based on designated criteria.
  4. Notifications are directed to the applicable users.
- Open question / clarification: What notification channels, trigger catalog, subscription options, and escalation rules are required?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F1.3 - Task Creation and Assignment

Feature backlog: 5 must-have, 0 nice-to-have, 5 total.

##### US-1.3.1 - B.COM.11

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to create work tasks, so that legislative work can be formally initiated and tracked.
- Acceptance criteria:
  1. An authorized user can create a task.
  2. The task is stored in the solution.
  3. The task receives the attributes required to support assignment, workflow, search, and reporting.
- Open question / clarification: What fields are mandatory when a task is created?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-1.3.2 - B.COM.12

- Priority: Must Have
- User role: authorized DOR user
- User story: As a authorized DOR user, I want to assign and reassign tasks to users within DOR, so that work can be routed to the appropriate resource.
- Acceptance criteria:
  1. An authorized user can assign a task to a DOR user.
  2. An authorized user can subsequently reassign the task.
  3. The currently assigned user is identifiable.
  4. Reassignment does not remove the work product or supporting information.
- Open question / clarification: Must assignment history, including prior assignees and dates, be retained?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-1.3.3 - B.COM.13

- Priority: Must Have
- User role: work coordinator
- User story: As a work coordinator, I want to assign multiple users to the same task with due dates based on each user role or required work, so that collaborative activities can be scheduled independently.
- Acceptance criteria:
  1. More than one user can be assigned to a task or assignment.
  2. A due date can be associated with each applicable user role or work assignment.
  3. Users can identify their individual responsibilities and applicable due dates.
  4. Multiple due dates can coexist on the same overall task.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-1.3.4 - B.COM.14

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to track the customer due date for each work product, so that DOR can meet external or internal delivery commitments.
- Acceptance criteria:
  1. Each applicable work product can store a customer due date.
  2. The customer due date can be viewed with the applicable work product.
  3. The customer due date remains associated with the product throughout its workflow.
- Open question / clarification: Who is considered a "customer," and can a work product have more than one customer due date?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-1.3.5 - B.COM.10

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want a work queue showing all work assigned to me, so that I can manage my workload, rework, status, priority, and volume.
- Acceptance criteria:
  1. A user can view work assigned to that user.
  2. The queue identifies rework.
  3. The queue provides status information.
  4. The queue provides priority information.
  5. The queue supports visibility into workload quantity.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F1.4 - Task Maintenance

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-1.4.1 - B.COM.18

- Priority: Must Have
- User role: authorized DOR user
- User story: As a authorized DOR user, I want to update, cancel, change, duplicate, or correct tasks, so that the system reflects changing legislative circumstances.
- Acceptance criteria:
  1. Authorized users can update tasks.
  2. Authorized users can cancel tasks.
  3. Authorized users can change applicable task information.
  4. Authorized users can duplicate a task.
  5. Authorized users can make corrections to task information.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-1.4.2 - B.COM.23

- Priority: Must Have
- User role: authorized DOR user
- User story: As a authorized DOR user, I want to reassign, update, cancel, or change work after submission or approval, so that DOR can respond when circumstances change.
- Acceptance criteria:
  1. The capability is limited to authorized users.
  2. Submitted work can be reassigned where permitted.
  3. Submitted or approved work can be corrected, updated, changed, or canceled where permitted.
  4. The prior approved or submitted state is not silently lost.
- Open question / clarification: Does changing an approved product automatically reopen its review and approval workflow?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E2 - Identification, Relationships, and Legislative Work Organization

Epic backlog: 5 must-have, 0 nice-to-have, 5 total.

#### F2.1 - Unique Identification

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-2.1.1 - B.COM.03

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want every work task, product, and document type to receive a unique identifier, so that individual items can be reliably tracked and referenced.
- Acceptance criteria:
  1. The solution automatically assigns an identifier to applicable items.
  2. Identifiers uniquely distinguish individual records.
  3. The capability applies to bill analyses, fiscal notes, fiscal estimates, data requests, and other applicable work types.
- Open question / clarification: What identifier format is required? Must identifiers remain unique across work types, years, and biennia?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-2.1.2 - B.RFA.03

- Priority: Must Have
- User role: authorized RFA user
- User story: As a authorized RFA user, I want to override an automatically assigned identifier, so that exceptional fiscal work can use the appropriate business identifier.
- Acceptance criteria:
  1. Authorized users can replace or modify an automatically generated identifier.
  2. The system prevents an override from creating an impermissible duplicate identifier.
  3. The override is retained with the applicable work product.
- Open question / clarification: Which roles may override identifiers and what audit trail is required?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F2.2 - Work Relationships and Packages

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-2.2.1 - B.COM.05

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want related data, tasks, products, assignments, and documents linked together, so that the complete history and context of legislative work can be accessed from related records.
- Acceptance criteria:
  1. Work can be linked by topic.
  2. Work can be linked by document type.
  3. Work can be linked by unique legislative identifier such as bill number.
  4. Work can be linked by named package.
  5. Bill proposals, drafts, and bill versions can remain related as the bill progresses through stages.
  6. Fiscal notes, fiscal estimates, sponsor letters, staff emails, data requests, and hearing reports can be linked to applicable legislative work or packages.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-2.2.2 - B.RFA.06

- Priority: Must Have
- User role: RFA user
- User story: As a RFA user, I want to combine several work products into one deliverable and evaluate the package status, so that related fiscal work can be managed and delivered together.
- Acceptance criteria:
  1. Multiple work products can be associated with one package.
  2. The package can be delivered as one product.
  3. Users can determine the status of the package.
  4. The underlying work products remain identifiable.
- Open question / clarification: Page 1 defines a package as a set of fiscal estimates, while the RFA description refers to packages containing estimates, fiscal notes, and data requests. Which definition controls?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F2.3 - Categorization

Feature backlog: 1 must-have, 0 nice-to-have, 1 total.

##### US-2.3.1 - B.COM.04

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to identify, sort, filter, and group information using designated criteria, so that I can organize legislative work for my business needs.
- Acceptance criteria:
  1. Users can identify records using designated criteria.
  2. Users can sort information.
  3. Users can filter information.
  4. Users can group information.
  5. Supported criteria include, at minimum, Confidential, Executive Review, On Hold, Work Type, and Package where applicable.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase 2 - Workflow, Review, Authoring, and Document Production

Turns the intake model into usable legislative work production with review and approval controls.

Phase backlog: 19 must-have, 0 nice-to-have, 19 total.

Candidate sprint planning units:
- Configurable workflow, multi-reviewer routing, executive review, priorities, and due dates
- Rich-text authoring, attachments, work-in-progress save, templates, generated documents, and reuse

### E3 - Workflow, Review, Approval, and Submission

Epic backlog: 7 must-have, 0 nice-to-have, 7 total.

#### F3.1 - Configurable Workflow

Feature backlog: 3 must-have, 0 nice-to-have, 3 total.

##### US-3.1.1 - B.COM.15

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want tasks to follow a defined workflow in which work and approval responsibilities can be separated, so that legislative products receive required review before completion.
- Acceptance criteria:
  1. A task can proceed through a defined workflow.
  2. One or more users can perform the work.
  3. One or more different users can review and approve the work.
  4. Workflow status can be determined.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-3.1.2 - B.COM.19

- Priority: Must Have
- User role: DOR reviewer
- User story: As a DOR reviewer, I want completed products and documentation routed through multiple subject matter experts when required, so that appropriate review occurs before submission.
- Acceptance criteria:
  1. A completed product can enter a review workflow.
  2. Multiple subject matter experts can participate in review and approval.
  3. Review occurs before applicable internal or external submission.
  4. The product approval state can be determined.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-3.1.3 - B.COM.20

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want finalized products packaged and prepared for submission, so that approved documentation can be delivered to internal and external stakeholders.
- Acceptance criteria:
  1. Only the appropriate reviewed or approved products can be prepared as finalized products.
  2. Finalized documentation can be packaged for submission.
  3. Packages can support applicable internal recipients.
  4. Packages can support applicable external recipients.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F3.2 - RFA Executive Review

Feature backlog: 4 must-have, 0 nice-to-have, 4 total.

##### US-3.2.1 - B.RFA.01

- Priority: Must Have
- User role: authorized executive reviewer
- User story: As a authorized executive reviewer, I want permission to conduct Executive Reviews of fiscal notes and fiscal estimates, so that cross-division agreement can be obtained before external release.
- Acceptance criteria:
  1. Executive Review capability is restricted to designated users.
  2. Fiscal notes can undergo Executive Review.
  3. Fiscal estimates can undergo Executive Review.
  4. Review occurs prior to applicable release to OFM or another external stakeholder.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-3.2.2 - B.RFA.02

- Priority: Must Have
- User role: executive reviewer
- User story: As a executive reviewer, I want to perform the entire fiscal-product Executive Review inside the solution, so that review, correction, completion, and reviewer handoffs can occur without a separate process.
- Acceptance criteria:
  1. Each applicable reviewer can access the fiscal product.
  2. A reviewer can adjust or correct the product as permitted.
  3. A reviewer can indicate completion of that review.
  4. Prior reviewer or reviewers can be notified as required.
  5. The next reviewer can be notified automatically.
  6. The workflow continues until the required Executive Review is complete.
- Open question / clarification: Is Executive Review sequential, parallel, or configurable by product?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-3.2.3 - B.RFA.04

- Priority: Must Have
- User role: RFA coordinator
- User story: As a RFA coordinator, I want a due date for each step or subtask within a work product, so that time-sensitive fiscal work can be actively managed.
- Acceptance criteria:
  1. Individual workflow steps or subtasks can have due dates.
  2. Multiple step-level due dates can exist within one product.
  3. Users can determine which due date applies to each step.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-3.2.4 - B.RFA.05

- Priority: Must Have
- User role: RFA user
- User story: As a RFA user, I want to set a priority for each work product, so that limited resources can be focused on the most urgent legislative work.
- Acceptance criteria:
  1. Each applicable product can be assigned a priority.
  2. Priority can be viewed by users managing the work.
  3. Priority can be used in workload management.
- Open question / clarification: What priority values and escalation rules are required?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E4 - Content Authoring, Documents, and Templates

Epic backlog: 12 must-have, 0 nice-to-have, 12 total.

#### F4.1 - Rich-Text Authoring

Feature backlog: 4 must-have, 0 nice-to-have, 4 total.

##### US-4.1.1 - B.COM.17

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to draft, edit, and review designated work products using a self-contained rich-text editor with spell check, so that I can prepare professional work products inside the solution.
- Acceptance criteria:
  1. Designated work products can be drafted in the solution.
  2. Designated work products can be edited.
  3. Designated work products can be reviewed.
  4. The editor supports rich-text functionality.
  5. The editor includes spell check.
- Open question / clarification: Which work-product types are "designated"?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-4.1.2 - B.RFA.09

- Priority: Must Have
- User role: RFA user
- User story: As a RFA user, I want to draft and review fiscal estimates and data requests in a self-contained rich-text editor with spell check, so that these products can be completed entirely within the solution.
- Acceptance criteria:
  1. Fiscal estimates can be drafted and reviewed in the solution.
  2. Data requests can be drafted and reviewed.
  3. Rich-text formatting is available.
  4. Spell check is available.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-4.1.3 - B.RFA.10

- Priority: Must Have
- User role: RFA user
- User story: As a RFA user, I want to draft and review fiscal notes using a limited editor with spell check, so that fiscal-note content conforms to the required authoring model.
- Acceptance criteria:
  1. Fiscal notes can be drafted in the solution.
  2. Fiscal notes can be reviewed in the solution.
  3. Spell check is provided.
  4. Editing capabilities are limited as required by DOR.
- Open question / clarification: What functionality distinguishes the "limited editor" from the rich-text editor?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-4.1.4 - B.LNP.04 (Must-Have occurrence)

- Priority: Must Have
- User role: L&P user
- User story: As a L&P user, I want to draft and review work products in a self-contained rich-text editor with spell check, so that legislative analysis can be prepared within the solution.
- Acceptance criteria:
  1. Applicable L&P products can be drafted.
  2. Applicable L&P products can be reviewed.
  3. The editor supports rich text.
  4. Spell check is available.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F4.2 - Attachments and Work in Progress

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-4.2.1 - B.COM.21

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to attach multiple documents and file formats to work tasks, so that supporting evidence and correspondence remain with the legislative record.
- Acceptance criteria:
  1. A task can contain multiple attachments.
  2. Supported formats include PDF, email, and Excel.
  3. Other permitted document types can be attached.
  4. Attachments remain associated with the applicable task.
- Open question / clarification: What file types, file-size limits, malware scanning rules, and storage limits apply?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-4.2.2 - B.COM.22

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to save incomplete work products and tasks, so that I can resume work when needed without losing progress.
- Acceptance criteria:
  1. Incomplete work can be saved.
  2. Saved work can be reopened.
  3. Saving work does not require completion or approval.
  4. Existing associations and task information are retained.
- Open question / clarification: Is autosave required in addition to explicit save, and what versioning behavior is expected for work in progress?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F4.3 - Templates and Generated Documents

Feature backlog: 4 must-have, 0 nice-to-have, 4 total.

##### US-4.3.1 - B.COM.24

- Priority: Must Have
- User role: DOR business-area user
- User story: As a DOR business-area user, I want to generate customized documentation using system data, so that outputs appropriate to different business areas can be shared inside and outside DOR.
- Acceptance criteria:
  1. Documentation can be generated from system data.
  2. Different business areas can use applicable customized documentation.
  3. Generated documentation can be used internally.
  4. Generated documentation can be used externally.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-4.3.2 - B.COM.25

- Priority: Must Have
- User role: authorized DOR user
- User story: As a authorized DOR user, I want to modify and update custom templates, so that documentation can adapt when business needs change.
- Acceptance criteria:
  1. Authorized users can modify custom templates.
  2. Authorized users can update templates.
  3. Templates can be maintained separately for applicable work-product or document types.
  4. Template changes can accommodate changing business needs.
- Open question / clarification: Does template modification require an administrator, business owner, approval workflow, or version control?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-4.3.3 - B.COM.26

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want templates and work papers automatically populated with applicable system data, so that documentation can be produced consistently without redundant data entry.
- Acceptance criteria:
  1. System data can populate applicable templates or work papers.
  2. Population can be based on work type.
  3. Population can be based on indicators.
  4. Population can be based on flags.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-4.3.4 - B.COM.27

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to share documentation and document templates stored in the solution, so that authorized users can reuse common materials.
- Acceptance criteria:
  1. Stored documentation can be shared with authorized users.
  2. Stored templates can be shared with authorized users.
  3. Existing security restrictions remain enforced.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F4.4 - Reuse Existing Work

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-4.4.1 - B.COM.28

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to transfer applicable work from an existing product to a new product without copy-and-paste, so that prior work can be reused efficiently and accurately.
- Acceptance criteria:
  1. Applicable existing work can be transferred to a new product.
  2. Manual copy-and-paste is not required.
  3. The destination product remains independently identifiable.
- Open question / clarification: Which fields or content are transferable and which must remain product-specific?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-4.4.2 - B.COM.29

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to transfer data from prior work products into products for similar bills in a subsequent year without copy-and-paste, so that prior analysis can be reused and rework reduced.
- Acceptance criteria:
  1. Prior-year product data can be located.
  2. Applicable data can be transferred into a new product.
  3. Manual copy-and-paste is not required.
  4. The prior product remains separately available.
- Open question / clarification: Which prior-year fields may be reused automatically versus explicitly selected by a user?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase 3 - Legislative Data Lifecycle, Search, and Reporting

Adds the bill lifecycle data, version history, and find/report capabilities needed for scale.

Phase backlog: 12 must-have, 0 nice-to-have, 12 total.

Candidate sprint planning units:
- External legislative updates, bill status updates, amendments, version retention, and comparison
- Enterprise search, predetermined reports, saved queries, database query access, and extracts

### E5 - Legislative Data, Bill Lifecycle, and Version Management

Epic backlog: 7 must-have, 0 nice-to-have, 7 total.

#### F5.1 - External Legislative Updates

Feature backlog: 3 must-have, 0 nice-to-have, 3 total.

##### US-5.1.1 - B.COM.31

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want external bill language and legislative changes automatically detected and brought into the solution in real time, so that DOR always works from current legislative information.
- Acceptance criteria:
  1. Applicable external agency systems can be interfaced with.
  2. New bill language can be identified.
  3. New or changed bill information can be retrieved.
  4. Corresponding bills in the DOR solution are updated.
  5. Legislative lifecycle changes such as HB to SHB can be represented.
  6. Relevant hearing schedule changes can be received and updates occur according to the agreed meaning of real time.
- Open question / clarification: What maximum latency satisfies "real-time"?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-5.1.2 - B.COM.32

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want bill status changes from external agency systems automatically reflected in the DOR solution, so that I do not have to manually maintain legislative status.
- Acceptance criteria:
  1. Applicable external bill-status sources can be interfaced with.
  2. New bill statuses can be detected.
  3. Status information can be retrieved.
  4. Corresponding DOR records are updated in real time.
- Open question / clarification: Which system is authoritative when externally sourced status data conflicts with DOR-entered information?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-5.1.3 - B.LNP.01

- Priority: Must Have
- User role: L&P analyst
- User story: As a L&P analyst, I want proposed and unadopted amendments imported and tracked from external legislative systems, so that I can analyze amendatory language before or during legislative action.
- Acceptance criteria:
  1. Proposed or unadopted amendment information can be retrieved from supported external sources.
  2. Imported amendment data can be tracked in the solution.
  3. Changes to amendatory language can update applicable system data.
  4. The solution can interface with applicable sources identified by the RFP, including leg.wa.gov, Electronic Bill Book, and the Legislative Service Center API.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F5.2 - Version History and Comparison

Feature backlog: 4 must-have, 0 nice-to-have, 4 total.

##### US-5.2.1 - B.COM.33

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want prior versions of bills and work products retained when updates occur, so that historical states remain available.
- Acceptance criteria:
  1. Prior bill versions are retained.
  2. Prior work-product versions are retained.
  3. An update does not overwrite the only copy of the previous version.
  4. Users with appropriate access can retrieve retained versions.
- Open question / clarification: B.COM.33 says versions are retained before each update as described in B.COM.26 and B.COM.27, but those requirements concern template population and sharing. Is that cross-reference incorrect?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-5.2.2 - B.COM.35

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to view and retain every version of work papers, tasks, and documents as they progress, so that the full evolution of a work product is available.
- Acceptance criteria:
  1. Versions are retained through completion.
  2. Authorized users can view retained versions.
  3. Version history applies to applicable work papers, tasks, and documents.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-5.2.3 - B.LNP.02

- Priority: Must Have
- User role: L&P analyst
- User story: As a L&P analyst, I want to compare a draft bill with another bill version, including a version from a prior session, so that I can identify differences between the documents.
- Acceptance criteria:
  1. A user can select two applicable bill documents for comparison.
  2. Draft bills can be compared with prior versions.
  3. Prior-session versions can participate in comparison.
  4. Differences between the two documents are presented to the user.
- Open question / clarification: What comparison presentation is required: redline, side-by-side, semantic comparison, or another method?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-5.2.4 - B.COM.43

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to view a bill complete history across years within a biennium, so that prior work can be reused and unnecessary rework reduced.
- Acceptance criteria:
  1. Bills can be followed across applicable years of a biennium.
  2. Associated work products remain connected to that history.
  3. Authorized users can view historical bill information.
  4. Historical records can support current-year work.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E6 - Search, Reporting, and Decision Support

Epic backlog: 5 must-have, 0 nice-to-have, 5 total.

#### F6.1 - Enterprise Search

Feature backlog: 1 must-have, 0 nice-to-have, 1 total.

##### US-6.1.1 - B.COM.09

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want robust search across system functionality, so that I can quickly locate tasks, documents, bills, and other information.
- Acceptance criteria:
  1. Users can search for tasks.
  2. Users can search for documents.
  3. Users can search for bills.
  4. Users can search other supported system information.
  5. Search respects security restrictions.
- Open question / clarification: Is full-text search of document contents and attachments required, or only structured metadata search?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F6.2 - Standard and Ad Hoc Reporting

Feature backlog: 4 must-have, 0 nice-to-have, 4 total.

##### US-6.2.1 - B.COM.38

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want predetermined reports for commonly required data, so that recurring operational and performance information can be generated efficiently.
- Acceptance criteria:
  1. Predetermined reports can be executed.
  2. Reports can include performance measures.
  3. Reports can include outstanding fiscal tasks.
  4. The capability supports other agreed recurring report sets.
- Open question / clarification: Which predefined reports must be delivered at go-live?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-6.2.2 - B.COM.39

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to create and save custom queries and reports, so that recurring analytical needs do not have to be recreated each time.
- Acceptance criteria:
  1. Authorized users can create custom queries.
  2. Authorized users can create custom reports.
  3. Custom queries can be saved.
  4. Custom reports can be saved and subsequently reused.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-6.2.3 - B.COM.40

- Priority: Must Have
- User role: authorized DOR user
- User story: As a authorized DOR user, I want access to the database for queries and display of data, so that information can be analyzed as needed.
- Acceptance criteria:
  1. Authorized users can query system data.
  2. Query results can be displayed.
  3. Applicable security controls remain enforced.
  4. Access covers the data required to satisfy the RFP entire-database requirement.
- Open question / clarification: Does "access the entire database" mean direct database access, a governed query layer, reporting semantic model, API access, or equivalent functional access?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-6.2.4 - B.COM.34

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want to extract work products in the forms required for internal or external delivery, so that work can be distributed when needed.
- Acceptance criteria:
  1. Applicable work products can be extracted from the solution.
  2. Extracted outputs can be used internally.
  3. Extracted outputs can be used externally.
  4. Required output formats can be supported once defined.
- Open question / clarification: What exact export formats are mandatory?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase 4 - Fiscal Analysis, Financial Inputs, and Productivity Integration

Connects legislative work to fiscal analysis, budget inputs, and productivity tools.

Phase backlog: 6 must-have, 0 nice-to-have, 6 total.

Candidate sprint planning units:
- Fiscal data retrieval, fiscal-work documentation, budget reconciliation, demographics, and expense estimates
- Microsoft 365 integration for Teams, Outlook, Excel, and Word productivity workflows

### E7 - Fiscal Analysis and Financial Data

Epic backlog: 4 must-have, 0 nice-to-have, 4 total.

#### F7.1 - Fiscal Data Integration

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-7.1.1 - B.COM.36

- Priority: Must Have
- User role: fiscal analyst
- User story: As a fiscal analyst, I want the solution to retrieve, calculate, and update fiscal data from internal DOR systems, so that fiscal notes and estimates use current agency financial information.
- Acceptance criteria:
  1. Applicable internal DOR systems can be interfaced with.
  2. FTE information can be retrieved or used.
  3. Applicable cost rules can be incorporated.
  4. Fiscal-note and fiscal-estimate calculations and editing are supported.
  5. Revenue funds and source information can be incorporated.
  6. Updated fiscal data can be reflected in applicable products.
- Open question / clarification: Which DOR systems are sources? Which calculations occur in the LTS versus the source systems? Who owns and maintains cost rules?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-7.1.2 - B.COM.37

- Priority: Must Have
- User role: fiscal analyst
- User story: As a fiscal analyst, I want documentation showing how fiscal work papers and tasks were completed, so that historical work can improve consistency, accuracy, and efficiency in current fiscal analysis.
- Acceptance criteria:
  1. Supporting documentation for fiscal work can be stored.
  2. Historical fiscal notes and estimates can be researched.
  3. Supporting documentation can be retrieved with historical products.
  4. Authorized users can use historical material when preparing current work.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F7.2 - RFA Supporting Analysis

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-7.2.1 - B.RFA.07

- Priority: Must Have
- User role: authorized DOR user
- User story: As a authorized DOR user, I want to flag and view bills included in DOR budget and compare them with associated fiscal notes, so that budget assumptions and fiscal analyses can be reconciled.
- Acceptance criteria:
  1. Authorized users can flag applicable budget bills.
  2. Users can view flagged bills.
  3. Associated fiscal notes can be viewed for comparison.
  4. The comparison relationship remains associated with the bill.
- Open question / clarification: How is a bill identified as part of the DOR budget, who can set or remove the flag, and what constitutes a valid comparison?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-7.2.2 - B.RFA.08

- Priority: Must Have
- User role: RFA analyst
- User story: As a RFA analyst, I want to store and retrieve current and previous demographic data by legislative session, so that I can explain expense differences and demographic trends between bills and years.
- Acceptance criteria:
  1. Current demographic data can be stored.
  2. Historical demographic data can be retained.
  3. Data can be associated with relevant legislative sessions.
  4. Users can retrieve information needed to compare expenses between bills or years.
  5. Users can access information needed to identify demographic changes or trends.
- Open question / clarification: What demographic datasets, sources, dimensions, and update schedules are required?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E8 - Microsoft 365 and External System Integration

Epic backlog: 1 must-have, 0 nice-to-have, 1 total.

#### F8.1 - Productivity Suite Integration

Feature backlog: 1 must-have, 0 nice-to-have, 1 total.

##### US-8.1.1 - B.COM.30

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want the solution integrated with Teams, Outlook, Excel, and Word, so that system data can populate working documents and be distributed without redundant manual processing.
- Acceptance criteria:
  1. Applicable integration with Teams is supported.
  2. Applicable integration with Outlook is supported.
  3. Applicable integration with Excel is supported.
  4. Applicable integration with Word is supported.
  5. System data can populate applicable templates.
  6. Applicable outputs can be emailed to internal and external stakeholders.
- Open question / clarification: What specific use cases and direction of integration are required for each Microsoft application?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E14 - Budget Office Fiscal Inputs

Epic backlog: 1 must-have, 0 nice-to-have, 1 total.

#### F14.1 - Expense Estimate Management

Feature backlog: 1 must-have, 0 nice-to-have, 1 total.

##### US-14.1.1 - B.BGT.01

- Priority: Must Have
- User role: Budget Office user
- User story: As a Budget Office user, I want to enter and update the elements used to calculate expense estimates, so that fiscal work products reflect current agency cost assumptions.
- Acceptance criteria:
  1. Authorized users can enter applicable expense-estimate elements.
  2. Authorized users can update those elements.
  3. Supported elements include costs of goods and services.
  4. Supported elements include percentages of salary where applicable.
  5. Applicable values can be incorporated into fiscal work products.
- Open question / clarification: Are the Budget Office calculations expected to be performed automatically by the solution? What formulas, rounding rules, effective dates, and approval processes govern the calculations?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase 5 - Security, Operations, Migration, and Historical Reference

Hardens the solution for enterprise operation and preserves historical context.

Phase backlog: 6 must-have, 0 nice-to-have, 6 total.

Candidate sprint planning units:
- Role-based security, partial work-product restrictions, concurrency, and availability
- Legacy migration and 10-year historical fiscal reference access

### E9 - Security, Access, Performance, and Operations

Epic backlog: 4 must-have, 0 nice-to-have, 4 total.

#### F9.1 - Role-Based Security

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-9.1.1 - B.COM.06

- Priority: Must Have
- User role: DOR security administrator
- User story: As a DOR security administrator, I want permissions based on user roles, so that users can perform only the activities authorized for their responsibilities.
- Acceptance criteria:
  1. Permissions can distinguish preparation activity.
  2. Permissions can distinguish approval activity.
  3. Permissions can distinguish delivery activity.
  4. Read-only access to final products can be provided.
  5. Users cannot perform restricted functions outside their assigned permissions.
- Open question / clarification: What is the authoritative role and permission matrix?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-9.1.2 - B.COM.16

- Priority: Must Have
- User role: DOR security administrator
- User story: As a DOR security administrator, I want access restrictions at portions of a work task or product, so that sensitive information can be restricted by data type or user type.
- Acceptance criteria:
  1. Access can be restricted within applicable work tasks or products.
  2. Restrictions can be based on data type.
  3. Restrictions can be based on user type.
  4. Unauthorized users cannot access restricted information.
- Open question / clarification: What level of granularity is required: field, section, document, attachment, comment, or another level?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F9.2 - Concurrent Use and Availability

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-9.2.1 - B.COM.07

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want more than 60 users to use the solution concurrently without degradation, so that agency-wide collaboration can continue during legislative peaks.
- Acceptance criteria:
  1. More than 60 users can access the solution concurrently.
  2. Concurrent activity does not cause unacceptable degradation.
  3. Normal collaboration capabilities remain available during the supported concurrency level.
- Open question / clarification: What measurable response-time or performance threshold defines "without degradation"?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-9.2.2 - B.COM.42

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want the solution and support available 24/7 during peak legislative months and as needed during the remainder of the year, so that legislative deadlines can be met regardless of normal business hours.
- Acceptance criteria:
  1. The solution supports 24/7 availability during November through June.
  2. Support is available 24/7 during November through June.
  3. Year-round availability and support are provided as required by DOR.
  4. Support accommodates legislative work outside normal business hours.
- Open question / clarification: What SLA applies? What does "as needed year-round" mean operationally? What incident response and resolution targets apply?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E10 - Data Migration and Historical Records

Epic backlog: 2 must-have, 0 nice-to-have, 2 total.

#### F10.1 - Legacy Migration

Feature backlog: 1 must-have, 0 nice-to-have, 1 total.

##### US-10.1.1 - B.COM.41

- Priority: Must Have
- User role: DOR user
- User story: As a DOR user, I want legacy-system data migrated into the new solution, so that historical legislative and fiscal information remains available after replacement of the existing system.
- Acceptance criteria:
  1. Applicable legacy data can be migrated.
  2. Migrated records remain usable in the new solution.
  3. Required relationships between migrated records are retained or reconstructed.
  4. Migrated information is accessible according to applicable permissions.
- Open question / clarification: Which legacy systems are in scope? What volumes, formats, attachments, historical periods, quality rules, and reconciliation requirements apply?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F10.2 - Financial Historical Reference

Feature backlog: 1 must-have, 0 nice-to-have, 1 total.

##### US-10.2.1 - B.EXP.01

- Priority: Must Have
- User role: B&FS financial user
- User story: As a B&FS financial user, I want to view all work products from the prior 10 years, so that my input on current work is consistent with historical analysis.
- Acceptance criteria:
  1. Authorized users can access work products covering the required 10-year period.
  2. Historical products can be viewed.
  3. Historical information can be used to inform current work.
  4. Access restrictions continue to apply.
- Open question / clarification: Is "10 years" based on calendar year, legislative session, biennium, or a rolling 10-year period?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase 6 - Specialized Legislative Programs and Executive Experience

Layers specialized workflows and executive experiences on top of the established system capabilities.

Phase backlog: 4 must-have, 7 nice-to-have, 11 total.

Candidate sprint planning units:
- L&P correspondence and legislative implementation management
- Remote/mobile executive access, executive bill view, and executive bill-page discussion

### E11 - L&P Legislative Analysis and Correspondence

Epic backlog: 1 must-have, 0 nice-to-have, 1 total.

#### F11.1 - Correspondence Tracking

Feature backlog: 1 must-have, 0 nice-to-have, 1 total.

##### US-11.1.1 - B.LNP.03

- Priority: Must Have
- User role: L&P user
- User story: As a L&P user, I want to track correspondence sent to recipients and whether responses were received, so that bill-related communications can be followed through completion.
- Acceptance criteria:
  1. A sent correspondence item can be recorded.
  2. The recipient can be identified.
  3. The system can indicate whether a response was received.
  4. Correspondence remains connected with applicable legislative work.
- Open question / clarification: Must email correspondence be automatically captured from Outlook, manually logged, or both?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E12 - Legislative Implementation Management

Epic backlog: 0 must-have, 6 nice-to-have, 6 total.

#### F12.1 - Implementation Task Management

Feature backlog: 0 must-have, 6 nice-to-have, 6 total.

##### US-12.1.1 - B.LNP.04 (Nice-to-Have occurrence)

- Priority: Nice to Have
- User role: L&P user
- User story: As a L&P user, I want legislative implementation tasks assigned and reassigned across the agency, so that responsibility for implementing enacted legislation is clearly tracked.
- Acceptance criteria:
  1. Implementation tasks can be assigned to individuals across DOR.
  2. Tasks can be reassigned.
  3. The responsible individual division can be identified.
  4. Required work can be identified.
  5. Due dates can be tracked.
  6. Completion can be tracked.
- Open question / clarification: The source uses B.LNP.04 twice. What identifier should replace this Nice-to-Have occurrence?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-12.1.2 - B.LNP.05

- Priority: Nice to Have
- User role: user collaborating on a legislative implementation plan
- User story: As a user collaborating on a legislative implementation plan, I want to share related documents through an interface that displays the required implementation information, so that participants can coordinate from one location.
- Acceptance criteria:
  1. Implementation-plan collaborators can share related documents.
  2. Shared documents remain associated with applicable implementation work.
  3. Required implementation information is displayed in the interface.
- Open question / clarification: B.LNP.05 says "the information above, in B.LNP.06," although B.LNP.06 follows it and specifies a status report. What exact information must be displayed?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-12.1.3 - B.LNP.06

- Priority: Nice to Have
- User role: L&P user
- User story: As a L&P user, I want a report showing the status of legislative implementation tasks, so that implementation progress can be monitored.
- Acceptance criteria:
  1. The solution can generate an implementation-task status report.
  2. Applicable task status information is included.
  3. Authorized users can access the report.
- Open question / clarification: What fields must appear in the legislative-implementation status report?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-12.1.4 - B.LNP.07

- Priority: Nice to Have
- User role: DOR user
- User story: As a DOR user, I want to indicate that a bill requires legislative implementation and notify an L&P manager, so that implementation planning can begin.
- Acceptance criteria:
  1. An applicable bill can be marked as requiring legislative implementation.
  2. An L&P manager is notified.
  3. The indication remains associated with the bill.
- Open question / clarification: What event determines that a bill has entered the legislative implementation process?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-12.1.5 - B.LNP.08

- Priority: Nice to Have
- User role: L&P Manager
- User story: As a L&P Manager, I want to assign legislative implementation tasks to staff, so that implementation responsibility is formally established.
- Acceptance criteria:
  1. An authorized L&P Manager can assign implementation tasks.
  2. Tasks can be assigned to applicable staff.
  3. Assigned responsibility can be viewed.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-12.1.6 - B.LNP.09

- Priority: Nice to Have
- User role: L&P user
- User story: As a L&P user, I want to review entire fiscal notes, so that legislative policy work can consider the complete fiscal analysis.
- Acceptance criteria:
  1. Authorized L&P users can access applicable fiscal notes.
  2. The entire fiscal note can be reviewed.
  3. Applicable security restrictions remain enforced.
- Open question / clarification: None recorded.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E13 - Executive User Experience

Epic backlog: 3 must-have, 1 nice-to-have, 4 total.

#### F13.1 - Remote and Mobile Access

Feature backlog: 2 must-have, 0 nice-to-have, 2 total.

##### US-13.1.1 - B.EXEC.01

- Priority: Must Have
- User role: Executive Division user
- User story: As a Executive Division user, I want convenient access to the solution from any location without connecting to the DOR VPN, so that I can perform legislative work wherever needed.
- Acceptance criteria:
  1. Authorized Executive users can access the solution remotely.
  2. VPN connectivity is not required.
  3. Security and authorization remain enforced.
- Open question / clarification: What authentication, conditional-access, device-management, and network-security standards apply to non-VPN access?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-13.1.2 - B.EXEC.02

- Priority: Must Have
- User role: Executive Division user
- User story: As a Executive Division user, I want to use the solution from a DOR cell phone, so that I can review legislative information while mobile.
- Acceptance criteria:
  1. The solution is usable from supported DOR cell phones.
  2. Core required Executive functions can be accessed.
  3. Security restrictions are maintained.
- Open question / clarification: Is a responsive web application sufficient, or is a native mobile application required?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F13.2 - Executive Bill View

Feature backlog: 1 must-have, 1 nice-to-have, 2 total.

##### US-13.2.1 - B.EXEC.03

- Priority: Must Have
- User role: Executive Division user
- User story: As a Executive Division user, I want the most important bill information, including analysis and fiscal notes or estimates, available on one screen, so that I can rapidly understand a bill status and impact.
- Acceptance criteria:
  1. A consolidated bill view is provided.
  2. Applicable bill analysis is accessible from that view.
  3. Applicable fiscal notes are accessible.
  4. Applicable fiscal estimates are accessible.
  5. The most commonly needed information can be obtained without navigating through multiple unrelated screens.
- Open question / clarification: What exact information constitutes the "most common, important bill information"?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### US-13.2.2 - B.EXEC.04

- Priority: Nice to Have
- User role: Executive Division user
- User story: As a Executive Division user, I want to discuss Executive work products directly on the bill page and notify participants of questions and answers, so that bill-related discussion remains connected to the underlying work.
- Acceptance criteria:
  1. Executive users can post applicable discussion on the bill page.
  2. Questions can be associated with bill analysis or fiscal work.
  3. Answers can be posted.
  4. Users can be notified when applicable questions or answers are posted.
  5. Discussion remains associated with the bill.
- Open question / clarification: Are Executive bill discussions considered part of the official record and therefore subject to retention and audit requirements?
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Source Trace Matrix

| User Story ID | Source Requirement | Priority | Phase | Epic | Feature |
| --- | --- | --- | --- | --- | --- |
| US-1.1.1 | B.COM.01 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.1 - Centralized Collaboration |
| US-1.1.2 | B.COM.08 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.1 - Centralized Collaboration |
| US-1.2.1 | B.COM.02 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.2 - Notifications |
| US-1.3.1 | B.COM.11 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.3 - Task Creation and Assignment |
| US-1.3.2 | B.COM.12 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.3 - Task Creation and Assignment |
| US-1.3.3 | B.COM.13 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.3 - Task Creation and Assignment |
| US-1.3.4 | B.COM.14 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.3 - Task Creation and Assignment |
| US-1.3.5 | B.COM.10 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.3 - Task Creation and Assignment |
| US-1.4.1 | B.COM.18 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.4 - Task Maintenance |
| US-1.4.2 | B.COM.23 | Must Have | Phase 1 | E1 - Collaborative Legislative Work Management | F1.4 - Task Maintenance |
| US-2.1.1 | B.COM.03 | Must Have | Phase 1 | E2 - Identification, Relationships, and Legislative Work Organization | F2.1 - Unique Identification |
| US-2.1.2 | B.RFA.03 | Must Have | Phase 1 | E2 - Identification, Relationships, and Legislative Work Organization | F2.1 - Unique Identification |
| US-2.2.1 | B.COM.05 | Must Have | Phase 1 | E2 - Identification, Relationships, and Legislative Work Organization | F2.2 - Work Relationships and Packages |
| US-2.2.2 | B.RFA.06 | Must Have | Phase 1 | E2 - Identification, Relationships, and Legislative Work Organization | F2.2 - Work Relationships and Packages |
| US-2.3.1 | B.COM.04 | Must Have | Phase 1 | E2 - Identification, Relationships, and Legislative Work Organization | F2.3 - Categorization |
| US-3.1.1 | B.COM.15 | Must Have | Phase 2 | E3 - Workflow, Review, Approval, and Submission | F3.1 - Configurable Workflow |
| US-3.1.2 | B.COM.19 | Must Have | Phase 2 | E3 - Workflow, Review, Approval, and Submission | F3.1 - Configurable Workflow |
| US-3.1.3 | B.COM.20 | Must Have | Phase 2 | E3 - Workflow, Review, Approval, and Submission | F3.1 - Configurable Workflow |
| US-3.2.1 | B.RFA.01 | Must Have | Phase 2 | E3 - Workflow, Review, Approval, and Submission | F3.2 - RFA Executive Review |
| US-3.2.2 | B.RFA.02 | Must Have | Phase 2 | E3 - Workflow, Review, Approval, and Submission | F3.2 - RFA Executive Review |
| US-3.2.3 | B.RFA.04 | Must Have | Phase 2 | E3 - Workflow, Review, Approval, and Submission | F3.2 - RFA Executive Review |
| US-3.2.4 | B.RFA.05 | Must Have | Phase 2 | E3 - Workflow, Review, Approval, and Submission | F3.2 - RFA Executive Review |
| US-4.1.1 | B.COM.17 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.1 - Rich-Text Authoring |
| US-4.1.2 | B.RFA.09 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.1 - Rich-Text Authoring |
| US-4.1.3 | B.RFA.10 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.1 - Rich-Text Authoring |
| US-4.1.4 | B.LNP.04 (Must-Have occurrence) | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.1 - Rich-Text Authoring |
| US-4.2.1 | B.COM.21 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.2 - Attachments and Work in Progress |
| US-4.2.2 | B.COM.22 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.2 - Attachments and Work in Progress |
| US-4.3.1 | B.COM.24 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.3 - Templates and Generated Documents |
| US-4.3.2 | B.COM.25 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.3 - Templates and Generated Documents |
| US-4.3.3 | B.COM.26 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.3 - Templates and Generated Documents |
| US-4.3.4 | B.COM.27 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.3 - Templates and Generated Documents |
| US-4.4.1 | B.COM.28 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.4 - Reuse Existing Work |
| US-4.4.2 | B.COM.29 | Must Have | Phase 2 | E4 - Content Authoring, Documents, and Templates | F4.4 - Reuse Existing Work |
| US-5.1.1 | B.COM.31 | Must Have | Phase 3 | E5 - Legislative Data, Bill Lifecycle, and Version Management | F5.1 - External Legislative Updates |
| US-5.1.2 | B.COM.32 | Must Have | Phase 3 | E5 - Legislative Data, Bill Lifecycle, and Version Management | F5.1 - External Legislative Updates |
| US-5.1.3 | B.LNP.01 | Must Have | Phase 3 | E5 - Legislative Data, Bill Lifecycle, and Version Management | F5.1 - External Legislative Updates |
| US-5.2.1 | B.COM.33 | Must Have | Phase 3 | E5 - Legislative Data, Bill Lifecycle, and Version Management | F5.2 - Version History and Comparison |
| US-5.2.2 | B.COM.35 | Must Have | Phase 3 | E5 - Legislative Data, Bill Lifecycle, and Version Management | F5.2 - Version History and Comparison |
| US-5.2.3 | B.LNP.02 | Must Have | Phase 3 | E5 - Legislative Data, Bill Lifecycle, and Version Management | F5.2 - Version History and Comparison |
| US-5.2.4 | B.COM.43 | Must Have | Phase 3 | E5 - Legislative Data, Bill Lifecycle, and Version Management | F5.2 - Version History and Comparison |
| US-6.1.1 | B.COM.09 | Must Have | Phase 3 | E6 - Search, Reporting, and Decision Support | F6.1 - Enterprise Search |
| US-6.2.1 | B.COM.38 | Must Have | Phase 3 | E6 - Search, Reporting, and Decision Support | F6.2 - Standard and Ad Hoc Reporting |
| US-6.2.2 | B.COM.39 | Must Have | Phase 3 | E6 - Search, Reporting, and Decision Support | F6.2 - Standard and Ad Hoc Reporting |
| US-6.2.3 | B.COM.40 | Must Have | Phase 3 | E6 - Search, Reporting, and Decision Support | F6.2 - Standard and Ad Hoc Reporting |
| US-6.2.4 | B.COM.34 | Must Have | Phase 3 | E6 - Search, Reporting, and Decision Support | F6.2 - Standard and Ad Hoc Reporting |
| US-7.1.1 | B.COM.36 | Must Have | Phase 4 | E7 - Fiscal Analysis and Financial Data | F7.1 - Fiscal Data Integration |
| US-7.1.2 | B.COM.37 | Must Have | Phase 4 | E7 - Fiscal Analysis and Financial Data | F7.1 - Fiscal Data Integration |
| US-7.2.1 | B.RFA.07 | Must Have | Phase 4 | E7 - Fiscal Analysis and Financial Data | F7.2 - RFA Supporting Analysis |
| US-7.2.2 | B.RFA.08 | Must Have | Phase 4 | E7 - Fiscal Analysis and Financial Data | F7.2 - RFA Supporting Analysis |
| US-8.1.1 | B.COM.30 | Must Have | Phase 4 | E8 - Microsoft 365 and External System Integration | F8.1 - Productivity Suite Integration |
| US-9.1.1 | B.COM.06 | Must Have | Phase 5 | E9 - Security, Access, Performance, and Operations | F9.1 - Role-Based Security |
| US-9.1.2 | B.COM.16 | Must Have | Phase 5 | E9 - Security, Access, Performance, and Operations | F9.1 - Role-Based Security |
| US-9.2.1 | B.COM.07 | Must Have | Phase 5 | E9 - Security, Access, Performance, and Operations | F9.2 - Concurrent Use and Availability |
| US-9.2.2 | B.COM.42 | Must Have | Phase 5 | E9 - Security, Access, Performance, and Operations | F9.2 - Concurrent Use and Availability |
| US-10.1.1 | B.COM.41 | Must Have | Phase 5 | E10 - Data Migration and Historical Records | F10.1 - Legacy Migration |
| US-10.2.1 | B.EXP.01 | Must Have | Phase 5 | E10 - Data Migration and Historical Records | F10.2 - Financial Historical Reference |
| US-11.1.1 | B.LNP.03 | Must Have | Phase 6 | E11 - L&P Legislative Analysis and Correspondence | F11.1 - Correspondence Tracking |
| US-12.1.1 | B.LNP.04 (Nice-to-Have occurrence) | Nice to Have | Phase 6 | E12 - Legislative Implementation Management | F12.1 - Implementation Task Management |
| US-12.1.2 | B.LNP.05 | Nice to Have | Phase 6 | E12 - Legislative Implementation Management | F12.1 - Implementation Task Management |
| US-12.1.3 | B.LNP.06 | Nice to Have | Phase 6 | E12 - Legislative Implementation Management | F12.1 - Implementation Task Management |
| US-12.1.4 | B.LNP.07 | Nice to Have | Phase 6 | E12 - Legislative Implementation Management | F12.1 - Implementation Task Management |
| US-12.1.5 | B.LNP.08 | Nice to Have | Phase 6 | E12 - Legislative Implementation Management | F12.1 - Implementation Task Management |
| US-12.1.6 | B.LNP.09 | Nice to Have | Phase 6 | E12 - Legislative Implementation Management | F12.1 - Implementation Task Management |
| US-13.1.1 | B.EXEC.01 | Must Have | Phase 6 | E13 - Executive User Experience | F13.1 - Remote and Mobile Access |
| US-13.1.2 | B.EXEC.02 | Must Have | Phase 6 | E13 - Executive User Experience | F13.1 - Remote and Mobile Access |
| US-13.2.1 | B.EXEC.03 | Must Have | Phase 6 | E13 - Executive User Experience | F13.2 - Executive Bill View |
| US-13.2.2 | B.EXEC.04 | Nice to Have | Phase 6 | E13 - Executive User Experience | F13.2 - Executive Bill View |
| US-14.1.1 | B.BGT.01 | Must Have | Phase 4 | E14 - Budget Office Fiscal Inputs | F14.1 - Expense Estimate Management |
