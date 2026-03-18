# ADR-018: Mapster for Object-to-Object Mapping

## Status

Accepted

## Date

2026-03-17

## Context

The data layer frequently requires mapping between:

- **Entity** (database row/document) ↔ **Model** (DTO for API responses or business logic).
- `EFEntity` ↔ `EFModel`, `NoSqlEntity` ↔ `NoSqlModel`, etc.

AutoMapper was considered but has known performance overhead and a history of breaking changes. `Mapster` is a high-performance alternative with a similar configuration API and source-generator support for near-zero-alloc mapping.

## Decision

`xSdk.Data` uses **Mapster** (via `MapsterMapper`) for all object-to-object mapping.

### MappingProfile

`MappingProfile` is an abstract base class. Concrete mapping configurations extend it:

```csharp
public abstract class MappingProfile
{
    protected abstract void Configure(TypeAdapterConfig config);

    internal TypeAdapterConfig CreateConfig() { ... }
    internal TypeAdapterConfig CreateConfig(Action<TypeAdapterConfig> additional) { ... }
}
```

`Configure(TypeAdapterConfig)` is the single method concrete profiles must implement, using Mapster's fluent `config.NewConfig<TSource, TDest>()` API.

Pre-built SDK profiles:

| Profile | Purpose |
|---|---|
| `EntityMappingProfile` | `IEntity` → `IModel` and reverse |
| `ModelMappingProfile` | `IModel` → `IEntity` and reverse |

### MappingFactory

```csharp
var mapper = MappingFactory.CreateMapper<MyProfile>();
var mapper = MappingFactory.CreateMapper<MyProfile>(config => { ... });
```

`MappingFactory` isolates application code from Mapster's `TypeAdapter` static API, making it replaceable and testable.

### Usage Pattern

```csharp
var mapper = MappingFactory.CreateMapper<MyMappingProfile>();
var dto = mapper.Map<CustomerModel>(customerEntity);
var entity = mapper.Map<CustomerEntity>(customerDto);
```

### Provider-Specific Converters

Each data provider (`xSdk.Data.EntityFramework.MongoDB/Converters/`, `xSdk.Data.NoSql/Converters/`) contains `System.Text.Json` converters for provider-specific types. These are separate from the Mapster mapping profiles because they address JSON serialization (for HTTP), not object-to-object mapping.

## Consequences

### Positive

- Mapster is significantly faster than AutoMapper at runtime.
- `MappingFactory` decouples consumers from Mapster's API — swapping the mapping library requires changes only in `MappingFactory` and `MappingProfile`.
- Profile-per-domain-area keeps mapping configurations organized.
- Source generator support (`Mapster.Tool`) is available for compile-time mapping generation if performance becomes critical.

### Negative

- Mapster's configuration API is less widely known than AutoMapper's; developers familiar with AutoMapper face a learning curve.
- `MappingFactory.CreateMapper<TProfile>()` creates a new `TypeAdapterConfig` per call; callers should cache the result for hot paths.
- The SDK does not enforce caching of `IMapper` instances — individual repositories or services may inadvertently create new config+mapper pairs on every operation.
