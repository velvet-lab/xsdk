# ADR-014: OpenTelemetry for Observability (Metrics, Tracing, Logging)

## Status

Accepted (extended 2026-03-27 — supersedes [ADR-013](ADR-013-nlog-logging-framework.md))

## Date

2026-03-17

## Context

Modern cloud-native applications require unified observability: distributed tracing to understand request flow across services, metrics for SLA monitoring, and structured log export to a central collector. The SDK must:

1. Provide a consistent, vendor-neutral observability setup.
2. Allow applications to opt in to tracing, metrics, and log export independently.
3. Support the OTLP (OpenTelemetry Protocol) for export to tools like Jaeger, Prometheus, Grafana, Datadog, or any OTLP-compatible backend.
4. Let plugin extensions contribute additional instrumentation (e.g., EF Core traces, HTTP client metrics).
5. Be entirely optional — applications that do not need telemetry should not pay the cost.

**Extension (2026-03-27):** NLog (ADR-013) is fully superseded. OpenTelemetry is now the sole observability stack for logging, tracing, and metrics. The `Microsoft.Extensions.Logging` abstraction (`ILogger<T>`) remains the logging API; OTel replaces NLog as the backend provider.

## Decision

`xSdk.Extensions.Telemetry` provides OpenTelemetry integration as an opt-in xSdk plugin.

### Activation

```csharp
builder.EnableTelemetry();
```

This registers `TelemetryPlugin` via the plugin system ([ADR-003](ADR-003-plugin-extensibility-model.md)).

### TelemetrySetup

`TelemetrySetup : Setup` exposes all telemetry settings as `Variable`-backed configuration properties:

| Property                 | Variable Key                         | Default |
|--------------------------|--------------------------------------|---------|
| `IsDisabled`             | `{prefix}_TELEMETRY_DISABLE`         | `false` |
| `IsLoggingDisabled`      | `{prefix}_TELEMETRY_LOGGING_DISABLE` | `false` |
| `IsTracingDisabled`      | `{prefix}_TELEMETRY_TRACING_DISABLE` | `false` |
| `IsMetricsDisabled`      | `{prefix}_TELEMETRY_METRICS_DISABLE` | `false` |
| `IsOtlpExporterDisabled` | `{prefix}_TELEMETRY_OTLP_DISABLE`    | `true`  |
| `IsConsoleEnabled`       | `{prefix}_TELEMETRY_CONSOLE_ENABLE`  | `false` |
| `Token`                  | `{prefix}_TELEMETRY_TOKEN`           | —       |
| Endpoint URL             | `{prefix}_TELEMETRY_ENDPOINT`        | —       |

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

### Namenskonventionen (Naming Schema)

Um Konsistenz in allen Monitoring-Backends (Grafana, Jaeger, Datadog etc.) zu gewährleisten, gilt folgendes dreigeteiltes Schema:

| Telemetrie-Komponente | OTel-Attribut / Typ | Namensschema | Beispiel |
| :--- | :--- | :--- | :--- |
| **Service-Name** | `service.name` (Resource) | `kebab-case` — beschreibt die logische Anwendung nach außen | `order-api`, `billing-service` |
| **Source- / Meter-Name** | `ActivitySource` / `Meter` | Assembly-Name des Pakets (`PascalCase`) — eine `Diagnostics`-Klasse pro Paket | `xSdk.Core`<br>`xSdk.Data.Vault` |
| **Metrik-Namen** | `Instrument` (Counter, Histogram …) | `lowercase.with.dots` — beginnt mit dem funktionalen Objekt | `redis.cache.hits`<br>`rabbitmq.message.duration` |

### VariableResourceDetector

`VariableResourceDetector` implements OpenTelemetry's `IResourceDetector` interface. It reads all `Variable` instances that have a non-null `TelemetryResourceValue` delegate and contributes them as OTEL resource attributes. This makes SDK-level configuration (app name, version, stage) automatically visible as resource attributes in all telemetry data without any application code.

### Dezentralisierung: Kein `ITelemetryService` (superseded)

> **Update (2026-05-30):** Ein zentraler, via DI injizierter `ITelemetryService` wird **nicht** implementiert. Die Datei `ITelemetryService.cs` in `xSdk.Core` ist vollständig auskommentiert und gilt als aufgegeben.
>
> **Begründung:** .NET besitzt mit `System.Diagnostics` (`ActivitySource`) und `System.Diagnostics.Metrics` (`Meter`) ein hochoptimiertes, laufzeitnahes Instrumentierungs-System. Klassen und Plugins erzeugen ihre Telemetriesignale autark über **statische Instanzen** direkt in ihrem eigenen Namespace. Das SDK nimmt ausschließlich die Rolle der **Infrastruktur-Konfiguration** (Listener-Registrierung) ein.

**Korrekte Verwendung (statisches Muster, eine `Diagnostics`-Klasse pro Paket):**

```csharp
// In jedem SDK-Paket oder Plugin — eine Datei Diagnostics.cs, kein DI
internal static class Diagnostics
{
    internal const string SourceName = "xSdk.Core"; // explizite Konstante, kein Reflection
    internal static readonly ActivitySource Source = new(SourceName);
    internal static readonly Meter Meter = new(SourceName);
}
```

### Logging: Replacing NLog with ILogger\<T\> + OTel

`HostLoggingManager` is rewritten to remove all NLog dependencies:

```csharp
internal static void ConfigureLogging(ILoggingBuilder builder)
{
    builder.ClearProviders();
    builder.SetMinimumLevel(ConvertLogLevel(envSetup)); // using MEL LogLevel directly
    if (Debugger.IsAttached || envSetup.IsDotNetRunningInContainer)
        builder.AddConsole();
    // OTLP log export is configured by TelemetryPlugin
}
```

The `ProcessExit` hook (`LogManager.Flush/Shutdown`) is removed — `IHost.StopAsync()` provides the graceful shutdown guarantee for OTel exporters.

All SDK classes migrate from `LogManager.GetCurrentClassLogger()` to `ILogger<T>` constructor injection. Static classes that only performed log-then-rethrow have their try/catch blocks removed entirely.

### Tracing: Target Locations

Tracing is applied at **boundary layers only** — operations with measurable latency or external dependencies. The following are the designated instrumentation points:

**Priority HIGH — external I/O**

| Class                                           | Span names                                                    | Reason                       |
|-------------------------------------------------|---------------------------------------------------------------|------------------------------|
| `VaultRepository` / `ReadOnlyVaultRepository`   | `vault.read_secret`, `vault.write_secret`                     | HTTP call to HashiCorp Vault |
| `NoSqlRepository` (LiteDB)                      | `nosql.query`, `nosql.insert`, `nosql.update`, `nosql.delete` | File-based DB operations     |
| `HttpClientBuilder`                             | automatic via `AddHttpClientInstrumentation()`                | All outbound HTTP            |
| `CloudEventFactory` / `CloudEventWebExtensions` | `cloudevent.publish`, `cloudevent.receive`                    | Event boundary               |
| `PluginItem`                                    | `plugin.load`, `plugin.initialize`                            | Assembly + filesystem I/O    |

**Priority MEDIUM — internal boundaries worth observing**

| Class                                                      | Reason                            |
|------------------------------------------------------------|-----------------------------------|
| `FileSystemService`                                        | File I/O that can fail            |
| `CredentialManager` / `CertAuthSetup` / `AppRoleAuthSetup` | Security operations — audit trail |
| `VariableService`                                          | Configuration resolution failures |

**Explicitly excluded from tracing**

- All `Converters.Mapper` classes (nanosecond pure functions, no I/O, no meaningful latency)
- EF Core repositories (covered automatically via `AddEntityFrameworkCoreInstrumentation()`)

### Metrics: Target Instruments

Metrics are added at locations where aggregated data has operational value (alerting, dashboards, capacity planning):

| Metric name                           | Type          | Location                             | Operational value             |
|---------------------------------------|---------------|--------------------------------------|-------------------------------|
| `xsdk.repository.operations.duration` | Histogram     | `NoSqlRepository`, `VaultRepository` | Latency alerting              |
| `xsdk.repository.operations.errors`   | Counter       | All repositories                     | Error rate / SLA              |
| `xsdk.plugin.load.duration`           | Histogram     | `PluginItem`                         | Plugin startup performance    |
| `xsdk.plugin.loaded.count`            | UpDownCounter | Plugin system                        | Active plugin count           |
| `xsdk.http.requests.duration`         | Histogram     | `HttpClientBuilder`                  | automatic via instrumentation |
| `xsdk.vault.requests.total`           | Counter       | `VaultRepository`                    | Vault call volume             |
| `xsdk.variable.resolution.failures`   | Counter       | `VariableService`                    | Configuration error detection |

All custom instruments are created directly via the statischen `Meter`-Instanz der jeweiligen Klasse/des Plugins (z. B. `Telemetry.Meter.CreateCounter<long>("vault.requests.total", ...)`). Direkte Referenzen auf `System.Diagnostics` und `System.Diagnostics.Metrics` sind in SDK-Bibliotheken explizit erlaubt und erwünscht — `ITelemetryService` als Wrapper entfällt.

### Migration Sequence

```
1. HostLoggingManager      NLog targets → AddConsole() + OTel logging
2. TelemetryConfigurator   Remove NLog circular dependency (static _logger)
3. Static SDK classes      Remove log-then-rethrow try/catch blocks
4. DI-managed classes      ILogger<T> constructor injection
5. Repository layer        Add Tracing + Metrics via static ActivitySource/Meter
                           (NOT via ITelemetryService — see Dezentralisierung section)
6. HttpClientBuilder       AddHttpClientInstrumentation() in TelemetryPlugin
7. Plugin system           Tracing for load/initialize
8. Host.cs / TestHost      Remove LogManager.Flush/Shutdown hooks
9. TelemetryPluginHost     Delegiert vollständig an ITelemetryPluginBuilder-Implementierungen;
                           AddXSdkInstrumentation() steht als opt-in Extension Method bereit
                           und wird vom App-seitigen PluginBuilder explizit aufgerufen
```

## Consequences

### Positive

- Vendor-neutral via OTLP — works with any compatible backend (Jaeger, Grafana, Datadog, Azure Monitor, etc.).
- Per-signal opt-out (tracing, metrics, logging independently) via environment variables.
- Extension point (`ITelemetryPluginBuilder`) allows other SDK plugins to contribute instrumentation.
- `VariableResourceDetector` automatically enriches telemetry with application metadata.
- Fully optional — zero cost if `EnableTelemetry()` is not called.

### Negative

- OTLP exporter is disabled by default (`IsOtlpExporterDisabled = true`), requiring explicit activation; this is easy to forget in production deployments.
- The logging configuration (`ConfigureOtlpExporter`) in `TelemetryPlugin` is always applied even when `IsLoggingDisabled == true` — this is a known code-level oversight that must be fixed during the NLog migration.
- OpenTelemetry SDK versions are still evolving rapidly; some packages (`OpenTelemetry.Instrumentation.EntityFrameworkCore`) are in beta.
- Static SDK classes that cannot receive `ILogger<T>` via DI must either be refactored to non-static or forgo logging entirely — the latter is acceptable when the class is a pure function with no external dependencies.
- The `TelemetryConfigurator` class currently uses NLog for its own internal logging, creating a circular dependency — this must be resolved by either making the class non-static with `ILogger<T>` injection or replacing the single warning with a `Console.Error.WriteLine` during bootstrap.
