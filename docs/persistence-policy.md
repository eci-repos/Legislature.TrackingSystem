# Persistence Policy

Status: active

Last updated: 2026-09-06

## Decision

The no-connection in-memory fallback **remains a supported local/offline development mode**, but **production requires PostgreSQL**. The persistence policy is enforced at startup so a production deployment cannot silently run on the in-memory fallback.

## Modes

| Mode | Selection | Lifetime | Use |
| --- | --- | --- | --- |
| PostgreSQL | A `ConnectionStrings:Default` connection string is configured | Scoped (EF Core `DbContext`) | Production and any environment with a database |
| In-memory | No connection string is configured | Singleton (shared state) | Local/offline development without a database |

The mode is resolved by `PersistencePolicy.ResolveMode(connectionString)` and enforced by `PersistencePolicy.IsAllowed(mode, environmentName)`.

## Enforcement

- **Production** (`ASPNETCORE_ENVIRONMENT=Production`) with no connection string → the app **fails fast at startup** with an actionable error. The in-memory fallback is not allowed in production.
- **Non-Production** with no connection string → the app starts and logs a **warning** that it is running on the in-memory fallback.

## Rationale

- The in-memory fallback keeps the offline development workspace and local runs runnable without a PostgreSQL dependency, which is valuable for the POC and for contributors without a database.
- Production requires durable, shared, transactional persistence (PostgreSQL via EF Core) for correctness, concurrency, and recovery. Running production on the in-memory fallback would lose data on restart and provide no multi-instance consistency.

## Parity guarantees

- The in-memory adapters implement the same repository interfaces as the EF Core adapters, so application code is agnostic to the mode.
- The in-memory adapters are **not** a substitute for PostgreSQL in production; they are a development convenience. Behavior differences (e.g., transactional semantics, query translation, concurrency) are not guaranteed to match EF Core exactly.

## Migration path

- To run with PostgreSQL, set `ConnectionStrings:Default` to a valid PostgreSQL connection string (see `docs/database/lts-postgresql-ddl.sql` for the schema).
- The `DatabaseHealthCheck` verifies the PostgreSQL connection when a connection string is configured and reports healthy otherwise (in-memory dev boundary).

## Residual risks and deferrals

- The in-memory fallback is not a production persistence target; it is retained for local/offline development.
- A future sprint may add a runtime mode indicator (e.g., a readiness detail or a startup banner) if observability requires it.
