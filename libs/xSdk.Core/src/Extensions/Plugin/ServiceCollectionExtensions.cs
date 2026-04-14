using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Hosting;

namespace xSdk.Extensions.Plugin;

public static class ServiceCollectionExtensions
{
    public static bool InvokePluginBuilder<TPluginBuilder>(this IServiceProvider provider, Action<TPluginBuilder> factory)
        where TPluginBuilder : IPluginBuilder
    {
        var plugins = provider
            .GetServices<TPluginBuilder>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<TPluginBuilder>()
            .ToList();

        foreach (TPluginBuilder plugin in plugins)
        {
            factory?.Invoke(plugin);
        }
        return plugins.Any();
    }

    public static TPluginBuilder? RetrievePluginBuilder<TPluginBuilder>(this IServiceProvider provider)
        where TPluginBuilder : IPluginBuilder
    {
        var pluginBuilders = provider.GetServices<TPluginBuilder>();

        if (pluginBuilders.Any())
        {
            var pluginBuilder = pluginBuilders
                .Cast<PluginDescription>()
                .OrderBy(p => p.Order)
                .Cast<TPluginBuilder>()
                .Single();

            return pluginBuilder;
        }

        return default;        
    }
}
