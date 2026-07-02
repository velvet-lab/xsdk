using xSdk.Plugins.Telemetry;


namespace xSdk.Demos;

public static class TelemetryConfiguration
{
    public static void Default(PluginOptions options)
    {
        options.LoggingEnabled = true;
        options.TracingEnabled = true;
        options.MetricsEnabled = true;
    }
}
