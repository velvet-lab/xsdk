using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Telemetry;
using xSdk.Extensions.Variable;
using xSdk.Hosting;

namespace xSdk.Plugins.Telemetry;

internal class DefaultTelemetryPluginBuilder : PluginBuilderBase, ITelemetryPluginBuilder
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
        var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();

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
