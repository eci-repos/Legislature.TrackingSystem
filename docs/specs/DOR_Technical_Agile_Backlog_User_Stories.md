# DOR Technical Agile Backlog User Stories

Generated on: 2026-09-04

Source workbook: `docs/specs/DOR_Technical_Agile_Backlog_User_Stories.xlsx`

Source worksheet: `Technical Agile Backlog`

Purpose: This Markdown file converts the technical agile backlog into a sprint-planning-ready reference. It preserves technical story IDs, source requirement IDs, requirement types, acceptance criteria, conditional applicability, open questions, and source-document references from the source workbook.

Authority: The original workbook remains the source file. This Markdown file is a generated planning aid and MUST NOT replace the original `.xlsx` source.

Regeneration: Run `python tools/convert_dor_technical_backlog.py` from the repository root after the source workbook changes.

## Summary

- Technical stories converted: 94
- Technical epics converted: 17
- Sprint use: promote final implementation scope into `docs/02-Current-Sprint.md` before coding.
- Traceability: link implementation, tests, configuration, and documentation back to technical story IDs and source requirement IDs.

## Stories by Epic

### E1 - Architecture and Technology Standards

#### Solution Architecture and Technology Strategy

##### TS-1.1

- Source requirement: TR-101
- Requirement type: MS
- User role: DOR architecture evaluator
- User story: As a DOR architecture evaluator, I want the solution's codebase origin and implications clearly described, so that DOR can evaluate delivery, influence, and ownership considerations.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The vendor identifies whether the solution is a new custom build, configured existing product, or hybrid.
  2. The vendor explains the impact on delivery timeline.
  3. The vendor explains DOR's ability to influence functionality.
  4. The vendor explains how the approach affects ownership and rights under Section 4.13.
- Open question / clarification: None recorded.
- Source section: 4.1 Architecture & Technology Standards
- Source page: 4
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-1.2

- Source requirement: TR-104
- Requirement type: S
- User role: DOR architecture evaluator
- User story: As a DOR architecture evaluator, I want the vendor's supported and preferred hosting platform identified and justified, so that DOR can evaluate cost, security, and assurance over the environment.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The supported hosting platform is identified.
  2. The preferred hosting platform is identified.
  3. The choice is justified against cost.
  4. The choice is justified against security.
  5. The choice is justified against DOR's ability to gain assurance over the environment.
  6. AWS/Azure preference is addressed without assuming another platform is prohibited.
- Open question / clarification: None recorded.
- Source section: 4.1 Architecture & Technology Standards
- Source page: 4
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-1.3

- Source requirement: TR-105
- Requirement type: S
- User role: DOR technology owner
- User story: As a DOR technology owner, I want the solution to limit avoidable platform lock-in, so that DOR can maintain portability and exercise future exit rights.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The vendor describes its lock-in mitigation approach.
  2. Proprietary/nonportable services are identified where used.
  3. Reasonable portable alternatives are considered.
  4. The approach supports Section 4.13 data portability and exit provisions.
- Open question / clarification: None recorded.
- Source section: 4.1 Architecture & Technology Standards
- Source page: 4
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-1.4

- Source requirement: TR-106
- Requirement type: M
- User role: solution user
- User story: As a solution user, I want every interactive element to respond to one standard click or tap, so that I do not have to repeat an interaction or manipulate focus before the system responds.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Every interactive element responds reliably to a single standard interaction.
  2. A second click/tap is not required because the first interaction failed.
  3. An unrelated action is not required merely to establish focus.
  4. The incumbent LTS interaction defect described in the requirement is not reproduced.
- Open question / clarification: None recorded.
- Source section: 4.1 Architecture & Technology Standards
- Source page: 4
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-1.5

- Source requirement: TR-107
- Requirement type: M
- User role: DOR technology owner
- User story: As a DOR technology owner, I want all solution technologies to remain actively supported, so that DOR does not implement an already obsolete platform.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Every technology-stack component is supported by its maintainer/publisher at proposal submission.
  2. No component has reached end-of-life.
  3. No component has reached end-of-support.
- Open question / clarification: None recorded.
- Source section: 4.1 Architecture & Technology Standards
- Source page: 4
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E2 - Hosting, Infrastructure, and Environment Management

#### Hosting Governance

##### TS-2.1

- Source requirement: TR-201
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want the hosting environment to comply with applicable Washington cybersecurity requirements, so that DOR's solution operates within the required state security baseline.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The hosting environment complies with the referenced WaTech cybersecurity policy.
  2. The requirement applies whether DOR or the vendor owns the hosting environment.
  3. The environment meets at least the data-classification floor referenced by TR-308.
- Open question / clarification: TR-201 references a “data-classification floor established in TR-308,” but TR-308 is a WAF/DDoS requirement rather than a data-classification requirement. What requirement was intended?
- Source section: 4.2 Hosting, Infrastructure & Environment Management
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-2.2

- Source requirement: TR-202
- Requirement type: MS
- User role: DOR architecture evaluator
- User story: As a DOR architecture evaluator, I want the hosting ownership and operating model clearly defined, so that DOR can evaluate control, security, visibility, and long-term supportability.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The vendor identifies who owns the environment.
  2. The vendor identifies who deploys it.
  3. The vendor identifies who operates it.
  4. Infrastructure definition/change methods are described.
  5. DOR visibility and control are explained.
  6. Security and long-term supportability are addressed.
- Open question / clarification: None recorded.
- Source section: 4.2 Hosting, Infrastructure & Environment Management
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-2.3

- Source requirement: TR-204
- Requirement type: S
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want the environment access-control model based on least privilege, so that infrastructure access is appropriately restricted.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The infrastructure access-control model is described.
  2. Least privilege is explicitly addressed.
  3. The model is appropriate to the proposed operating posture.
- Open question / clarification: None recorded.
- Source section: 4.2 Hosting, Infrastructure & Environment Management
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-2.4

- Source requirement: TR-205
- Requirement type: S
- User role: DOR financial/technology owner
- User story: As a DOR financial/technology owner, I want infrastructure cost-management and resource-tagging practices described, so that technology costs can be understood and allocated.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The vendor describes cost-management practices.
  2. Resource tagging is addressed.
  3. Cost-allocation use of tagging is addressed.
- Open question / clarification: None recorded.
- Source section: 4.2 Hosting, Infrastructure & Environment Management
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

#### DOR-Operated Hosting

##### TS-2.5

- Source requirement: TR-206
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want vendor personnel excluded from DOR-owned infrastructure, so that DOR maintains exclusive operational control over its environment.
- Conditional applicability: Applies where DOR owns and provisions infrastructure.
- Acceptance criteria:
  1. Applies when DOR owns/provisions infrastructure.
  2. Vendor has no standing access.
  3. Vendor has no temporary access.
  4. This includes deployment, troubleshooting, and support.
  5. Vendor development/testing occurs only in vendor-controlled environments.
  6. Vendor uses synthetic or de-identified data in those environments.
- Open question / clarification: None recorded.
- Source section: 4.2 Hosting, Infrastructure & Environment Management
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

#### Vendor-Hosted Environment

##### TS-2.6

- Source requirement: TR-207
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want a vendor-hosted environment independently security-authorized for government use, so that DOR can rely on externally assessed controls.
- Conditional applicability: Applies where Vendor hosts the solution on an ongoing basis.
- Acceptance criteria:
  1. Applies to ongoing vendor hosting.
  2. The environment maintains GovRAMP, FedRAMP Moderate, or an equivalent independently assessed framework.
  3. All subprocessors are disclosed.
  4. Data-center locations are disclosed.
  5. DOR receives advance notice of changes.
- Open question / clarification: None recorded.
- Source section: 4.2 Hosting, Infrastructure & Environment Management
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-2.7

- Source requirement: TR-208
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want appropriate security evidence and pre-production security review support, so that security findings can be identified and remediated before go-live.
- Conditional applicability: Obligations vary according to Vendor-hosted versus DOR-operated posture.
- Acceptance criteria:
  1. Vendor-hosted solutions provide a current third-party audit report at least annually.
  2. Vendor-hosted solutions support a DOR security review before go-live.
  3. Agreed findings are remediated within an agreed timeframe.
  4. DOR-operated solutions receive documentation/support required for the WaTech Security Design Review.
  5. Vendor-delivered-code findings are remediated within an agreed timeframe.
- Open question / clarification: None recorded.
- Source section: 4.2 Hosting, Infrastructure & Environment Management
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-2.8

- Source requirement: TR-209
- Requirement type: MS
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want logical tenant-isolation controls described for a multi-tenant solution, so that another tenant cannot access DOR data.
- Conditional applicability: Applies if Vendor hosting is multi-tenant.
- Acceptance criteria:
  1. Applies when vendor hosting is multi-tenant.
  2. Logical isolation controls are described.
  3. Controls specifically address prevention of cross-tenant access to DOR data.
- Open question / clarification: None recorded.
- Source section: 4.2 Hosting, Infrastructure & Environment Management
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E3 - Security and Identity

#### Security Baseline and Cryptography

##### TS-3.1

- Source requirement: TR-301
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want the solution to comply fully with the referenced WaTech cybersecurity policy, so that state security requirements are met.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Full compliance with the cited WaTech policy is demonstrated.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-3.2

- Source requirement: TR-302
- Requirement type: M
- User role: DOR data owner
- User story: As a DOR data owner, I want data encrypted in transit and at rest, so that DOR information is protected from unauthorized disclosure.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Data in transit uses TLS 1.2 or higher.
  2. Data at rest uses AES-256 or another NIST-validated cryptographic module.
  3. Post-quantum algorithms are treated as a preference rather than a mandatory requirement.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 5
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

#### Identity and Authorization

##### TS-3.3

- Source requirement: TR-303
- Requirement type: M
- User role: DOR staff user
- User story: As a DOR staff user, I want to authenticate through DOR's Microsoft Entra ID, so that I use the state's established identity environment rather than a separate application account.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Staff authentication integrates with the WaTech-managed Entra ID tenant.
  2. OpenID Connect is supported as the preferred mechanism or SAML is supported.
  3. The solution does not maintain a standalone/custom DOR staff credential store.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-3.4

- Source requirement: TR-304
- Requirement type: MS
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want solution secrets securely managed, so that credentials and keys are not exposed through code or configuration practices.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The secrets-management approach is documented.
  2. Credentials/keys/secrets are not hardcoded.
  3. Credentials/keys/secrets are not committed to source control.
  4. Entra ID/OpenID Connect preference is addressed.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-3.5

- Source requirement: TR-305
- Requirement type: M
- User role: DOR security administrator
- User story: As a DOR security administrator, I want application access controlled through least-privilege RBAC, so that users only receive permissions appropriate to their roles.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. RBAC is implemented.
  2. RBAC applies to all application-level roles.
  3. Least privilege governs permissions.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-3.6

- Source requirement: TR-306
- Requirement type: S
- User role: DOR security evaluator
- User story: As a DOR security evaluator, I want the vendor's SSDLC described, so that I can evaluate how application vulnerabilities are prevented and detected.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. SSDLC practices are described.
  2. SAST approach/tools are identified.
  3. DAST approach/tools are identified.
  4. Automated dependency-vulnerability scanning is addressed.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-3.7

- Source requirement: TR-308
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want all solution components protected against web attacks and denial-of-service attacks, so that the service remains secure and available.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. All applicable solution components are protected by a WAF.
  2. DDoS mitigation is provided.
  3. Controls are appropriate to the hosting platform.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-3.8

- Source requirement: TR-309
- Requirement type: M
- User role: DOR security administrator
- User story: As a DOR security administrator, I want permission provisioning automated and consistent across environments, so that security behavior does not vary because of manual grants.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Role/permission provisioning is automated.
  2. It behaves identically across all environments.
  3. No environment depends on unique out-of-band permission grants.
  4. The incumbent inconsistency identified by DOR is not reproduced.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-3.9

- Source requirement: TR-310
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want the hosting environment to maintain a recognized security certification, so that the environment has an independently recognized security baseline.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Hosting maintains at least one of: ISO/IEC 27001, FedRAMP Moderate, or CSA STAR Level 2.
- Open question / clarification: None recorded.
- Source section: 4.3 Security & Identity
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E4 - Data Management

#### Data Protection, Recovery, and Retention

##### TS-4.1

- Source requirement: TR-402
- Requirement type: MS
- User role: DOR data owner
- User story: As a DOR data owner, I want development and test data handled according to the operating posture, so that DOR production information is protected.
- Conditional applicability: Behavior varies according to whether Vendor accesses/hosts DOR live data.
- Acceptance criteria:
  1. If vendor never accesses actual DOR data, development/test data is synthetic or de-identified.
  2. Such data remains structurally representative.
  3. If vendor hosts live DOR data, required IRS safeguard agreements are executed before go-live.
- Open question / clarification: The source says “Vendor shall execute any additional safeguard agreements DOR's IRS Safeguards liaison requires.” What data classification/use case makes IRS Safeguards applicable to this project?
- Source section: 4.4 Data Management
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-4.2

- Source requirement: TR-403
- Requirement type: M
- User role: DOR service owner
- User story: As a DOR service owner, I want automated backups meeting defined recovery targets, so that service and data can be recovered after a failure.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Backups are automated.
  2. RPO is no more than 4 hours.
  3. RTO is no more than 24 hours.
  4. Responsibility follows the party operating the solution.
- Open question / clarification: None recorded.
- Source section: 4.4 Data Management
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-4.3

- Source requirement: TR-404
- Requirement type: M
- User role: DOR data owner
- User story: As a DOR data owner, I want DOR data kept within the continental United States unless expressly approved otherwise, so that data residency is controlled.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. DOR data resides in continental U.S. data centers.
  2. Data is not processed outside the continental U.S. without prior written DOR approval.
  3. Data is not replicated outside the continental U.S. without prior written DOR approval.
- Open question / clarification: None recorded.
- Source section: 4.4 Data Management
- Source page: 6
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-4.4

- Source requirement: TR-405
- Requirement type: S
- User role: DOR records owner
- User story: As a DOR records owner, I want a documented retention and secure-deletion approach, so that records are managed according to applicable state and agency schedules.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. The vendor describes retention practices.
  2. Secure deletion is described.
  3. Retention aligns to applicable SGGRRS series.
  4. DOR agency-specific schedules are addressed where applicable.
- Open question / clarification: None recorded.
- Source section: 4.4 Data Management
- Source page: 6-7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E5 - Accessibility

#### Accessibility Compliance

##### TS-5.1

- Source requirement: TR-501
- Requirement type: M
- User role: user with accessibility needs
- User story: As a user with accessibility needs, I want the solution to conform to WCAG 2.2 Level AA, so that I can use the system effectively.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Solution conforms to WCAG 2.2 AA.
- Open question / clarification: None recorded.
- Source section: 4.5 Accessibility
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-5.2

- Source requirement: TR-502
- Requirement type: M
- User role: DOR accessibility stakeholder
- User story: As a DOR accessibility stakeholder, I want a current ACR/VPAT before go-live, so that accessibility conformance can be reviewed.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. ACR/VPAT is delivered before go-live.
- Open question / clarification: None recorded.
- Source section: 4.5 Accessibility
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-5.3

- Source requirement: TR-503
- Requirement type: S
- User role: DOR accessibility evaluator
- User story: As a DOR accessibility evaluator, I want the testing methodology described, so that I can assess how accessibility conformance will be validated.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Automated testing is described.
  2. Manual testing is described.
  3. Assistive-technology testing is described.
- Open question / clarification: None recorded.
- Source section: 4.5 Accessibility
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E6 - Integration and Interoperability

#### APIs and External Data Exchange

##### TS-6.1

- Source requirement: TR-601
- Requirement type: M
- User role: DOR integration engineer
- User story: As a DOR integration engineer, I want integrations exposed or consumed through documented, versioned APIs, so that interfaces remain secure and maintainable.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. RESTful APIs or equivalent are used.
  2. APIs are documented.
  3. APIs are versioned.
  4. OAuth 2.0 is used for API authentication.
- Open question / clarification: None recorded.
- Source section: 4.6 Integration & Interoperability
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-6.2

- Source requirement: xx-xxx
- Requirement type: Not shown in source
- User role: DOR integration engineer
- User story: As a DOR integration engineer, I want multiple supported mechanisms for sharing and consuming external state-system data, so that systems with differing interface capabilities can exchange information with LTS.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. SOAP/REST web services are addressed.
  2. XML exchange is addressed.
  3. CSV exchange is addressed.
  4. SharePoint is addressed.
  5. Data export is addressed.
  6. Standard BI tools are addressed.
- Open question / clarification: What stable requirement ID and requirement type should be assigned?
- Source section: 4.6 Integration & Interoperability
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-6.3

- Source requirement: TR-602
- Requirement type: MS
- User role: DOR integration engineer
- User story: As a DOR integration engineer, I want LTS integrated with all systems identified in the System Integration Inventory, so that required authoritative data can flow between systems.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Each inventory integration is supported.
  2. The specified protocol is used.
  3. The specified data contract is used.
- Open question / clarification: The System Integration Inventory is explicitly described as “not yet created.” It must be supplied before this story can be fully refined.
- Source section: 4.6 Integration & Interoperability
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-6.4

- Source requirement: TR-603
- Requirement type: S
- User role: DOR integration owner
- User story: As a DOR integration owner, I want the vendor's API documentation and versioning strategy described, so that integrations remain maintainable over time.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. API documentation approach is described.
  2. Versioning strategy is described.
  3. Long-term maintainability is addressed.
- Open question / clarification: None recorded.
- Source section: 4.6 Integration & Interoperability
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

#### SharePoint Integration

##### TS-6.5

- Source requirement: TR-604
- Requirement type: M
- User role: DOR user
- User story: As a DOR user, I want tracked-legislation documents stored and retrieved through DOR M365 SharePoint, so that legislative documents use DOR's established document environment.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Upload/storage is supported.
  2. Retrieval is supported.
  3. Bill-related documents listed in the requirement are supported.
  4. Entra ID authentication is used consistently with TR-303.
- Open question / clarification: None recorded.
- Source section: 4.6 Integration & Interoperability
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-6.6

- Source requirement: TR-605
- Requirement type: S
- User role: DOR service owner
- User story: As a DOR service owner, I want SharePoint document interactions resilient to transient throttling, so that temporary Graph/SharePoint constraints do not unnecessarily interrupt business processing.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Upload behavior is addressed.
  2. Retrieval behavior is addressed.
  3. Metadata-association behavior is addressed.
  4. Retry/backoff behavior consistent with TR-1114 is described.
- Open question / clarification: None recorded.
- Source section: 4.6 Integration & Interoperability
- Source page: 7
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-6.7

- Source requirement: TR-607
- Requirement type: S
- User role: DOR administrator
- User story: As a DOR administrator, I want SharePoint document containers provisioned automatically before a legislative session, so that bill creation does not depend on manual email requests to a SharePoint administrator.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Automated folder/library provisioning approach is described.
  2. Provisioning can occur before a new session.
  3. Manual email-dependent provisioning is eliminated by the proposed approach.
- Open question / clarification: None recorded.
- Source section: 4.6 Integration & Interoperability
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E7 - Software Engineering and Source Code

#### Source Control and Architecture

##### TS-7.1

- Source requirement: TR-701
- Requirement type: MS
- User role: DOR technology owner
- User story: As a DOR technology owner, I want source code version-controlled and periodically exported when DOR will own it, so that DOR receives a complete development history without requiring vendor access to a DOR repository.
- Conditional applicability: Periodic export obligations apply where DOR will take ownership of the code.
- Acceptance criteria:
  1. Vendor maintains source in vendor-controlled Git.
  2. If DOR owns the code, exports include commit history, branches, and tags.
  3. Exports occur at least every 30 days during development.
  4. A final export occurs at delivery.
  5. Transfer uses a DOR-approved secure method.
- Open question / clarification: None recorded.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.2

- Source requirement: xx-xxx
- Requirement type: S
- User role: DOR architecture evaluator
- User story: As a DOR architecture evaluator, I want the technology stack identified and its supportability and TCO justified, so that DOR can evaluate long-term sustainability.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Technology stack is identified for every approach.
  2. C#/.NET preference is addressed for a new custom build.
  3. Long-term supportability is justified.
  4. Total cost of ownership is addressed.
- Open question / clarification: Assign stable requirement ID.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.3

- Source requirement: xx-xxx
- Requirement type: M
- User role: DOR architecture evaluator
- User story: As a DOR architecture evaluator, I want the proposed architecture described and justified, so that its fit for scale, scope, and long-term supportability can be evaluated.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Architecture is described.
  2. Fit to solution scope is justified.
  3. Expected scale is considered.
  4. Long-term supportability is justified.
- Open question / clarification: Assign stable requirement ID.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.4

- Source requirement: xx-xxx
- Requirement type: MS occurrence
- User role: DOR release stakeholder
- User story: As a DOR release stakeholder, I want logically separate development, test, and production tiers, so that untested changes do not enter production.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Dev/test/prod are logically separated.
  2. Untested changes are not deployed directly to production.
- Open question / clarification: This exact requirement appears again on page 9 as an xx-xxx Mandatory requirement. Which type controls?
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.5

- Source requirement: TR-702
- Requirement type: S
- User role: DOR engineering evaluator
- User story: As a DOR engineering evaluator, I want coding standards and automated static controls defined, so that delivered code is consistently maintainable.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Technology-appropriate coding standard is documented.
  2. Standard is enforced.
  3. Automated linting/static analysis is included.
- Open question / clarification: None recorded.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.6

- Source requirement: TR-703
- Requirement type: M
- User role: DOR engineering stakeholder
- User story: As a DOR engineering stakeholder, I want every code change peer reviewed before merge, so that code quality and correctness receive independent review.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Every merge to main occurs through pull/merge request.
  2. Code is peer-reviewed before merge.
  3. Vendor documents the review process.
- Open question / clarification: None recorded.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.7

- Source requirement: TR-704
- Requirement type: S
- User role: DOR engineering evaluator
- User story: As a DOR engineering evaluator, I want technical-debt and code-quality practices described, so that maintainability can be assessed throughout development.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Technical-debt management is described.
  2. Code-quality metrics are described.
  3. Examples such as test coverage and complexity are addressed as applicable.
  4. Reporting throughout development is described.
- Open question / clarification: None recorded.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.8

- Source requirement: TR-705
- Requirement type: MS
- User role: DOR technology owner
- User story: As a DOR technology owner, I want the vendor to describe source-escrow options when it retains ownership and hosting, so that continuity options exist if the vendor can no longer maintain service.
- Conditional applicability: Applies where Vendor retains ownership and hosts the solution code.
- Acceptance criteria:
  1. Applies where vendor owns/hosts code.
  2. Vendor states whether escrow will be used.
  3. DOR-specific configuration is addressed where applicable.
  4. Release events include material breach, insolvency, or failure to maintain service as stated.
- Open question / clarification: None recorded.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

#### Automated Build and Deployment

##### TS-7.9

- Source requirement: TR-801
- Requirement type: M
- User role: DOR release stakeholder
- User story: As a DOR release stakeholder, I want all builds and deployments executed through CI/CD, so that releases are repeatable and controlled.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Builds are automated.
  2. Deployments are automated.
  3. Manual production deployment is prohibited except documented emergencies.
- Open question / clarification: None recorded.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.10

- Source requirement: TR-802
- Requirement type: MS
- User role: DOR DevOps owner
- User story: As a DOR DevOps owner, I want the CI/CD pipeline and testing gates documented and transferable when DOR operates it, so that DOR can run releases without vendor dependency.
- Conditional applicability: Pipeline-as-code transfer applies where DOR operates the pipeline after delivery.
- Acceptance criteria:
  1. Pipeline/tooling is described.
  2. Automated testing gates are described.
  3. When DOR operates it, pipeline definition is delivered as code compatible with DOR's environment.
- Open question / clarification: None recorded.
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 8-9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-7.11

- Source requirement: xx-xxx
- Requirement type: M occurrence
- User role: DOR release stakeholder
- User story: As a DOR release stakeholder, I want logically separate development, test, and production tiers, so that untested changes do not enter production.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Dev/test/prod are logically separated.
  2. Untested changes are not deployed directly to production.
- Open question / clarification: Is this a duplicate of TS-7.4, and should it be M or MS?
- Source section: 4.7 Software Engineering & Source Code Standards
- Source page: 9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E8 - DevOps and Release Management

#### Change and Release Management

##### TS-8.1

- Source requirement: TR-803
- Requirement type: M
- User role: DOR release stakeholder
- User story: As a DOR release stakeholder, I want production changes governed by a documented release process, so that DOR receives advance awareness and traceability of significant releases.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Change/release process is documented.
  2. DOR receives advance notice when functionality, security, or data handling could be affected.
  3. Production deployments include release notes.
- Open question / clarification: None recorded.
- Source section: 4.8 DevOps, CI/CD & Release Management
- Source page: 9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-8.2

- Source requirement: TR-804
- Requirement type: S
- User role: DOR operations stakeholder
- User story: As a DOR operations stakeholder, I want the recommended release and rollback strategy described, so that failed releases can be safely reversed.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Release strategy is described.
  2. Rollback strategy is described.
  3. Blue/green or canary approaches are addressed where applicable.
- Open question / clarification: None recorded.
- Source section: 4.8 DevOps, CI/CD & Release Management
- Source page: 9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E9 - Testing and Quality Assurance

#### Test Strategy and Acceptance

##### TS-9.1

- Source requirement: TR-901
- Requirement type: M
- User role: DOR quality stakeholder
- User story: As a DOR quality stakeholder, I want a documented test strategy, so that all required forms of testing and environment responsibilities are known.
- Conditional applicability: Section introduction says requirements are only required for Custom-Built DOR-hosted service.
- Acceptance criteria:
  1. Unit testing is covered.
  2. Integration testing is covered.
  3. System testing is covered.
  4. Regression testing is covered.
  5. Vendor and DOR-facing environment responsibilities are identified.
- Open question / clarification: None recorded.
- Source section: 4.9 Testing & Quality Assurance
- Source page: 9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-9.2

- Source requirement: TR-902
- Requirement type: MS
- User role: DOR quality stakeholder
- User story: As a DOR quality stakeholder, I want an automated unit-test coverage threshold proposed and measured, so that code quality can be objectively monitored.
- Conditional applicability: Section introduction says requirements are only required for Custom-Built DOR-hosted service.
- Acceptance criteria:
  1. Vendor proposes a minimum threshold.
  2. Measurement method is defined.
  3. Reporting to DOR throughout development is defined.
- Open question / clarification: None recorded.
- Source section: 4.9 Testing & Quality Assurance
- Source page: 9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-9.3

- Source requirement: TR-903
- Requirement type: M
- User role: DOR business tester
- User story: As a DOR business tester, I want to conduct UAT in an appropriate non-production environment with vendor defect support, so that DOR can validate the solution before go-live.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. DOR conducts UAT.
  2. Environment matches proposed operating model.
  3. Synthetic/de-identified data is used.
  4. Vendor triages defects.
  5. Vendor remediates defects.
  6. Vendor does not require standing access to DOR's environment/data.
- Open question / clarification: Section 4.9 states these requirements apply only to a DOR-hosted custom build, but TR-903 explicitly includes a vendor-hosted staging environment. Should these requirements apply to both approaches?
- Source section: 4.9 Testing & Quality Assurance
- Source page: 9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-9.4

- Source requirement: TR-904
- Requirement type: S
- User role: DOR quality evaluator
- User story: As a DOR quality evaluator, I want the performance/load-testing approach described, so that production-readiness under expected demand can be assessed.
- Conditional applicability: Section introduction says requirements are only required for Custom-Built DOR-hosted service.
- Acceptance criteria:
  1. Approach is described.
  2. Testing occurs before go-live.
  3. Synthetic/non-production data is used.
- Open question / clarification: None recorded.
- Source section: 4.9 Testing & Quality Assurance
- Source page: 9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E10 - Monitoring, Logging, and Observability

#### Operational Observability

##### TS-10.1

- Source requirement: TR-1001
- Requirement type: M
- User role: DOR operations/security user
- User story: As a DOR operations/security user, I want centralized application and infrastructure logs, so that audit and troubleshooting can be performed.
- Conditional applicability: Log-access mechanism varies according to operating posture.
- Acceptance criteria:
  1. Application logs are centralized.
  2. Infrastructure logs are centralized.
  3. Logs support audit/troubleshooting.
  4. DOR-operated logs are directly accessible to DOR.
  5. Vendor-hosted solutions provide a DOR-accessible review mechanism.
- Open question / clarification: None recorded.
- Source section: 4.10 Monitoring, Logging & Observability
- Source page: 9
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-10.2

- Source requirement: TR-1002
- Requirement type: M
- User role: DOR operations user
- User story: As a DOR operations user, I want monitoring and alerts for critical system-health indicators, so that operational problems are identified promptly.
- Conditional applicability: Alert management varies according to operating posture.
- Acceptance criteria:
  1. Critical system-health monitoring exists.
  2. Alerts are generated.
  3. DOR-operated alert routing is configurable by DOR.
  4. Vendor-hosted critical incidents trigger DOR notification according to an agreed timeframe.
- Open question / clarification: None recorded.
- Source section: 4.10 Monitoring, Logging & Observability
- Source page: 9-10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-10.3

- Source requirement: TR-1003
- Requirement type: S
- User role: DOR operations evaluator
- User story: As a DOR operations evaluator, I want distributed tracing and error-tracking practices described, so that complex failures can be diagnosed.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Distributed-tracing approach is described.
  2. Error-tracking approach is described.
  3. Proposed tools are identified where applicable.
- Open question / clarification: None recorded.
- Source section: 4.10 Monitoring, Logging & Observability
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-10.4

- Source requirement: TR-1004
- Requirement type: M
- User role: DOR security auditor
- User story: As a DOR security auditor, I want security-relevant events captured in audit logs, so that authentication, authorization, and sensitive-data activity can be investigated.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Authentication events are logged.
  2. Authorization failures are logged.
  3. Changes to Category 3+ data are logged.
  4. Logging is consistent with the cited SEC-08 expectations.
- Open question / clarification: None recorded.
- Source section: 4.10 Monitoring, Logging & Observability
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E11 - Performance, Scalability, Capacity, and Resilience

#### Performance

##### TS-11.1

- Source requirement: TR-1101
- Requirement type: MS
- User role: DOR performance stakeholder
- User story: As a DOR performance stakeholder, I want performance benchmarks proposed for approximately 100 simultaneous users, so that production performance can be objectively assessed.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Vendor proposes applicable performance benchmarks.
  2. Approximately 100 concurrent users are addressed.
  3. TR-1104, TR-1107, and TR-1108 floors are incorporated.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.2

- Source requirement: TR-1102
- Requirement type: S
- User role: DOR service owner
- User story: As a DOR service owner, I want the scaling approach described for peak-demand periods, so that statutory workload spikes can be supported.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Horizontal/vertical/provider-managed scaling approach is described.
  2. Peak-demand periods are addressed.
  3. Statutory filing/payment deadlines are considered.
- Open question / clarification: The requirement says DOR will supply recurring deadline dates/cycles, but they are not provided in this document.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.3

- Source requirement: TR-1103
- Requirement type: M
- User role: DOR service owner
- User story: As a DOR service owner, I want production designed without a single point of failure within at least one hosting region, so that individual component failures do not take down the service.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Production supports high availability.
  2. No single point of failure exists in production architecture.
  3. At least a single-region HA architecture is provided.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.4

- Source requirement: xx-xxx
- Requirement type: M
- User role: DOR service owner
- User story: As a DOR service owner, I want service availability of at least 99.9%, so that LTS remains available for legislative operations.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Availability is at least 99.9%.
  2. Source defines annual downtime equivalent as no more than approximately 8h46m/year.
- Open question / clarification: Assign stable requirement ID and clarify whether planned maintenance is included/excluded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.5

- Source requirement: TR-1104
- Requirement type: MS
- User role: API consumer
- User story: As a API consumer, I want standard reads to meet defined latency targets, so that system interactions remain responsive.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. p95 <= 500 ms.
  2. p99 <= 1,500 ms.
  3. Applies to standard reads.
  4. Measurement occurs at API gateway.
  5. Client network latency is excluded.
- Open question / clarification: Source explicitly labels these figures “placeholder targets.” What final targets apply?
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.6

- Source requirement: TR-1105
- Requirement type: S
- User role: DOR performance stakeholder
- User story: As a DOR performance stakeholder, I want latency targets documented for operations outside standard API reads, so that performance expectations exist for long-running operations.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Applicable operations are identified.
  2. Expected latency is documented.
  3. Appropriate targets are proposed.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.7

- Source requirement: TR-1106
- Requirement type: MS
- User role: web user
- User story: As a web user, I want pages to render meaningful content quickly, so that the application feels responsive.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. LCP <= 2.5 seconds under typical network conditions.
- Open question / clarification: Define “typical network conditions” and test configuration.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 10
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.8

- Source requirement: TR-1107
- Requirement type: MS
- User role: DOR service owner
- User story: As a DOR service owner, I want the system to sustain at least 50 transactions per second under normal load, so that expected transaction volume can be processed without violating latency targets.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. At least 50 TPS sustained.
  2. TR-1104 latency targets are not exceeded.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 10-11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.9

- Source requirement: TR-1108
- Requirement type: MS
- User role: DOR service owner
- User story: As a DOR service owner, I want the system to absorb temporary bursts, so that short-lived demand spikes do not cause errors or data loss.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Handles 3x sustained throughput.
  2. Sustains burst for at least 5 minutes.
  3. No error responses due to burst.
  4. TR-1104 latency targets remain satisfied.
  5. No data loss.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

#### Rate Limiting and Resilience

##### TS-11.10

- Source requirement: TR-1109
- Requirement type: MS
- User role: DOR security/service owner
- User story: As a DOR security/service owner, I want external API traffic rate-limited by consumer, so that abuse and accidental overload are controlled.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. All externally exposed APIs are rate-limited.
  2. Limits apply per consumer.
  3. Default is at least 100 requests/minute unless higher is requested.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.11

- Source requirement: TR-1110
- Requirement type: M
- User role: API consumer
- User story: As a API consumer, I want an explicit standards-based response when rate limits are exceeded, so that my application can retry appropriately.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. HTTP 429 is returned.
  2. Retry-After header is present.
  3. System does not silently fail/time out/return unhandled server error solely for rate limiting.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.12

- Source requirement: TR-1111
- Requirement type: MS
- User role: DOR architecture evaluator
- User story: As a DOR architecture evaluator, I want connection and pool limits documented with graceful-limit behavior, so that capacity exhaustion does not cause cascading failure.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Maximum concurrent connections are documented.
  2. DB connection-pool limits are documented.
  3. Behavior at limits is documented.
  4. Graceful queuing/rejection or equivalent is addressed.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.13

- Source requirement: TR-1112
- Requirement type: MS
- User role: DOR service owner
- User story: As a DOR service owner, I want the compute tier to autoscale according to measurable demand, so that capacity adjusts to workload without unnecessary manual intervention.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Autoscaling is implemented.
  2. Scaling is triggered by defined utilization/request-rate thresholds.
  3. Minimum capacity bounds are proposed.
  4. Maximum capacity bounds are proposed.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.14

- Source requirement: TR-1113
- Requirement type: MS
- User role: DOR service owner
- User story: As a DOR service owner, I want timeouts and fallback behavior defined for synchronous dependencies, so that downstream failures do not indefinitely block the solution.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Timeout values are documented.
  2. Database dependencies are addressed.
  3. Downstream API dependencies are addressed.
  4. Fallback/error behavior is described.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-11.15

- Source requirement: TR-1114
- Requirement type: S
- User role: DOR architecture evaluator
- User story: As a DOR architecture evaluator, I want downstream-call resilience patterns described, so that transient dependency failures are handled safely.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Circuit-breaker use is addressed.
  2. Retry/exponential-backoff use is addressed.
  3. Bulkhead isolation is addressed where appropriate.
- Open question / clarification: None recorded.
- Source section: 4.11 Performance, Scalability, Capacity & Resilience
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E12 - Disaster Recovery and Business Continuity

#### Disaster Recovery

##### TS-12.1

- Source requirement: TR-1201
- Requirement type: MS
- User role: DOR continuity stakeholder
- User story: As a DOR continuity stakeholder, I want a DR approach meeting the defined RPO/RTO, so that service recovery is planned and aligned with WaTech expectations.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. DR approach is proposed.
  2. TR-403 RPO is met.
  3. TR-403 RTO is met.
  4. WaTech disaster-preparedness expectations are addressed.
- Open question / clarification: None recorded.
- Source section: 4.12 Disaster Recovery & Business Continuity
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-12.2

- Source requirement: TR-1202
- Requirement type: S
- User role: DOR continuity stakeholder
- User story: As a DOR continuity stakeholder, I want the DR-testing approach described, so that recovery capability is periodically validated.
- Conditional applicability: Ongoing testing requirement applies where Vendor operates the solution long-term.
- Acceptance criteria:
  1. Engagement-period DR testing is addressed.
  2. Ongoing testing is addressed if vendor operates solution long term.
- Open question / clarification: None recorded.
- Source section: 4.12 Disaster Recovery & Business Continuity
- Source page: 11
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E13 - Ownership, Licensing, and Data Rights

#### Ownership and Portability

##### TS-13.1

- Source requirement: TR-1301
- Requirement type: M
- User role: DOR data owner
- User story: As a DOR data owner, I want unrestricted ownership of all DOR data, so that platform/code ownership does not limit DOR rights to its information.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. DOR owns all entered/generated DOR data.
  2. Includes bill records.
  3. Includes documents.
  4. Includes DOR-created reports/configurations.
  5. Applies regardless of platform/code ownership.
- Open question / clarification: None recorded.
- Source section: 4.13 Ownership, Licensing & Data Rights
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-13.2

- Source requirement: TR-1302
- Requirement type: M
- User role: DOR technology owner
- User story: As a DOR technology owner, I want custom code intended for DOR ownership treated as DOR property, so that DOR receives complete ownership rights.
- Conditional applicability: Applies where Vendor delivers custom-built source code that DOR is to own outright.
- Acceptance criteria:
  1. Applies when custom-built code is to be owned by DOR.
  2. Code/documentation/architecture artifacts are treated as work made for hire where legally applicable.
  3. Assignment documents are executed where needed.
  4. No additional ownership-assignment fee is charged.
- Open question / clarification: None recorded.
- Source section: 4.13 Ownership, Licensing & Data Rights
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-13.3

- Source requirement: TR-1303
- Requirement type: M
- User role: DOR technology owner
- User story: As a DOR technology owner, I want proprietary vendor components controlled where they could impair DOR rights or migration, so that DOR can exercise ownership and portability rights.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Such components require prior written DOR approval.
  2. Components do not impair TR-1301/1302 rights.
  3. Required licensing supports TR-1306 portability.
- Open question / clarification: None recorded.
- Source section: 4.13 Ownership, Licensing & Data Rights
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-13.4

- Source requirement: TR-1304
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want an SBOM or equivalent third-party component inventory, so that security exposure can be assessed.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Significant third-party/open-source components are disclosed.
  2. Disclosure level is appropriate to proposed approach.
  3. Information is sufficient for security-exposure assessment.
- Open question / clarification: None recorded.
- Source section: 4.13 Ownership, Licensing & Data Rights
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-13.5

- Source requirement: TR-1305
- Requirement type: M
- User role: DOR data owner
- User story: As a DOR data owner, I want vendor use of DOR data limited to contracted services, so that DOR information is not repurposed without authorization.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. No use outside contracted service without prior written consent.
  2. No AI/ML training without consent.
  3. No third-party analytics products without consent.
  4. No marketing use without consent.
- Open question / clarification: None recorded.
- Source section: 4.13 Ownership, Licensing & Data Rights
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-13.6

- Source requirement: TR-1306
- Requirement type: M
- User role: DOR data owner
- User story: As a DOR data owner, I want complete data export and verified deletion when a contract ends, so that DOR can migrate without losing control of its information.
- Conditional applicability: Data export/deletion obligations are inherently satisfied where DOR already independently possesses complete code and data.
- Acceptance criteria:
  1. Complete export within 30 days of DOR request.
  2. Export uses non-proprietary/common format.
  3. Vendor deletion within 60 days after DOR confirms successful export.
  4. Written deletion certification is provided.
  5. Requirement is inherently satisfied where DOR already possesses complete code/data.
- Open question / clarification: None recorded.
- Source section: 4.13 Ownership, Licensing & Data Rights
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-13.7

- Source requirement: TR-1307
- Requirement type: MS
- User role: DOR transition owner
- User story: As a DOR transition owner, I want post-contract migration assistance, so that DOR or a successor vendor can assume the solution effectively.
- Conditional applicability: Applies to the extent DOR does not already possess everything needed to operate or migrate independently.
- Acceptance criteria:
  1. Assistance lasts at least 60 days after termination where needed.
  2. Data-mapping support is included as applicable.
  3. Knowledge transfer to successor/DOR is included as applicable.
  4. Applies to extent DOR does not already possess everything required.
- Open question / clarification: None recorded.
- Source section: 4.13 Ownership, Licensing & Data Rights
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E14 - Knowledge Transfer, Transition, and Support

#### Knowledge Transfer and Operational Independence

##### TS-14.1

- Source requirement: TR-1401
- Requirement type: M
- User role: DOR service owner
- User story: As a DOR service owner, I want a DOR-approved knowledge-transfer plan executed, so that DOR can operate or oversee the solution confidently.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Plan is created.
  2. DOR approves it.
  3. Documentation, training, and artifacts are included.
  4. Final work-product delivery is covered where applicable.
- Open question / clarification: None recorded.
- Source section: 4.14 Knowledge Transfer, Transition & Support
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-14.2

- Source requirement: TR-1402
- Requirement type: M
- User role: designated DOR staff member
- User story: As a designated DOR staff member, I want knowledge transfer throughout the engagement, so that operational knowledge is developed before go-live rather than transferred only at project close.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. KT occurs throughout engagement.
  2. Documentation is provided.
  3. Live/recorded training is provided.
  4. Codebase/architecture/deployment/runbook walkthroughs are included as applicable.
  5. Formal training occurs before go-live.
  6. TR-206 access restrictions are maintained.
- Open question / clarification: None recorded.
- Source section: 4.14 Knowledge Transfer, Transition & Support
- Source page: 12
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-14.3

- Source requirement: TR-1403
- Requirement type: M
- User role: DOR operations owner
- User story: As a DOR operations owner, I want complete as-built and runbook documentation before go-live, so that the production solution can be understood and supported.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. As-built architecture documentation.
  2. Data model/ER diagrams.
  3. Deployment runbook.
  4. Rollback runbook.
  5. Incident-response guidance.
  6. Common troubleshooting guidance.
  7. Delivered before go-live.
- Open question / clarification: None recorded.
- Source section: 4.14 Knowledge Transfer, Transition & Support
- Source page: 13
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-14.4

- Source requirement: TR-1404
- Requirement type: M
- User role: DOR technical operator
- User story: As a DOR technical operator, I want vendor guidance while DOR personnel perform deployment and validation activities, so that DOR can become operationally independent without granting vendor environment access.
- Conditional applicability: Applies where DOR deploys and configures the delivered solution in its own environment.
- Acceptance criteria:
  1. Applies when DOR deploys/configures.
  2. Vendor provides live guidance.
  3. Documentation walkthroughs are provided.
  4. Troubleshooting advice is provided.
  5. DOR personnel perform all actions.
  6. TR-206 restrictions are preserved.
- Open question / clarification: None recorded.
- Source section: 4.14 Knowledge Transfer, Transition & Support
- Source page: 13
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-14.5

- Source requirement: TR-1405
- Requirement type: MS
- User role: DOR service owner
- User story: As a DOR service owner, I want a support commitment appropriate to the delivery posture, so that DOR has defined support expectations before acceptance and during ongoing hosted service.
- Conditional applicability: Support obligations differ between DOR-operated and Vendor-hosted postures.
- Acceptance criteria:
  1. DOR-operated approach includes at least 60 days pre-acceptance consultative support.
  2. DOR performs operational duties during that period.
  3. Vendor obligations end after successful completion as stated.
  4. Vendor-hosted approach provides at least 99.9% monthly uptime.
  5. Vendor-hosted approach defines service credits.
  6. Confirmed security incidents affecting DOR data are reported within 24 hours.
- Open question / clarification: None recorded.
- Source section: 4.14 Knowledge Transfer, Transition & Support
- Source page: 13
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-14.6

- Source requirement: TR-1406
- Requirement type: M
- User role: DOR security stakeholder
- User story: As a DOR security stakeholder, I want vendor access restricted according to operating posture, so that environment/data access remains consistent with contractual responsibilities.
- Conditional applicability: Access restrictions differ between DOR-operated and Vendor-hosted postures.
- Acceptance criteria:
  1. DOR-operated: vendor has no DOR environment/system/repository access during or after engagement.
  2. No support exception is permitted.
  3. Vendor-hosted: access is limited to contracted-service necessity.
  4. Vendor-hosted access remains subject to TR-208 audit/review rights.
- Open question / clarification: None recorded.
- Source section: 4.14 Knowledge Transfer, Transition & Support
- Source page: 13
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E15 - Open Source Software Governance

#### FOSS Disclosure and Licensing

##### TS-15.1

- Source requirement: TR-1501
- Requirement type: M
- User role: DOR technology/security owner
- User story: As a DOR technology/security owner, I want all FOSS dependencies disclosed, so that operational, security, and licensing exposure can be assessed.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Every FOSS library/component/product used or required is identified.
  2. Build tooling is included.
  3. Deployment tooling is included.
  4. Monitoring/operational tooling is included.
  5. Disclosure is not limited to application-code dependencies.
- Open question / clarification: None recorded.
- Source section: 4.15 Open Source Software Usage
- Source page: 13
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-15.2

- Source requirement: TR-1502
- Requirement type: M
- User role: DOR legal/technology owner
- User story: As a DOR legal/technology owner, I want every FOSS license identified and assessed, so that open-source obligations do not compromise DOR ownership rights.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. License type is identified for every disclosed FOSS item.
  2. Vendor confirms licensing does not compromise Section 4.13 rights.
  3. Copyleft implications are specifically addressed where applicable.
- Open question / clarification: None recorded.
- Source section: 4.15 Open Source Software Usage
- Source page: 13
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E16 - Artificial Intelligence Governance

#### AI Disclosure and Policy Compliance

##### TS-16.1

- Source requirement: TR-1601
- Requirement type: M
- User role: DOR technology/security owner
- User story: As a DOR technology/security owner, I want every AI dependency disclosed, so that DOR understands all AI present in application and operational tooling.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Every AI library/component/product is identified.
  2. Build tooling is included.
  3. Deployment tooling is included.
  4. Monitoring/operational tooling is included.
  5. Disclosure is not limited to embedded application code.
- Open question / clarification: None recorded.
- Source section: 4.16 Artificial Intelligence (AI) tooling Integration or Usage
- Source page: 13-14
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-16.2

- Source requirement: TR-1602
- Requirement type: M
- User role: DOR data owner
- User story: As a DOR data owner, I want the use of AI agents/tools and associated DOR data sharing disclosed, so that DOR can understand how its information interacts with AI technology.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. AI agent/tool usage is identified for every applicable AI dependency.
  2. All AI types are included.
  3. How DOR data is shared with each AI technology is identified.
- Open question / clarification: None recorded.
- Source section: 4.16 Artificial Intelligence (AI) tooling Integration or Usage
- Source page: 14
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-16.3

- Source requirement: xx-xxx
- Requirement type: M
- User role: DOR governance stakeholder
- User story: As a DOR governance stakeholder, I want AI technologies used by the solution to comply with WaTech DATA-04, so that AI use meets applicable statewide policy obligations.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Applicable DATA-04 obligations are identified.
  2. AI technologies comply with those obligations.
- Open question / clarification: Assign stable TR identifier.
- Source section: 4.16 Artificial Intelligence (AI) tooling Integration or Usage
- Source page: 14
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

### E17 - Documentation Deliverables

#### Technical and User Documentation

##### TS-17.1

- Source requirement: TR-1701
- Requirement type: M
- User role: DOR technology/service owner
- User story: As a DOR technology/service owner, I want complete technical, operational, security, and user documentation, so that the solution can be operated and maintained without undocumented vendor knowledge.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. System architecture documentation.
  2. Data dictionary/ER diagrams.
  3. API documentation.
  4. Deployment/runbook or operations documentation.
  5. Security documentation.
  6. End-user documentation.
  7. Administrator documentation.
- Open question / clarification: None recorded.
- Source section: 4.17 Documentation Deliverables
- Source page: 14
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

##### TS-17.2

- Source requirement: TR-1702
- Requirement type: M
- User role: DOR documentation owner
- User story: As a DOR documentation owner, I want project documentation in an editable DOR-standard format and directly accessible to DOR, so that DOR can maintain it without vendor dependence.
- Conditional applicability: Always applicable unless narrowed by a future promoted sprint.
- Acceptance criteria:
  1. Documentation is editable.
  2. Format is Markdown or Microsoft Word.
  3. Documentation is stored where DOR can access it without vendor involvement.
- Open question / clarification: None recorded.
- Source section: 4.17 Documentation Deliverables
- Source page: 14
- Source document: Technical Requirements.docx — Exhibit B, RFP 20226-02 Legislative Tracking System

## Source Trace Matrix

| Technical Story | Source Requirement | Requirement Type | Epic | Feature | Source Section | Source Page |
| --- | --- | --- | --- | --- | --- | --- |
| TS-1.1 | TR-101 | MS | E1 - Architecture and Technology Standards | Solution Architecture and Technology Strategy | 4.1 Architecture & Technology Standards | 4 |
| TS-1.2 | TR-104 | S | E1 - Architecture and Technology Standards | Solution Architecture and Technology Strategy | 4.1 Architecture & Technology Standards | 4 |
| TS-1.3 | TR-105 | S | E1 - Architecture and Technology Standards | Solution Architecture and Technology Strategy | 4.1 Architecture & Technology Standards | 4 |
| TS-1.4 | TR-106 | M | E1 - Architecture and Technology Standards | Solution Architecture and Technology Strategy | 4.1 Architecture & Technology Standards | 4 |
| TS-1.5 | TR-107 | M | E1 - Architecture and Technology Standards | Solution Architecture and Technology Strategy | 4.1 Architecture & Technology Standards | 4 |
| TS-2.1 | TR-201 | M | E2 - Hosting, Infrastructure, and Environment Management | Hosting Governance | 4.2 Hosting, Infrastructure & Environment Management | 5 |
| TS-2.2 | TR-202 | MS | E2 - Hosting, Infrastructure, and Environment Management | Hosting Governance | 4.2 Hosting, Infrastructure & Environment Management | 5 |
| TS-2.3 | TR-204 | S | E2 - Hosting, Infrastructure, and Environment Management | Hosting Governance | 4.2 Hosting, Infrastructure & Environment Management | 5 |
| TS-2.4 | TR-205 | S | E2 - Hosting, Infrastructure, and Environment Management | Hosting Governance | 4.2 Hosting, Infrastructure & Environment Management | 5 |
| TS-2.5 | TR-206 | M | E2 - Hosting, Infrastructure, and Environment Management | DOR-Operated Hosting | 4.2 Hosting, Infrastructure & Environment Management | 5 |
| TS-2.6 | TR-207 | M | E2 - Hosting, Infrastructure, and Environment Management | Vendor-Hosted Environment | 4.2 Hosting, Infrastructure & Environment Management | 5 |
| TS-2.7 | TR-208 | M | E2 - Hosting, Infrastructure, and Environment Management | Vendor-Hosted Environment | 4.2 Hosting, Infrastructure & Environment Management | 5 |
| TS-2.8 | TR-209 | MS | E2 - Hosting, Infrastructure, and Environment Management | Vendor-Hosted Environment | 4.2 Hosting, Infrastructure & Environment Management | 5 |
| TS-3.1 | TR-301 | M | E3 - Security and Identity | Security Baseline and Cryptography | 4.3 Security & Identity | 5 |
| TS-3.2 | TR-302 | M | E3 - Security and Identity | Security Baseline and Cryptography | 4.3 Security & Identity | 5 |
| TS-3.3 | TR-303 | M | E3 - Security and Identity | Identity and Authorization | 4.3 Security & Identity | 6 |
| TS-3.4 | TR-304 | MS | E3 - Security and Identity | Identity and Authorization | 4.3 Security & Identity | 6 |
| TS-3.5 | TR-305 | M | E3 - Security and Identity | Identity and Authorization | 4.3 Security & Identity | 6 |
| TS-3.6 | TR-306 | S | E3 - Security and Identity | Identity and Authorization | 4.3 Security & Identity | 6 |
| TS-3.7 | TR-308 | M | E3 - Security and Identity | Identity and Authorization | 4.3 Security & Identity | 6 |
| TS-3.8 | TR-309 | M | E3 - Security and Identity | Identity and Authorization | 4.3 Security & Identity | 6 |
| TS-3.9 | TR-310 | M | E3 - Security and Identity | Identity and Authorization | 4.3 Security & Identity | 6 |
| TS-4.1 | TR-402 | MS | E4 - Data Management | Data Protection, Recovery, and Retention | 4.4 Data Management | 6 |
| TS-4.2 | TR-403 | M | E4 - Data Management | Data Protection, Recovery, and Retention | 4.4 Data Management | 6 |
| TS-4.3 | TR-404 | M | E4 - Data Management | Data Protection, Recovery, and Retention | 4.4 Data Management | 6 |
| TS-4.4 | TR-405 | S | E4 - Data Management | Data Protection, Recovery, and Retention | 4.4 Data Management | 6-7 |
| TS-5.1 | TR-501 | M | E5 - Accessibility | Accessibility Compliance | 4.5 Accessibility | 7 |
| TS-5.2 | TR-502 | M | E5 - Accessibility | Accessibility Compliance | 4.5 Accessibility | 7 |
| TS-5.3 | TR-503 | S | E5 - Accessibility | Accessibility Compliance | 4.5 Accessibility | 7 |
| TS-6.1 | TR-601 | M | E6 - Integration and Interoperability | APIs and External Data Exchange | 4.6 Integration & Interoperability | 7 |
| TS-6.2 | xx-xxx | Not shown in source | E6 - Integration and Interoperability | APIs and External Data Exchange | 4.6 Integration & Interoperability | 7 |
| TS-6.3 | TR-602 | MS | E6 - Integration and Interoperability | APIs and External Data Exchange | 4.6 Integration & Interoperability | 7 |
| TS-6.4 | TR-603 | S | E6 - Integration and Interoperability | APIs and External Data Exchange | 4.6 Integration & Interoperability | 7 |
| TS-6.5 | TR-604 | M | E6 - Integration and Interoperability | SharePoint Integration | 4.6 Integration & Interoperability | 7 |
| TS-6.6 | TR-605 | S | E6 - Integration and Interoperability | SharePoint Integration | 4.6 Integration & Interoperability | 7 |
| TS-6.7 | TR-607 | S | E6 - Integration and Interoperability | SharePoint Integration | 4.6 Integration & Interoperability | 8 |
| TS-7.1 | TR-701 | MS | E7 - Software Engineering and Source Code | Source Control and Architecture | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.2 | xx-xxx | S | E7 - Software Engineering and Source Code | Source Control and Architecture | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.3 | xx-xxx | M | E7 - Software Engineering and Source Code | Source Control and Architecture | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.4 | xx-xxx | MS occurrence | E7 - Software Engineering and Source Code | Source Control and Architecture | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.5 | TR-702 | S | E7 - Software Engineering and Source Code | Source Control and Architecture | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.6 | TR-703 | M | E7 - Software Engineering and Source Code | Source Control and Architecture | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.7 | TR-704 | S | E7 - Software Engineering and Source Code | Source Control and Architecture | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.8 | TR-705 | MS | E7 - Software Engineering and Source Code | Source Control and Architecture | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.9 | TR-801 | M | E7 - Software Engineering and Source Code | Automated Build and Deployment | 4.7 Software Engineering & Source Code Standards | 8 |
| TS-7.10 | TR-802 | MS | E7 - Software Engineering and Source Code | Automated Build and Deployment | 4.7 Software Engineering & Source Code Standards | 8-9 |
| TS-7.11 | xx-xxx | M occurrence | E7 - Software Engineering and Source Code | Automated Build and Deployment | 4.7 Software Engineering & Source Code Standards | 9 |
| TS-8.1 | TR-803 | M | E8 - DevOps and Release Management | Change and Release Management | 4.8 DevOps, CI/CD & Release Management | 9 |
| TS-8.2 | TR-804 | S | E8 - DevOps and Release Management | Change and Release Management | 4.8 DevOps, CI/CD & Release Management | 9 |
| TS-9.1 | TR-901 | M | E9 - Testing and Quality Assurance | Test Strategy and Acceptance | 4.9 Testing & Quality Assurance | 9 |
| TS-9.2 | TR-902 | MS | E9 - Testing and Quality Assurance | Test Strategy and Acceptance | 4.9 Testing & Quality Assurance | 9 |
| TS-9.3 | TR-903 | M | E9 - Testing and Quality Assurance | Test Strategy and Acceptance | 4.9 Testing & Quality Assurance | 9 |
| TS-9.4 | TR-904 | S | E9 - Testing and Quality Assurance | Test Strategy and Acceptance | 4.9 Testing & Quality Assurance | 9 |
| TS-10.1 | TR-1001 | M | E10 - Monitoring, Logging, and Observability | Operational Observability | 4.10 Monitoring, Logging & Observability | 9 |
| TS-10.2 | TR-1002 | M | E10 - Monitoring, Logging, and Observability | Operational Observability | 4.10 Monitoring, Logging & Observability | 9-10 |
| TS-10.3 | TR-1003 | S | E10 - Monitoring, Logging, and Observability | Operational Observability | 4.10 Monitoring, Logging & Observability | 10 |
| TS-10.4 | TR-1004 | M | E10 - Monitoring, Logging, and Observability | Operational Observability | 4.10 Monitoring, Logging & Observability | 10 |
| TS-11.1 | TR-1101 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 10 |
| TS-11.2 | TR-1102 | S | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 10 |
| TS-11.3 | TR-1103 | M | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 10 |
| TS-11.4 | xx-xxx | M | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 10 |
| TS-11.5 | TR-1104 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 10 |
| TS-11.6 | TR-1105 | S | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 10 |
| TS-11.7 | TR-1106 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 10 |
| TS-11.8 | TR-1107 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 10-11 |
| TS-11.9 | TR-1108 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Performance | 4.11 Performance, Scalability, Capacity & Resilience | 11 |
| TS-11.10 | TR-1109 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Rate Limiting and Resilience | 4.11 Performance, Scalability, Capacity & Resilience | 11 |
| TS-11.11 | TR-1110 | M | E11 - Performance, Scalability, Capacity, and Resilience | Rate Limiting and Resilience | 4.11 Performance, Scalability, Capacity & Resilience | 11 |
| TS-11.12 | TR-1111 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Rate Limiting and Resilience | 4.11 Performance, Scalability, Capacity & Resilience | 11 |
| TS-11.13 | TR-1112 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Rate Limiting and Resilience | 4.11 Performance, Scalability, Capacity & Resilience | 11 |
| TS-11.14 | TR-1113 | MS | E11 - Performance, Scalability, Capacity, and Resilience | Rate Limiting and Resilience | 4.11 Performance, Scalability, Capacity & Resilience | 11 |
| TS-11.15 | TR-1114 | S | E11 - Performance, Scalability, Capacity, and Resilience | Rate Limiting and Resilience | 4.11 Performance, Scalability, Capacity & Resilience | 11 |
| TS-12.1 | TR-1201 | MS | E12 - Disaster Recovery and Business Continuity | Disaster Recovery | 4.12 Disaster Recovery & Business Continuity | 11 |
| TS-12.2 | TR-1202 | S | E12 - Disaster Recovery and Business Continuity | Disaster Recovery | 4.12 Disaster Recovery & Business Continuity | 11 |
| TS-13.1 | TR-1301 | M | E13 - Ownership, Licensing, and Data Rights | Ownership and Portability | 4.13 Ownership, Licensing & Data Rights | 12 |
| TS-13.2 | TR-1302 | M | E13 - Ownership, Licensing, and Data Rights | Ownership and Portability | 4.13 Ownership, Licensing & Data Rights | 12 |
| TS-13.3 | TR-1303 | M | E13 - Ownership, Licensing, and Data Rights | Ownership and Portability | 4.13 Ownership, Licensing & Data Rights | 12 |
| TS-13.4 | TR-1304 | M | E13 - Ownership, Licensing, and Data Rights | Ownership and Portability | 4.13 Ownership, Licensing & Data Rights | 12 |
| TS-13.5 | TR-1305 | M | E13 - Ownership, Licensing, and Data Rights | Ownership and Portability | 4.13 Ownership, Licensing & Data Rights | 12 |
| TS-13.6 | TR-1306 | M | E13 - Ownership, Licensing, and Data Rights | Ownership and Portability | 4.13 Ownership, Licensing & Data Rights | 12 |
| TS-13.7 | TR-1307 | MS | E13 - Ownership, Licensing, and Data Rights | Ownership and Portability | 4.13 Ownership, Licensing & Data Rights | 12 |
| TS-14.1 | TR-1401 | M | E14 - Knowledge Transfer, Transition, and Support | Knowledge Transfer and Operational Independence | 4.14 Knowledge Transfer, Transition & Support | 12 |
| TS-14.2 | TR-1402 | M | E14 - Knowledge Transfer, Transition, and Support | Knowledge Transfer and Operational Independence | 4.14 Knowledge Transfer, Transition & Support | 12 |
| TS-14.3 | TR-1403 | M | E14 - Knowledge Transfer, Transition, and Support | Knowledge Transfer and Operational Independence | 4.14 Knowledge Transfer, Transition & Support | 13 |
| TS-14.4 | TR-1404 | M | E14 - Knowledge Transfer, Transition, and Support | Knowledge Transfer and Operational Independence | 4.14 Knowledge Transfer, Transition & Support | 13 |
| TS-14.5 | TR-1405 | MS | E14 - Knowledge Transfer, Transition, and Support | Knowledge Transfer and Operational Independence | 4.14 Knowledge Transfer, Transition & Support | 13 |
| TS-14.6 | TR-1406 | M | E14 - Knowledge Transfer, Transition, and Support | Knowledge Transfer and Operational Independence | 4.14 Knowledge Transfer, Transition & Support | 13 |
| TS-15.1 | TR-1501 | M | E15 - Open Source Software Governance | FOSS Disclosure and Licensing | 4.15 Open Source Software Usage | 13 |
| TS-15.2 | TR-1502 | M | E15 - Open Source Software Governance | FOSS Disclosure and Licensing | 4.15 Open Source Software Usage | 13 |
| TS-16.1 | TR-1601 | M | E16 - Artificial Intelligence Governance | AI Disclosure and Policy Compliance | 4.16 Artificial Intelligence (AI) tooling Integration or Usage | 13-14 |
| TS-16.2 | TR-1602 | M | E16 - Artificial Intelligence Governance | AI Disclosure and Policy Compliance | 4.16 Artificial Intelligence (AI) tooling Integration or Usage | 14 |
| TS-16.3 | xx-xxx | M | E16 - Artificial Intelligence Governance | AI Disclosure and Policy Compliance | 4.16 Artificial Intelligence (AI) tooling Integration or Usage | 14 |
| TS-17.1 | TR-1701 | M | E17 - Documentation Deliverables | Technical and User Documentation | 4.17 Documentation Deliverables | 14 |
| TS-17.2 | TR-1702 | M | E17 - Documentation Deliverables | Technical and User Documentation | 4.17 Documentation Deliverables | 14 |
