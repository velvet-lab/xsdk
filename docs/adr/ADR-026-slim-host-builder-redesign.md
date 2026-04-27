---
title: "ADR-026: SlimHost Builder-Scoped Redesign"
status: "Accepted"
date: "2026-04-27"
authors: "xSdk Team"
tags: ["architecture", "hosting", "slim-host"]
supersedes: "ADR-002"
superseded_by: ""
---

# ADR-026: SlimHost Builder-Scoped Redesign

## Status

Accepted

## Date

2026-04-27

## Context

The original `SlimHost` design (see [ADR-002](ADR-002-slim-host-singleton.md)) used a **process-wide static singleton** (`SlimHost.Instance`) accessible from anywhere. It exposed three top-level sub-services — `FileSystem`, `VariableSystem`, and `PluginSystem` — that were required before the `IHost` DI container was fully built.

Several architectural shortcomings emerged:

1. **Test isolation failures** — a static singleton shared state across tests even when separate `TestHost` instances were intended to be independent.
2. **Parallel build impossibility** — two `IHostBuilder` instances in the same process (e.g., in integration test suites or MAUI + background host scenarios) would corrupt each other's `SlimHost.Instance`.
3. **Unclear ownership** — code could call `SlimHost.Instance` at any time, including before `Initialize()` was called, producing a `NullReferenceException` instead of a structured error.
4. **Over-broad surface area** — the `FileSystem`, `VariableSystem`, and `PluginSystem` properties added coupling; consumers only needed specific services but received the whole facade.
5. **New plugin model** — the introduction of the `IPluginHost`-based DI model (see [ADR-027](ADR-027-plugin-host-model.md)) made most of `SlimHost.Instance`'s API redundant.

## Decision

`SlimHost` is redesigned as a **builder-scoped object** stored in `IHostBuilder.Properties` rather than a static field.

### Core API

```csharp
// Internal: created during Host.CreateBuilder
var slimHost = SlimHost.InitializeSlimHost(args, appOptions);

// Stored in builder.Properties under a well-known key
builder.SetSlimHost(slimHost);

// Retrieved by any extension that needs it
SlimHost slimHost = builder.GetSlimHost();
```

`GetSlimHost()` throws a structured `SdkException` if called before `InitializeSlimHost`, preventing silent null-reference failures.

### SlimHost Responsibilities

`SlimHost` now has a focused set of responsibilities:

| Method / Property | Purpose |
|---|---|
| `InitializeSlimHost(args, appOptions)` | Factory — creates and seeds the slim DI container with defaults |
| `PostConfigure(appServices)` | Merges slim-registered services into the full application `IServiceCollection` |
| `RegisterPluginHost<TInterface, TImpl>()` | Adds a plugin host type to the internal type catalog |
| `RegisterPluginBuilder<TBuilder, TImpl>()` | Adds a plugin builder as a keyed singleton in the slim container |
| `RegisterPluginHostOptions<TOptions>()` | Registers an options type for a plugin host |
| `ConfigurePluginHost<T>(action)` | Iterates registered `IPluginHost` instances of type `T` and invokes `action` |
| `GetPluginHosts<T>()` | Returns ordered `IEnumerable<T>` of registered plugin hosts |
| `GetEnvironment()` | Returns `EnvironmentOptions` resolved from the slim container |

### No More Static Singleton

`SlimHost.Instance` is **removed**. All access goes through the builder:

```csharp
// Old (removed)
SlimHost.Instance.PluginSystem.Invoke<PluginBase>(x => x.ConfigureServices(services));

// New
var slimHost = builder.GetSlimHost();
slimHost.ConfigurePluginHost<IPluginHost>(plugin => plugin.ConfigureServices(services));
```

### Initialization Sequence

```
Host.CreateBuilder(args, appName, appCompany, appPrefix)
 └── SlimHost.InitializeSlimHost(args, appOptions)
      ├── new ServiceCollection()
      ├── ConfigureDefaults(slimServices)   ← logging, file system, variable service, options
      └── return SlimHost instance
 └── builder.SetSlimHost(slimHost)          ← stored in builder.Properties
 └── builder.ConfigureServices(...)
      └── slimHost.PostConfigure(appServices) ← slim services merged into app DI
```

### Test Isolation

`TestHostFactory.CreateTestHostBuilder()` calls `SlimHost.InitializeSlimHost()` independently per test, creating a fully isolated `SlimHost`. No shared static state exists between tests.

## Consequences

### Positive

- **POS-001**: Parallel host builds (e.g., multiple integration test hosts in one process) are now safe — each builder has its own `SlimHost`.
- **POS-002**: Test isolation is guaranteed; no static state can leak between test classes.
- **POS-003**: A structured `SdkException` is thrown on misuse instead of a silent `NullReferenceException`.
- **POS-004**: Slim host surface area is reduced to what is actually needed; the `FileSystem`/`VariableSystem`/`PluginSystem` properties no longer expose unused surface area.

### Negative

- **NEG-001**: Breaking change — any consumer code calling `SlimHost.Instance` must be updated to use `builder.GetSlimHost()`.
- **NEG-002**: `SlimHost` is no longer accessible outside the builder pipeline (e.g., not from application startup code that receives only `IServiceProvider`). Code that previously called `SlimHost.Instance.PluginSystem` must instead use DI-resolved services.

## Alternatives Considered

##### Keep Static Singleton with Thread-Local Override

- **ALT-001**: **Description**: Maintain `SlimHost.Instance` but add a `[ThreadStatic]` or `AsyncLocal<>` override for tests.
- **ALT-002**: **Rejection Reason**: `AsyncLocal` adds complexity and the override is error-prone. Does not solve the fundamental design issue of global mutable state.

##### Use IServiceProvider Directly

- **ALT-003**: **Description**: Eliminate `SlimHost` entirely and require callers to use the standard `IServiceProvider`.
- **ALT-004**: **Rejection Reason**: The primary purpose of `SlimHost` is to provide services *before* the DI container is built. `IServiceProvider` is not available at that stage.

## Implementation Notes

- **IMP-001**: `SlimHostExtensions.SetSlimHost` / `GetSlimHost` use the constant key `"xSdk.Hosting.SlimHost"` in `IHostBuilder.Properties`.
- **IMP-002**: `SlimHost.InitializeSlimHost` is `internal` and only called by `Host.CreateBuilder` and `TestHostFactory`.
- **IMP-003**: `PluginHostExtensions.SetServiceProvider` wires the full `IServiceProvider` into each `PluginHost.Services` after the DI container is built, so plugin hosts can resolve application-level services.

## References

- **REF-001**: [ADR-002](ADR-002-slim-host-singleton.md) — Superseded design
- **REF-002**: [ADR-027](ADR-027-plugin-host-model.md) — Plugin host model that replaced `IPlugin`/`PluginBase`
- **REF-003**: [ADR-003](ADR-003-plugin-extensibility-model.md) — Superseded plugin model
