using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using xSdk.Extensions.Options;
using xSdk.Extensions.Telemetry;
using xSdk.Extensions.Variable;

namespace xSdk.Demos.Builder;

public class AITelemetryBuilder(IVariableService variableService, IOptions<EnvironmentOptions> environmentOptions) : TelemetryBuilder
{
    internal const string OtlpEndpoint = "http://192.168.189.31:4317";

    public override void ConfigureBuilder()
    {
        this.WithLogging(ConfigureLoggingProvider, ConfigureLoggingOptions)
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
            .AddDetector(variableService.CreateResourceDetector)
            .AddService(serviceName: setup.ServiceName, serviceNamespace: setup.ServiceNamespace, serviceVersion: setup.ServiceVersion);
    }

    private static void ConfigureLoggingOptions(OpenTelemetryLoggerOptions options)
    {
        options.IncludeFormattedMessage = true;
        options.IncludeScopes = true;
    }

    private static void ConfigureLoggingProvider(LoggerProviderBuilder builder)
    {
        builder
            // Add Exporters
            .AddOtlpExporter(ConfigureOtlp);
    }

    private static void ConfigureMetrics(MeterProviderBuilder builder)
    {
        builder
            .AddAIInstrumentation()
            .AddAspNetCoreInstrumentation()
            //.AddEventCountersInstrumentation()
            .AddHttpClientInstrumentation()
            //.AddRuntimeInstrumentation()
            //.AddProcessInstrumentation()
            // Add Exporters
            .AddOtlpExporter(ConfigureOtlp);
    }

    private static void ConfigureTracing(TracerProviderBuilder builder)
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
