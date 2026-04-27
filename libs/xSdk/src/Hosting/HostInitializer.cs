using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Hosting.Managers;

namespace xSdk.Hosting;

internal sealed class HostInitializer(IPluginService pluginService, ILoggerFactory factory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Initialize the LogManager with the provided ILoggerFactory
        LogManager.Initialize(factory);

        Type[] pluginTypes = SlimHost.Instance.GetPluginHosts<IPluginHost>()
            .Select(p => p.GetType())
            .Distinct()
            .ToArray();

        // Register plugins with the plugin service
        if (pluginService != null && pluginTypes.Any())
        {
            foreach (Type pluginType in pluginTypes)
            {
                await pluginService.AddPluginAsync(pluginType, cancellationToken);
            }
        }                
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
