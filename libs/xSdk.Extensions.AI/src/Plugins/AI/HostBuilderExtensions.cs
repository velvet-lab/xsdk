using Microsoft.Extensions.Hosting;
using xSdk.Extensions.AI;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;
using xSdk.Plugins.Telemetry;

namespace xSdk.Plugins.AI;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnableAI<TPluginBuilder>(this IHostBuilder hostBuilder)
        where TPluginBuilder : class, IAIPluginBuilder
        => hostBuilder.EnableAI<TPluginBuilder>(options => { });

    public static IHostBuilder EnableAI<TPluginBuilder>(this IHostBuilder hostBuilder, Action<AIPluginOptions> configureOptions)
        where TPluginBuilder : class, IAIPluginBuilder
    {
        return hostBuilder
            .RegisterPluginHost<AIPluginHost>()
            .RegisterPluginHostOptions<AIPluginOptions>(configureOptions)
            .RegisterPluginBuilder<IAIPluginBuilder, TPluginBuilder>();
    }
}
