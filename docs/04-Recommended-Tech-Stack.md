# Recommended Tech Stack

Status: active

Last updated: 2026-09-03

## Decision Summary

The source documentation allows a .NET-based custom build. It does not prescribe a different programming language, application framework, database engine, AWS compute service, or infrastructure-as-code tool. The high-level architecture identifies a modular, layered, API-centric web application in the DOR AWS solution space, with a responsive web UI, stateless application compute, a structured relational system of record, SharePoint document storage, integration adapters, Entra-based authentication, OAuth 2.0 for APIs, RBAC, centralized logging, monitoring, encryption, accessibility, backup/recovery, CI/CD, testing, documentation, and knowledge transfer.

The recommended POC stack is therefore an ASP.NET Core / Blazor WebAssembly stack on the current supported .NET LTS release, with clean architecture boundaries and replaceable adapters for data, identity, SharePoint, and external integrations.

## Source Fit

This recommendation is grounded in these source constraints:

- `DOR_High_Level_Architecture_Design.docx`: describes a modular, layered, API-centric custom web application with responsive UI, stateless compute, relational data, SharePoint document repository, integration adapters, Entra authentication, OAuth 2.0 APIs, RBAC, logging, monitoring, encryption, accessibility, backup/recovery, and CI/CD.
- `DOR_High_Level_Architecture_Design.docx`: states that compute service, programming language, application framework, database engine, and infrastructure-as-code product are intentionally not named because the mandatory requirements do not prescribe them.
- `DOR_Technical_Agile_Backlog_User_Stories.xlsx`: TS-7.2 requires the technology stack to be identified and says the C#/.NET preference must be addressed for a new custom build.
- `DOR_Technical_Agile_Backlog_User_Stories.xlsx`: TS-1.5 / TR-107 requires all technology-stack components to remain actively supported.
- `DOR_Technical_Agile_Backlog_User_Stories.xlsx`: TS-3.3 / TR-303 requires staff authentication through Microsoft Entra ID using OpenID Connect as preferred or SAML.
- `DOR_Technical_Agile_Backlog_User_Stories.xlsx`: TS-6.1 / TR-601 requires documented, versioned RESTful APIs or equivalent and OAuth 2.0 API authentication.
- `DOR_Technical_Agile_Backlog_User_Stories.xlsx`: TS-6.4 / TR-604 requires tracked-legislation document storage and retrieval through DOR Microsoft 365 SharePoint.

## Recommended Enterprise POC Stack

### Runtime and Language

- .NET 10 LTS.
- C# for server, shared domain, application, and Blazor UI code.
- ASP.NET Core for web hosting, APIs, authentication integration, health checks, rate limiting, dependency injection, configuration, and logging.

Rationale: .NET 10 is the current supported LTS release as of 2026-09-03, and an LTS baseline best satisfies TR-107 supportability expectations.

### Web UI

- Blazor Web App with WebAssembly interactivity for the client experience.
- Prefer an ASP.NET Core-hosted Blazor architecture using a Backend-for-Frontend (BFF) style so browser clients do not own direct privileged integration concerns.
- Use a restrained enterprise component approach with accessible semantic HTML and a small, auditable component library only if needed.

Rationale: Blazor supports rich interactive UI with C#, shared .NET code, HTML/CSS rendering for modern desktop and mobile browsers, and WebAssembly client interactivity.

### Application and API Layer

- ASP.NET Core Web API endpoints with versioned routes.
- OpenAPI documentation generated from the API surface.
- OAuth 2.0 / OpenID Connect integration through Microsoft Entra ID.
- Application-level RBAC expressed through policies and explicit permission requirements.
- Built-in ASP.NET Core rate limiting for externally exposed APIs.

### Architecture Pattern

Use a modular monolith for the POC, with clear internal module boundaries. Do not start with microservices unless a later sprint establishes a requirement.

Recommended project boundaries:

- `Domain`: entities, value objects, domain rules, identifiers, and invariants.
- `Application`: use cases, commands, queries, validators, authorization requirements, and interfaces.
- `Infrastructure`: persistence, SharePoint, Entra, external-system adapters, clocks, file/document abstractions, and telemetry exporters.
- `Web`: Blazor UI, BFF/API endpoints, authentication, authorization policies, and composition root.
- `Tests`: unit, integration, component, and end-to-end tests.

Rationale: this matches the source architecture's modular, layered, API-centric style while keeping the POC operable and easy to evolve.

### Data and Persistence

- EF Core for persistence abstraction and migrations.
- PostgreSQL for the POC relational database, preferably via Docker Compose for local development.
- Keep persistence behind repository/query abstractions only where those abstractions represent real use cases or test seams.
- Use explicit IDs, optimistic concurrency tokens, timestamps, audit fields, and source trace references.

Rationale: the source architecture requires a structured relational system of record but does not prescribe the database engine. PostgreSQL provides a portable, cost-conscious default for a POC and maps well to future AWS managed relational options.

### Documents and Microsoft 365 Integration

- Store document metadata and work-product relationships in the application database.
- Represent SharePoint integration through an interface in the POC.
- Use a fake/in-memory/local adapter for the first POC unless a later sprint provides DOR tenant details.
- Plan a Microsoft Graph-based SharePoint adapter for real integration.

Rationale: source requirements require SharePoint document storage, but a POC can prove boundaries and workflows without requiring production tenant access.

### Identity and Security

- Microsoft Entra ID through OpenID Connect for real environments.
- Local development authentication may use a deterministic dev identity provider or fake authentication handler, clearly isolated from production configuration.
- ASP.NET Core authorization policies for role and permission checks.
- No secrets in source control; use user secrets locally and environment or managed secret providers in deployed environments.
- TLS everywhere outside local-only development.

### Observability and Operations

- Microsoft.Extensions.Logging with structured logs.
- Serilog only if richer sinks are needed in the POC.
- OpenTelemetry for traces, metrics, and logs instrumentation.
- ASP.NET Core health checks.
- Audit-log model for authentication, authorization failures, and sensitive data changes.

### Testing and Quality

- xUnit for unit and integration tests.
- FluentAssertions or Shouldly for readable assertions.
- bUnit for Blazor component tests.
- Playwright for browser-level POC workflow tests and accessibility checks.
- axe-core integration in Playwright where practical.
- Static analysis through .NET analyzers, nullable reference types, warnings-as-errors for project code, and dependency vulnerability scanning.

### Local Development and CI/CD

- Docker Compose for local PostgreSQL and optional observability dependencies.
- Azure Pipelines as the expected CI/CD orchestrator because the architecture identifies Azure DevOps as the assumed engineering toolchain.
- Pipeline stages should restore, build, test, collect coverage, run static analysis, publish artifacts, and later package deployment artifacts.

### AWS Deployment Direction

The POC may run locally first. If deployment is promoted later, the recommended AWS direction is:

- Containerized ASP.NET Core application.
- AWS Application Load Balancer behind WAF.
- ECS Fargate or another DOR-approved stateless compute service.
- Managed relational database such as Amazon RDS or Aurora PostgreSQL.
- Centralized logs, metrics, alerts, encryption, backup, and point-in-time recovery using DOR-approved AWS services.
- Terraform for infrastructure as code unless DOR standards prescribe another product.

This deployment direction remains a recommendation, not an implementation mandate, until a sprint promotes deployment work.

## POC Scope Supported by This Stack

The recommended stack is sufficient to implement the Phase 1 POC vertical slice:

- Legislative work intake.
- Unique identifiers for tasks, work products, and packages.
- Assignment and reassignment.
- Due dates, priority, status, and work queues.
- Related-record links between tasks, products, assignments, documents, topics, identifiers, and packages.
- Sorting, filtering, and grouping by designated criteria.
- Source traceability from UI/API behavior to user story IDs and requirement IDs.

## Technology Decisions Deferred

These decisions remain open until the corresponding sprint promotes them:

- Final AWS compute service.
- Final managed relational database engine.
- Production Entra tenant/app registrations.
- Real SharePoint library/folder provisioning model.
- External legislative source contracts.
- Internal DOR fiscal-system integration contracts.
- Production logging, SIEM, alerting, WAF, DDoS, backup, and disaster-recovery products.
