# ADR-014: OpenTelemetry for Observability (Metrics, Tracing, Logging)

## Status

Accepted

## Date

2026-03-17

## Context

Modern cloud-native applications require unified observability: distributed tracing to understand request flow across services, metrics for SLA monitoring, and structured log export to a central collector. The SDK must:

1. Provide a consistent, vendor-neutral observability setup.
2. Allow applications to opt in to tracing, metrics, and log export independently.
3. Support the OTLP (OpenTelemetry Protocol) for export to tools like Jaeger, Prometheus, Grafana, Datadog, or any OTLP-compatible backend.
4. Let plugin extensions contribute additional instrumentation (e.g., EF Core traces, HTTP client metrics).
5. Be entirely optional — applications that do not need telemetry should not pay the cost.

## Decision

`xSdk.Extensions.Telemetry` provides OpenTelemetry integration as an opt-in xSdk plugin.

### Activation

```csharp
builder.EnableTelemetry();
```

This registers `TelemetryPlugin` via the plugin system ([ADR-003](ADR-003-plugin-extensibility-model.md)).

### TelemetrySetup

`TelemetrySetup : Setup` exposes all telemetry settings as `Variable`-backed configuration properties:

| Property | Variable Key | Default |
|---|---|---|
| `IsDisabled` | `{prefix}_TELEMETRY_DISABLE` | `false` |
| `IsLoggingDisabled` | `{prefix}_TELEMETRY_LOGGING_DISABLE` | `false` |
| `IsTracingDisabled` | `{prefix}_TELEMETRY_TRACING_DISABLE` | `false` |
| `IsMetricsDisabled` | `{prefix}_TELEMETRY_METRICS_DISABLE` | `false` |
| `IsOtlpExporterDisabled` | `{prefix}_TELEMETRY_OTLP_DISABLE` | `true` |
| `IsConsoleEnabled` | `{prefix}_TELEMETRY_CONSOLE_ENABLE` | `false` |
| `Token` | `{prefix}_TELEMETRY_TOKEN` | — |
| Endpoint URL | `{prefix}_TELEMETRY_ENDPOINT` | — |

### TelemetryPlugin.ConfigureServices

When `IsDisabled == false`, the plugin:

1. Creates an `OpenTelemetryBuilder` via `services.AddOpenTelemetry()`.
2. **Tracing** (if not disabled): configures OTLP exporter; invokes all registered `ITelemetryPluginBuilder` implementations for additional instrumentation.
3. **Metrics** (if not disabled): configures OTLP exporter; invokes all registered `ITelemetryPluginBuilder` implementations.
4. **Logging**: always configures the OTLP log exporter via `builder.AddLogging(loggingBuilder => loggingBuilder.ConfigureOtlpExporter(...))`.

### Extension Point: ITelemetryPluginBuilder

Third-party plugins can contribute additional instruments by implementing `ITelemetryPluginBuilder`:

```csharp
public interface ITelemetryPluginBuilder : IPlugin
{
    void ConfigureTracing(TracerProviderBuilder builder);
    void ConfigureMetrics(MeterProviderBuilder builder);
    void ConfigureLogging(OpenTelemetryLoggerOptions options);
}
```

Example — EF Core tracing is contributed by a plugin that implements `ITelemetryPluginBuilder.ConfigureTracing(builder => builder.AddEntityFrameworkCoreInstrumentation(...))`.

### VariableResourceDetector

`VariableResourceDetector` implements OpenTelemetry's `IResourceDetector` interface. It reads all `Variable` instances that have a non-null `TelemetryResourceValue` delegate and contributes them as OTEL resource attributes. This makes SDK-level configuration (app name, version, stage) automatically visible as resource attributes in all telemetry data without any application code.

### TelemetryService / ITelemetryService

An injectable `ITelemetryService` provides application-level access to the configured `ActivitySource` and `Meter` instances, allowing application code to create custom spans and metrics without referencing OpenTelemetry APIs directly.

## Consequences

### Positive

- Vendor-neutral via OTLP — works with any compatible backend (Jaeger, Grafana, Datadog, Azure Monitor, etc.).
- Per-signal opt-out (tracing, metrics, logging independently) via environment variables.
- Extension point (`ITelemetryPluginBuilder`) allows other SDK plugins to contribute instrumentation.
- `VariableResourceDetector` automatically enriches telemetry with application metadata.
- Fully optional — zero cost if `EnableTelemetry()` is not called.

### Negative

- OTLP exporter is disabled by default (`IsOtlpExporterDisabled = true`), requiring explicit activation; this is easy to forget in production deployments.
- The logging configuration (`ConfigureOtlpExporter`) in `TelemetryPlugin` is always applied even when `IsLoggingDisabled == true` — this appears to be a code-level oversight that should be guarded.
- OpenTelemetry SDK versions are still evolving rapidly; some packages (`OpenTelemetry.Instrumentation.EntityFrameworkCore`) are in beta.
