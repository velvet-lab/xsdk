using Microsoft.Extensions.Logging;
using xSdk.Extensions.IO;

namespace xSdk.Extensions.Plugin;

internal partial class PluginService(IFileSystemService fsService, ILogger<PluginService> logger) : IPluginService
{
    private readonly List<PluginItem> _plugins = new();

    public Task<TPlugin?> GetPluginAsync<TPlugin>(CancellationToken token = default) =>
        GetPluginsAsync<TPlugin>(token).ContinueWith(task => task.Result.FirstOrDefault());

    public async Task<IList<TPlugin>> GetPluginsAsync<TPlugin>(CancellationToken token = default)
    {
        var searchResult = new List<PluginItem>();

        await LoadPluginsAsync();
        foreach (var item in _plugins)
        {
            if (item.Plugin is TPlugin concretePlugin)
            {
                searchResult.Add(item);
            }
        }

        return searchResult.OrderBy(x => x.Order).Select(x => x.Plugin).Cast<TPlugin>().ToList();
    }
}
