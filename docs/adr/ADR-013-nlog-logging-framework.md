# ADR-013: NLog as Logging Framework

## Status

Superseded by [ADR-033](ADR-033-logging-architecture.md)

## Date

2026-03-17

## Supersession Note (2026-06-12)

NLog is being replaced by a new logging architecture that uses a QueueLogger pattern to handle the two-phase hosting pattern (SlimHost → Host). The decision is driven by:

1. **Dual LoggerFactories** — Previously, both SlimHost and Host created separate ILoggerFactory instances, causing configuration loss
2. **Plugin Filtering** — The new architecture enables proper plugin-specific logging configuration
3. **Early Logging Preservation** — Log messages before Host start are now properly buffered and flushed
4. **Single Source of Truth** — One logging infrastructure shared between both phases

See [ADR-033](ADR-033-logging-architecture.md) for the new architecture.

## Context

The SDK requires structured, performant logging that:

1. Works before the `IHost` is fully built (i.e., during the bootstrap configuration phase).
2. Supports multiple output targets: console, file, and a NLog viewer endpoint.
3. Integrates with `Microsoft.Extensions.Logging` so application code uses the standard `ILogger<T>` / `ILoggingBuilder` API.
4. Adapts log level based on the current deployment `Stage` (debug verbosity in Development, warnings-and-above in Production).
5. Flushes and shuts down cleanly on process exit.

## Decision

**NLog** is chosen as the logging framework, integrated via `NLog.Extensions.Logging`.

### Configuration

`HostLoggingManager` is the single point of truth for logging setup:

```csharp
services.AddLogging(HostLoggingManager.ConfigureLogging);
```

`ConfigureLogging` performs:

1. **Reset** — calls `LogManager.Factory.ResetDefaultConfiguration()` to clear any previous configuration.
2. **Console target** — enabled in `Development` and `Integration` stages; color-coded layout.
3. **File target** — enabled in non-slim mode; writes to `{machine_data}/logs/{appname}-{date}.log` with archive-on-size behavior.
4. **NLog Viewer target** — enabled always; allows real-time log streaming to the NLog Viewer tool over UDP.
5. **Minimum level** — derived from `EnvironmentSetup.Stage`:
   - `Development` → `Trace`
   - `Integration` → `Debug`
   - `PreProduction` → `Info`
   - `Production` → `Warn`
6. **NLog adapter** — `builder.AddNLog(configuration)` bridges `Microsoft.Extensions.Logging` to NLog.

### Slim Mode

For the pre-host initialization phase, `ConfigureSlimLogging` is used instead. It is identical but skips the file target to avoid circular dependencies during file path resolution (which itself depends on the partially constructed host).

### Process Exit Hook

```csharp
AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
{
    LogManager.Flush();
    LogManager.Shutdown();
};
```

Registered in `Host.CreateBuilder` to ensure all buffered log messages are written before the process exits.

### Usage in SDK Libraries

All SDK classes use NLog directly via:

```csharp
private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
```

This allows logging in static contexts and before DI is available. Application code uses the standard `ILogger<T>` injected via DI.

### Configuration Files

NLog is initialized programmatically (not via `nlog.config` XML) to ensure stage-appropriate targets are configured dynamically.

## Consequences

### Positive

- NLog's `LogManager.GetCurrentClassLogger()` works in static and pre-DI contexts.
- Multiple targets (console, file, viewer) with independent layout configuration.
- `NLog.Extensions.Logging` ensures application code uses the standard .NET logging abstractions.
- Clean shutdown via `LogManager.Flush()` prevents log loss during application termination.
- Stage-based minimum level prevents verbose logs in production without configuration file changes.

### Negative

- Direct `LogManager` static API usage throughout the codebase creates a tight coupling to NLog; replacing NLog would require changes across all SDK libraries.
- The programmatic NLog configuration bypasses the standard `nlog.config` mechanism — experienced NLog users may find the setup unfamiliar.
- `ConfigureSlimLogging` vs `ConfigureLogging` is a subtle distinction; using the wrong one during bootstrap leaves file logging unconfigured.

## Migration Outcome

The following NLog constructs are removed as part of the migration to ADR-014:

| NLog construct                       | Replacement                                       |
|--------------------------------------|---------------------------------------------------|
| `LogManager.GetCurrentClassLogger()` | `ILogger<T>` via constructor injection            |
| `NLog.Extensions.Logging` adapter    | `builder.AddOpenTelemetry()` in `TelemetryPlugin` |
| `ConsoleTarget`                      | `builder.AddConsole()` (MEL built-in)             |
| `FileTarget` + XML config            | `builder.AddFile()` or structured provider        |
| `Log4JXmlTarget` (UDP NLogViewer)    | removed — replaced by OTLP export                 |
| `LogManager.Flush()` / `Shutdown()`  | `IHost.StopAsync()` handles graceful shutdown     |
| `NLog.LogLevel` conversion           | direct `Microsoft.Extensions.Logging.LogLevel`    |

Static classes that previously used `LogManager.GetCurrentClassLogger()` only for log-then-rethrow patterns have the try/catch removed entirely — the exception propagates unchanged to the boundary layer where it is captured by OTel tracing.
