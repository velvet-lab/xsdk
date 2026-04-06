using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting.Managers;

internal sealed class HostPluginManager
{
    private IServiceCollection _services = new ServiceCollection();
    private IServiceProvider? _serviceProvider;

    private static readonly Lazy<HostPluginManager> _instance = new(() => new HostPluginManager());

    internal static HostPluginManager Instance => _instance.Value;

    private HostPluginManager() { }

    private IServiceProvider Provider => _serviceProvider ??= _services.BuildServiceProvider();

    internal void AddPluginHost<TPluginHost>()
        where TPluginHost : class, IPluginHost
        => _services.AddSingleton<IPluginHost, TPluginHost>();

    internal void AddPluginBuilder<TPluginBuilder>()
        where TPluginBuilder : class, IPluginBuilder
        => _services.AddSingleton<IPluginBuilder, TPluginBuilder>();


    internal IEnumerable<IPlugin> Plugins => Provider.GetServices<IPluginHost>().Cast<IPlugin>()
        .Concat(Provider.GetServices<IPluginBuilder>().Cast<IPlugin>());


    internal void ConfigureHost<TPluginHost>(Action<TPluginHost> factory)
        where TPluginHost : IPluginHost
    {
        var plugins = Provider.GetServices<IPluginHost>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<TPluginHost>()
            .ToList();

        foreach (var plugin in plugins)
        {
            factory?.Invoke(plugin);
        }
    }

    internal bool ConfigurePlugin<TPluginBuilder>(Action<TPluginBuilder> factory)
        where TPluginBuilder : IPluginBuilder
    {
        var plugins = Provider
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
}
