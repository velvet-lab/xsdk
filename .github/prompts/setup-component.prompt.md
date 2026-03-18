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

- **Read the relevant ADR(s) in `docs/adr/`** — the [ADR index](../../docs/adr/README.md) maps each component area to its decision record. ADRs document the chosen patterns, known limitations, and rejected alternatives.
- Existing similar components in the codebase
- Established patterns and conventions
- Common abstractions and base types
- Naming conventions
- Project structure

Look at existing projects in `libs/` directory for patterns.

## Step 3: Create Project Structure

Based on the component type, create the appropriate structure:

### For a New Data Provider (`xSdk.Data.{ProviderName}`)

A data provider requires exactly four implementation classes (see [ADR-006](../../docs/adr/ADR-006-provider-agnostic-data-layer.md)):

```
libs/
└── xSdk.Data.{ProviderName}/
    ├── src/
    │   ├── xSdk.Data.{ProviderName}.csproj
    │   ├── {ProviderName}Database.cs              (IDatabase implementation)
    │   ├── {ProviderName}ConnectionBuilder.cs     (IConnectionBuilder implementation)
    │   ├── {ProviderName}DatabaseSetup.cs         (IDatabaseSetup implementation)
    │   ├── {ProviderName}Repository.cs            (Repository<TEntity> subclass)
    │   └── Extensions/
    │       └── IDatalayerBuilderExtensions.cs
    └── tests/
        ├── xSdk.Data.{ProviderName}.Tests.csproj
        └── {ProviderName}RepositoryTests.cs
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
    <TargetFramework>net10.0</TargetFramework>
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

A new data provider requires four components (see ADR-006):

### Create Database class

```csharp
namespace xSdk.Data.{ProviderName};

/// <summary>
/// Manages the {Provider} connection lifecycle.
/// </summary>
public class {ProviderName}Database : Database
{
    // Override Open<TConnection>() to return the provider-specific connection
}
```

### Create ConnectionBuilder class

```csharp
namespace xSdk.Data.{ProviderName};

/// <summary>
/// Builds the connection string for {Provider} from setup properties.
/// </summary>
public class {ProviderName}ConnectionBuilder : ConnectionBuilder
{
    // Override Build() to assemble the provider-specific connection string
}
```

### Create DatabaseSetup class

```csharp
namespace xSdk.Data.{ProviderName};

/// <summary>
/// Configuration for a {Provider} database instance.
/// </summary>
public class {ProviderName}DatabaseSetup : DatabaseSetup
{
    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    // Add provider-specific settings here
}
```

### Create Repository class

```csharp
namespace xSdk.Data.{ProviderName};

/// <summary>
/// {Provider} repository implementation.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public class {ProviderName}Repository<TEntity> : Repository<TEntity>
    where TEntity : class, IEntity
{
    protected override Task<TResult> ExecuteInternalAsync<TResult>(
        Func<Task<TResult>> operation,
        bool withTransaction,
        CancellationToken cancellationToken)
    {
        // Provider-specific execution logic
    }

    // InsertAsync, SelectAsync, SelectListAsync, UpdateAsync, UpsertAsync, RemoveAsync
}
```

## Step 6: Create Dependency Injection Extension

```csharp
namespace xSdk.Data.{ProviderName}.Extensions;

/// <summary>
/// Extension methods for registering the {Provider} data provider.
/// </summary>
public static class IDatalayerBuilderExtensions
{
    /// <summary>
    /// Registers the {Provider} database with the datalayer builder.
    /// </summary>
    /// <param name="builder">The datalayer builder.</param>
    /// <param name="name">The logical database name.</param>
    /// <param name="configure">Action to configure the database setup.</param>
    /// <returns>The builder for chaining.</returns>
    public static IDatalayerBuilder Use{ProviderName}(
        this IDatalayerBuilder builder,
        string name,
        Action<{ProviderName}DatabaseSetup> configure)
    {
        builder.ConfigureDatabase<
            {ProviderName}Database,
            {ProviderName}DatabaseSetup,
            {ProviderName}ConnectionBuilder>(name, configure);

        return builder;
    }
}
```

Registration in host startup:

```csharp
services.AddDatalayer(datalayer =>
{
    datalayer.Use{ProviderName}("MyDatabase", config =>
    {
        config.ConnectionString = "your-connection-string";
    });
    datalayer.MapRepository<IMyRepository, MyRepository>("MyDatabase");
});
```

## Step 7: Create Initial Tests

```csharp
namespace xSdk.Data.{ProviderName}.Tests;

public class {ProviderName}RepositoryTests
{
    [Fact]
    public async Task InsertAsync_WithValidEntity_InsertsSuccessfully()
    {
        var factory = BuildTestFactory();
        var repo = factory.CreateRepository<IMyRepository>("TestDb");
        var entity = new TestEntity { Id = "test-id", Name = "Test" };

        await repo.InsertAsync(entity);

        var result = await repo.SelectAsync("test-id");
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task SelectAsync_WhenEntityNotFound_ReturnsNull()
    {
        var factory = BuildTestFactory();
        var repo = factory.CreateRepository<IMyRepository>("TestDb");

        var result = await repo.SelectAsync("non-existent");

        Assert.Null(result);
    }
}

public class TestEntity : {ProviderName}Entity
{
    public string Name { get; set; } = string.Empty;
}
```

## Step 8: Create README

Create a README.md in the project folder:

````markdown
# xSdk.Data.{ProviderName}

Provides {Provider}-based data storage for xSDK.

## Installation

```powershell
dotnet add package xSdk.Data.{ProviderName}
```

## Usage

```csharp
services.AddDatalayer(datalayer =>
{
    datalayer.Use{ProviderName}("MyDatabase", config =>
    {
        config.ConnectionString = "your-connection-string";
    });
    datalayer.MapRepository<IMyRepository, MyRepository>("MyDatabase");
});
```

## Configuration

| Property | Description | Default |
|----------|-------------|---------|
| `ConnectionString` | Connection string for {Provider} | Empty |

## License

Apache-2.0 License - see LICENSE file for details
````

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
