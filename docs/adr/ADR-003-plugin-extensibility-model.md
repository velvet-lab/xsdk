# ADR-003: Plugin-Based Extensibility Model

## Status

Accepted

## Date

2026-03-17

## Context

The xSDK must be extensible: third-party code must be able to register services, configure MVC, add telemetry instruments, and contribute command-line options — without modifying the core SDK libraries. At the same time consumers must be able to opt into capabilities declaratively.

Two composability axes must be served:

1. **Static plugins** — known at compile time, registered explicitly by the host application.
2. **Dynamic plugins** — discovered at runtime from the file system (e.g., plugin assemblies in a `plugins/` folder), using `Weikio.PluginFramework`.

## Decision

A unified plugin model is introduced with the following hierarchy:

```
IPlugin                  ← marker interface (xSdk.Plugin)
  └── PluginDescription  ← metadata: name, version, description, order
        └── PluginBase   ← configures IServiceCollection (xSdk.Plugin)
              ├── HostPluginBase   ← adds HostBuilderContext access
              └── WebHostPluginBase ← adds MVC/middleware configuration
```

**Registration (at host startup):**

```csharp
builder.EnablePlugin<TPlugin>();
builder.EnablePlugin<TPlugin, TPluginBuilder>();   // plugin + builder
```

These call `SlimHost.Instance.PluginSystem.AddPlugin<T>()` and store the type in an in-memory catalog.

**Invocation (at DI configuration time):**

```csharp
SlimHost.Instance.PluginSystem.Invoke<PluginBase>(x => x.ConfigureServices(services));
SlimHost.Instance.PluginSystem.Invoke<HostPluginBase>(x => x.ConfigureServices(context, services));
SlimHost.Instance.PluginSystem.Invoke<WebHostPluginBase>(x => x.ConfigureMvc(options));
```

`IPluginService.Invoke<T>` iterates all loaded plugins that implement `T` and calls the provided action, ordered by `PluginDescription.Order`.

**Dynamic Plugin Loading** uses `Weikio.PluginFramework` under the hood inside `PluginService` (see [ADR-022](ADR-022-weikio-plugin-framework.md)). Plugins can be loaded from:

- In-process assemblies (type-based registration).
- External assemblies on the file system (resolved via `IFileSystemService`).

**Extension packages** expose their capabilities through dedicated `PluginBase` subclasses and `HostBuilderExtensions`:

```csharp
// Commands extension
builder.EnableCommands();        // registers CommandPlugin

// Telemetry extension
builder.EnableTelemetry();       // registers TelemetryPlugin

// CloudEvents extension
builder.EnableCloudEvents();     // registers CloudEventPlugin
```

### Builder Pattern for Configuration

For plugins that need configuration, a companion `IPluginBuilder` interface exists. The builder is registered alongside the plugin and participates in the same `Invoke` pattern via `ITelemetryPluginBuilder`, `IWebApiPluginBuilder`, etc.

## Consequences

### Positive

- Core libraries are never modified for new features; extensions ship as separate packages.
- Ordering of plugins is deterministic via `Order` property.
- Dynamic discovery allows zero-code-change plugin deployment.
- Each extension library uses the same pattern, making it easy to learn and extend.

### Negative

- Plugin loading errors (missing assemblies, version mismatches) surface at runtime, not compile time.
- Dynamic plugins require careful security consideration — loading arbitrary assemblies from the file system can execute untrusted code.
- The `Invoke` pattern requires callers to know the correct plugin interface; there is no static type-safety guarantee across plugin boundaries.
