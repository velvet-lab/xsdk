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

using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using xSdk.Extensions.Builder;

namespace xSdk.Extensions.Telemetry;

public sealed class TelemetryBuilder : BuilderBase
{
    private OpenTelemetryBuilder? _telemetryBuilder;
    private ResourceBuilder? _resourceBuilder;

    internal TelemetryOptions Options => Services.GetService<IOptions<TelemetryOptions>>()?.Value ?? new TelemetryOptions();

    internal Action<ResourceBuilder>? ResourceBuilderAction;
    internal Action<MeterProviderBuilder>? ConfigureMetricsAction;
    internal Action<TracerProviderBuilder>? ConfigureTracingAction;
    internal Action<LoggerProviderBuilder>? ConfigureLoggingAction;
    internal Action<OpenTelemetryLoggerOptions>? ConfigureLoggingOptionsAction;

    internal void Build(IServiceCollection? services)
    {
        Guard.IsNotNull(services);

        // Create an builder
        _telemetryBuilder = services
            .AddOpenTelemetry();

        // ConfigureResource on OpenTelemetryBuilder invokes the callback once per active signal
        // (Tracing, Metrics, Logging). Pre-building the ResourceBuilder here ensures
        // InvokeBuilders<ConfigureResources> is called exactly once.
        _resourceBuilder = ResourceBuilder.CreateDefault();
        ResourceBuilderAction?.Invoke(_resourceBuilder);

        if (Options.TracingEnabled)
        {
            BuildTracing();
        }

        if (Options.MetricsEnabled)
        {
            BuildMetrics();
        }

        if (Options.LoggingEnabled)
        {
            BuildLogging();
        }
    }

    private void BuildMetrics()
    {
        _telemetryBuilder?.WithMetrics(metricsBuilder =>
        {
            metricsBuilder.SetResourceBuilder(_resourceBuilder ?? ResourceBuilder.CreateDefault());
            // Call metrics configuration from possible other Startups
            ConfigureMetricsAction?.Invoke(metricsBuilder);
        });
    }

    private void BuildTracing()
    {
        _telemetryBuilder?.WithTracing(tracingBuilder =>
        {
            tracingBuilder.SetResourceBuilder(_resourceBuilder ?? ResourceBuilder.CreateDefault());

            // Call tracing configuration from possible other Startups
            ConfigureTracingAction?.Invoke(tracingBuilder);
        });
    }

    private void BuildLogging()
    {
        _telemetryBuilder?.WithLogging(loggingBuilder =>
        {
            loggingBuilder.SetResourceBuilder(_resourceBuilder ?? ResourceBuilder.CreateDefault());
            ConfigureLoggingAction?.Invoke(loggingBuilder);
        },
        options => ConfigureLoggingOptionsAction?.Invoke(options));
    }
}
