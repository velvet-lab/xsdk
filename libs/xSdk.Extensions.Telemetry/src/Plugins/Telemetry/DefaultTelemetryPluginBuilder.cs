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

using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Telemetry;
using xSdk.Extensions.Variable;
using xSdk.Hosting;

namespace xSdk.Plugins.Telemetry;

internal class DefaultTelemetryPluginBuilder : PluginBuilder<TelemetrySetup>, ITelemetryPluginBuilder
{
    public void ConfigureLoggingProvider(LoggerProviderBuilder builder)
    {
        builder
            .AddConsoleExporter()
            .AddOtlpExporter(ConfigureOtlpExporter);
    }

    public void ConfigureLoggingOptions(OpenTelemetryLoggerOptions options)
    {
        options.IncludeFormattedMessage = true;
        options.IncludeScopes = true;
    }

    public void ConfigureMetrics(MeterProviderBuilder builder)
    {
        var envSetup = LoadSetup<EnvironmentSetup>();

        builder
            .AddMeter(envSetup.ServiceFullName)
            .AddAspNetCoreInstrumentation()
            .AddEventCountersInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter(ConfigureOtlpExporter);
    }

    public void ConfigureTracing(TracerProviderBuilder builder)
    {
        var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
        builder
            .AddSource(envSetup.ServiceFullName)
            .AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRedisInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter(ConfigureOtlpExporter);
    }

    private void ConfigureOtlpExporter(OtlpExporterOptions options)
    {
        var telemetrySetup = SlimHost.Instance.VariableSystem.GetSetup<TelemetrySetup>();
        if (telemetrySetup.IsValid(true))
        {
            // Adding the OtlpExporter creates a GrpcChannel.
            // This switch must be set before creating a GrpcChannel when calling an insecure gRPC service.
            // See: https://docs.microsoft.com/aspnet/core/grpc/troubleshoot#call-insecure-grpc-services-with-net-core-client
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            options.Protocol = OtlpExportProtocol.Grpc;
            options.Endpoint = new Uri(telemetrySetup.Endpoint);
            // options.Headers = $"OTEL_KEY={telemetrySetup.Token}";


        }
    }
}
