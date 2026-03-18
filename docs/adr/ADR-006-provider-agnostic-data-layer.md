# ADR-006: Provider-Agnostic Data Layer Abstraction

## Status

Accepted

## Date

2026-03-17

## Context

The SDK provides multiple data storage backends (EF Core, LiteDB, FlatFile, Vault). Application code must be able to switch between them without changing repository logic. The abstraction layer must also provide:

- A unified entity identity model (`IEntity`, `IPrimaryKey`).
- Object mapping utilities for DTO projection.
- Fake/demo data generation for development and demo environments.
- Cross-provider connection string building.

## Decision

`xSdk.Data` defines all provider-agnostic contracts and utilities.

### Entity Model

```
IEntity                 ← requires PrimaryKey (IPrimaryKey) and Id (object)
  └── IEntity<TKey>     ← typed primary key
```

`IPrimaryKey` wraps the actual primary key value and provides `GetValue()` / `GetValue<T>()`.

Entity base classes provided per provider:

| Provider | Entity Base | Model Base |
|---|---|---|
| EF Core | `EFEntity` | `EFModel` |
| MongoDB EF | `MongoDbEntity` / `MongoDbEntityPK` | `MongoDbModel` / `MongoDbModelPK` |
| FlatFile | `FlatFileEntity` | `FlatFileModel` |
| LiteDB NoSql | `NoSqlEntity` / `NoSqlEntityPK` | `NoSqlModel` / `NoSqlModelPK` |

### Connection Abstraction

```
IConnectionBuilder
  └── ConnectionBuilder   ← placeholder-based connection string builder
        ├── EntityFrameworkConnectionBuilder
        ├── FlatFileConnectionBuilder
        ├── NoSqlConnectionBuilder
        └── VaultConnectionBuilder
```

`ConnectionBuilder.ResolvePlaceholders(template)` replaces `{name}` tokens in a template string with the registered connection properties — a simple, provider-neutral way to build connection strings from configuration values.

### Database Abstraction

```
IDatabase
  └── Database   ← ConcurrentDictionary-based connection cache, Open<TConnection>(), Close()
        ├── EntityFrameworkDatabase<TDbContext>
        ├── FlatFileDatabase
        ├── NoSqlDatabase
        └── VaultDatabase
```

`Database.Open<TConnection>(persistConnection)` supports both transient and singleton connection modes. Persistent connections are cached by a Base64-encoded name key. `ProcessExit` event handlers ensure connections are closed on application shutdown.

### Setup Abstraction

```
IDatabaseSetup
  └── DatabaseSetup   ← base: Name, validates connection config
        ├── EntityFrameworkDatabaseSetup
        ├── FlatFileDatabaseSetup
        ├── NoSqlDatabaseSetup
        └── VaultDatabaseSetup
```

### Object Mapping

`MappingFactory.CreateMapper<TProfile>()` wraps the Mapster `TypeAdapter`/`Mapper` API behind an SDK-specific `MappingProfile` base class. This isolates application code from Mapster's API surface and allows the underlying mapper to be replaced.

`EntityMappingProfile` and `ModelMappingProfile` are pre-built profiles for mapping between entity and model types.

### Fake/Demo Data

`FakeGenerator` (backed by `Bogus`) generates synthetic data for demos. `FakeRepository<TEntity>` is an in-memory `Repository<TEntity>` implementation that uses a `Collection<TEntity>` as storage. See [ADR-012](ADR-012-demo-fake-repository-mode.md) for details.

### Data Converters

`xSdk.Data.*/Converters/` folders contain provider-specific JSON converters (e.g., `IPrimaryKey` JSON serialization for LiteDB, MongoDB BSON converters), keeping serialization concerns close to the provider that needs them.

## Consequences

### Positive

- Adding a new data provider requires implementing four types only: `IDatabase`, `IConnectionBuilder`, `IDatabaseSetup`, and `Repository<TEntity>` subclass.
- Entity base classes enforce consistent identity across providers.
- `MappingFactory` centralizes object projection; consumers are not directly coupled to Mapster.

### Negative

- The `IDatabase.Open<TConnection>()` method requires callers to know the concrete connection type (e.g., `DbContext`, `LiteDatabaseAsync`) — a small provider leak through the abstraction.
- The connection cache in `Database` uses a mutable static `ConcurrentDictionary`; the comment in the code acknowledges it is "not really threadsafe" for the `_wait4Connection` pattern and requires future improvement.
