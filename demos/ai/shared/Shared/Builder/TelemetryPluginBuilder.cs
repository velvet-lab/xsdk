using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Telemetry;
using xSdk.Extensions.Variable;

namespace xSdk.Demos.Builder;

public class TelemetryPluginBuilder(IVariableService variableService, IOptions<EnvironmentOptions> environmentOptions) : PluginBuilder, ITelemetryPluginBuilder
{
    internal const string OtlpEndpoint = "http://192.168.189.31:4317";

    public void ConfigureResources(ResourceBuilder builder)
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
            .AddDetector(variableService.CreateResourceDetector)
            .AddService(serviceName: setup.ServiceName, serviceNamespace: setup.ServiceNamespace, serviceVersion: setup.ServiceVersion);
    }

    public void ConfigureLoggingOptions(OpenTelemetryLoggerOptions options)
    {
        options.IncludeFormattedMessage = true;
        options.IncludeScopes = true;        
    }

    public void ConfigureLoggingProvider(LoggerProviderBuilder builder)
    {
        builder
            // Add Exporters
            .AddOtlpExporter(ConfigureOtlp);
    }

    public void ConfigureMetrics(MeterProviderBuilder builder)
    {
        builder
            .AddAIInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddEventCountersInstrumentation()            
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            // Add Exporters
            .AddOtlpExporter(ConfigureOtlp);
    }

    public void ConfigureTracing(TracerProviderBuilder builder)
    {
        builder
            .AddAIInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddHttpClientInstrumentation()            
            // Add Exporters
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
    }
}
