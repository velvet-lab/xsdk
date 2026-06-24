using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.AI;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;
using xSdk.Plugins.Telemetry;

namespace xSdk.Plugins.AI;

public static class HostBuilderExtensions
{
    extension(IHostBuilder hostBuilder)
    {
        public IHostBuilder EnableAI<TPluginBuilder>()
            where TPluginBuilder : class, IAIPluginBuilder
            => hostBuilder.EnableAI<TPluginBuilder>(options => { });

        public IHostBuilder EnableAI<TPluginBuilder>(Action<AIPluginOptions> configureOptions)
            where TPluginBuilder : class, IAIPluginBuilder
        {
            return hostBuilder
                .RegisterPluginHost<AIPluginHost>()
                .RegisterPluginHostOptions<AIPluginOptions>(configureOptions)
                .RegisterPluginBuilder<IAIPluginBuilder, TPluginBuilder>();
        }
    }
}
