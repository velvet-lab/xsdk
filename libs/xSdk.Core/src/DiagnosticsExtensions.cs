using System;
using System.Collections.Generic;
using System.Text;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace xSdk;

public static class DiagnosticsExtensions
{
    /// <summary>
    /// Aktiviert Tracing für den xSdk Framework-Kern (<c>xSdk</c>, <c>xSdk.Core</c>).
    /// Optionale Bibliotheken registrieren ihre ActivitySources selbst via
    /// <see cref="ITelemetryPluginBuilder.ConfigureTracing"/>.
    /// </summary>
    /// <param name="builder">Der <see cref="TracerProviderBuilder"/>.</param>
    public static TracerProviderBuilder AddSdkInstrumentation(this TracerProviderBuilder builder)
        => builder
            .AddSource(Diagnostics.SourceName);

    /// <summary>
    /// Aktiviert Metriken für den xSdk Framework-Kern (<c>xSdk</c>, <c>xSdk.Core</c>).
    /// Optionale Bibliotheken registrieren ihre Meters selbst via
    /// <see cref="ITelemetryPluginBuilder.ConfigureMetrics"/>.
    /// </summary>
    /// <param name="builder">Der <see cref="MeterProviderBuilder"/>.</param>
    public static MeterProviderBuilder AddSdkInstrumentation(this MeterProviderBuilder builder)
        => builder
            .AddMeter(Diagnostics.SourceName);
}
