<!-- Based on: https://github.com/github/awesome-copilot/blob/main/instructions/csharp.instructions.md -->
---
description: 'C# and .NET development guidelines for xSDK project'
applyTo: '**/*.cs'
---

# C# Development Guidelines for xSDK

## C# and .NET Version

- Always use C# 12 features (latest for .NET 8)
- Project targets .NET 8.0 (see `global.json`)
- Do not change the target framework or SDK version without explicit approval
- Write clear and concise comments for public APIs and complex logic

## General Principles

- Make only high confidence suggestions when reviewing code changes
- Write code with good maintainability practices
- Handle edge cases and write clear exception handling
- For libraries or external dependencies, mention their usage and purpose in comments
- Follow the project's conventions first, then standard C# conventions

## Naming Conventions

- Use PascalCase for: types, methods, properties, events, namespaces
- Use camelCase for: private fields, local variables, parameters
- Prefix interfaces with "I" (e.g., `IDataStore`, `IHostingService`)
- Prefix type parameters with "T" (e.g., `TEntity`, `TContext`)
- Use descriptive names that convey intent
- Avoid abbreviations unless widely recognized (e.g., `Id`, `Dto`)

## Formatting and Style

- Apply code-formatting style defined in `.editorconfig`
- Use file-scoped namespace declarations (C# 10+)
- Use single-line using directives
- Insert a newline before opening curly braces of code blocks
- Ensure final return statement is on its own line
- Use pattern matching and switch expressions where appropriate
- Use `nameof` instead of string literals when referring to member names
- Limit line length to 120 characters where practical

## Nullable Reference Types

This project has nullable reference types enabled.

- Declare variables non-nullable by default
- Use `?` suffix for nullable reference types
- Always use `is null` or `is not null` instead of `== null` or `!= null`
- Trust the C# null annotations; don't add null checks when the type system guarantees non-null
- Check for `null` at entry points (public API boundaries)
- Use `ArgumentNullException.ThrowIfNull(parameter)` for parameter validation
- For strings, use `string.IsNullOrWhiteSpace()` or `string.IsNullOrEmpty()`

## Exception Handling

- Choose precise exception types: `ArgumentException`, `ArgumentNullException`, `InvalidOperationException`
- Don't throw or catch base `Exception` type
- No silent catches - always log or rethrow
- Use `ArgumentNullException.ThrowIfNull(x)` for parameter checks
- Include meaningful error messages with context
- Document thrown exceptions with `<exception>` XML tags

## Async Programming

All async methods must follow these guidelines:

- Name all async methods with `Async` suffix
- Always await async operations - no fire-and-forget
- Accept and pass through `CancellationToken` parameters
- Use `ConfigureAwait(false)` in library code (this is a library project)
- Use `Task.Delay(ms, cancellationToken)` to make delays cancelable
- Call `ThrowIfCancellationRequested()` in long-running loops
- For timeout scenarios, use linked `CancellationTokenSource` with `CancelAfter`
- Prefer `Task` over `ValueTask` unless measured performance benefit
- Use `await using` for async disposable resources
- Don't create async wrappers that just return the task directly

## Data Access Patterns

This project provides multiple data layer abstractions:

- Follow repository pattern implementations in `xSdk.Data.*` projects
- All data access operations must be async
- Pass `CancellationToken` through all data access calls
- Use appropriate EF Core patterns to avoid N+1 queries
- Implement proper transaction handling where needed
- Consider query performance and use pagination for large result sets
- Use projection (`Select`) to load only needed fields

## Dependency Injection

- Register services in extension methods following the pattern: `Add[ServiceName]`
- Use appropriate lifetime: Singleton, Scoped, or Transient
- Prefer constructor injection over property injection
- Don't create service locator pattern (anti-pattern)
- Add XML documentation to DI extension methods explaining service registration

## XML Documentation

All public APIs must have XML documentation:

```csharp
/// <summary>
/// Provides data storage operations for entities.
/// </summary>
/// <typeparam name="TEntity">The type of entity to store.</typeparam>
public interface IDataStore<TEntity> where TEntity : class
{
    /// <summary>
    /// Retrieves an entity by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
```

## Performance Considerations

- Use `Span<T>` and `Memory<T>` for memory-efficient operations
- Avoid unnecessary allocations in hot paths
- Use object pooling (`ArrayPool<T>`) for large temporary buffers
- Stream large data; don't load entirely into memory
- Use `StringBuilder` for string concatenation in loops
- Consider `stackalloc` for small, short-lived arrays
- Profile before optimizing; don't prematurely optimize

## Testing Requirements

- All public APIs require unit tests
- Test projects follow naming: `[ProjectName].Tests`
- Tests mirror source structure
- Use xUnit (`[Fact]`, `[Theory]`)
- Follow AAA pattern (Arrange-Act-Assert)
- Test method naming: `MethodName_Scenario_ExpectedBehavior`
- Don't add "Arrange", "Act", "Assert" comments
- Use FluentAssertions if available
- Mock external dependencies, not domain logic
- Tests should be independent and runnable in any order

## Code Organization

- One type per file (class, interface, enum, etc.)
- File name matches the type name
- Organize using statements: System namespaces first, then others, then project namespaces
- Group members by type: fields, constructors, properties, methods, nested types
- Order by accessibility: public, internal, protected, private

## Security Best Practices

- Never commit secrets or connection strings to source control
- Use configuration providers for sensitive data
- Validate all inputs at API boundaries
- Use parameterized queries (EF Core does this)
- Follow principle of least privilege
- Log security events appropriately (audit trail)
- Don't log sensitive information (passwords, tokens, PII)

## Common Patterns

**Options Pattern for Configuration:**
```csharp
public class MyServiceOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public int Timeout { get; set; } = 30;
}

// Registration
services.Configure<MyServiceOptions>(configuration.GetSection("MyService"));

// Usage via constructor injection
public MyService(IOptions<MyServiceOptions> options)
{
    _options = options.Value;
}
```

**Disposal Pattern:**
```csharp
public class MyResource : IDisposable, IAsyncDisposable
{
    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            // Async disposal logic
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
```

## Project-Specific Guidelines

- Follow existing patterns in the codebase
- Check similar implementations before creating new abstractions
- Reuse existing extension methods and helper utilities
- Maintain consistency with the established architecture
- Don't add interfaces unless needed for testing or external dependencies
- Keep the API surface minimal - internal by default, public when necessary
