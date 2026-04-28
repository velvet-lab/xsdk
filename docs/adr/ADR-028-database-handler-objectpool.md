---
title: "ADR-028: Database Connection Management via IDatabaseHandler and ObjectPool"
status: "Accepted"
date: "2026-04-27"
authors: "xSdk Team"
tags: ["architecture", "data", "connection-management", "performance"]
supersedes: ""
superseded_by: ""
---

# ADR-028: Database Connection Management via IDatabaseHandler and ObjectPool

## Status

Accepted

## Date

2026-04-27

## Context

The provider-agnostic data layer ([ADR-006](ADR-006-provider-agnostic-data-layer.md)) originally exposed `IDatabase` directly to repositories. Each repository held a reference to its `IDatabase` connection object and managed its lifecycle internally via a `ConcurrentDictionary`-based connection cache inside `Database.Open<TConnection>()`.

This design had limitations:

1. **No pooling** — every `DatalayerFactory.CreateRepository<T>()` call created a new DI scope and a new database connection. Under load, connection exhaustion was a real risk for providers with expensive connections (e.g., EF Core against SQL Server or MongoDB).
2. **Unclear ownership** — repositories directly owned `IDatabase` but disposal semantics were inconsistent across providers.
3. **Tight coupling** — `Repository<TEntity>` held a direct `IDatabase` reference, making it harder to change connection management strategies per provider.
4. **Named database routing** — the previous `InternalDatabaseSetup` approach manually matched repositories to databases by name. With .NET 8+ keyed services, the platform provides a cleaner mechanism.

## Decision

Database connection management is redesigned around **`IDatabaseHandler`**, backed by **`Microsoft.Extensions.ObjectPool`**, with named (keyed) DI registration.

### IDatabaseHandler

```csharp
public interface IDatabaseHandler
{
    string? DatalayerName { get; }
    IServiceProvider? Services { get; }
    IDatabase? Retrieve();
    void Return(IDatabase? database);
}

public interface IDatabaseHandler<TDatabase> : IDatabaseHandler
    where TDatabase : class, IDatabase { }
```

`Retrieve()` fetches a database instance from the pool; `Return()` releases it back. This is an explicit acquire/release contract rather than implicit scoped-lifetime management.

### ObjectPool<TDatabase>

`DatalayerBuilder.ConfigureDatabase<TDatabase, TDatabaseOptions>(name, factory)` registers:

1. **`ObjectPool<TDatabase>`** (keyed singleton, key = `name`) — backed by `DefaultObjectPool<TDatabase>` using `DatabasePoolPolicy<TDatabase>`.
2. **`IDatabaseHandler<TDatabase>`** (keyed singleton, key = `name`) — wraps the pool and exposes `Retrieve`/`Return`.
3. **`TDatabaseOptions`** (named options, name = `name`) — database-specific configuration bound via `factory`.

`DatabasePoolPolicy<TDatabase>` implements `IPooledObjectPolicy<TDatabase>`:
- `Create()` — instantiates a new database via `ActivatorUtilities.CreateInstance`.
- `Return(item)` — always returns `true` (pooled), allowing the pool to reuse instances.

### Repository Access Pattern

`Repository` accesses its database exclusively through the handler:

```csharp
protected IDatabaseHandler? DatabaseHandler
    => Services?.GetRequiredKeyedService<IDatabaseHandler>(DatalayerName);
```

Concrete repositories call `DatabaseHandler.Retrieve()` at the start of an operation and `DatabaseHandler.Return(db)` in a `finally` block:

```csharp
var db = DatabaseHandler.Retrieve();
try { /* use db */ }
finally { DatabaseHandler.Return(db); }
```

### DatalayerFactory (Keyed Services)

`DatalayerFactory.CreateRepository<TRepository>(name)` uses .NET 8 keyed service resolution:

```csharp
TRepository repo = scope.ServiceProvider.GetRequiredKeyedService<TRepository>(name);
```

Repositories are registered as keyed scoped services by `DatabaseBuilder.MapRepository<TInterface, TImpl>(name)`. This replaces the previous `InternalDatabaseSetup` metadata-record matching approach.

### Registration Summary

```
services.AddDatalayer(datalayer =>
{
    datalayer
        .ConfigureDatabase<EntityFrameworkDatabase<MyDbContext>, EntityFrameworkDatabaseOptions>(
            "MainDb", opts => { opts.ConnectionString = "..."; })
        .MapRepository<ICustomerRepository, CustomerRepository>();
});
```

Internally this registers:
- `ObjectPool<EntityFrameworkDatabase<MyDbContext>>` (keyed: "MainDb")
- `IDatabaseHandler<EntityFrameworkDatabase<MyDbContext>>` (keyed: "MainDb")
- `EntityFrameworkDatabaseOptions` named options (name: "MainDb")
- `ICustomerRepository` keyed scoped (key: "MainDb") → `CustomerRepository`

## Consequences

### Positive

- **POS-001**: Connection pooling reduces connection churn for providers with expensive `Open()` calls (SQL Server, MongoDB, Vault).
- **POS-002**: `IDatabaseHandler` decouples repositories from direct `IDatabase` ownership, allowing pool tuning (min/max size) per provider without changing repository code.
- **POS-003**: Named (keyed) DI registration replaces manual metadata-record matching, leveraging built-in .NET 8 platform features instead of custom infrastructure.
- **POS-004**: `DatabaseHandler.Return()` is an explicit contract, making connection lifecycle visible in repository code rather than implicit via `Dispose`.

### Negative

- **NEG-001**: Repositories must follow the `Retrieve / try-finally / Return` pattern. Forgetting `Return` leaks pool slots; no compile-time enforcement.
- **NEG-002**: `ObjectPool` is a singleton; pooled database objects must be thread-safe or repositories must be scoped so at most one goroutine holds a given instance.
- **NEG-003**: `GetRequiredKeyedService` requires .NET 8+; not compatible with older `IServiceProvider` implementations.

## Alternatives Considered

##### Scoped IDatabase via Scoped DI Lifetime

- **ALT-001**: **Description**: Register `IDatabase` as `Scoped` so each request creates and disposes a connection automatically.
- **ALT-002**: **Rejection Reason**: Scoped lifetime destroys the connection on scope disposal, preventing reuse for multi-repository operations within a single scope. ObjectPool explicitly controls reuse.

##### Retain ConcurrentDictionary-Based Cache

- **ALT-003**: **Description**: Keep `Database.Open<T>(persistConnection)` with `ConcurrentDictionary` caching.
- **ALT-004**: **Rejection Reason**: Cache entries are never evicted under memory pressure; ObjectPool integrates with `IMemoryPressureNotifications` and can trim under pressure.

## Implementation Notes

- **IMP-001**: `DatabasePoolPolicy<TDatabase>` must be registered before `DatabaseHandler<TDatabase>` because the policy is constructor-injected into `DefaultObjectPool`.
- **IMP-002**: `HostBuilderExtensions` for `xSdk.Data` registers `ObjectPoolProvider` as `DefaultObjectPoolProvider` singleton — this is required before any `ObjectPool<T>` can be created.
- **IMP-003**: `DatabaseHandler<TDatabase>.Retrieve()` sets `DatalayerName` and `Services` on the pooled `Database` instance before returning it, enabling the database to access named options via `GetOptions<TOptions>(OptionsScope.Datalayer)`.

## References

- **REF-001**: [ADR-006](ADR-006-provider-agnostic-data-layer.md) — Provider-agnostic data layer (extended by this ADR)
- **REF-002**: [ADR-005](ADR-005-repository-pattern-with-factory.md) — Repository pattern (keyed-service routing supersedes `InternalDatabaseSetup` pattern)
- **REF-003**: [Microsoft ObjectPool documentation](https://learn.microsoft.com/en-us/aspnet/core/performance/objectpool)
- **REF-004**: [.NET 8 Keyed Services](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8/runtime#keyed-di-services)
