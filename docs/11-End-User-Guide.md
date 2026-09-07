# LTS Web Application - End-User Guide (Draft)

Status: draft

Last updated: 2026-09-06

## 1. Introduction

The **Legislative Tracking System (LTS)** is a web application that helps Department of Revenue (DOR) staff track legislation, fiscal work products, collaboration, review and approval, document handling, search, reporting, and executive access. This guide describes how to use the web application.

> **Draft note:** This is a draft end-user guide for the proof of concept (POC). Some areas are still being refined, and the exact wording and screens may change before final release. See Section 9 for known limitations.

> **Vocabulary:** For a plain-language reference to the terms used across the application (bills, work items, workflow, packages, roles, status values, and more), see the [End-User Glossary](12-End-User-Glossary.md).

## 2. Getting Started

### 2.1 Signing in

- Open the LTS web application in your browser.
- Sign in with your DOR account. The application uses your organization's identity provider (Entra ID) for authentication.
- After signing in, you land on the **Home** page.

### 2.2 Navigation

The left-hand navigation menu groups the application into areas. The menu items you see depend on your role and permissions. Common areas include:

| Menu item | Purpose |
| --- | --- |
| Home | Landing page and system overview |
| Work Intake | Create new work items |
| Work Queue | See work assigned to you |
| Packages | Group work products into deliverables |
| Work Items | Browse, sort, filter, and group all work |
| Bills | Track bills and their versions/amendments |
| Search | Find work across the system |
| Reports | Run standard and custom reports |
| Fiscal | Manage fiscal data and work papers |
| Budget Bills | Flag and track budget bills |
| Demographics | Manage demographic data |
| Productivity | Productivity and integration records |
| Security | Manage users and access (administrators) |
| Migration | Import legacy data (migration users) |
| Historical | View historical references |
| Correspondence | Record correspondence |
| Implementation | Track implementation tasks |
| Executive | Executive review and discussion |
| Notifications | View your notifications |

## 3. Working with Work Items

### 3.1 Creating a work item (Work Intake)

1. Open **Work Intake**.
2. Enter a **title** for the work item.
3. Select the **type** (for example, Fiscal Note, Bill Analysis, Fiscal Estimate, Data Request, Work Product).
4. Set the **priority**, **status**, and optional **due date** and **owner**.
5. Enter the **source trace** fields (Story ID, Requirement ID, Requirement Type, Source Document) so the work can be traced to its source.
6. Submit. The system assigns a unique **identifier** (for example, `LTS-FN-...`) and confirms the new work item.

### 3.2 Viewing your work (Work Queue)

- Open **Work Queue** and enter your user key to see the work assigned to you.
- The queue shows each item's **status**, **priority**, your **role**, the **per-assignment due date**, and a **rework** indicator where applicable.

### 3.3 Browsing and organizing work (Work Items)

- Open **Work Items** to see all work.
- **Sort** by Title, Type, Priority, Status, Due Date, or Created At (ascending or descending).
- **Filter** by Confidential, Executive Review, On Hold status, Work Type, or Package.
- **Group** by Type, Status, Package, Confidential, or Executive Review to organize the list.

### 3.4 Linking related work

- Related work can be linked by **Legislative Identifier**, **Topic**, **Document Type**, or **Package**.
- From either side of a link you can see the complete context of a work item.

## 4. Review and Approval

### 4.1 Submitting for review

- From a work item, submit it for review and enter the **reviewer keys** (the users who must approve it).
- The system validates that the required number of reviewers for the work type is met.
- Each reviewer receives a **notification** that a review has been requested.

### 4.2 Recording a decision

- A reviewer opens the work item and records a decision (**Approve** or **Reject**) with an optional comment.
- When all required reviewers have approved, the work item moves to **Approved**.
- A rejection moves the work item to **Rejected** with the reviewer's comment retained.

### 4.3 Finalizing

- Only **Approved** work items can be **finalized** for packaging and submission.

### 4.4 Executive review

- For work under executive review, authorized users can **start** an executive review, **begin** a review step, **adjust** (add a note), and **complete** a review step.
- Executive reviewers are ordered, and each step can be completed in sequence.

## 5. Authoring and Documents

### 5.1 Authoring content

- Open the **Authoring** area to write the content of a work product.
- Most work types use a rich-text editor; fiscal notes use a limited editor.
- You can **save incomplete work** without completing or approving it.

### 5.2 Attachments

- Attach files to a work item. Each attachment records its file name, content type, and size.

### 5.3 Templates and generated documents

- Open **Templates** to manage reusable document templates.
- A template body can contain **merge fields** such as `{{Title}}`, `{{Identifier}}`, and `{{Type}}`.
- Generate a document from a template for a work item; the system fills in the merge fields from the work item's data.

### 5.4 Reuse

- Reuse content from one work item in another without copy-and-paste.

## 6. Search and Reporting

### 6.1 Search

- Open **Search** to find work across the system by keyword and other criteria.

### 6.2 Reports

- Open **Reports** to run **standard reports**.
- Authorized users can create and run **custom reports** by name, owner, and query.

## 7. Fiscal, Budget, and Data

### 7.1 Fiscal

- Manage **fiscal data** (categories, names, values, units) and **fiscal work papers**.
- Manage **expense estimate elements** and link **fiscal notes** to work tasks.

### 7.2 Budget Bills

- Flag bills as **budget bills** and track them.

### 7.3 Demographics

- Manage **demographic data** by session and category.

## 8. Notifications

- Open **Notifications** to see messages directed to you.
- Notifications are generated when work is **assigned** to you, when a **review is requested**, and for other events.
- Mark a notification as **read** when you have acted on it.

## 9. Known Limitations (POC)

- **Data persistence:** In the POC, data may be held in memory and reset when the application restarts. In a production deployment, data is stored in PostgreSQL.
- **Authentication:** The POC may use a development sign-in; in production, sign-in uses the organization's Entra ID identity provider.
- **External integrations:** External connectors are configured for live use in production; the POC may use development substitutes.
- **User acceptance:** This guide has not yet been validated by real DOR users against real data.

## 10. Getting Help

- For access or permission issues, contact your system administrator.
- For questions about a specific work item, contact the work item's owner or your supervisor.
