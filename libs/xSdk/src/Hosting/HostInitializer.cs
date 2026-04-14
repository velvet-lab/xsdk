using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Hosting.Managers;

namespace xSdk.Hosting;

internal sealed class HostInitializer(IServiceProvider provider, IPluginService pluginService, ILoggerFactory factory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        LogManager.Initialize(factory);

        var pluginAssemblies = HostPluginManager.Instance.Plugins
            .Select(p => p.GetType().Assembly)
            .Distinct()
            .ToArray();

        if (pluginService != null)
        {
            if (pluginAssemblies.Any())
            {
                await pluginService.AddPluginsFromAsync(pluginAssemblies, cancellationToken);
            }

            var pluginBuilders = pluginService.GetPlugins<IPluginBuilder>();
            if (pluginBuilders.Any())
            {
                foreach(IPluginBuilder builder in pluginBuilders)
                {
                    builder.SetServiceProvider(provider);
                }
            }
        }        
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
