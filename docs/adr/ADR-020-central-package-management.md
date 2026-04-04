# ADR-020: Central NuGet Package Management

## Status

Accepted

## Date

2026-03-17

## Context

The xSDK consists of 14 library projects and multiple demo/test projects. Without centralized version management, each project specifies its own package versions, which leads to:

- Version skew between projects (e.g., `xSdk.Data` uses `Mapster 10.0.5` while `xSdk.Data.EntityFramework` uses `Mapster 10.0.6`).
- Upgrade sprawl — bumping a package requires editing every `.csproj` file.
- Inconsistent transitive resolution, causing runtime binding failures.

## Decision

All NuGet package versions are managed centrally via `Directory.Packages.props` at the solution root, using MSBuild's **Central Package Management (CPM)** feature:

```xml
<PropertyGroup>
  <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
</PropertyGroup>
<ItemGroup>
  <PackageVersion Include="Mapster" Version="10.0.6" />
  <PackageVersion Include="LiteDB.Async" Version="0.1.8" />
  <!-- ... all packages declared here ... -->
</ItemGroup>
```

Individual projects declare package references **without** version attributes:

```xml
<PackageReference Include="Mapster" />
```

### GlobalPackageReference

Two packages are declared as `GlobalPackageReference` — they are automatically applied to every project:

- `JsonPeek 1.2.0` — reads JSON values during the MSBuild process (used in build customization).
- `Microsoft.SourceLink.GitHub 10.0.201` — enables source-link for debuggable NuGet packages (applied only to non-test projects via a condition).

### Shared Build Configuration

`Directory.Build.props` (solution root) applies to every project:

- `TargetFramework`: `net10.0`
- `LangVersion`: `latest`
- `Nullable`: `enable`
- `ImplicitUsings`: `enable`
- `RuntimeIdentifiers`: `win-x64;linux-x64`
- `BaseOutputPath`: `$(SolutionDir)dist\$(MSBuildProjectName)` — all build outputs land in a single `dist/` folder.

`Directory.Build.targets` applies post-build customization (e.g., README file copying to NuGet packages, version stamping).

`global.json` pins the .NET SDK version to `10.0.200` with `rollForward: latestFeature`.

### InternalsVisibleTo

`Directory.Build.props` automatically declares `InternalsVisibleTo` for each assembly's corresponding test project (`$(AssemblyName).Tests`) and `DynamicProxyGenAssembly2` (Moq). This avoids repeating these declarations in every project file.

## Consequences

### Positive

- Single location to upgrade any NuGet package across the entire solution.
- Tool support: `dotnet list package --outdated` works at the solution level.
- Consistent transitive resolution — all projects agree on exact versions.
- `GlobalPackageReference` for SourceLink ensures every NuGet package automatically embeds source-link metadata.

### Negative

- All projects are constrained to the same version; if two projects genuinely need different versions of the same package, CPM does not support this without overrides.
- Developers not familiar with CPM may be confused by `<PackageReference Include="Foo" />` without a version.
- `Directory.Build.props` inheritance means a project in a subdirectory cannot easily opt out of global settings without explicit overrides.
