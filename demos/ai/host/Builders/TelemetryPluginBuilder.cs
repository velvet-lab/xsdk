using System;
using System.Collections.Generic;
using System.Text;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Telemetry;

namespace xSdk.Demos.Builders;

internal class TelemetryPluginBuilder : PluginBuilder, ITelemetryPluginBuilder
{
    public void ConfigureLoggingOptions(OpenTelemetryLoggerOptions options) => throw new NotImplementedException();
    public void ConfigureLoggingProvider(LoggerProviderBuilder builder) => throw new NotImplementedException();
    public void ConfigureMetrics(MeterProviderBuilder builder) => throw new NotImplementedException();
    public void ConfigureResources(ResourceBuilder builder) => throw new NotImplementedException();
    public void ConfigureTracing(TracerProviderBuilder builder) => throw new NotImplementedException();
}
