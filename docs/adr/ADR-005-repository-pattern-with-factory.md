# ADR-005: Repository Pattern with Factory-Based Initialization

## Status

Accepted

## Date

2026-03-17

## Context

Data access in the SDK must be:

1. **Decoupled from the storage backend** — the same `ISampleRepository` can use EF Core, LiteDB, or FlatFile depending on the configured provider.
2. **Scoped within a DI lifetime** — each request/unit-of-work gets its own repository instance with its own database connection.
3. **Testable** — a `FakeRepository` must be injectable without a real database.
4. **Named** — an application can register multiple databases of the same type (e.g., two SQL databases, or EF Core + FlatFile simultaneously) and route repositories to specific named databases.

Standard `IRepository` registration via `AddScoped<TRepository>()` does not handle the multi-database routing or the two-step initialization (create repository → assign database) cleanly via DI alone.

## Decision

A `DatalayerFactory` / `DatalayerBuilder` pattern is introduced.

### DatalayerBuilder (DI setup time)

Registered via `services.AddDatalayer(builder => { ... })` within host startup.

```
IDatalayerBuilder
  └── DatalayerBuilder
        ├── ConfigureDatabase<TDatabase, TSetup, TConnectionBuilder>(name)
        └── ConfigureRepository<TInterface, TImpl>(dataProviders[])
```

`ConfigureDatabase` registers:
- `TConnectionStringBuilder` as **Singleton**
- `TDatabase` as **Scoped**
- `InternalDatabaseSetup` (a metadata record linking name, types) as **Singleton**

`ConfigureRepository` uses `TryAddScoped` with a factory that:
1. Creates the `TImplementation` via `ActivatorUtilities`.
2. Resolves all `InternalDatabaseSetup` singletons.
3. Filters by the `dataProviders` names the repository was mapped to.
4. Assigns the matching `IDatabase` and `IConnectionBuilder` to the repository.

### DatalayerFactory (runtime)

```csharp
IDatalayerFactory factory = services.GetRequiredService<IDatalayerFactory>();
var repo = factory.CreateRepository<ISampleRepository>("MyDatabase");
```

Creates a new DI scope, resolves the repository from that scope (triggering the scoped factory above), and returns the fully initialized repository. The caller is responsible for the scope lifetime.

### Repository Hierarchy

```
IRepository
  └── Repository              ← base: Dispose, IsDemoMode, GetTableName
        └── Repository<TEntity>   ← CRUD contract + demo mode branching
              ├── EntityFrameworkRepository<TDbContext, TEntity>
              ├── FlatFileRepository<TEntity>
              ├── NoSqlRepository<TEntity>       (LiteDB)
              └── ReadOnlyVaultRepository        (Vault read)
                    └── VaultRepository          (Vault write)
```

`Repository<TEntity>` exposes:
- `InsertAsync`, `InsertAsync(bulk)`, `RemoveAsync`, `SelectAsync`, `SelectListAsync`, `UpdateAsync`, `UpsertAsync`
- `ExecuteAsDemoIfEnabledAsync<TResult>` — transparent delegation to `FakeRepository<TEntity>` when `IsDemoMode == true`

### Table Name Resolution

`Repository.GetTableName()` uses (in order):
1. `[Table("name")]` attribute on `TEntity`
2. `[Table("name")]` attribute on the repository type itself
3. Repository type name with `Repository` / `Store` suffix stripped

## Consequences

### Positive

- Fully decoupled from the storage backend — switching providers requires only a DI reconfiguration.
- Named database routing allows multiple backends simultaneously in one application.
- Demo mode is transparent to consuming code.
- Scoped repositories ensure proper connection lifecycle management.

### Negative

- The two-step initialization (DI resolves the repo, then `Configure(database)` is called) is non-standard and bypasses constructor injection for the database — internal state mutation after construction.
- `DatalayerFactory.CreateRepository` creates a new `IServiceScope` but requires the caller to manage its lifetime; not disposing it leaks scoped services.
- `services.BuildServiceProvider()` is called inside `AddDatalayer(...)` to pre-build the factory; this is an anti-pattern in ASP.NET Core and can cause the container to be built twice.
