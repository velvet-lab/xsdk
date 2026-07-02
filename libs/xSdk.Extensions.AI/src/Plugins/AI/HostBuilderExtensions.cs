using Microsoft.Extensions.Hosting;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

public static class HostBuilderExtensions
{
    extension(IHostBuilder hostBuilder)
    {
        public IHostBuilder EnableAI<TPluginBuilder>()
            where TPluginBuilder : class, IAIPluginBuilder
            => hostBuilder.EnableAI<TPluginBuilder>(options => { });

        public IHostBuilder EnableAI<TPluginBuilder>(Action<PluginOptions> configureOptions)
            where TPluginBuilder : class, IAIPluginBuilder
        {
            return hostBuilder
                .RegisterPluginHost<PluginHost>()
                .RegisterPluginHostOptions<PluginOptions>(configureOptions)
                .RegisterPluginBuilder<IAIPluginBuilder, TPluginBuilder>();
        }
    }
}
