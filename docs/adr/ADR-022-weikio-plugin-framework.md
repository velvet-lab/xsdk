# ADR-022: Weikio.PluginFramework for Runtime Plugin Loading

## Status

Superseded by [ADR-027](ADR-027-plugin-host-model.md)

## Date

2026-03-17

## Supersession Note (2026-04-27)

Weikio.PluginFramework is still used for **dynamic** (filesystem-based) assembly loading inside `PluginService`. However, the primary plugin-registration model for all built-in SDK extensions is now `IPluginHostCollection`-based (see [ADR-027](ADR-027-plugin-host-model.md)). `PluginItem`/`CatalogHelper` still wrap Weikio types, but the consumer-facing API (`RegisterPluginHost<T>`) is entirely independent of Weikio.

## Context

The SDK's plugin model ([ADR-003](ADR-003-plugin-extensibility-model.md)) must support two plugin discovery strategies:

1. **Static / in-process** — types known at compile time, registered explicitly via `builder.EnablePlugin<TPlugin>()`.
2. **Dynamic / runtime** — assemblies in a file system folder, discovered and loaded at runtime.

Strategy 2 requires assembly loading, type scanning, and instantiation that is non-trivial to implement from scratch (AppDomain management, `AssemblyLoadContext` isolation, dependency resolution).

## Decision

`xSdk` uses **Weikio.PluginFramework** for dynamic plugin loading inside `PluginService`.

### PluginService

```csharp
internal partial class PluginService(IFileSystemService fsService, ILogger<PluginService> logger) : IPluginService
{
    private readonly List<PluginItem> _plugins = new();

    public Task<IList<TPlugin>> GetPluginsAsync<TPlugin>(CancellationToken token = default);
    public Task<TPlugin?> GetPluginAsync<TPlugin>(CancellationToken token = default);
}
```

`PluginService` is split into partial classes:
- `PluginService.cs` — public API
- `PluginService.Catalog.cs` — manages the plugin catalog (in-memory `List<PluginItem>`)
- `PluginService.AddRemove.cs` — `AddPlugin<T>()`, removes

### PluginItem

```csharp
internal class PluginItem
{
    public object Plugin { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
}
```

Plugins are stored as untyped `object` references and cast to the requested type via `TPlugin` in `GetPluginsAsync<TPlugin>`.

### Static Plugin Registration

```csharp
pluginService.AddPlugin<TPlugin>();
```

Instantiates `TPlugin` using `Activator.CreateInstance` (no DI), stores it in `_plugins`.

### Dynamic Plugin Loading

`LoadPluginsAsync()` is called lazily on first `GetPluginsAsync` call. It uses `Weikio.PluginFramework`:

1. Scans the `{machine}/plugins/` directory via `IFileSystemService`.
2. Uses `PluginFramework.Catalogs.FolderPluginCatalog` to discover assemblies.
3. For each discovered type implementing a known plugin interface: instantiates the type and adds to `_plugins`.

### CatalogHelper

`CatalogHelper` is a utility that bridges `Weikio.PluginFramework` catalog types with the SDK's `IFileSystemService` path conventions.

### Invocation

```csharp
SlimHost.Instance.PluginSystem.Invoke<PluginBase>(plugin => plugin.ConfigureServices(services));
```

`Invoke<T>` calls `GetPluginsAsync<T>()` synchronously (blocking) and iterates results in `Order` sequence.

## Consequences

### Positive

- Zero-boilerplate dynamic plugin loading via folder scanning.
- `Weikio.PluginFramework` handles `AssemblyLoadContext` management and dependency resolution.
- Unified in-memory + file-system catalog through the same `IPluginService` API.
- Plugin ordering via `PluginItem.Order` ensures deterministic configuration.

### Negative

- `Weikio.PluginFramework` is a relatively niche library; long-term maintenance is uncertain.
- Dynamic plugin loading from the file system is a significant security surface: loading untrusted assemblies can execute arbitrary code. The SDK does not currently implement any signature verification or allowlist for plugin assemblies.
- `Invoke<T>` calls `GetPluginsAsync` synchronously (`.GetAwaiter().GetResult()`) in the DI configuration phase; this can cause deadlocks in certain synchronization contexts.
- Plugin instances are created via `Activator.CreateInstance` — no constructor injection from the DI container for dynamically loaded plugins.
