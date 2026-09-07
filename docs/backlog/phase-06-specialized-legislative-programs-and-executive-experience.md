# Backlog - Phase 6: Specialized Legislative Programs and Executive Experience

Status: proposed

Last updated: 2026-09-04

## Planning Objective

Add L&P correspondence, legislative implementation management, remote/mobile access, executive bill view, and executive discussion.

Backlog size: 4 must-have, 7 nice-to-have, 11 total.

Development path: Specialized capability expansion.

## Technical Baseline

- ASP.NET Core / Blazor WebAssembly POC stack
- Modular monolith with Domain, Application, Infrastructure, Web, and Tests boundaries
- Versioned API and UI behavior traceable to source story IDs
- Unit/API/component tests for promoted acceptance criteria
- Documentation updates in sprint, handoff, and traceability records
- PM Validation report after completed and verified sprint and/or promoted use case scope

## Backlog Items

### E11 - L&P Legislative Analysis and Correspondence

#### F11.1 - Correspondence Tracking

##### BI-US-11.1.1

- Source user story: US-11.1.1
- Source requirement: B.LNP.03
- Priority: Must Have
- User role: L&P user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a L&P user, I want to track correspondence sent to recipients and whether responses were received, so that bill-related communications can be followed through completion.
- Acceptance criteria:
  1. A sent correspondence item can be recorded.
  2. The recipient can be identified.
  3. The system can indicate whether a response was received.
  4. Correspondence remains connected with applicable legislative work.
- Open question / clarification: Must email correspondence be automatically captured from Outlook, manually logged, or both?
- Development notes:
  - Capture correspondence recipient, sent state, response state, and linked bill/work context.
  - Add or update tests that assert the promoted acceptance criteria for `US-11.1.1`.
  - Preserve traceability to `B.LNP.03` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E12 - Legislative Implementation Management

#### F12.1 - Implementation Task Management

##### BI-US-12.1.1

- Source user story: US-12.1.1
- Source requirement: B.LNP.04 (Nice-to-Have occurrence)
- Priority: Nice to Have
- User role: L&P user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a L&P user, I want legislative implementation tasks assigned and reassigned across the agency, so that responsibility for implementing enacted legislation is clearly tracked.
- Acceptance criteria:
  1. Implementation tasks can be assigned to individuals across DOR.
  2. Tasks can be reassigned.
  3. The responsible individual division can be identified.
  4. Required work can be identified.
  5. Due dates can be tracked.
  6. Completion can be tracked.
- Open question / clarification: The source uses B.LNP.04 twice. What identifier should replace this Nice-to-Have occurrence?
- Development notes:
  - Keep implementation tasks distinct from pre-enactment legislative work tasks.
  - Add or update tests that assert the promoted acceptance criteria for `US-12.1.1`.
  - Preserve traceability to `B.LNP.04 (Nice-to-Have occurrence)` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-12.1.2

- Source user story: US-12.1.2
- Source requirement: B.LNP.05
- Priority: Nice to Have
- User role: user collaborating on a legislative implementation plan
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a user collaborating on a legislative implementation plan, I want to share related documents through an interface that displays the required implementation information, so that participants can coordinate from one location.
- Acceptance criteria:
  1. Implementation-plan collaborators can share related documents.
  2. Shared documents remain associated with applicable implementation work.
  3. Required implementation information is displayed in the interface.
- Open question / clarification: B.LNP.05 says "the information above, in B.LNP.06," although B.LNP.06 follows it and specifies a status report. What exact information must be displayed?
- Development notes:
  - Keep implementation tasks distinct from pre-enactment legislative work tasks.
  - Add or update tests that assert the promoted acceptance criteria for `US-12.1.2`.
  - Preserve traceability to `B.LNP.05` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-12.1.3

- Source user story: US-12.1.3
- Source requirement: B.LNP.06
- Priority: Nice to Have
- User role: L&P user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a L&P user, I want a report showing the status of legislative implementation tasks, so that implementation progress can be monitored.
- Acceptance criteria:
  1. The solution can generate an implementation-task status report.
  2. Applicable task status information is included.
  3. Authorized users can access the report.
- Open question / clarification: What fields must appear in the legislative-implementation status report?
- Development notes:
  - Keep implementation tasks distinct from pre-enactment legislative work tasks.
  - Add or update tests that assert the promoted acceptance criteria for `US-12.1.3`.
  - Preserve traceability to `B.LNP.06` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-12.1.4

- Source user story: US-12.1.4
- Source requirement: B.LNP.07
- Priority: Nice to Have
- User role: DOR user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a DOR user, I want to indicate that a bill requires legislative implementation and notify an L&P manager, so that implementation planning can begin.
- Acceptance criteria:
  1. An applicable bill can be marked as requiring legislative implementation.
  2. An L&P manager is notified.
  3. The indication remains associated with the bill.
- Open question / clarification: What event determines that a bill has entered the legislative implementation process?
- Development notes:
  - Keep implementation tasks distinct from pre-enactment legislative work tasks.
  - Add or update tests that assert the promoted acceptance criteria for `US-12.1.4`.
  - Preserve traceability to `B.LNP.07` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-12.1.5

- Source user story: US-12.1.5
- Source requirement: B.LNP.08
- Priority: Nice to Have
- User role: L&P Manager
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a L&P Manager, I want to assign legislative implementation tasks to staff, so that implementation responsibility is formally established.
- Acceptance criteria:
  1. An authorized L&P Manager can assign implementation tasks.
  2. Tasks can be assigned to applicable staff.
  3. Assigned responsibility can be viewed.
- Open question / clarification: None recorded.
- Development notes:
  - Keep implementation tasks distinct from pre-enactment legislative work tasks.
  - Add or update tests that assert the promoted acceptance criteria for `US-12.1.5`.
  - Preserve traceability to `B.LNP.08` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-12.1.6

- Source user story: US-12.1.6
- Source requirement: B.LNP.09
- Priority: Nice to Have
- User role: L&P user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a L&P user, I want to review entire fiscal notes, so that legislative policy work can consider the complete fiscal analysis.
- Acceptance criteria:
  1. Authorized L&P users can access applicable fiscal notes.
  2. The entire fiscal note can be reviewed.
  3. Applicable security restrictions remain enforced.
- Open question / clarification: None recorded.
- Development notes:
  - Keep implementation tasks distinct from pre-enactment legislative work tasks.
  - Add or update tests that assert the promoted acceptance criteria for `US-12.1.6`.
  - Preserve traceability to `B.LNP.09` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

### E13 - Executive User Experience

#### F13.1 - Remote and Mobile Access

##### BI-US-13.1.1

- Source user story: US-13.1.1
- Source requirement: B.EXEC.01
- Priority: Must Have
- User role: Executive Division user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a Executive Division user, I want convenient access to the solution from any location without connecting to the DOR VPN, so that I can perform legislative work wherever needed.
- Acceptance criteria:
  1. Authorized Executive users can access the solution remotely.
  2. VPN connectivity is not required.
  3. Security and authorization remain enforced.
- Open question / clarification: What authentication, conditional-access, device-management, and network-security standards apply to non-VPN access?
- Development notes:
  - Design responsive access and identity boundaries before mobile-specific refinements.
  - Add or update tests that assert the promoted acceptance criteria for `US-13.1.1`.
  - Preserve traceability to `B.EXEC.01` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-13.1.2

- Source user story: US-13.1.2
- Source requirement: B.EXEC.02
- Priority: Must Have
- User role: Executive Division user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a Executive Division user, I want to use the solution from a DOR cell phone, so that I can review legislative information while mobile.
- Acceptance criteria:
  1. The solution is usable from supported DOR cell phones.
  2. Core required Executive functions can be accessed.
  3. Security restrictions are maintained.
- Open question / clarification: Is a responsive web application sufficient, or is a native mobile application required?
- Development notes:
  - Design responsive access and identity boundaries before mobile-specific refinements.
  - Add or update tests that assert the promoted acceptance criteria for `US-13.1.2`.
  - Preserve traceability to `B.EXEC.02` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

#### F13.2 - Executive Bill View

##### BI-US-13.2.1

- Source user story: US-13.2.1
- Source requirement: B.EXEC.03
- Priority: Must Have
- User role: Executive Division user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a Executive Division user, I want the most important bill information, including analysis and fiscal notes or estimates, available on one screen, so that I can rapidly understand a bill status and impact.
- Acceptance criteria:
  1. A consolidated bill view is provided.
  2. Applicable bill analysis is accessible from that view.
  3. Applicable fiscal notes are accessible.
  4. Applicable fiscal estimates are accessible.
  5. The most commonly needed information can be obtained without navigating through multiple unrelated screens.
- Open question / clarification: What exact information constitutes the "most common, important bill information"?
- Development notes:
  - Make executive bill view read-optimized and source-linked.
  - Add or update tests that assert the promoted acceptance criteria for `US-13.2.1`.
  - Preserve traceability to `B.EXEC.03` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

##### BI-US-13.2.2

- Source user story: US-13.2.2
- Source requirement: B.EXEC.04
- Priority: Nice to Have
- User role: Executive Division user
- Planning status: Promoted and implemented in Sprint 14.
- User story: As a Executive Division user, I want to discuss Executive work products directly on the bill page and notify participants of questions and answers, so that bill-related discussion remains connected to the underlying work.
- Acceptance criteria:
  1. Executive users can post applicable discussion on the bill page.
  2. Questions can be associated with bill analysis or fiscal work.
  3. Answers can be posted.
  4. Users can be notified when applicable questions or answers are posted.
  5. Discussion remains associated with the bill.
- Open question / clarification: Are Executive bill discussions considered part of the official record and therefore subject to retention and audit requirements?
- Development notes:
  - Make executive bill view read-optimized and source-linked.
  - Add or update tests that assert the promoted acceptance criteria for `US-13.2.2`.
  - Preserve traceability to `B.EXEC.04` in implementation notes, tests, seed data, or UI/API metadata as appropriate.
- Source document: Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System

## Phase Trace Matrix

| Backlog Item | User Story | Source Requirement | Priority | Feature |
| --- | --- | --- | --- | --- |
| BI-US-11.1.1 | US-11.1.1 | B.LNP.03 | Must Have | F11.1 - Correspondence Tracking |
| BI-US-12.1.1 | US-12.1.1 | B.LNP.04 (Nice-to-Have occurrence) | Nice to Have | F12.1 - Implementation Task Management |
| BI-US-12.1.2 | US-12.1.2 | B.LNP.05 | Nice to Have | F12.1 - Implementation Task Management |
| BI-US-12.1.3 | US-12.1.3 | B.LNP.06 | Nice to Have | F12.1 - Implementation Task Management |
| BI-US-12.1.4 | US-12.1.4 | B.LNP.07 | Nice to Have | F12.1 - Implementation Task Management |
| BI-US-12.1.5 | US-12.1.5 | B.LNP.08 | Nice to Have | F12.1 - Implementation Task Management |
| BI-US-12.1.6 | US-12.1.6 | B.LNP.09 | Nice to Have | F12.1 - Implementation Task Management |
| BI-US-13.1.1 | US-13.1.1 | B.EXEC.01 | Must Have | F13.1 - Remote and Mobile Access |
| BI-US-13.1.2 | US-13.1.2 | B.EXEC.02 | Must Have | F13.1 - Remote and Mobile Access |
| BI-US-13.2.1 | US-13.2.1 | B.EXEC.03 | Must Have | F13.2 - Executive Bill View |
| BI-US-13.2.2 | US-13.2.2 | B.EXEC.04 | Nice to Have | F13.2 - Executive Bill View |
