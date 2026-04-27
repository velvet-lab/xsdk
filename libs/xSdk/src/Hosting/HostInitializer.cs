using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Plugin;
using xSdk.Hosting.Managers;

namespace xSdk.Hosting;

internal sealed class HostInitializer(IPluginService pluginService, IPluginHostCollection pluginHostCollection, ILoggerFactory factory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Initialize the LogManager with the provided ILoggerFactory
        LogManager.Initialize(factory);

        // Register plugins with the plugin service
        foreach (Type pluginType in pluginHostCollection)
        {
            await pluginService.AddPluginAsync(pluginType, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
