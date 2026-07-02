---
title: "ADR-032: Plugin Host Lifecycle Extension"
status: "Accepted"
date: "2026-06-11"
authors: "xSdk Team"
tags: ["architecture", "plugin", "lifecycle", "logging", "configuration"]
supersedes: ""
superseded_by: ""
---

# ADR-032: Plugin Host Lifecycle Extension

## Status

Accepted

## Date

2026-06-11

## Context

The Plugin Host Model ([ADR-027](ADR-027-plugin-host-model.md)) established `IPluginHost` as the core abstraction for plugin integration with a single `ConfigureServices(HostBuilderContext, IServiceCollection)` method. While this enabled plugins to register services in the DI container, it had significant limitations:

### Problems with the Original Design

- **POS-001**: **No Logging Control** — Plugins could not influence logging configuration. The `LoggingManager.ConfigureLogging` was called from within service registration, making it impossible for plugins to add custom log providers or filters.
- **POS-002**: **Missing Configuration Phases** — Plugins had no access to `ConfigureHostConfiguration` and `ConfigureAppConfiguration`, preventing them from adding configuration sources or transforming configuration data early in the host lifecycle.
- **POS-003**: **Timing Issues** — Logging was configured after service registration, but some plugin initialization logic needed logging already available (chicken-and-egg problem).
- **POS-004**: **Centralized Logging Manager** — The `LoggingManager` class created a centralized bottleneck; plugins requiring custom logging behavior had to modify a shared static class.
- **POS-005**: **Inconsistent Plugin Lifecycle** — Web plugins could configure middleware via `Configure(IApplicationBuilder)`, but regular plugins had no equivalent early-stage hooks.

### Specific Use Cases That Required This Change

1. **Command Plugin Host** — The Spectre.Console-based command plugin needs to suppress `Microsoft.Agents` logging noise and filter out `Information` level logs during command execution.
2. **AI Plugin Host** — AI agent execution requires fine-grained control over logging from `Microsoft.Agents.AI` and `Microsoft.Extensions.AI` components.
3. **Telemetry Plugin Host** — OpenTelemetry integration needs to add OTLP exporters and configure log enrichment before services are registered.
4. **Custom Configuration Providers** — Plugins like `xSdk.Data.Vault` need to inject configuration from HashiCorp Vault or Consul early in the host configuration phase.

### Existing Architecture

From ADR-027, the `IPluginHost` interface provided:

```csharp
public interface IPluginHost : IPluginDescription
{
    IServiceProvider? Services { get; }
    void ConfigureServices(HostBuilderContext context, IServiceCollection services);
}
```

The host builder invoked plugin configuration only during service registration:

```csharp
builder.ConfigureServices((context, services) => 
    slimHost.ConfigurePluginHost(x => x.ConfigureServices(context, services)));
```

## Decision

Extend `IPluginHost` with three additional lifecycle hooks that mirror the standard `IHostBuilder` configuration pipeline, giving plugins full control over host, application, and logging configuration.

### Extended Interface

```csharp
public interface IPluginHost : IPluginDescription
{
    IServiceProvider? Services { get; }

    void ConfigureHostConfiguration(IConfigurationBuilder builder);

    void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder);

    void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder);

    void ConfigureServices(HostBuilderContext context, IServiceCollection services);
}
```

### Base Implementation in `PluginHost`

All methods have default no-op implementations to preserve backward compatibility:

```csharp
public abstract class PluginHost : PluginDescription, IPluginHost
{
    public IServiceProvider? Services { get; internal set; }

    internal protected virtual bool IsWebPluginHost => false;

    public virtual void ConfigureHostConfiguration(IConfigurationBuilder builder) { }

    public virtual void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder) { }

    public virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder) { }

    public virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services) { }
}
```

Existing plugin hosts automatically inherit these no-op methods; no breaking changes.

### Host Integration

The `Host.CreateBuilder` method now invokes plugin lifecycle hooks at each corresponding phase:

```csharp
IHostBuilder builder = new HostBuilder()
    .SetSlimHost(slimHost)
    .ConfigureHostConfiguration(configBuilder =>
    {
        ConfigurationManager.LoadHostConfiguration(configBuilder, appOptions);
        slimHost.ConfigurePluginHost(x => x.ConfigureHostConfiguration(configBuilder));
    })
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        context.EnrichEnvironment(slimEnvironmentOptions);
        ConfigurationManager.LoadAppConfiguration(context, configBuilder, appOptions);
        slimHost.ConfigurePluginHost(x => x.ConfigureAppConfiguration(context, configBuilder));
    })
    .ConfigureLogging((context, loggingBuilder) =>
    {
        context.EnrichEnvironment(slimEnvironmentOptions);
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(slimEnvironmentOptions.LogLevel);
        slimHost.ConfigurePluginHost(x => x.ConfigureLogging(context, loggingBuilder));
    })
    .ConfigureServices((context, services) =>
    {
        context.EnrichEnvironment(slimEnvironmentOptions);
        slimHost.ConfigurePluginHost(x => x.ConfigureServices(context, services));
    });
```

### Removal of Centralized `LoggingManager`

The `LoggingManager.ConfigureLogging` static method is removed. Instead, logging configuration is distributed across plugins via `ConfigureLogging`:

**Before (centralized):**

```csharp
services.AddLogging(logBuilder => LoggingManager.ConfigureLogging(logBuilder, options));
```

**After (distributed):**

```csharp
public override void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
{
    builder.AddFilter((level) => level != LogLevel.Information);
    builder.AddFilter("Microsoft.Agents", LogLevel.None);
}
```

### Context Enrichment

The `HostBuilderContext` is enriched with `EnvironmentOptions` via `context.EnrichEnvironment(slimEnvironmentOptions)` in each phase, ensuring plugins have access to environment-specific settings (Stage, LogLevel, machine data paths, etc.) without additional service resolution.

### LogManager Relocation

`LogManager` and `StackTraceUtils` are moved from `xSdk.Core/src/Hosting/` to `xSdk.Core/src/Extensions/Logging/` to reflect their role as logging infrastructure rather than host lifecycle concerns.

### Example: Command Plugin Host

```csharp
public sealed class CommandPluginHost(ICommandsPluginBuilder builder) : PluginHost
{
    public override void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
    {
        // Suppress Information logs during command execution
        builder.AddFilter((level) => level != LogLevel.Information);
        
        // Silence Microsoft.Agents noise
        builder.AddFilter("Microsoft.Agents", LogLevel.None);
    }

    public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.TryAddSingleton(provider =>
        {
            ICommandApp app = builder.CreateApplication(services);
            app.Configure(config => builder.Configure(config));
            PromptFactory.Factory = builder.PromptFactory;
            return app;
        });
    }
}
```

## Consequences

### Positive

- **POS-001**: **Fine-Grained Logging Control** — Plugins can now add custom log providers (OTLP, file sinks, enrichment) and configure filters without modifying a shared centralized class.
- **POS-002**: **Early Configuration Access** — Plugins can inject configuration sources (Vault, Consul, environment) in the `ConfigureHostConfiguration` phase before application code runs.
- **POS-003**: **Correct Lifecycle Order** — Logging is configured in the standard `ConfigureLogging` phase, ensuring loggers are available when service registration occurs.
- **POS-004**: **Distributed Responsibility** — Each plugin owns its logging and configuration concerns; no shared mutable static `LoggingManager` class.
- **POS-005**: **Consistency Across Plugin Types** — Regular plugins now have the same lifecycle hooks as web plugins, making the model uniform.
- **POS-006**: **Backward Compatibility** — Existing plugins continue to work without changes; new lifecycle methods have no-op defaults in `PluginHost`.

### Negative

- **NEG-001**: **More Methods to Override** — Plugin authors must understand four lifecycle methods instead of one, increasing cognitive load for complex plugins.
- **NEG-002**: **Order-Dependent Configuration** — Plugins are invoked in registration order; incorrect ordering (e.g., telemetry registered after a plugin that needs logging) causes subtle bugs.
- **NEG-003**: **Duplicated Context Enrichment** — `context.EnrichEnvironment` is called in every lifecycle phase, which is a minor performance overhead (though negligible in practice).
- **NEG-004**: **Testing Complexity** — Unit tests for plugin hosts now need to mock `IConfigurationBuilder` and `ILoggingBuilder` in addition to `IServiceCollection`.

## Alternatives Considered

### ALT-001: Keep Centralized `LoggingManager` and Add Plugin Callbacks

- **ALT-001**: **Description**: Add a static `LoggingManager.RegisterPluginCallback(Action<ILoggingBuilder>)` method that plugins call during service registration.
- **ALT-001**: **Rejection Reason**: This maintains the centralized bottleneck and doesn't solve the early configuration problem. Plugin callbacks would still execute after logging is configured, defeating the purpose.

### ALT-002: Introduce a Separate `IPluginLifecycle` Interface

- **ALT-002**: **Description**: Create a separate `IPluginLifecycle` interface with `ConfigureHostConfiguration`, `ConfigureAppConfiguration`, `ConfigureLogging` methods. Plugins opt in by implementing both `IPluginHost` and `IPluginLifecycle`.
- **ALT-002**: **Rejection Reason**: This creates two parallel plugin abstractions, fragmenting the model. Developers would be confused about which interface to implement. The single `IPluginHost` interface with optional overrides is simpler.

### ALT-003: Use a Builder Pattern for Plugin Configuration

- **ALT-003**: **Description**: Introduce `IPluginConfigurationBuilder` with fluent methods like `.ConfigureLogging(action)`, `.ConfigureAppConfiguration(action)`. Plugins return this builder from a single method.
- **ALT-003**: **Rejection Reason**: This adds unnecessary abstraction layers. The direct method override pattern matches .NET's `IHostBuilder` API, making it familiar to developers. A builder pattern would require additional types and indirection without providing tangible benefits.

### ALT-004: Event-Based Plugin Notifications

- **ALT-004**: **Description**: Plugins subscribe to events like `HostConfiguring`, `LoggingConfiguring`, `ServicesConfiguring` via an event bus.
- **ALT-004**: **Rejection Reason**: Event-based notification is harder to reason about (order dependencies, unsubscription, error handling). It also makes testing more difficult compared to simple method overrides. The synchronous lifecycle model is more predictable.

## Implementation Notes

- **IMP-001**: **Migration Path** — Existing plugin hosts do not need changes; the base `PluginHost` class provides no-op implementations for all new methods.
- **IMP-002**: **Testing Strategy** — Unit tests should mock `IConfigurationBuilder`, `ILoggingBuilder`, and verify that plugin lifecycle methods are invoked in the correct order.
- **IMP-003**: **Documentation** — The Plugin Host Guide must be updated to explain the four lifecycle methods and provide examples for common scenarios (custom log providers, configuration injection).
- **IMP-004**: **Performance** — `context.EnrichEnvironment` is called four times per plugin per application startup. This is negligible overhead (< 1ms per plugin) but could be optimized by caching enriched contexts if profiling reveals a bottleneck.
- **IMP-005**: **Rollout** — This change is part of the xSdk 2.x release cycle. All first-party plugin hosts (Telemetry, Commands, AI, AspNetCore extensions) are updated to use `ConfigureLogging` where appropriate.

## References

- **REF-001**: [ADR-027: Plugin Host Model](ADR-027-plugin-host-model.md) — Established `IPluginHost` as the plugin abstraction.
- **REF-002**: [ADR-013: NLog Logging Framework](ADR-013-nlog-logging-framework.md) — Original logging architecture with centralized `LoggingManager`.
- **REF-003**: [ADR-014: OpenTelemetry Observability](ADR-014-opentelemetry-observability.md) — Current telemetry architecture; benefits from distributed logging configuration.
- **REF-004**: [ADR-026: SlimHost Builder Redesign](ADR-026-slim-host-builder-redesign.md) — Removed the singleton `SlimHost.Instance`; plugins must integrate via DI.
- **REF-005**: [Microsoft.Extensions.Hosting Documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host) — Standard host builder lifecycle phases.
