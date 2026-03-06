---
description: 'Plan and set up a new component, feature, or module in the xSDK project'
---

# Setup Component/Module

You are helping to plan and set up a new component, feature, or module for the xSDK project.

## Step 1: Gather Requirements

Ask for the following information if not provided:

1. **Component Name**: What is the name of the component/module?
2. **Purpose**: What problem does it solve?
3. **Component Type**: Is it:
   - Core SDK functionality (`xSdk.*`)
   - Data provider (`xSdk.Data.*`)
   - Extension library (`xSdk.Extensions.*`)
   - Plugin (`xSdk.Plugin.*`)
4. **Dependencies**: What other xSDK components or external libraries does it depend on?
5. **Public API Surface**: What are the main public types/interfaces?

## Step 2: Analyze Project Structure

Before creating anything, analyze:

- Existing similar components in the codebase
- Established patterns and conventions
- Common abstractions and base types
- Naming conventions
- Project structure

Look at existing projects in `libs/` directory for patterns.

## Step 3: Create Project Structure

Based on the component type, create the appropriate structure:

### For a New Data Provider (`xSdk.Data.{ProviderName}`)

```
libs/
└── xSdk.Data.{ProviderName}/
    ├── src/
    │   ├── xSdk.Data.{ProviderName}.csproj
    │   ├── {ProviderName}DataStore.cs
    │   ├── {ProviderName}DataStoreOptions.cs
    │   └── Extensions/
    │       └── ServiceCollectionExtensions.cs
    └── tests/
        ├── xSdk.Data.{ProviderName}.Tests.csproj
        └── {ProviderName}DataStoreTests.cs
```

### For a New Extension (`xSdk.Extensions.{ExtensionName}`)

```
libs/
└── xSdk.Extensions.{ExtensionName}/
    ├── src/
    │   ├── xSdk.Extensions.{ExtensionName}.csproj
    │   ├── {ExtensionName}Service.cs
    │   ├── {ExtensionName}Options.cs
    │   └── Extensions/
    │       └── ServiceCollectionExtensions.cs
    └── tests/
        ├── xSdk.Extensions.{ExtensionName}.Tests.csproj
        └── {ExtensionName}ServiceTests.cs
```

## Step 4: Define Project Files

### Create .csproj File

Follow the existing pattern from other projects:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <!-- Reference required xSDK projects -->
    <ProjectReference Include="..\..\..\xSdk\src\xSdk.csproj" />
  </ItemGroup>

  <ItemGroup>
    <!-- Add NuGet packages to Directory.Packages.props -->
  </ItemGroup>

</Project>
```

### Add Packages to Directory.Packages.props

Add any new package dependencies:

```xml
<PackageVersion Include="NewPackage" Version="1.0.0" />
```

## Step 5: Define Core Abstractions

### Create Main Interface

```csharp
namespace xSdk.Data.{ProviderName};

/// <summary>
/// Provides {provider-specific} data storage operations.
/// </summary>
/// <typeparam name="TEntity">The type of entity to store.</typeparam>
public interface I{ProviderName}DataStore<TEntity> : IDataStore<TEntity> 
    where TEntity : class
{
    // Provider-specific methods if needed
}
```

### Create Implementation

```csharp
namespace xSdk.Data.{ProviderName};

/// <summary>
/// {Provider} implementation of the data store.
/// </summary>
/// <typeparam name="TEntity">The type of entity to store.</typeparam>
public class {ProviderName}DataStore<TEntity> : I{ProviderName}DataStore<TEntity> 
    where TEntity : class
{
    private readonly {ProviderName}DataStoreOptions _options;
    private readonly ILogger<{ProviderName}DataStore<TEntity>> _logger;

    public {ProviderName}DataStore(
        IOptions<{ProviderName}DataStoreOptions> options,
        ILogger<{ProviderName}DataStore<TEntity>> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<TEntity?> GetByIdAsync(
        string id, 
        CancellationToken cancellationToken = default)
    {
        // Implementation
    }

    // Other IDataStore methods...
}
```

### Create Options Class

```csharp
namespace xSdk.Data.{ProviderName};

/// <summary>
/// Configuration options for {Provider} data store.
/// </summary>
public class {ProviderName}DataStoreOptions
{
    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}
```

## Step 6: Create Dependency Injection Extensions

```csharp
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering {Provider} data store services.
/// </summary>
public static class {ProviderName}DataStoreServiceCollectionExtensions
{
    /// <summary>
    /// Adds {Provider} data store to the service collection.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection Add{ProviderName}DataStore<TEntity>(
        this IServiceCollection services,
        Action<{ProviderName}DataStoreOptions>? configureOptions = null)
        where TEntity : class
    {
        if (configureOptions is not null)
        {
            services.Configure(configureOptions);
        }

        services.AddSingleton<IDataStore<TEntity>, {ProviderName}DataStore<TEntity>>();

        return services;
    }
}
```

## Step 7: Create Initial Tests

```csharp
namespace xSdk.Data.{ProviderName}.Tests;

public class {ProviderName}DataStoreTests
{
    [Fact]
    public async Task GetByIdAsync_WhenEntityExists_ReturnsEntity()
    {
        // Arrange
        var options = Options.Create(new {ProviderName}DataStoreOptions
        {
            ConnectionString = "test-connection"
        });
        var logger = NullLogger<{ProviderName}DataStore<TestEntity>>.Instance;
        var store = new {ProviderName}DataStore<TestEntity>(options, logger);

        // Act
        var result = await store.GetByIdAsync("test-id");

        // Assert
        result.Should().NotBeNull();
    }
}

public class TestEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
```

## Step 8: Create README

Create a README.md in the project folder:

```markdown
# xSdk.Data.{ProviderName}

Provides {Provider}-based data storage for xSDK.

## Installation

```powershell
dotnet add package xSdk.Data.{ProviderName}
```

## Usage

```csharp
services.Add{ProviderName}DataStore<MyEntity>(options =>
{
    options.ConnectionString = "your-connection-string";
});
```

## Configuration

| Option | Description | Default |
|--------|-------------|---------|
| ConnectionString | Connection string for {Provider} | Empty |
| TimeoutSeconds | Timeout in seconds | 30 |

## License

MIT
```

## Step 9: Update Solution File

Add the new project to the solution file:

```powershell
dotnet sln xsdk.sln add libs/xSdk.Data.{ProviderName}/src/xSdk.Data.{ProviderName}.csproj
dotnet sln xsdk.sln add libs/xSdk.Data.{ProviderName}/tests/xSdk.Data.{ProviderName}.Tests.csproj
```

## Step 10: Verify Build and Tests

```powershell
# Build the solution
dotnet build

# Run tests
dotnet test

# Check for errors
dotnet build --no-incremental
```

## Guidelines

- Follow existing patterns from similar components
- Maintain consistency with project conventions
- Use nullable reference types correctly
- Add XML documentation to all public APIs
- Follow async/await best practices
- Pass `CancellationToken` through all async methods
- Use `ConfigureAwait(false)` in library code
- Write tests for all public APIs
- Keep the API surface minimal

## Checklist

Before completing:

- [ ] Project structure follows conventions
- [ ] All public APIs have XML documentation
- [ ] Dependency injection extension methods created
- [ ] Initial tests written and passing
- [ ] README.md created
- [ ] Added to solution file
- [ ] Solution builds successfully
- [ ] All tests pass
- [ ] Follows naming conventions
- [ ] Uses nullable reference types correctly
