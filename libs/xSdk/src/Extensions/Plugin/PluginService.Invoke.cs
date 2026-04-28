namespace xSdk.Extensions.Plugin;

internal partial class PluginService
{
    public Task InvokePluginAsync<TPlugin>(Action<TPlugin> action, CancellationToken token = default)
        where TPlugin : IPlugin
    {
        return GetPluginAsync<TPlugin>(token)
            .ContinueWith(task =>
            {
                if (task.Result != null)
                {
                    action?.Invoke(task.Result);
                }
            }, token);
    }

    public Task InvokePluginsAsync<TPlugin>(Action<TPlugin> action, CancellationToken token = default)
        where TPlugin : IPlugin
    {
        return GetPluginsAsync<TPlugin>(token)
            .ContinueWith(task =>
            {
                IEnumerable<TPlugin> plugins = task.Result ?? new List<TPlugin>();
                if (plugins.Any())
                {
                    IEnumerable<TPlugin> sortedPlugins = plugins
                        .Cast<PluginDescription>()
                        .OrderBy(o => o.Order)
                        .Cast<TPlugin>();

                    foreach (var plugin in sortedPlugins)
                    {
                        action?.Invoke(plugin);
                    }
                }
            }, token);
    }
}
