# ADR-002: SlimHost as Central Singleton Facade

## Status

Accepted

## Date

2026-03-17

## Context

Many subsystems throughout the SDK need access to shared services before the full Microsoft.Extensions.Hosting `IHost` is built. Examples include:

- The `Variable`/`Setup` system must read configuration before DI is complete.
- `FileSystemService` is required during configuration loading.
- `PluginService` must register plugins before `IHostBuilder.ConfigureServices` runs.
- Logging (`NLog`) must be configured before any library code is called.

Using `IServiceProvider` directly at this stage is not possible because the DI container has not yet been built. Passing these services through explicit constructor chains would create complex setup ceremony for consumers.

## Decision

A lightweight, pre-DI host abstraction called `SlimHost` is introduced. It is structured as a three-layer design:

```
ISlimHost                  ← interface (in xSdk.Plugin, no runtime deps)
  └── SlimHostBase         ← base implementation holding IServiceProvider reference
        └── SlimHostInternal ← concrete singleton, initialized in Host.CreateBuilder
```

`SlimHost.Instance` is a static accessor returning the singleton `ISlimHost`. It is populated during `Host.CreateBuilder` (or `Host.CreateTestBuilder`) before `IHostBuilder.Build()` is called.

`ISlimHost` exposes three subservices:

| Property | Type | Purpose |
|---|---|---|
| `FileSystem` | `IFileSystemService` | Platform-agnostic file I/O |
| `VariableSystem` | `IVariableService` | Configuration and setup access |
| `PluginSystem` | `IPluginService` | Plugin catalog management |

`SlimHostInternal.Initialize` follows a two-phase build:

1. **PreBuild** — creates the `ISlimHost` instance immediately so it is accessible via `SlimHost.Instance` before services are resolved.
2. **Build** — constructs the full `ServiceCollection`, resolves all slim services, and finalizes the host with `SlimHostBuilder.Build()`.

The singleton is guarded by a `null` check — multiple calls to `Initialize` are idempotent (the host is created only once per process lifetime).

### Initialization Sequence

```
Host.CreateBuilder(args, appName, appCompany, appPrefix)
 └── SlimHostInternal.Initialize(...)
      ├── PreBuild() → SlimHost.Instance available (unfinished)
      ├── ConfigurationBuilder: load host + app config
      ├── ServiceCollection: logging, file services, plugin services, variable services
      └── SlimHostBuilder.Build() → SlimHost.Instance fully initialized
```

### Test Support

`TestHost` and `TestHostFixture` call `InitializeTestHost` instead of `Initialize`. This variant skips file-based configuration sources and loads only in-memory test configuration, preventing cross-test pollution.

## Consequences

### Positive

- Single, globally accessible entry point to core services — no constructor injection required at early bootstrap stages.
- Enables Configuration loading to use the `FileSystemService` and `VariableService` before DI is built.
- Test isolation via dedicated test initialization path.
- `xSdk.Plugin` defines `ISlimHost` without any dependency on Microsoft.Extensions.Hosting, allowing external plugin assemblies to reference only slim abstractions.

### Negative

- The global singleton pattern deviates from pure DI principles, making unit testing of code that calls `SlimHost.Instance` harder without the fixture helpers.
- The two-phase initialization (PreBuild → Build) is subtle — code that calls `SlimHost.Instance` between the two phases gets a partially initialized host.
- Only one `SlimHost` instance can exist per process; multi-host scenarios within a single process are not supported.
