# ADR-001: Modular Library Architecture

## Status

Accepted

## Date

2026-03-17

## Context

xSDK is a framework for building .NET applications. The framework must support a wide range of scenarios — from simple console applications and CLI tools to web APIs, event-driven services, and applications requiring multiple data storage backends simultaneously.

A monolithic single-library design would force all consumers to accept transitive dependencies for features they do not use (e.g., a console application would pull in MongoDB drivers). This increases binary size, slows startup, and creates unnecessary version conflicts.

## Decision

The SDK is split into independently deployable NuGet packages, each with a clearly scoped responsibility:

| Package                             | Responsibility                                                                                                                                                                                                                                                                                                              |
|-------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `xSdk.Core`                         | Foundation primitives: `ISlimHost`, `IPluginHost`, `IPluginDescription`, `SdkException`, `Stage`, `SemVer`, all cross-cutting interfaces (`IPluginBuilder`, `IRepository`, `IEntity`, `ISetup`, `IVariable`, `IFileSystemService`, …) — minimal runtime dependencies (see [ADR-024](ADR-024-xsdk-core-foundation-layer.md)) |
| `xSdk`                              | Host layer: `Host.CreateBuilder`, `SlimHost` (per-builder), configuration loading, `FileSystemService`, `PluginService`, `VariableService`, Weikio plugin wiring (see [ADR-026](ADR-026-slim-host-builder-redesign.md))                                                                                                     |
| `xSdk.Data`                         | Provider-agnostic data layer: `IDatabase`, `IRepository<TEntity>`, `DatalayerFactory`, `DatalayerBuilder`, `MappingFactory`, `FakeRepository`                                                                                                                                                                               |
| `xSdk.Data.EntityFramework`         | EF Core relational provider implementation                                                                                                                                                                                                                                                                                  |
| `xSdk.Data.EntityFramework.MongoDB` | MongoDB EF Core context extension                                                                                                                                                                                                                                                                                           |
| `xSdk.Data.FlatFile`                | JsonFlatFileDataStore provider                                                                                                                                                                                                                                                                                              |
| `xSdk.Data.NoSql`                   | LiteDB embedded NoSQL provider (**in development** — lives in `think-tank/libs/`, not yet released as production package; see [ADR-008](ADR-008-nosql-litedb-provider.md))                                                                                                                                                  |
| `xSdk.Data.Vault`                   | HashiCorp Vault KV secret store provider                                                                                                                                                                                                                                                                                    |
| `xSdk.Data.Consul`                  | HashiCorp Consul service-discovery & key/value configuration provider (**in development** — lives in `think-tank/libs/`, not yet released as production package; see [ADR-025](ADR-025-consul-data-provider.md))                                                                                                            |
| `xSdk.Extensions.AspNetCore`        | Web host extensions, controllers, Kestrel configuration, security                                                                                                                                                                                                                                                           |
| `xSdk.Extensions.AspNetCore.Links`  | Hypermedia links for REST APIs                                                                                                                                                                                                                                                                                              |
| `xSdk.Extensions.CloudEvents`       | CloudNative.CloudEvents ASP.NET Core integration                                                                                                                                                                                                                                                                            |
| `xSdk.Extensions.Commands`          | Spectre.Console.Cli command infrastructure                                                                                                                                                                                                                                                                                  |
| `xSdk.Extensions.Telemetry`         | OpenTelemetry metrics, tracing, and log export                                                                                                                                                                                                                                                                              |

Each library follows the same folder convention:
```
libs/{LibraryName}/
├── src/   ← production code
└── tests/ ← unit tests
```

`xSdk.Core` contains all cross-cutting abstractions and is the lowest-level dependency. All other libraries reference it. This inverted dependency prevents cycles and allows external plugins to depend only on the slim abstractions package.

## Consequences

### Positive

- Consumers reference only the packages they need; no unnecessary transitive dependencies.
- Each library can be versioned and released independently.
- Supports opt-in features: a project using only flat-file storage does not depend on EF Core.
- Clear ownership boundaries make it easy to add new data providers without modifying existing ones.
- Testing is isolated per library.

### Negative

- More projects to maintain and keep in sync (currently 14 library projects).
- Cross-library integration changes require coordinating multiple packages.
- Central Package Management (`Directory.Packages.props`) is required to avoid version skew.
