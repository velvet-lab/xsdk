using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Hosting;

public static class PluginHostExtensions
{
    public static void SetServiceProvider(this IPluginHost host, IServiceProvider serviceProvider)
    {
        if (host is PluginHost pluginHost)
        {
            pluginHost.Services = serviceProvider;
        }
        else
        {
            throw new InvalidOperationException($"The plugin host '{host.Name}' does not support setting a service provider.");
        }
    }
}
