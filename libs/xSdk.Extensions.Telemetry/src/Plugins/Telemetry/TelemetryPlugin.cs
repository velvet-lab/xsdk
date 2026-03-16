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
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;

namespace xSdk.Plugins.Telemetry;

internal class TelemetryPlugin : PluginBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var plugins = SlimHost.Instance.PluginSystem.GetPlugins();

        var telemetrySetup = SlimHost.Instance.VariableSystem.GetSetup<TelemetrySetup>();
        if (!telemetrySetup.IsDisabled)
        {
            telemetrySetup.Validate();

            // Create an builder
            var telemetryBuilder = services.AddOpenTelemetry();

            // Configure Tracing
            if (!telemetrySetup.IsTracingDisabled)
            {
                telemetryBuilder.WithTracing(options =>
                {
                    // Configure Exporter
                    options.ConfigureOtlpExporter(efPostSetup => { });

                    // Call tracing configuration from possible other Startups
                    SlimHost.Instance.PluginSystem.Invoke<ITelemetryPluginBuilder>(plugin => plugin.ConfigureTracing(options));
                });
            }

            // Configure Metrics
            if (!telemetrySetup.IsMetricsDisabled)
            {
                telemetryBuilder.WithMetrics(options =>
                {
                    // Configure Exporter
                    options.ConfigureOtlpExporter();

                    // Call metrics configuration from possible other Startups
                    SlimHost.Instance.PluginSystem.Invoke<ITelemetryPluginBuilder>(plugin => plugin.ConfigureMetrics(options));
                });
            }
        }

        // Configure Logging
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder
            // Configure Default Logging
            // .ConfigureAutomationLogging()

            // Configure Exporter
            .ConfigureOtlpExporter(options =>
            {
                // Call logging configuration from possible other Startups
                SlimHost.Instance.PluginSystem.Invoke<ITelemetryPluginBuilder>(plugin => plugin.ConfigureLogging(options));
            });
        });
    }
}
