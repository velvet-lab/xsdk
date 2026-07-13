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
