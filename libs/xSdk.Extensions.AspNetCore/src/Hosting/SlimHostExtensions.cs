using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

internal static class SlimHostExtensions
{
    public static void ConfigureWebPluginHost(this SlimHost slimHost, Action<IWebPluginHost> factory)
    {
        IEnumerable<IWebPluginHost> plugins = slimHost.GetPluginHosts<IWebPluginHost>();
        foreach (IWebPluginHost plugin in plugins)
        {
            factory?.Invoke(plugin);
        }
    }
}
