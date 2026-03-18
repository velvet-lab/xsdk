# ADR-011: MongoDB Access via EF Core Provider

## Status

Accepted

## Date

2026-03-17

## Context

MongoDB is a widely-used document database. The SDK already uses EF Core as the relational abstraction layer ([ADR-007](ADR-007-entity-framework-data-provider.md)). MongoDB's official EF Core provider (`MongoDB.EntityFrameworkCore`) allows using `DbContext` with MongoDB, which would let MongoDB share the existing `EntityFrameworkRepository` infrastructure.

The alternative would be to create a separate `xSdk.Data.MongoDB` library that wraps the MongoDB C# driver directly (similar to how `xSdk.Data.NoSql` wraps LiteDB). This was rejected for the following reasons:

1. It would duplicate the EF Core repository infrastructure.
2. Application developers familiar with EF Core would face a different API surface.
3. MongoDB's EF Core provider is officially maintained by MongoDB, Inc.

## Decision

MongoDB is supported via the `xSdk.Data.EntityFramework.MongoDB` library, which provides:

### MongoDbContext

```csharp
public class MongoDbContext<TContext> : DbContext
    where TContext : DbContext
{
    public MongoDbContext(DbContextOptions<TContext> options) : base(options)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}
```

`AutoTransactionBehavior.Never` is set because MongoDB's EF provider does not support `DbContext`-level transactions in the same way as relational providers. Applications must use `TransactionsEnabled = false` in their `EntityFrameworkDatabaseSetup`.

### Entity Base Classes

Four base classes are provided to accommodate MongoDB's BSON requirements:

| Class | Description |
|---|---|
| `MongoDbEntity` | String-keyed entity with `[BsonId]` on `Id` |
| `MongoDbEntityPK` | Entity with `ObjectId`-based primary key |
| `MongoDbModel` | DTO model (string key) |
| `MongoDbModelPK` | DTO model with `ObjectId` key |

### BSON Converters

`xSdk.Data.EntityFramework.MongoDB/Converters/` contains System.Text.Json converters for MongoDB-specific types (e.g., `ObjectId`). These are needed when entities are serialized over HTTP (e.g., in API responses) because `ObjectId` is a MongoDB-specific type not understood by `System.Text.Json` out of the box.

### DbContext Registration

Applications register a `MongoDbContext<TContext>` subclass the same way as any EF Core context:

```csharp
services.AddDbContextFactory<MyMongoDbContext>(options =>
    options.UseMongoDB(connectionString, databaseName));

builder.UseEntityFramework<MyMongoDbContext>("mongo", config =>
{
    config.TransactionsEnabled = false;
});
```

### Repository

No separate repository class is needed — `EntityFrameworkRepository<MyMongoDbContext, TEntity>` works directly, since the `MongoDbContext` is a standard `DbContext`.

## Consequences

### Positive

- Reuses the entire EF Core repository stack — no duplicated CRUD implementation.
- Application code is identical for SQL and MongoDB (same repository interface, same DI setup pattern).
- LINQ queries work against MongoDB collections via the EF provider.

### Negative

- MongoDB's EF provider is still maturing — not all EF Core features (complex joins, raw SQL, full-text search) are supported.
- `AutoTransactionBehavior.Never` means EF Core change tracking and SaveChanges work differently; developers must understand the implications.
- `ObjectId` serialization converters must be explicitly registered for JSON APIs — missing this causes serialization failures.
- The split across two packages (`xSdk.Data.EntityFramework` + `xSdk.Data.EntityFramework.MongoDB`) requires both to be referenced for MongoDB support.
