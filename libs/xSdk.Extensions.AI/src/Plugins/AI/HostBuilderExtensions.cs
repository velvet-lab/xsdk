using Microsoft.Extensions.Hosting;
using xSdk.Extensions.AI;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnableAgents<TPluginBuilder>(this IHostBuilder hostBuilder, Action<AgentsPluginOptions> configureOptions)
        where TPluginBuilder : class, IAgentsPluginBuilder
    {
        return hostBuilder
            .RegisterPluginHost<AgentsPluginHost>()
            .RegisterPluginHostOptions<AgentsPluginOptions>(configureOptions)
            .RegisterPluginBuilder<IAgentsPluginBuilder, TPluginBuilder>();
    }
}
