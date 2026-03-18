---
description: 'Documentation standards for xSDK project'
applyTo: '**/*.cs,**/*.md'
---

# Documentation Standards

## XML Documentation for Public APIs

All public APIs (classes, interfaces, methods, properties, events) must have XML documentation comments.

### Required Elements

**Classes and Interfaces:**
```csharp
/// <summary>
/// Provides data storage and retrieval operations for entities.
/// </summary>
/// <typeparam name="TEntity">The type of entity to manage.</typeparam>
public interface IDataStore<TEntity> where TEntity : class
{
}
```

**Methods:**
```csharp
/// <summary>
/// Retrieves an entity from the store by its unique identifier.
/// </summary>
/// <param name="id">The unique identifier of the entity to retrieve.</param>
/// <param name="cancellationToken">
/// A token to cancel the asynchronous operation.
/// </param>
/// <returns>
/// A task that represents the asynchronous operation. The task result contains
/// the entity if found; otherwise, null.
/// </returns>
/// <exception cref="ArgumentNullException">
/// Thrown when <paramref name="id"/> is null or empty.
/// </exception>
Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
```

**Properties:**
```csharp
/// <summary>
/// Gets or sets the unique identifier of the entity.
/// </summary>
public string Id { get; set; } = string.Empty;
```

**Events:**
```csharp
/// <summary>
/// Occurs when an entity is successfully saved to the store.
/// </summary>
public event EventHandler<EntitySavedEventArgs>? EntitySaved;
```

### XML Documentation Tags

Use these standard XML tags:

- `<summary>`: Brief description of what the API does
- `<param>`: Description of each parameter
- `<typeparam>`: Description of generic type parameters
- `<returns>`: Description of return value
- `<exception>`: Exceptions that can be thrown
- `<remarks>`: Additional detailed information
- `<example>`: Code examples demonstrating usage
- `<seealso>`: References to related types or members
- `<value>`: Description of property values

### Code Examples

Include code examples for complex APIs:

```csharp
/// <summary>
/// Configures the data store with the specified options.
/// </summary>
/// <param name="options">The configuration options for the data store.</param>
/// <returns>The configured data store builder.</returns>
/// <example>
/// <code>
/// services.AddDataStore&lt;User&gt;(options =>
/// {
///     options.ConnectionString = "Server=localhost;Database=users";
///     options.EnableCaching = true;
/// });
/// </code>
/// </example>
public static DataStoreBuilder<TEntity> ConfigureDataStore<TEntity>(
    Action<DataStoreOptions> options)
    where TEntity : class
{
    // Implementation
}
```

## README Documentation

Each library project should have a README.md with:

1. **Project Overview**: Brief description of what the library does
2. **Installation**: NuGet package installation instructions
3. **Quick Start**: Minimal code example to get started
4. **Key Features**: List of main capabilities
5. **Usage Examples**: Common scenarios with code samples
6. **Configuration**: Available options and settings
7. **Dependencies**: Required packages and versions
8. **Contributing**: Link to contribution guidelines
9. **License**: Project license information

### README Template

```markdown
# xSdk.Data.EntityFramework

Provides Entity Framework Core-based data access implementation for xSDK.

## Installation

```powershell
dotnet add package xSdk.Data.EntityFramework
```

## Quick Start

```csharp
services.AddDatalayer(datalayer =>
{
    datalayer.UseEntityFramework<MyDbContext>("AppDb", config =>
    {
        config.TransactionsEnabled = true;
    });
    datalayer.MapRepository<IMyRepository, MyRepository>("AppDb");
});
```

## Features

- `EntityFrameworkRepository<TDbContext, TEntity>` repository implementation
- Async/await support with `CancellationToken` propagation
- LINQ filter helpers via protected `SelectAsync`/`SelectListAsync` overloads
- Configurable transaction support (`TransactionsEnabled`)
- Demo mode via `CreateFakesAsync()` override

## Configuration

[Detailed configuration options...]

## Dependencies

- Microsoft.EntityFrameworkCore 10.0+
- xSdk.Data 1.0+

## License

Apache-2.0 License - see LICENSE file for details
```

## Inline Comments

Use inline comments sparingly and only when necessary:

**When to use comments:**
- Explaining **why** something is done (not what or how)
- Documenting workarounds or non-obvious solutions
- Explaining complex algorithms or business rules
- Providing context for future maintainers

**When NOT to use comments:**
- Stating the obvious
- Repeating what the code already says
- Commenting out code (delete it instead)
- Adding TODO without a tracking issue

### Good Comments

```csharp
// Use ConfigureAwait(false) because this is library code
// and we don't want to capture the synchronization context
await repository.SaveAsync(entity).ConfigureAwait(false);

// Workaround for MongoDB provider limitation with DateTime comparison
// See: https://github.com/mongodb/mongo-efcore-provider/issues/123
var date = DateTime.UtcNow.Date;
```

### Bad Comments

```csharp
// Get user by ID
var user = await repository.GetByIdAsync(id);

// Loop through items
foreach (var item in items)
{
    // Process item
    ProcessItem(item);
}
```

## Architecture Documentation

For significant architectural decisions, create Architecture Decision Records (ADRs) in `/docs/adr/`:

```markdown
# ADR-001: Use Repository Pattern for Data Access

## Status
Accepted

## Context
We need a consistent way to abstract data access across multiple providers.

## Decision
Implement the repository pattern with generic interfaces.

## Consequences
- Provides consistent API across all data providers
- Enables easier testing with mock repositories
- Adds abstraction layer (slight performance overhead)
```

## API Documentation Generation

The project uses XML documentation to generate API documentation:

- XML documentation files are generated during build
- Documentation is included in NuGet packages
- Use /// comments, not /* */ or //

## Changelog

Maintain a CHANGELOG.md following [Keep a Changelog](https://keepachangelog.com/) format:

```markdown
# Changelog

## [Unreleased]

### Added
- New feature X

### Changed
- Updated behavior of Y

### Deprecated
- Method Z will be removed in v2.0

### Removed
- Obsolete method A

### Fixed
- Bug in B

### Security
- Patched vulnerability in C

## [1.0.0] - 2024-01-15

### Added
- Initial release
```

## Documentation Best Practices

1. **Write documentation first**: Document the API before implementing
2. **Keep it current**: Update docs when code changes
3. **Be concise**: Clear and brief is better than verbose
4. **Use examples**: Show, don't just tell
5. **Think about the reader**: Write for developers using your library
6. **Spell check**: Use proper grammar and spelling
7. **Link references**: Link to related APIs and external docs
8. **Version awareness**: Document version-specific behavior

## Documentation Review Checklist

Before submitting code:

- [ ] All public APIs have XML documentation
- [ ] XML docs include all required tags (summary, param, returns)
- [ ] Exceptions are documented
- [ ] Complex APIs have code examples
- [ ] README is updated if public API changed
- [ ] CHANGELOG is updated
- [ ] No spelling or grammar errors
- [ ] Links are valid
