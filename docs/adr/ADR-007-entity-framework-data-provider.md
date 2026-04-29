# ADR-007: Entity Framework Core as Relational Data Provider

## Status

Accepted

## Date

2026-03-17

## Context

Applications built on xSDK frequently require a relational database backend. The .NET ecosystem provides Entity Framework Core as the standard ORM, with broad provider support (SQL Server, PostgreSQL, SQLite, in-memory, etc.). The SDK's data layer abstraction must expose EF Core's capabilities while hiding its complexity from application repositories.

## Decision

`xSdk.Data.EntityFramework` provides:

### Abstractions

- `EntityFrameworkDatabase<TDbContext>` — wraps `IDbContextFactory<TDbContext>` for scoped `DbContext` access.
- `EntityFrameworkConnectionBuilder` — builds the EF Core connection setup.
- `EntityFrameworkDatabaseSetup` — holds `TransactionsEnabled` flag and validates EF Core setup.
- `EntityFrameworkRepository<TDbContext, TEntity>` — concrete `Repository<TEntity>` subclass implementing all CRUD operations via `DbContext`.

### Transaction Management

All mutating operations (`InsertAsync`, `UpdateAsync`, `RemoveAsync`, `UpsertAsync`) call `ExecuteInternalAsync(func, withTransaction, token)` which:

1. Opens a `DbContext` via `IDbContextFactory`.
2. Optionally wraps in `IDbContextTransaction` (when `TransactionsEnabled = true`).
3. Commits on success; rolls back on exception.
4. Returns the result.

Transactions can be disabled per database setup (`TransactionsEnabled = false`) — required when using in-memory providers (EF Core in-memory does not support transactions) or MongoDB EF provider.

### Query Extensions

`EntityFrameworkRepository<TDbContext, TEntity>` provides additional `protected` methods for filtering:

- `SelectAsync(Expression<Func<TEntity, bool>> filter)` — single entity
- `SelectListAsync(Expression<Func<TEntity, bool>> filter)` — list with predicate
- `SelectListAsync(IQueryable<TEntity> query)` — arbitrary LINQ query

These are not part of the base `IRepository<TEntity>` interface; they are accessible only within concrete EF Core repository subclasses, preserving the provider-agnostic base contract.

### Registration Helper

```csharp
builder.UseEntityFramework<TDbContext>(name, config => { config.TransactionsEnabled = true; });
```

Internally calls `ConfigureDatabase<EntityFrameworkDatabase<TDbContext>, EntityFrameworkDatabaseSetup, EntityFrameworkConnectionBuilder>(name, setup => ...)`.

### MongoDB EF Variant

`xSdk.Data.EntityFramework.MongoDB` provides `MongoDbContext<TContext> : DbContext` with `AutoTransactionBehavior.Never` forced, because MongoDB's EF Core driver does not support transactions in the same way as relational providers. See [ADR-011](ADR-011-mongodb-via-efcore.md) for the full MongoDB integration design.

Separate entity base classes (`MongoDbEntity`, `MongoDbEntityPK`, `MongoDbModel`, `MongoDbModelPK`) handle BSON-specific requirements.

## Consequences

### Positive

- Full EF Core ecosystem: migrations, providers, LINQ, change tracking.
- Scoped `DbContext` lifetime via `IDbContextFactory` — correct for web and hosted service scenarios.
- Transaction control is configurable per database instance, enabling in-memory and MongoDB usage.
- Query flexibility through protected filter/query helpers without polluting the base interface.

### Negative

- `DbContext` must be registered separately by the application (e.g., `AddDbContextFactory<SampleDbContext>`); the SDK does not do this automatically.
- EF Core migrations are the application's responsibility — the SDK provides no migration runner.
- The `ExecuteInternalAsync` wrapping of every operation adds overhead for simple reads that do not need transactions.
