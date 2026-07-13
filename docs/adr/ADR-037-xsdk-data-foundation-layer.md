---
title: "ADR-037: xSdk.Data Foundation Layer"
status: "Accepted"
date: "2026-07-13"
authors: "xSdk Team"
tags: ["architecture", "data", "dependency-injection"]
supersedes: "ADR-005, ADR-006"
superseded_by: ""
---

# ADR-037: xSdk.Data Foundation Layer

## Status

Accepted

**Supersedes:** [ADR-005](ADR-005-repository-pattern-with-factory.md) and [ADR-006](ADR-006-provider-agnostic-data-layer.md)

This ADR consolidates and extends the concepts from ADR-005 (Repository Pattern with Factory) and ADR-006 (Provider-Agnostic Data Layer) into a comprehensive foundation layer documentation.

## Context

The xSdk data layer abstractions ([ADR-006](ADR-006-provider-agnostic-data-layer.md)) define interfaces (`IDatabase`, `IRepository`, `IConnectionBuilder`) but need a concrete implementation layer that handles:

1. **Repository Base Implementation** — common CRUD operations and demo-mode delegation shared by all providers
2. **Database Lifecycle Management** — connection pooling, keyed DI registration, and handler-based access ([ADR-028](ADR-028-database-handler-objectpool.md))
3. **Factory and Builder Pattern** — unified API for configuring and creating data layer components
4. **Fake/Demo Data Support** — in-memory test repositories for development and demos ([ADR-012](ADR-012-demo-fake-repository-mode.md))
5. **Object Mapping** — DTO/entity projection utilities ([ADR-018](ADR-018-mapster-object-mapping.md))

Without a dedicated foundation library, each provider would duplicate repository base logic, DI registration patterns, and mapping utilities.

## Decision

`xSdk.Data` is introduced as the foundation layer that implements core data access patterns and provides the registration API used by all provider-specific libraries.

### Package

| Property         | Value                                                                               |
|------------------|-------------------------------------------------------------------------------------|
| Library          | `libs/xSdk.Data/`                                                                   |
| Package name     | `xSdk.Data`                                                                         |
| Target framework | `net10.0`                                                                           |
| Dependencies     | `xSdk.Core` (abstractions), `Mapster`, `Bogus` (fake data), `ObjectPool` (pooling) |

### Core Components

#### Repository Base Classes

```
Repository                 ← abstract base: Dispose, IsDemoMode, GetTableName
  └── Repository<TEntity>  ← CRUD contract + demo mode branching
```

`Repository` provides:
- `IsDemoMode` — when `true`, all operations delegate to `FakeRepository<TEntity>`
- `GetTableName()` — resolves table/collection name from `[Table]` attribute or type name
- `Dispose()` — base cleanup for connection resources

`Repository<TEntity>` defines CRUD methods:
- `InsertAsync`, `InsertAsync(bulk)`, `RemoveAsync`, `SelectAsync`, `SelectListAsync`, `UpdateAsync`, `UpsertAsync`
- `ExecuteAsDemoIfEnabledAsync<TResult>` — transparent demo delegation

Provider-specific repositories inherit from `Repository<TEntity>` and implement the abstract methods.

#### DatalayerBuilder

```csharp
public interface IDatalayerBuilder
{
    IDatalayerBuilder ConfigureDatabase<TDatabase, TDatabaseOptions>(
        string name, Action<TDatabaseOptions> factory)
        where TDatabase : class, IDatabase;

    IDatalayerBuilder MapRepository<TInterface, TImplementation>(
        params string[] dataProviders)
        where TImplementation : class, TInterface;
}
```

`ConfigureDatabase` registers:
1. `ObjectPool<TDatabase>` (keyed singleton, key = `name`)
2. `IDatabaseHandler<TDatabase>` (keyed singleton, key = `name`)
3. `TDatabaseOptions` (named options, name = `name`)

`MapRepository` registers:
- `TInterface` (keyed scoped, key = `dataProviders[0]`) → `TImplementation`

Keyed service resolution (.NET 8+) enables multi-database scenarios where different repositories route to different named databases.

#### DatalayerFactory

```csharp
public interface IDatalayerFactory
{
    TRepository CreateRepository<TRepository>(string name);
}
```

`CreateRepository` creates a DI scope and resolves `TRepository` via keyed service (`name`). The caller is responsible for disposing the scope.

#### FakeRepository

```csharp
public class FakeRepository<TEntity> : Repository<TEntity>
    where TEntity : class, IEntity
```

In-memory implementation using `Collection<TEntity>`. Seed data is generated via `FakeGenerator` (Bogus-backed). Enabled when `IsDemoMode == true`.

#### Mapping Utilities

- `MappingFactory.CreateMapper<TProfile>()` — creates a Mapster-based mapper with custom profile
- `EntityMappingProfile` — entity ↔ DTO projection
- `ModelMappingProfile` — model ↔ entity projection

Isolates application code from Mapster's API surface, enabling future mapper replacement.

### Keyed Services and Named Options

The library uses .NET 8 keyed services for named database routing:

```csharp
services.AddDatalayer(datalayer =>
{
    datalayer
        .ConfigureDatabase<EntityFrameworkDatabase<MyDbContext>, EntityFrameworkDatabaseOptions>(
            "MainDb", opts => { opts.ConnectionString = "..."; })
        .MapRepository<ICustomerRepository, CustomerRepository>("MainDb");
});
```

At runtime:
```csharp
var repo = factory.CreateRepository<ICustomerRepository>("MainDb");
```

The factory resolves `GetRequiredKeyedService<ICustomerRepository>("MainDb")` from a scoped container.

### Object Pooling

Database connections are pooled via `Microsoft.Extensions.ObjectPool`. `DatabaseHandler<TDatabase>` wraps the pool and exposes:
- `Retrieve()` — gets a connection from the pool
- `Return(database)` — releases it back

Repositories follow the pattern:
```csharp
var db = DatabaseHandler.Retrieve();
try { /* use db */ }
finally { DatabaseHandler.Return(db); }
```

See [ADR-028](ADR-028-database-handler-objectpool.md) for details.

## Consequences

### Positive

- **POS-001**: Centralized repository base classes eliminate code duplication across providers (EF, MongoDB, FlatFile, Vault).
- **POS-002**: Keyed service registration enables multi-database scenarios without custom metadata matching.
- **POS-003**: Object pooling via `IDatabaseHandler` reduces connection churn for expensive providers (SQL Server, MongoDB).
- **POS-004**: Demo mode is transparent to consuming code — same repository interface works in both demo and production.
- **POS-005**: `MappingFactory` abstracts Mapster, enabling future mapping library replacement without breaking consumers.
- **POS-006**: Named options pattern integrates with ASP.NET Core configuration system.

### Negative

- **NEG-001**: Keyed services require .NET 8+; not compatible with older frameworks.
- **NEG-002**: Repositories must follow `Retrieve / try-finally / Return` pattern; forgetting `Return` leaks pool slots with no compile-time enforcement.
- **NEG-003**: `CreateRepository` creates a new DI scope; caller must dispose it or risk memory leaks.
- **NEG-004**: The library couples to Mapster for mapping; replacing it requires touching `MappingFactory` internals.
- **NEG-005**: `FakeRepository` uses in-memory storage; not suitable for large-scale demo scenarios or concurrent access patterns.

## Alternatives Considered

##### Keyed Services via Custom Metadata

- **ALT-001**: **Description**: Use `InternalDatabaseSetup` metadata records (original ADR-005 pattern) instead of .NET 8 keyed services.
- **ALT-002**: **Rejection Reason**: Custom metadata requires manual matching at runtime; keyed services provide built-in platform support with less code.

##### Scoped IDatabase Lifetime

- **ALT-003**: **Description**: Register `IDatabase` as scoped and let DI manage lifecycle automatically.
- **ALT-004**: **Rejection Reason**: Scoped lifetime prevents connection reuse across repositories in a single scope; ObjectPool allows explicit control.

##### Separate Demo Library

- **ALT-005**: **Description**: Move `FakeRepository` and `FakeGenerator` to a dedicated `xSdk.Data.Demo` package.
- **ALT-006**: **Rejection Reason**: Demo mode is frequently used in tests and development; separating it would require additional package references for most consumers.

## Implementation Notes

- **IMP-001**: `AddDatalayer` extension is exposed on `IServiceCollection` and returns `IHostBuilder` for chaining.
- **IMP-002**: `DatabasePoolPolicy<TDatabase>` implements `IPooledObjectPolicy<TDatabase>` and is registered before `ObjectPool<TDatabase>`.
- **IMP-003**: Repository base classes use protected abstract methods for provider-specific operations; concrete providers implement `InsertInternalAsync`, `SelectInternalAsync`, etc.
- **IMP-004**: `GetTableName()` checks `[Table]` attribute on entity, then repository type, then strips "Repository" suffix from type name.

## References

- **REF-001**: [ADR-006](ADR-006-provider-agnostic-data-layer.md) — Provider-Agnostic Data Layer Abstraction
- **REF-002**: [ADR-028](ADR-028-database-handler-objectpool.md) — Database Connection Management via IDatabaseHandler and ObjectPool
- **REF-003**: [ADR-005](ADR-005-repository-pattern-with-factory.md) — Repository Pattern with Factory-Based Initialization
- **REF-004**: [ADR-012](ADR-012-demo-fake-repository-mode.md) — Demo/Fake Repository Mode
- **REF-005**: [ADR-018](ADR-018-mapster-object-mapping.md) — Mapster Object Mapping
- **REF-006**: [.NET 8 Keyed Services](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8/runtime#keyed-di-services)
