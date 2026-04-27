# ADR-008: LiteDB (LiteDB.Async) as Embedded NoSQL Provider

## Status

Accepted (implementation in think-tank — not yet promoted to production `libs/`)

## Date

2026-03-17

## Implementation Note (2026-04-27)

`xSdk.Data.NoSql` resides in `think-tank/libs/xSdk.Data.NoSql/` and has not yet been promoted to the production `libs/` folder. The design described in this ADR reflects the intended architecture. The package will be added to `libs/` and released as a NuGet package once stabilized.

## Context

Some applications require a local, file-based NoSQL store without dependency on an external server. Use cases include:

- Edge devices or containerized workloads where a database server is unavailable.
- Developer workstations needing a zero-infrastructure backend.
- Integration tests running in CI without external services.
- Applications that need BSON-style document storage with indexing but not the overhead of MongoDB.

## Decision

`xSdk.Data.NoSql` uses **LiteDB** (via the `LiteDB.Async` wrapper) as the embedded NoSQL provider.

### Components

| Class | Responsibility |
|---|---|
| `NoSqlDatabase` | Manages `LiteDatabaseAsync` instance; opens with `persistConnection = true` to reuse across calls |
| `NoSqlConnectionBuilder` | Resolves the connection string from `Path` + `FileName` configuration |
| `NoSqlDatabaseSetup` | Holds `Path`, `FileName`, `Password`, `IsReadOnly`; validates that path and file name are set |
| `NoSqlEntity` / `NoSqlEntityPK` | Entity base classes with LiteDB BSON `[BsonId]` mapping |
| `NoSqlRepository<TEntity>` | Repository implementation |
| `IDatalayerBuilderExtensions` | `UseNoSql(name, config)` extension method |

### Repository Operations

`NoSqlRepository<TEntity>` is split across multiple partial classes for readability:

- `NoSqlRepository.Insert.cs` — `InsertAsync`, bulk insert
- `NoSqlRepository.Remove.cs` — remove by primary key, entity, or LINQ predicate
- `NoSqlRepository.Select.cs` — select single, select list (with optional filter expression)
- `NoSqlRepository.Update.cs` — update by primary key
- `NoSqlRepository.Upsert.cs` — insert-or-update

All operations execute through `ExecuteInternalAsync<TResult>(func, withTransaction, token)` which:

1. Opens the `LiteDatabaseAsync` with `persistConnection = true`.
2. Optionally wraps in a LiteDB transaction via `BeginTransactionAsync()`.
3. Resolves the collection by `GetTableName()`.
4. Calls the operation lambda.
5. Commits or rolls back as appropriate.

### Index Management

`NoSqlIndex` and `[NoSqlIndex]` attribute allow declarative index creation. `NoSqlIndexExtensions.EnsureIndexes(db, entity)` is called during `Initialize()` to create all declared indexes on startup.

### Field and Ignore Attributes

- `[NoSqlField]` — maps a property to a custom BSON field name.
- `[NoSqlIgnore]` — excludes a property from BSON serialization.

These mirror LiteDB's native `[BsonField]` and `[BsonIgnore]` but are wrapped in SDK attributes to avoid direct LiteDB type references in application entity classes.

### Initialization

`NoSqlRepository.Initialize()` (called by `Repository.Configure(database)`) creates the collection, ensures indexes, and optionally seeds fake data. This happens synchronously within a `lock` to prevent concurrent initialization.

### Registration

```csharp
builder.UseNoSql("MyDataStore", config =>
{
    config.Path = "/var/data";
    config.FileName = "app.store";
});
builder.MapRepository<IMyRepository, MyRepository>();
```

## Consequences

### Positive

- Zero external dependencies — the database file is created on first access.
- Async-first API via `LiteDB.Async`.
- LINQ query support via `AsQueryable()`.
- Transactions supported natively.
- Index management via attributes — no manual initialization code.

### Negative

- LiteDB is a single-file embedded database — not suitable for high-concurrency scenarios or distributed deployments.
- The `lock` in `Initialize()` blocks the thread; not suitable if the initialization is expected to be async.
- `LiteDB.Async` is a community wrapper around `LiteDB` — not an officially maintained async API, which carries maintenance risk.
- LiteDB's transaction model differs from relational databases; the `withTransaction` flag in `ExecuteInternalAsync` may be misleading for developers expecting ACID semantics over multiple collections.
