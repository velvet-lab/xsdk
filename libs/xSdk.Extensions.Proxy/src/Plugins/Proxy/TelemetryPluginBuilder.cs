using System;
using System.Collections.Generic;
using System.Text;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using xSdk.Extensions.Plugin;
using xSdk.Plugins.Telemetry;
using Yarp.Telemetry.Consumption;

namespace xSdk.Plugins.Proxy;

internal class TelemetryPluginBuilder : PluginBuilderBase, ITelemetryPluginBuilder
{
    public void ConfigureLoggingOptions(OpenTelemetryLoggerOptions options) { }

    public void ConfigureLoggingProvider(LoggerProviderBuilder builder) { }

    public void ConfigureMetrics(MeterProviderBuilder builder) {

        
    }

    public void ConfigureTracing(TracerProviderBuilder builder) { }
}
