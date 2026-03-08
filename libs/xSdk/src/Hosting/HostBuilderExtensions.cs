using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnablePlugin<TPlugin>(this IHostBuilder builder)
    {
        SlimHostInternal.Instance.PluginSystem.AddPlugin<TPlugin>();
        return builder;
    }

    public static IHostBuilder EnablePlugin<TPlugin, TPluginBuilder>(this IHostBuilder builder)
        where TPlugin : IPlugin
        where TPluginBuilder : IPluginBuilder
    {
        SlimHostInternal.Instance.PluginSystem.AddPlugin<TPlugin>();
        SlimHostInternal.Instance.PluginSystem.AddPlugin<TPluginBuilder>();
        return builder;
    }

    public static IHostBuilder RegisterSetup<TSetup>(this IHostBuilder builder)
        where TSetup : class, ISetup, new()
    {
        SlimHostInternal.Instance.VariableSystem.RegisterSetup<TSetup>();
        return builder;
    }

    public static IHostBuilder RegisterSetup<TSetup>(this IHostBuilder builder, Action<TSetup>? configure)
        where TSetup : class, ISetup, new()
    {
        SlimHostInternal.Instance.VariableSystem.RegisterSetup<TSetup>(configure);
        return builder;
    }

    public static IHostBuilder RegisterSetup<TSetup>(this IHostBuilder builder, TSetup implementation)
        where TSetup : class, ISetup, new()
    {
        SlimHostInternal.Instance.VariableSystem.RegisterSetup<TSetup>(implementation);
        return builder;
    }
}
