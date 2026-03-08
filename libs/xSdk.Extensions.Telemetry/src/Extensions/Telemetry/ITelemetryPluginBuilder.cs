using OpenTelemetry.Instrumentation.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using xSdk.Extensions.Plugin;

namespace xSdk.Extensions.Telemetry;

public interface ITelemetryPluginBuilder : IPluginBuilder
{
    void ConfigureEntityFrameworkInstrumentation(EntityFrameworkInstrumentationOptions options) { }

    void ConfigureLogging(OpenTelemetryLoggerOptions builder) { }

    void ConfigureMetrics(MeterProviderBuilder builder) { }

    void ConfigureTracing(TracerProviderBuilder builder) { }
}
