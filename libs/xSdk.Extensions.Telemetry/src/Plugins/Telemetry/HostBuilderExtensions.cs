using xSdk.Extensions.Telemetry;
using xSdk.Hosting;
using Microsoft.Extensions.Hosting;

namespace xSdk.Plugins.Telemetry
{
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
}
