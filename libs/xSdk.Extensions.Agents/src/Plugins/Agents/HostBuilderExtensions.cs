using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;

namespace xSdk.Plugins.Agents;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnableAgents<TPluginBuilder>(this IHostBuilder hostBuilder)
        where TPluginBuilder : class, IAgentsPluginBuilder
    {
        return hostBuilder
            .RegisterPluginHost<AgentsPluginHost>()
            .RegisterPluginBuilder<IAgentsPluginBuilder, TPluginBuilder>();
    }
}
