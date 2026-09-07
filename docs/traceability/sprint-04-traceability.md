# Sprint 4 Traceability

Status: active

Last updated: 2026-09-04

## Scope

Sprint 4 implements the POC relationships, packages, and categorization vertical slice on top of Sprint 3 assignment/work queues: link related work items by topic, document type, legislative identifier, and named package; combine work products into a deliverable package with an evaluable status; and identify, sort, filter, and group work using designated criteria.

## Business Trace Matrix

| Business Story | Source Requirement | Sprint 4 Evidence | Artifact |
| --- | --- | --- | --- |
| US-2.2.1 | B.COM.05 | Link work by topic, document type, legislative identifier, and named package; bill versions remain related; fiscal notes/estimates/etc. linkable to work or packages. | `WorkItemRelationshipType`, `WorkItemRelationship`, `WorkItemRelationshipService`, `POST/DELETE/GET /api/v1/work-items/{id}/relationships` |
| US-2.2.2 | B.RFA.06 | Combine work products into one package, deliver as one product, determine package status, underlying products remain identifiable. | `Package`, `PackageMember`, `PackageStatus`, `PackageService`, `/api/v1/packages` endpoints |
| US-2.3.1 | B.COM.04 | Identify, sort, filter, and group by Confidential, Executive Review, On Hold, Work Type, and Package. | `WorkTask.IsConfidential`/`IsExecutiveReview`, `WorkItemQueryService`, `GET /api/v1/work-items`, Work Items page |

## Technical Trace Matrix

| Technical Story | Source Requirement | Sprint 4 Evidence | Artifact |
| --- | --- | --- | --- |
| TS-6.1 | TR-601 | Versioned RESTful API surface for relationships, packages, and work-item queries. | `src/Legislature.TrackingSystem.Web/Program.cs` |
| TS-7.5 | TR-702 | Coding standards and automated static controls applied. | `Directory.Build.props`, `dotnet format` |
| TS-9.2 | TR-902 | Automated tests asserting the promoted acceptance criteria. | `tests/Legislature.TrackingSystem.Domain.Tests/WorkItems/` (relationship, package, query tests) |

## POC Interpretation Notes

- A "work item" is represented by the `WorkTask` aggregate for the POC; relationships are first-class links with a type, source item, target item, and audit metadata.
- Categorization criteria map to explicit domain concepts: Confidential and Executive Review are boolean flags on a work item; On Hold is a work task status; Work Type is the work item type; Package is membership in a package.
- A package is a named deliverable that groups work products and carries its own status; the underlying work products remain identifiable.

## Persistence Decision

Sprint 4 extends the interim in-memory repository adapter with relationship and package stores plus query support. PostgreSQL persistence and EF Core migrations remain deferred to a later sprint.

## Verification Evidence

- `dotnet restore Legislature.TrackingSystem.sln -m:1` passed.
- `dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore` passed (exit 0).
- `dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 0 warnings and 0 errors (Domain, Application, Infrastructure, Web.Client incl. Blazor WASM output, Web, and Tests all build).
- `dotnet test Legislature.TrackingSystem.sln --configuration Release -m:1` passed with 40 tests (23 prior + 17 new relationship/package/query tests).
- Container build (`docker compose build web`), startup (`docker compose up -d web`; `lts-web` on `0.0.0.0:5088->8080`), and container smoke checks against `http://localhost:5088` passed: link by LegislativeIdentifier (queryable from target side), package create/add/deliver (Delivered, members retained), categorization (confidential), and work-item query filter by confidential, group by Type/Package, sort by Title.
- UI pages `/packages` and `/work-items` render HTTP 200; nav includes Packages and Work Items links.

> Environment note: This build/verification environment is a restricted sandbox. Solution restore/build/test use `-m:1` (single-node MSBuild) and elevated file/process permissions, and Docker daemon access requires elevated permissions; parallel MSBuild nodes, out-of-proc task hosts, and test-host parent-process monitoring are denied under process isolation without elevation. NuGet vulnerability audit is disabled in `Directory.Build.props` because the environment is offline (the offline `NU1900` diagnostic would otherwise be promoted to a build error by `TreatWarningsAsErrors=true`). These are environment constraints, not Sprint 4 defects; the standard AGENTS.md commands work on a normal developer/container host.
