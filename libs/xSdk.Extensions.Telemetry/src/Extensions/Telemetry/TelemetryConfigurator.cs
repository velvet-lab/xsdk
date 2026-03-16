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

using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Telemetry;

public static class TelemetryConfigurator
{
    //private static EnvironmentSetup _envSetup;
    //private static TelemetrySetup _telemetrySetup;
    //private static ResourceBuilder _resourceBuilder;

    private static readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

    private enum TelemetryType
    {
        Logging,
        Metrics,
        Tracing,
    }

    public static void ConfigureOtlpExporter(this MeterProviderBuilder builder)
    {
        Initialize(
            TelemetryType.Metrics,
            (envSetup, telemetrySetup, resourceBuilder) =>
            {
                builder
                    .AddMeter(envSetup.ServiceFullName)
                    .SetResourceBuilder(resourceBuilder)
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter((setup, metricsReader) => Configure(setup, telemetrySetup));
            }
        );
    }

    public static void ConfigureOtlpExporter(this TracerProviderBuilder builder, Action<EntityFrameworkInstrumentationOptions> efPostSetup)
    {
        Initialize(
            TelemetryType.Tracing,
            (envSetup, telemetrySetup, resourceBuilder) =>
            {
                builder
                    // Important! Main ActivitySource has the same name
                    .AddSource(envSetup.ServiceFullName)
                    .SetResourceBuilder(resourceBuilder)
                    // If Sampler is not set to always on, StartActivity will allways return null
                    // see https://stackoverflow.com/questions/76356454/opentelemetry-activitysource-startactivity-returns-null-activity-when-there-are
                    .SetSampler(new AlwaysOnSampler())
                    .AddEntityFrameworkCoreInstrumentation(options => efPostSetup?.Invoke(options))
                    .AddOtlpExporter(setup => Configure(setup, telemetrySetup));
            }
        );
    }

    public static void ConfigureOtlpExporter(this ILoggingBuilder builder, Action<OpenTelemetryLoggerOptions> postSetup)
    {
        Initialize(
            TelemetryType.Logging,
            (envSetup, telemetrySetup, resourceBuilder) =>
            {
                builder.AddOpenTelemetry(options =>
                {
                    options.IncludeFormattedMessage = true;
                    options.IncludeScopes = true;

                    options.SetResourceBuilder(resourceBuilder).AddOtlpExporter(setup => Configure(setup, telemetrySetup));

                    postSetup?.Invoke(options);
                });
            }
        );
    }

    private static void Initialize(TelemetryType type, Action<EnvironmentSetup, TelemetrySetup, ResourceBuilder> configure)
    {
        //var envSetup = SlimHost.Instance.Environment;
        //var telemetrySetup = SlimHost.Instance.GetSetup<TelemetrySetup>();

        //if (!telemetrySetup.IsDisabled)
        //{
        //    // Send data to central Otlp Exporter in the Cloud
        //    var isEndpointSetupValid = IsOtlpEndpointSetupValid(telemetrySetup);
        //    if (isEndpointSetupValid)
        //    {
        //        var resourceBuilder = TelemetryService.CreateResourceBuilder();
        //        if (type == TelemetryType.Metrics && !telemetrySetup.IsMetricsDisabled)
        //        {
        //            configure?.Invoke(envSetup, telemetrySetup, resourceBuilder);
        //        }
        //        else if (type == TelemetryType.Tracing && !telemetrySetup.IsTracingDisabled)
        //        {
        //            configure?.Invoke(envSetup, telemetrySetup, resourceBuilder);
        //        }
        //        else if (type == TelemetryType.Logging && !telemetrySetup.IsLoggingDisabled)
        //        {
        //            configure?.Invoke(envSetup, telemetrySetup, resourceBuilder);
        //        }
        //    }
        //}
    }

    private static bool IsOtlpEndpointSetupValid(TelemetrySetup telemetrySetup)
    {
        if (!string.IsNullOrEmpty(telemetrySetup.Endpoint) && !string.IsNullOrEmpty(telemetrySetup.Token))
        {
            return true;
        }

        _logger.Warn("Telemetry is enabled, but endpoint and token is missing. MaaS Observability is not possible.");

        return false;
    }

    private static void Configure(OtlpExporterOptions options, TelemetrySetup telemetrySetup)
    {
        // Adding the OtlpExporter creates a GrpcChannel.
        // This switch must be set before creating a GrpcChannel when calling an insecure gRPC service.
        // See: https://docs.microsoft.com/aspnet/core/grpc/troubleshoot#call-insecure-grpc-services-with-net-core-client
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        options.Protocol = OtlpExportProtocol.Grpc;
        options.Endpoint = new Uri(telemetrySetup.Endpoint);
        options.Headers = $"OTEL_KEY={telemetrySetup.Token}";
    }
}
