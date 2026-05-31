using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using xSdk.Extensions.AI;

namespace xSdk;

public static class DiagnosticsExtensions
{
    /// <summary>
    /// Aktiviert Tracing für das xSdk.Extensions.AI Paket.
    /// Optionale Bibliotheken registrieren ihre ActivitySources selbst via
    /// <see cref="ITelemetryPluginBuilder.ConfigureTracing"/>.
    /// </summary>
    /// <param name="builder">Der <see cref="TracerProviderBuilder"/>.</param>
    public static TracerProviderBuilder AddAIInstrumentation(this TracerProviderBuilder builder)
        => builder
            .AddSource(Diagnostics.SourceName);

    /// <summary>
    /// Aktiviert Metriken für das xSdk.Extensions.AI Paket.
    /// Optionale Bibliotheken registrieren ihre Meters selbst via
    /// <see cref="ITelemetryPluginBuilder.ConfigureMetrics"/>.
    /// </summary>
    /// <param name="builder">Der <see cref="MeterProviderBuilder"/>.</param>
    public static MeterProviderBuilder AddAIInstrumentation(this MeterProviderBuilder builder)
        => builder
            .AddMeter(Diagnostics.SourceName);
}
