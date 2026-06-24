using System;
using System.Collections.Generic;
using System.Text;
using xSdk.Extensions.AI;
using xSdk.Extensions.Telemetry;

namespace xSdk.Demos;

public static class TelemetryConfiguration
{
    public static void Default(TelemetryPluginOptions options)
    {
        options.LoggingEnabled = true;
        options.TracingEnabled = true;
        options.MetricsEnabled = true;
    }
}
