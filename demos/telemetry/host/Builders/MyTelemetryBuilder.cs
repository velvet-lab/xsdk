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

using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using xSdk.Extensions.Options;
using xSdk.Extensions.Telemetry;
using xSdk.Extensions.Variable;

namespace xSdk.Demos.Builders;

internal class MyTelemetryBuilder(IVariableService variableService, IOptions<EnvironmentOptions> environmentOptions) : TelemetryBuilder
{
    internal const string OtlpEndpoint = "http://localhost:4317";

    public override void ConfigureBuilder()
    {
        this
            .WithLogging(ConfigureLogging, ConfigureLoggingOptions)
            .WithMetrics(ConfigureMetrics)
            .WithTracing(ConfigureTracing)
            .WithResources(ConfigureResources);
    }

    private void ConfigureResources(ResourceBuilder builder)
    {
        EnvironmentOptions setup = environmentOptions.Value;

        builder
            .AddEnvironmentVariableDetector()
            .AddTelemetrySdk()
            .AddContainerDetector()
            .AddHostDetector()
            .AddOperatingSystemDetector()
            .AddProcessDetector()
            .AddProcessRuntimeDetector()
            // .AddAttributes(resources)
            .AddDetector(variableService.CreateResourceDetector)
            .AddService(serviceName: setup.ServiceName, serviceNamespace: setup.ServiceNamespace, serviceVersion: setup.ServiceVersion);
    }

    private static void ConfigureLoggingOptions(OpenTelemetryLoggerOptions options)
    {
        options.IncludeFormattedMessage = true;
        options.IncludeScopes = true;
    }

    private static void ConfigureLogging(LoggerProviderBuilder builder)
    {
        builder
            // Add Exporters
            .AddConsoleExporter()
            .AddOtlpExporter(ConfigureOtlp);
    }

    private static void ConfigureMetrics(MeterProviderBuilder builder)
    {
        builder
            .AddMeter(Diagnostics.SourceName)
            .AddAspNetCoreInstrumentation()
            .AddEventCountersInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            // Add Exporters
            .AddConsoleExporter()
            .AddOtlpExporter(ConfigureOtlp);
    }

    private static void ConfigureTracing(TracerProviderBuilder builder)
    {
        builder
            .AddSource(Diagnostics.SourceName)
            .AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRedisInstrumentation()
            // Add Exporters
            .AddConsoleExporter()
            .AddOtlpExporter(ConfigureOtlp);
    }

    private static void ConfigureOtlp(OtlpExporterOptions options)
    {
        // Adding the OtlpExporter creates a GrpcChannel.
        // This switch must be set before creating a GrpcChannel when calling an insecure gRPC service.
        // See: https://docs.microsoft.com/aspnet/core/grpc/troubleshoot#call-insecure-grpc-services-with-net-core-client
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        options.Protocol = OtlpExportProtocol.Grpc;
        options.Endpoint = new Uri(OtlpEndpoint);
        // options.Headers = $"OTEL_KEY={telemetrySetup.Token}";
    }
}
