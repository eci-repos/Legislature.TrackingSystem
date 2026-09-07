# POC Architecture Scaffold

Status: active

Last updated: 2026-09-04

## Purpose

This document records the Sprint 1 ASP.NET Core / Blazor WebAssembly scaffold for the Legislature.TrackingSystem POC.

## Solution Structure

| Path | Responsibility |
| --- | --- |
| `Legislature.TrackingSystem.sln` | Root solution file. |
| `src/Legislature.TrackingSystem.Domain` | Entities, value objects, source trace references, domain rules, and invariants. |
| `src/Legislature.TrackingSystem.Application` | Use case contracts, application services, command/query boundaries, and dependency registration. |
| `src/Legislature.TrackingSystem.Infrastructure` | Persistence, integration, identity, SharePoint, telemetry, and other adapter implementations as future sprints promote them. |
| `src/Legislature.TrackingSystem.Web` | ASP.NET Core host, API endpoints, BFF boundary, authorization policies, and composition root. |
| `src/Legislature.TrackingSystem.Web.Client` | Blazor WebAssembly UI. |
| `tests/Legislature.TrackingSystem.Domain.Tests` | Starter automated tests for domain and application foundation behavior. |

## Foundation Decisions

- The POC is a new custom build using .NET 10.
- The application starts as a modular monolith with replaceable internal boundaries.
- WebAssembly interactivity is served by an ASP.NET Core host.
- Dependency injection is registered by extension methods and consumed through interfaces.
- The first API surface is versioned at `/api/v1/readiness`.
- Sprint 1 does not implement real work intake, persistence, Entra, SharePoint, or AWS deployment.
- Sprint 1A adds local Docker packaging for the web host only; it is a testing posture and not a production deployment decision.

## Local Development Posture

The Web host can run locally with:

```powershell
dotnet run --project src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj --urls http://localhost:5088
```

The repository includes `docker-compose.yml` for local PostgreSQL readiness. The Sprint 1 application does not depend on PostgreSQL yet; persistence is deferred until a promoted sprint requires it.

The Web host can also run in a local Docker container with:

```powershell
docker compose build web
docker compose up -d web
```

The containerized app is available at `http://localhost:5088`; the container listens on port `8080` and Docker Compose maps it to host port `5088`.

## Traceability Pattern

Sprint 1 establishes `SourceTraceReference` in the Domain project and `ISprintReadinessService` in the Application project. Future implementation work should link source user story IDs, technical story IDs, and source requirement IDs through domain/application metadata, tests, seed data, and user-facing or API-visible trace evidence where appropriate.
