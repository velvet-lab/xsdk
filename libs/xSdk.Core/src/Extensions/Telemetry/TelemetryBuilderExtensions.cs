/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace xSdk.Extensions.Telemetry;

/// <summary>
/// Extension-Methoden analog zu <c>AddAspNetCoreInstrumentation()</c> für das xSdk.
/// Registriert die ActivitySources und Meters des Framework-Kerns explizit (ADR-014).
/// Optionale Bibliotheken (Data.*, Extensions.*) registrieren sich selbst via <see cref="ITelemetryPluginBuilder"/>.
/// </summary>
public static class TelemetryBuilderExtensions
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
