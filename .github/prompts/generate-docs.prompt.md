---
description: 'Generate comprehensive documentation for xSDK code'
---

# Documentation Generation Guide

You are helping to generate documentation for the xSDK project. The goal is to create clear, comprehensive, and useful documentation for developers using the SDK.

## Step 1: Identify Documentation Needs

Determine what documentation is needed:

1. **XML Documentation**: For public APIs (required)
2. **README**: For library projects
3. **Code Comments**: For complex logic
4. **Usage Examples**: For common scenarios
5. **Architecture Documentation**: For design decisions
6. **Migration Guides**: For breaking changes

## XML Documentation for Public APIs

All public APIs must have XML documentation.

### Class/Interface Documentation

```csharp
/// <summary>
/// Provides Entity Framework Core-based data storage operations.
/// This implementation uses <see cref="DbContext"/> to perform CRUD operations
/// on entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
/// The type of entity to manage. Must be a reference type.
/// </typeparam>
/// <remarks>
/// This data store implementation is optimized for relational databases.
/// For large result sets, consider using pagination methods like
/// <see cref="GetPagedAsync"/>.
/// </remarks>
public class EntityFrameworkDataStore<TEntity> : IDataStore<TEntity>
    where TEntity : class
{
}
```

### Method Documentation

```csharp
/// <summary>
/// Retrieves an entity by its unique identifier.
/// </summary>
/// <param name="id">
/// The unique identifier of the entity to retrieve.
/// Must not be null or empty.
/// </param>
/// <param name="cancellationToken">
/// A token to cancel the asynchronous operation.
/// Pass <see cref="CancellationToken.None"/> to never cancel.
/// </param>
/// <returns>
/// A task that represents the asynchronous operation.
/// The task result contains the entity if found; otherwise, <see langword="null"/>.
/// </returns>
/// <exception cref="ArgumentException">
/// Thrown when <paramref name="id"/> is null or empty.
/// </exception>
/// <exception cref="OperationCanceledException">
/// Thrown when the operation is canceled via <paramref name="cancellationToken"/>.
/// </exception>
/// <example>
/// <code>
/// var user = await dataStore.GetByIdAsync("user-123", cancellationToken);
/// if (user is not null)
/// {
///     Console.WriteLine($"Found user: {user.Name}");
/// }
/// </code>
/// </example>
public async Task<TEntity?> GetByIdAsync(
    string id, 
    CancellationToken cancellationToken = default)
{
    ArgumentException.ThrowIfNullOrEmpty(id);
    return await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
}
```

### Property Documentation

```csharp
/// <summary>
/// Gets or sets the unique identifier of the entity.
/// </summary>
/// <value>
/// A string that uniquely identifies the entity within the data store.
/// Must not be null or empty for saved entities.
/// </value>
public string Id { get; set; } = string.Empty;

/// <summary>
/// Gets or sets the connection string for the database.
/// </summary>
/// <value>
/// A valid connection string. Defaults to an empty string if not configured.
/// </value>
/// <remarks>
/// This should be loaded from configuration, not hardcoded.
/// See <see href="https://docs.microsoft.com/ef/core/dbcontext-configuration">
/// EF Core configuration documentation</see> for details.
/// </remarks>
public string ConnectionString { get; set; } = string.Empty;
```

### Event Documentation

```csharp
/// <summary>
/// Occurs when an entity has been successfully saved to the data store.
/// </summary>
/// <remarks>
/// Subscribers can use this event to perform post-save operations
/// such as cache invalidation or notification dispatch.
/// </remarks>
public event EventHandler<EntitySavedEventArgs>? EntitySaved;
```

### Enum Documentation

```csharp
/// <summary>
/// Specifies the sort order for query results.
/// </summary>
public enum SortOrder
{
    /// <summary>
    /// Sort in ascending order (A to Z, 0 to 9, oldest to newest).
    /// </summary>
    Ascending = 0,

    /// <summary>
    /// Sort in descending order (Z to A, 9 to 0, newest to oldest).
    /// </summary>
    Descending = 1
}
```

## README Documentation

Each library should have a comprehensive README.

### README Template

````markdown
# xSdk.Data.EntityFramework

Entity Framework Core-based data access layer for xSDK.

## Overview

This package provides an implementation of `IDataStore<TEntity>` using Entity Framework Core, enabling relational database access with LINQ query support, change tracking, and migration capabilities.

## Features

- ✅ Generic repository pattern implementation
- ✅ Full async/await support with cancellation
- ✅ Built-in pagination, filtering, and sorting
- ✅ Transaction management
- ✅ Change tracking integration
- ✅ Support for multiple database providers (SQL Server, PostgreSQL, SQLite, etc.)

## Installation

```powershell
dotnet add package xSdk.Data.EntityFramework
```

## Quick Start

### 1. Define Your Entity

```csharp
public class User
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

### 2. Create DbContext

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
```

### 3. Configure Services

```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddEntityFrameworkDataStore<AppDbContext, User>();
```

### 4. Use in Your Application

```csharp
public class UserService
{
    private readonly IDataStore<User> _dataStore;

    public UserService(IDataStore<User> dataStore)
    {
        _dataStore = dataStore;
    }

    public async Task<User?> GetUserAsync(string id, CancellationToken cancellationToken)
    {
        return await _dataStore.GetByIdAsync(id, cancellationToken);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        return await _dataStore.AddAsync(user, cancellationToken);
    }
}
```

## Configuration

### Basic Configuration

```csharp
services.AddEntityFrameworkDataStore<AppDbContext, User>(options =>
{
    options.UseQueryTracking = false;  // Disable change tracking for reads
    options.EnableSoftDelete = true;   // Enable soft delete support
});
```

### Advanced Configuration

| Option | Description | Default |
|--------|-------------|---------|
| `UseQueryTracking` | Enable change tracking for queries | `false` |
| `EnableSoftDelete` | Support soft delete operations | `false` |
| `CommandTimeout` | Database command timeout (seconds) | `30` |
| `EnableRetryOnFailure` | Retry failed operations | `true` |
| `MaxRetryCount` | Maximum retry attempts | `3` |

## Common Patterns

### Pagination

```csharp
var pagedResult = await _dataStore.GetPagedAsync(
    pageNumber: 1,
    pageSize: 20,
    orderBy: u => u.Name,
    ascending: true,
    cancellationToken);

Console.WriteLine($"Total: {pagedResult.TotalCount}");
Console.WriteLine($"Page {pagedResult.PageNumber} of {pagedResult.TotalPages}");
```

### Filtering

```csharp
var activeUsers = await _dataStore.FindAsync(
    u => u.IsActive && u.CreatedDate > DateTime.UtcNow.AddDays(-30),
    cancellationToken);
```

### Transactions

```csharp
await _dataStore.ExecuteInTransactionAsync(async () =>
{
    await _dataStore.AddAsync(user1, cancellationToken);
    await _dataStore.AddAsync(user2, cancellationToken);
}, cancellationToken);
```

## Database Providers

This package supports all Entity Framework Core database providers:

- SQL Server
- PostgreSQL
- MySQL/MariaDB
- SQLite
- Oracle
- Cosmos DB

## Performance Considerations

- Use `AsNoTracking()` for read-only queries
- Implement pagination for large result sets
- Use projection (Select) to load only needed columns
- Avoid N+1 queries with Include/ThenInclude
- Use compiled queries for frequently executed queries

## Migration Guide

### From 1.x to 2.x

Breaking changes in version 2.0:

1. `IDataStore.Save()` renamed to `IDataStore.AddAsync()`
2. All methods now require `CancellationToken`
3. `GetAll()` removed - use `GetPagedAsync()` instead

See [MIGRATION.md](./MIGRATION.md) for complete details.

## Troubleshooting

### Connection Issues

If you encounter connection errors, verify:

1. Connection string is correct
2. Database server is accessible
3. Firewall rules allow connection
4. Credentials have appropriate permissions

### Performance Issues

For slow queries:

1. Check query execution plan
2. Ensure proper indexes exist
3. Use pagination for large result sets
4. Consider using raw SQL for complex queries

## Contributing

See [CONTRIBUTING.md](../../CONTRIBUTING.md) in the root repository.

## License

MIT License - see [LICENSE](../../LICENSE)

## Links

- [Documentation](https://xsdk.dev/docs/data/entityframework)
- [API Reference](https://xsdk.dev/api/xsdk.data.entityframework)
- [Samples](../../demos/datalayer-entityframework)
- [Issue Tracker](https://github.com/velvet-lab/xsdk/issues)
````

## Inline Comments

Use inline comments sparingly for complex logic:

```csharp
// Use ConfigureAwait(false) because this is library code and we don't want
// to capture the synchronization context, which could cause deadlocks
await SaveCoreAsync(entity, cancellationToken).ConfigureAwait(false);

// MongoDB provider has a known limitation with DateTime comparison operations.
// Convert to Unix timestamp for reliable comparison.
// See: https://github.com/mongodb/mongo-efcore-provider/issues/123
var timestamp = dateTime.ToUniversalTime().Subtract(UnixEpoch).TotalSeconds;
```

## Architecture Documentation

For significant design decisions, create ADRs (Architecture Decision Records):

```markdown
# ADR-003: Use Repository Pattern for Data Access

## Status
Accepted

## Context
We need a consistent abstraction for data access across multiple storage providers
(SQL, NoSQL, file-based, etc.). Different storage technologies have different APIs
and capabilities.

## Decision
Implement the Repository pattern with a generic `IDataStore<TEntity>` interface
that provides standard CRUD operations. Each storage provider implements this
interface with provider-specific optimizations.

## Consequences

### Positive
- Consistent API regardless of storage technology
- Easy to swap implementations for testing
- Centralized data access logic
- Provider-agnostic business logic

### Negative
- Additional abstraction layer (slight complexity)
- May not expose all provider-specific features
- Can lead to leaky abstractions if not carefully designed

## Alternatives Considered

1. **Direct provider usage**: More flexible but no consistency
2. **Active Record pattern**: Simpler but couples entities to persistence
3. **CQRS with MediatR**: More complex, better for large systems
```

## Usage Examples

Create example projects in `demos/` folder with real-world scenarios.

## Documentation Checklist

- [ ] All public classes have XML `<summary>`
- [ ] All public methods have XML `<summary>` and `<param>` tags
- [ ] All parameters documented with `<param>`
- [ ] Return values documented with `<returns>`
- [ ] Exceptions documented with `<exception>`
- [ ] Complex APIs have `<example>` with code
- [ ] Type parameters have `<typeparam>`
- [ ] Properties have `<summary>` and optionally `<value>`
- [ ] README exists with installation, usage, and examples
- [ ] Architecture decisions documented in ADRs
- [ ] Inline comments explain "why", not "what"
- [ ] External references use `<see href="url">`
- [ ] Cross-references use `<see cref="Type"/>`

## Documentation Best Practices

1. **Write for your audience**: Assume readers are competent C# developers but new to your library
2. **Be concise**: Clear and brief is better than long and wordy
3. **Provide examples**: Show, don't just tell
4. **Keep it current**: Update docs when code changes
5. **Use proper grammar**: No typos or grammatical errors
6. **Link generously**: Cross-reference related APIs
7. **Document edge cases**: Explain behavior for null, empty, boundary values
8. **Explain exceptions**: Document when and why exceptions are thrown
9. **Show alternatives**: Document different ways to accomplish tasks
10. **Version awareness**: Note version-specific features or breaking changes

Remember: Good documentation makes your library easy to use and reduces support burden.
