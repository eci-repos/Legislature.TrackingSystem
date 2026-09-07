# Legislative Tracking System (LTS) — End-User Glossary

A plain-language reference for the vocabulary used across the application. Terms are grouped by topic; status values are listed in tables so you can read any badge or dropdown at a glance.

> This glossary is for end users. It describes what each term means in the application, not the internal implementation.

---

## 1. Core concepts & business areas

| Term | Description |
| --- | --- |
| **Department of Revenue (DOR)** | The state agency that uses the Legislative Tracking System to track legislation and produce fiscal analysis. |
| **Legislative Tracking System (LTS)** | The application described in this guide. It tracks bills, the work DOR performs on them, review and approval, and delivery of fiscal products. |
| **Research and Fiscal Analysis (RFA)** | The DOR business area that prepares fiscal notes, fiscal estimates, and bill analyses. |
| **Legislation & Policy (L&P)** | The DOR business area that tracks legislation, correspondence, and implementation of enacted laws. |
| **Budget & Fiscal Services (B&FS) / Budget Office** | The DOR business area that manages budget bills, expense estimates, and fiscal data. |
| **Biennium** | A two-year legislative period. Bills are tracked by the biennium they belong to (for example **2025-2026**). |
| **Session** | A legislative session, usually aligned to a biennium. Demographic data is stored by session. |
| **Source requirement** | The original DOR business requirement a feature or work item traces back to (for example **US-1.3.1**, **B.COM.11**). Shown on work items as traceability. |

---

## 2. Legislative objects

| Term | Description |
| --- | --- |
| **Bill** | A proposed law tracked by the system. Each bill has a number (for example **HB 1200**), a title, a status, a current version, and a biennium. |
| **Bill number** | The unique identifier of a bill (for example **HB 1200**, **SB 88**). "HB" = House Bill, "SB" = Senate Bill. |
| **Bill status** | The legislative lifecycle stage of a bill (see the Bill status table below). |
| **Bill version** | A snapshot of a bill's language at a point in time (for example **Original**, **Substitute**, **Engrossed**). Prior versions are retained so history can be compared. |
| **Amendment** | A proposed or unadopted change to a bill, tracked with its own number and language. |
| **Bill language** | The text of a bill version. |
| **Budget bill** | A bill flagged as part of the DOR budget, so its fiscal notes can be compared. |
| **Requires implementation** | A flag on a bill indicating that, if enacted, DOR must perform implementation work (see **Implementation task**). |

### Bill status values

| Value | Meaning |
| --- | --- |
| **Introduced** | The bill has been introduced in the legislature. |
| **InCommittee** | The bill is being considered by a committee. |
| **PassedHouse** | The bill passed the House of Representatives. |
| **PassedSenate** | The bill passed the Senate. |
| **Enacted** | The bill became law. |
| **Vetoed** | The bill was vetoed. |
| **Dead** | The bill is no longer being considered. |

---

## 3. Work items & work products

| Term | Description |
| --- | --- |
| **Work item / work task** | A unit of legislative work DOR performs, such as preparing a fiscal note or analyzing a bill. Each has an identifier, type, title, priority, status, owner, and due date. |
| **Work product** | A work item that produces authored content (for example a bill analysis or hearing report). |
| **Identifier** | The unique code assigned to a work item (for example **LTS-FiscalNote-0AF1983E**). |
| **Work type** | The kind of work item (see the Work type table below). |
| **Priority** | How urgent a work item is (see the Priority table below). |
| **Status** | The lifecycle stage of a work item (see the Work item status table below). |
| **Owner** | The DOR user responsible for a work item. |
| **Due date** | The date by which a work item should be completed. |
| **Customer due date** | A due date set for the customer/requestor of a work product; it stays with the product through its workflow. |
| **Assignment** | A record of a DOR user assigned to a work item in a specific role (see the Assignment role table below). |
| **Work queue** | The list of work items assigned to a particular user. |
| **Categorization** | Flags on a work item used for sorting and filtering: **Confidential** and **Executive review**. |
| **Confidential** | A flag marking a work item as sensitive; access can be restricted. |
| **Executive review** | A flag marking a work item that goes through the RFA Executive Review path. |
| **Content** | The authored text of a work product. Saving content does not require completion or approval. |
| **Attachment** | A document attached to a work item. |
| **Comment** | A collaboration note on a work item. |
| **Step / subtask** | A smaller task within a work product, each with its own due date. |
| **Relationship** | A link between two work items, for example by topic, document type, or legislative identifier (bill number). |
| **Reuse content** | Copying the content of one work product into another without copy-and-paste, so the destination remains independently identifiable. |

### Work type values

| Value | Description |
| --- | --- |
| **Task** | A general work item. |
| **BillAnalysis** | An analysis of a bill. |
| **FiscalNote** | A fiscal note estimating the fiscal impact of a bill. |
| **FiscalEstimate** | A detailed fiscal estimate for a bill. |
| **DataRequest** | A request for data (for example prior-year revenue). |
| **WorkProduct** | A general authored work product (for example a hearing report). |
| **Package** | A named deliverable combining work products. |
| **Document** | A document. |

### Priority values

| Value | Meaning |
| --- | --- |
| **Low** | Low urgency. |
| **Normal** | Normal urgency. |
| **High** | High urgency. |
| **Critical** | Critical urgency; needs attention first. |

### Work item status values

| Value | Meaning |
| --- | --- |
| **Proposed** | The work item has been proposed but not yet assigned. |
| **Assigned** | The work item has been assigned to a user. |
| **InProgress** | Work is underway. |
| **OnHold** | Work is paused. |
| **Submitted** | The work item has been submitted (for example for review). |
| **Approved** | The work item has been approved. |
| **Canceled** | The work item has been canceled. |
| **Completed** | The work item is complete. |

### Assignment role values

| Value | Meaning |
| --- | --- |
| **Owner** | The user responsible for the work item. |
| **Analyst** | A user performing the analysis/work. |
| **Reviewer** | A user reviewing the work. |
| **Approver** | A user approving the work. |
| **ExecutiveReviewer** | A user in the RFA Executive Review path. |

---

## 4. Workflow, review & approval

| Term | Description |
| --- | --- |
| **Workflow** | The review-and-approval path a work item follows: Draft → Pending Review → Under Review → Approved/Rejected → Finalized. |
| **Workflow status** | The current stage of a work item in the workflow (see the Workflow status table below). |
| **Submit for review** | Sending a work item for review with a set of required reviewers. |
| **Reviewer** | A user who records an approve/reject decision on a work item. |
| **Separation of duties** | A rule that at least one reviewer must not be a user who performed the work. |
| **Review decision** | A reviewer's **Approved** or **Rejected** decision on a work item. |
| **Finalize** | Marking an approved work item as final so it can be packaged and submitted. Only approved items can be finalized. |
| **Required reviewers** | The reviewers who must approve before a work item is approved. |

### Workflow status values

| Value | Meaning |
| --- | --- |
| **Draft** | The work item is being prepared and has not been submitted for review. |
| **PendingReview** | The work item has been submitted and is awaiting reviewer decisions. |
| **UnderReview** | Some reviewers have approved but not all required reviewers have. |
| **Approved** | All required reviewers have approved. |
| **Rejected** | A reviewer rejected the work item; it can be revised and resubmitted. |
| **Finalized** | The approved work item has been finalized for packaging and submission. |

---

## 5. RFA Executive Review

| Term | Description |
| --- | --- |
| **Executive review** | A governed review path for fiscal products, restricted to designated executive reviewers. |
| **Executive review status** | The overall state of the executive review path (see the table below). |
| **Executive reviewer** | A designated user who reviews a fiscal product in sequence. |
| **Reviewer step** | One executive reviewer's turn in the sequential path. |
| **Adjustment note** | A correction or note recorded by an executive reviewer. |
| **Begin / Complete step** | Starting and finishing an executive reviewer's step; when the last reviewer completes, the path is complete. |

### Executive review status values

| Value | Meaning |
| --- | --- |
| **NotStarted** | Executive review has not begun. |
| **InProgress** | Executive review is underway. |
| **Completed** | All executive reviewers have completed their steps. |

### Executive reviewer step values

| Value | Meaning |
| --- | --- |
| **Pending** | The reviewer's step has not started. |
| **InProgress** | The reviewer is working on their step. |
| **Completed** | The reviewer finished their step. |

---

## 6. Packages

| Term | Description |
| --- | --- |
| **Package** | A named deliverable that combines work products for submission. |
| **Package member** | A work product included in a package. |
| **Recipient** | A person or group the package is delivered to. |
| **Internal recipient** | A recipient inside DOR. |
| **External recipient** | A recipient outside DOR (for example a legislative committee). |
| **Finalize package** | Marking a package as final for submission. A package can only be finalized once every member work product is approved. |

### Package status values

| Value | Meaning |
| --- | --- |
| **Draft** | The package is being assembled. |
| **InProgress** | The package is being worked on. |
| **Finalized** | The package is final and ready for submission. |
| **Delivered** | The package has been delivered. |
| **Canceled** | The package was canceled. |

---

## 7. Fiscal data & budget

| Term | Description |
| --- | --- |
| **Fiscal data** | A data point (FTE, cost rule, revenue fund, or revenue source) retrieved or calculated from DOR systems. |
| **FTE** | Full-time equivalent — a measure of staffing. |
| **Cost rule** | A rule or value used to estimate costs. |
| **Revenue fund** | A fund that receives revenue. |
| **Revenue source** | A source of revenue (for example sales tax). |
| **Fiscal work paper** | Supporting documentation for how a fiscal task was completed. |
| **Expense estimate element** | An auditable cost element entered by Budget Office users (goods/services or a percentage of salary). |
| **Demographic data** | Population and related data stored by legislative session, used to compare trends. |
| **Fiscal note link** | An association between a fiscal note (work product) and a bill. |

### Fiscal data category values

| Value | Meaning |
| --- | --- |
| **Fte** | Full-time-equivalent staffing data. |
| **CostRule** | Cost estimation rules. |
| **RevenueFund** | Revenue fund data. |
| **RevenueSource** | Revenue source data. |

### Expense estimate element kind values

| Value | Meaning |
| --- | --- |
| **GoodsServices** | Cost of goods and services. |
| **SalaryPercentage** | A percentage of salary. |

---

## 8. Users, roles & permissions

| Term | Description |
| --- | --- |
| **User account** | A DOR user with a display name, a role, and an active/inactive state. |
| **User key** | The unique identifier of a user (for example **jdoe**). |
| **Role** | The functional role a user holds; it determines the permissions the user has (see the Role table below). |
| **Permission** | A discrete activity a user may perform (see the Permission table below). |
| **Access restriction** | A rule denying a user type access to a portion of a work item (for example content or attachments). |

### Role values

| Value | Meaning |
| --- | --- |
| **SecurityAdministrator** | Manages users, roles, and access. |
| **Analyst** | Prepares fiscal notes, estimates, and analyses. |
| **Reviewer** | Reviews work items. |
| **Approver** | Approves work items. |
| **ExecutiveReviewer** | Reviews fiscal products in the executive review path. |
| **FinancialUser** | Works with fiscal and budget data. |
| **ReadOnly** | Can view but not change data. |

### Permission values

| Value | Meaning |
| --- | --- |
| **Prepare** | Create and prepare work products. |
| **Approve** | Approve work items. |
| **Deliver** | Deliver packages. |
| **ReadOnly** | View data only. |
| **Administer** | Administer the system. |
| **ManageAccess** | Manage access restrictions. |
| **Migrate** | Run legacy data migration. |
| **ViewHistorical** | View historical reference data. |

---

## 9. Other features

| Term | Description |
| --- | --- |
| **Search** | Find bills, work items, and other content by keyword. |
| **Template** | A reusable document template with merge fields, used to generate documents. |
| **Generated document** | A document produced from a template and work-product data. |
| **Correspondence** | A communication sent to a recipient and tracked for a response, linked to a bill or work item. |
| **Implementation task** | A task assigned to a DOR division to implement an enacted law. |
| **Notification** | An in-app message to a user (for example an assignment or a review request). |
| **Custom report** | A saved custom query or report. |
| **Email dispatch** | A recorded email sent to a stakeholder. |
| **Migration batch** | A repeatable batch of legacy data migrated into the system. |
| **Migration record** | A single legacy record within a migration batch. |
| **Historical reference** | Viewing work products across a historical period. |
| **Shared document** | A document shared by implementation-plan collaborators. |

### Notification type values

| Value | Meaning |
| --- | --- |
| **Assignment** | A user was assigned to a work item. |
| **BillChange** | A bill changed. |
| **General** | A general notification. |

### Notification trigger values

| Value | Meaning |
| --- | --- |
| **Assignment** | Triggered by an assignment. |
| **BillChange** | Triggered by a bill change. |
| **ReviewRequested** | Triggered by a review request. |
| **ReviewCompleted** | Triggered by a review completing. |
| **General** | General trigger. |

### Implementation task status values

| Value | Meaning |
| --- | --- |
| **Assigned** | The implementation task has been assigned. |
| **InProgress** | Implementation work is underway. |
| **Completed** | The implementation task is complete. |

### Migration batch status values

| Value | Meaning |
| --- | --- |
| **Pending** | The batch has not run. |
| **Completed** | The batch completed. |
| **Failed** | The batch failed. |
| **RolledBack** | The batch was rolled back. |

### Migration record status values

| Value | Meaning |
| --- | --- |
| **Imported** | The record was imported successfully. |
| **Failed** | The record failed to import. |

---

## 10. Relationship types

| Value | Meaning |
| --- | --- |
| **Topic** | Work items linked by topic. |
| **DocumentType** | Work items linked by document type. |
| **LegislativeIdentifier** | Work items linked by a unique legislative identifier (for example a bill number). |
| **Package** | Work items linked to a named package. |
| **BillVersion** | Work items linked by bill version progression. |
