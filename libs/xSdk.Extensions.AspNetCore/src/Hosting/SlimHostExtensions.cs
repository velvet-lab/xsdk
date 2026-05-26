using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

internal static class SlimHostExtensions
{
    public static void ConfigureWebPluginHost(this SlimHost slimHost, Action<IWebPluginHost> factory)
    {
        IEnumerable<IPluginHost> plugins = slimHost.Provider.GetServices<IPluginHost>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<IPluginHost>();

        foreach (IPluginHost plugin in plugins)
        {
            Type pluginType = plugin.GetType();
            if (pluginType.IsAssignableTo(typeof(IWebPluginHost)) && plugin is IWebPluginHost webPlugin)
            {
                factory?.Invoke(webPlugin);
            }
        }        
    }
}
