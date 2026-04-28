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
using OpenTelemetry.Resources;
using xSdk.Extensions.Options;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;

namespace xSdk.Plugins.Telemetry;

public sealed class TelemetryPluginHost(IOptions<TelemetryOptions> telemetryOptions, IOptions<EnvironmentOptions> environmentOptions) : PluginHost
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var telemetrySetup = telemetryOptions.Value;

        if (!telemetrySetup.IsDisabled)
        {
            services.AddTelemetryServices();

            // Create an builder
            var telemetryBuilder = services
                .AddOpenTelemetry()
                .ConfigureResource(ConfigureResourceBuilder);

            // Configure Tracing
            if (!telemetrySetup.IsTracingDisabled)
            {
                telemetryBuilder.WithTracing(builder =>
                {
                    // Call tracing configuration from possible other Startups
                    InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureTracing(builder));
                });
            }

            // Configure Metrics
            if (!telemetrySetup.IsMetricsDisabled)
            {
                telemetryBuilder.WithMetrics(builder =>
                {
                    // Call metrics configuration from possible other Startups
                    InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureMetrics(builder));
                });
            }

            // Configure Logging
            if (!telemetrySetup.IsLoggingDisabled)
            {
                telemetryBuilder.WithLogging(builder =>
                {
                    // Call logging configuration from possible other Startups                    
                    InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureLoggingProvider(builder));
                },
                options =>
                {
                    // Call logging configuration from possible other Startups                    
                    InvokeBuilders<ITelemetryPluginBuilder>(plugin => plugin.ConfigureLoggingOptions(options));
                });
            }
        }
    }

    private void ConfigureResourceBuilder(ResourceBuilder resourceBuilder)
    {
        var setup = environmentOptions.Value;

        resourceBuilder
            .AddEnvironmentVariableDetector()
            .AddTelemetrySdk()
            .AddContainerDetector()
            .AddHostDetector()
            .AddOperatingSystemDetector()
            .AddProcessDetector()
            .AddProcessRuntimeDetector()
            // .AddAttributes(resources)            
            // .AddDetector(provider =>
            //{
            //    // Up2date Resources, is better than static resources
            //    var resources = SlimHost.Instance.VariableSystem.BuildResources();
            //    return new VariableResourceDetector(resources);
            //})
            .AddService(serviceName: setup.ServiceName, serviceNamespace: setup.ServiceNamespace, serviceVersion: setup.ServiceVersion);

    }
}
