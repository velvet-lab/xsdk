---
title: "ADR-027: Plugin Host Model (IPluginHost / WebPluginHost)"
status: "Accepted"
date: "2026-04-27"
authors: "xSdk Team"
tags: ["architecture", "plugin", "extensibility"]
supersedes: "ADR-003, ADR-022"
superseded_by: ""
---

# ADR-027: Plugin Host Model (IPluginHost / WebPluginHost)

## Status

Accepted

**Extended by [ADR-032](ADR-032-plugin-host-lifecycle-extension.md)** (2026-06-11) — `IPluginHost` was extended with additional lifecycle hooks: `ConfigureHostConfiguration`, `ConfigureAppConfiguration`, and `ConfigureLogging`, giving plugins full control over the host configuration pipeline.

## Date

2026-04-27

## Context

The previous plugin model ([ADR-003](ADR-003-plugin-extensibility-model.md)) defined a `IPlugin / PluginBase / HostPluginBase / WebHostPluginBase` type hierarchy. Plugins were activated via `EnablePlugin<T>()` and invoked via `SlimHost.Instance.PluginSystem.Invoke<T>(action)`. Dynamic assembly-level loading was delegated to Weikio.PluginFramework (see [ADR-022](ADR-022-weikio-plugin-framework.md)).

This design had several problems:

1. **Coupling to static singleton** — all activation paths went through `SlimHost.Instance.PluginSystem`, which was removed in the SlimHost redesign ([ADR-026](ADR-026-slim-host-builder-redesign.md)).
2. **No DI participation during plugin setup** — `PluginBase.ConfigureServices` had no access to already-registered services; plugins could not resolve options or other services at configuration time.
3. **Naming confusion** — `HostPluginBase` and `WebHostPluginBase` names conflicted with Microsoft's own `IHostBuilder` / `WebHostBuilder` terminology.
4. **Builder extension pattern** — each plugin extension package exposed `IXxxPluginBuilder` (e.g., `ITelemetryPluginBuilder`) to allow customization. The old model provided no standard wiring for these builder interfaces.

## Decision

The plugin model is redesigned around **`IPluginHost`** — a DI-registered service that participates in the normal `IServiceCollection` lifecycle.

### Type Hierarchy

```
IPluginDescription           ← name, version, description, order (xSdk.Core)
  └── IPluginHost            ← ConfigureServices(IServiceCollection), ConfigureServices(HostBuilderContext, IServiceCollection)
        └── PluginHost       ← abstract base; holds IServiceProvider Services; InvokeBuilder<T> helpers
              └── WebPluginHost  ← adds ConfigureServices(WebHostBuilderContext, IServiceCollection),
                                    Configure(WebHostBuilderContext, IApplicationBuilder),
                                    ConfigureDefaults, ConfigureEndpoint(IEndpointRouteBuilder)
```

`IPluginHost` is registered as a **singleton** in the slim DI container. The `SlimHost` iterates registered plugin types, creates instances, and sets `Services` after the full application DI container is built, giving each plugin access to resolved services.

### Registration API

```csharp
// Register a concrete plugin host
builder.RegisterPluginHost<TelemetryPluginHost>();

// Register a plugin builder (customization point)
builder.RegisterPluginBuilder<ITelemetryPluginBuilder, DefaultTelemetryPluginBuilder>();
// or with explicit implementation
builder.RegisterPluginBuilder<ITelemetryPluginBuilder, MyCustomTelemetryBuilder>();

// Register configuration options for a plugin host
builder.RegisterPluginHostOptions<TelemetryOptions>();
```

These extension methods on `IHostBuilder` delegate to `builder.GetSlimHost().RegisterPluginHost<I, T>()` etc.

### Activation Pattern for Extension Packages

Each extension package provides an `Enable*` extension method on `IHostBuilder`:

```csharp
// xSdk.Extensions.Telemetry
public static IHostBuilder EnableTelemetry(this IHostBuilder builder)
    => builder
        .RegisterPluginHost<TelemetryPluginHost>()
        .RegisterPluginHostOptions<TelemetryOptions>()
        .RegisterPluginBuilder<ITelemetryPluginBuilder, DefaultTelemetryPluginBuilder>();
```

This replaces the old `builder.EnablePlugin<TelemetryPlugin>()` call.

### Plugin Builder Invocation

Inside `PluginHost`, builders are invoked via helpers that resolve them from `Services`:

```csharp
// Invoke a single builder
InvokeBuilder<ITelemetryPluginBuilder>(x => x.ConfigureTracing(tracerBuilder));

// Invoke all registered builders
InvokeBuilders<IAuthenticationPluginBuilder>(x => x.ConfigureAuthentication(authBuilder));
```

This replaces `SlimHost.Instance.PluginSystem.Invoke<T>(action)`.

### IPluginHostCollection

`IPluginHostCollection` is a read-only list of registered plugin host `Type` objects stored in the slim DI container. It is used internally by `SlimHost.GetPluginHosts<T>()` to instantiate and filter plugin hosts at DI-configuration time:

```csharp
IEnumerable<TelemetryPluginHost> hosts = slimHost.GetPluginHosts<TelemetryPluginHost>();
```

### Dynamic Loading (Weikio – unchanged)

`PluginService` / `PluginItem` / `CatalogHelper` still use **Weikio.PluginFramework** for loading external assemblies from the file system `plugins/` folder at runtime. This capability is unchanged and independent of the `IPluginHost` registration model.

### Namespace Convention

All concrete plugin host implementations are placed in the `xSdk.Plugins.*` namespace (e.g., `xSdk.Plugins.Telemetry`, `xSdk.Plugins.Authentication`). The `HostBuilderExtensions` in each package uses a matching namespace so the extension method is available after a single `using xSdk.Plugins.Telemetry;`.

## Consequences

### Positive

- **POS-001**: Plugin hosts are proper DI citizens — they receive constructor-injected services (e.g., `IOptions<TelemetryOptions>`) and can call `InvokeBuilder<T>` to resolve registered customization points.
- **POS-002**: Clear separation between plugin *host* (configures ASP.NET Core services) and plugin *builder* (customizes the host's behavior), keeping extension packages extensible.
- **POS-003**: Eliminates dependency on `SlimHost.Instance` static singleton — all registration flows through the builder extension API.
- **POS-004**: Test hosts call the same registration flow, so integration tests exercise the real plugin wiring.

### Negative

- **NEG-001**: Breaking change — `EnablePlugin<T>()`, `PluginBase`, `HostPluginBase`, and `WebHostPluginBase` are removed; consumers must migrate to `RegisterPluginHost<T>()` and `PluginHost`/`WebPluginHost`.
- **NEG-002**: `IServiceProvider Services` on `PluginHost` is set *after* DI build, so it is `null` inside `ConfigureServices`. Calling `InvokeBuilder` from `ConfigureServices` works because `Services` is set before `Configure*` is called; calling it before that throws `InvalidOperationException`.

## Alternatives Considered

##### Retain PluginBase Hierarchy with Builder Injection

- **ALT-001**: **Description**: Keep `PluginBase` but add a `SetServiceProvider` call before invocation.
- **ALT-002**: **Rejection Reason**: Requires the same `Services` injection pattern as the new model but adds complexity by keeping two parallel hierarchies (`IPlugin` and `IPluginHost`). Consolidation is simpler.

##### Pure DI (no SlimHost involvement)

- **ALT-003**: **Description**: Register plugin hosts directly as `IHostedService` or via `IHostBuilder.ConfigureServices`.
- **ALT-004**: **Rejection Reason**: Plugin hosts must be discoverable and orderable as a group. The `IPluginHostCollection` catalog with `OrderBy` fulfills this; pure DI enumeration via `GetServices<IPluginHost>()` doesn't guarantee consistent ordering across builds.

## Implementation Notes

- **IMP-001**: `PluginDescription.Order` controls invocation order. The first plugin in `GetPluginHosts<WebPluginHost>()` is responsible for calling `ConfigureDefaults` (sets up routing, problem details, etc.).
- **IMP-002**: `WebPluginHost.ConfigureEndpoint(IEndpointRouteBuilder)` is called inside `app.UseEndpoints(...)` after all middleware is configured.
- **IMP-003**: `PluginHostExtensions.SetServiceProvider` is called after the full DI container is built to inject `IServiceProvider` into each `PluginHost.Services`.

## References

- **REF-001**: [ADR-003](ADR-003-plugin-extensibility-model.md) — Superseded design
- **REF-002**: [ADR-022](ADR-022-weikio-plugin-framework.md) — Weikio dynamic loading (still active for filesystem plugins)
- **REF-003**: [ADR-026](ADR-026-slim-host-builder-redesign.md) — SlimHost builder-scoped design
- **REF-004**: [ADR-032](ADR-032-plugin-host-lifecycle-extension.md) — Extended lifecycle hooks for configuration and logging control
