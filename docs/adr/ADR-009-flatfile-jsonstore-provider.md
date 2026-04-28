# ADR-009: JsonFlatFileDataStore as Flat-File Provider

## Status

Accepted

## Date

2026-03-17

## Context

Some applications need the simplest possible persistence: human-readable JSON files on disk, with no schema or migration management. Typical scenarios:

- Configuration stores managed by operations teams (editing JSON directly).
- Small reference data sets that rarely change.
- Local development environments where inspectable storage is preferred.
- Scenarios where Git-versioned data files serve as the data store.

## Decision

`xSdk.Data.FlatFile` uses the **JsonFlatFileDataStore** NuGet package as the storage engine.

### Components

| Class                                              | Responsibility                                                                  |
|----------------------------------------------------|---------------------------------------------------------------------------------|
| `FlatFileDatabase`                                 | Wraps `DataStore` (from `JsonFlatFileDataStore`); opens via `Open<DataStore>()` |
| `FlatFileConnectionBuilder`                        | Resolves file path and name from setup; supports placeholder substitution       |
| `FlatFileDatabaseSetup` / `IFlatFileDatabaseSetup` | Holds `Path`, `FileName`, `UseLowerCamelCase` flag                              |
| `FlatFileEntity`                                   | Entity base with Id property                                                    |
| `FlatFileModel`                                    | Model base                                                                      |
| `FlatFileRepository<TEntity>`                      | Repository implementation                                                       |
| `ServiceCollectionExtensions`                      | `UseFlatFile(name, config)` extension method                                    |

### Repository Operations

All CRUD operations delegate to `IDocumentCollection<TEntity>` provided by `JsonFlatFileDataStore`:

- `InsertAsync` → `col.InsertOneAsync(entity)`
- `InsertAsync(bulk)` → `col.InsertManyAsync(entities)`
- `RemoveAsync(primaryKey)` → `col.DeleteOneAsync(x => x.PrimaryKey == primaryKey)`
- `SelectAsync(primaryKey)` → `col.AsQueryable().SingleOrDefault(x => x.PrimaryKey == primaryKey)`
- `SelectListAsync()` → `col.AsQueryable()`
- `UpdateAsync` → `col.UpdateOneAsync(primaryKey, entity)`
- `UpsertAsync` → select, then `InsertOneAsync` or `UpdateOneAsync`

Additionally, protected helper methods expose LINQ expression-based filtering:

```csharp
protected Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> filter, ...)
protected Task<IEnumerable<TEntity>> SelectListAsync(Expression<Func<TEntity, bool>> filter, ...)
protected Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> filter, ...)
```

Expression trees are converted via an internal `ConvertFilter` method to `JsonFlatFileDataStore`'s predicate format.

### Table/Collection Name

Uses the inherited `Repository.GetTableName()` which checks `[Table]` attributes first, then falls back to the repository type name with suffixes stripped. Each entity type is stored in a separate JSON array within the single data store file.

### Registration

```csharp
builder.UseFlatFile("MyStore", config =>
{
    config.Path = "/var/data";
    config.FileName = "data.json";
    config.UseLowerCamelCase = true;
});
builder.MapRepository<IMyRepository, MyRepository>();
```

## Consequences

### Positive

- Human-readable JSON — trivial to inspect and edit without tooling.
- Zero infrastructure — no database server, no driver installation.
- LINQ query support via `AsQueryable()`.
- Suitable for simple key-by-value lookups and small datasets.

### Negative

- Not suitable for concurrent write scenarios — `JsonFlatFileDataStore` uses file locks; high concurrency leads to contention.
- No transaction support — operations are individually atomic at best.
- Performance degrades for large datasets (full file parse on every operation).
- The expression-to-predicate `ConvertFilter` translation has limitations; complex LINQ expressions may not translate correctly.
- Not suitable for production systems with data integrity requirements.
