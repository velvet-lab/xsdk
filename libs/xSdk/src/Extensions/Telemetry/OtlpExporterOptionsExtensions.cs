//using Microsoft.Extensions.Logging;
//using OpenTelemetry.Exporter;
//using xSdk.Hosting;

//namespace xSdk.Extensions.Telemetry;

//public static class OtlpExporterOptionsExtensions
//{
//    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

//    public static void Configure(this OtlpExporterOptions options, TelemetryPluginOptions? telemetryOptions)
//    {
//        if (telemetryOptions == null) return;        

//        if (telemetryOptions.Endpoint is not null)
//        {   

//            // Adding the OtlpExporter creates a GrpcChannel.
//            // This switch must be set before creating a GrpcChannel when calling an insecure gRPC service.
//            // See: https://docs.microsoft.com/aspnet/core/grpc/troubleshoot#call-insecure-grpc-services-with-net-core-client
//            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

//            options.Protocol = OtlpExportProtocol.Grpc;
//            options.Endpoint = new Uri(telemetryOptions.Endpoint);
//            // options.Headers = $"OTEL_KEY={telemetrySetup.Token}";
//        }
//        else
//        {
//            _logger.LogWarning("TelemetryPluginOptions is null. OtlpExporterOptions will not be configured.");            
//        }
//    }
//}
