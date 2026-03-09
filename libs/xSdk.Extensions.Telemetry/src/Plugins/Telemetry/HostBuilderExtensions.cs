using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;

namespace xSdk.Plugins.Telemetry;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnableTelemetry(this IHostBuilder builder)
    {
        builder.RegisterSetup<TelemetrySetup>().EnablePlugin<TelemetryPlugin>();

        return builder;
    }

    public static IHostBuilder EnableTelemetry<TPluginBuilder>(this IHostBuilder builder)
        where TPluginBuilder : ITelemetryPluginBuilder
    {
        builder.EnableTelemetry().EnablePlugin<TPluginBuilder>();

        return builder;
    }
}
