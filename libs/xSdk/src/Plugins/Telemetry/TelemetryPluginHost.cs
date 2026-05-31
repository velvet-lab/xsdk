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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;

namespace xSdk.Plugins.Telemetry;

public sealed class TelemetryPluginHost(IOptions<TelemetryPluginOptions> telemetryOptions) : PluginHost
{

    public override void ConfigureServices(IServiceCollection services)
    {
        TelemetryPluginOptions telemetrySetup = telemetryOptions.Value;

        // Create an builder
        OpenTelemetryBuilder telemetryBuilder = services
            .AddOpenTelemetry()
            .ConfigureResource(builder => InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureResources(builder)));

        // Configure Tracing
        if (telemetrySetup.TracingEnabled)
        {
            telemetryBuilder.WithTracing(builder =>
            {
                // Call tracing configuration from possible other Startups
                InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureTracing(builder));
            });
        }

        // Configure Metrics
        if (telemetrySetup.MetricsEnabled)
        {
            telemetryBuilder.WithMetrics(builder =>
            {
                // Call metrics configuration from possible other Startups
                InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureMetrics(builder));
            });
        }

        // Configure Logging
        if (telemetrySetup.LoggingEnabled)
        {
            telemetryBuilder.WithLogging(
                builder =>
                    // Call logging configuration from possible other Startups
                    InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureLoggingProvider(builder)),
                options =>
                    // Call logging configuration from possible other Startups
                    InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureLoggingOptions(options)));
        }
    }
}
