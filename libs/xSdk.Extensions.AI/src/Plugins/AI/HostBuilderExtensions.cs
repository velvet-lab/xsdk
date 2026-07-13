using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.AI;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableAI(Action<AIBuilder> configure)
            => builder.EnableAI<AIBuilder>(configure, _ => { });

        public IHostBuilder EnableAI(Action<AIBuilder> configure, Action<AIOptions> optionsConfigure)
            => builder.EnableAI<AIBuilder>(configure, optionsConfigure);

        public IHostBuilder EnableAI<TBuilder>()
            where TBuilder : AIBuilder
            => builder.EnableAI<TBuilder>(_ => { }, _ => { });

        public IHostBuilder EnableAI<TBuilder>(Action<AIOptions> configure)
            where TBuilder : AIBuilder
            => builder.EnableAI<TBuilder>(_ => { }, configure);

        public IHostBuilder EnableAI<TBuilder>(Action<TBuilder> configure, Action<AIOptions> optionsConfigure)               
            where TBuilder : AIBuilder   
        {
            return builder
                .RegisterPluginHost<PluginHost<TBuilder>>()
                .RegisterPluginHostOptions<AIOptions>(optionsConfigure)
                .RegisterPluginServices(services =>
                {
                    services
                        .AddSingleton<YamlDeclarationLoader>();
                })
                .RegisterBuilder<TBuilder>(configure)
                .RegisterBuilder<ClientBuilder<TBuilder>>(ServiceLifetime.Transient)
                .RegisterBuilder<AgentBuilder<TBuilder>>(ServiceLifetime.Transient)
                .RegisterBuilder<ToolBuilder<TBuilder>>(ServiceLifetime.Transient);
        }
    }
}
