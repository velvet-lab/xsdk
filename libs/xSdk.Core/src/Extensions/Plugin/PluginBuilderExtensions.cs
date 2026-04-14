using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Extensions.Plugin;

public static class PluginBuilderExtensions
{
    public static void SetServiceProvider(this IPluginBuilder builder, IServiceProvider provider)
    {
        if (builder is PluginBuilder pluginBuilder)
        {
            pluginBuilder.Services = provider;
        }
    }
}
