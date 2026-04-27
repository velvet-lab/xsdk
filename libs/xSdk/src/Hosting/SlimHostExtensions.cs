using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static class SlimHostExtensions
{
    private const string SlimHostKey = "xSdk.Hosting.SlimHost";

    /// <summary>
    /// Stores the SlimHost instance in the builder's Properties dictionary,
    /// binding it to this specific builder so parallel builds stay isolated.
    /// </summary>
    internal static IHostBuilder SetSlimHost(this IHostBuilder builder, SlimHost slimHost)
    {
        builder.Properties[SlimHostKey] = slimHost;
        return builder;
    }

    /// <summary>
    /// Retrieves the SlimHost instance bound to this builder.
    /// </summary>
    public static SlimHost GetSlimHost(this IHostBuilder builder)
    {
        if (builder.Properties.TryGetValue(SlimHostKey, out var value) && value is SlimHost slimHost)
            return slimHost;

        throw new SdkException("No SlimHost found on this IHostBuilder. Ensure InitializeSlimHost was called first.");
    }

    public static IHostBuilder RegisterPluginHost<TPluginHost>(this IHostBuilder builder)
        where TPluginHost : class, IPluginHost
    {
        builder.GetSlimHost().RegisterPluginHost<IPluginHost, TPluginHost>();
        return builder;
    }

    public static IHostBuilder RegisterPluginBuilder<TPluginBuilder>(this IHostBuilder builder)
        where TPluginBuilder : class, IPluginBuilder
    {
        builder.GetSlimHost().RegisterPluginBuilder<IPluginBuilder, TPluginBuilder>();
        return builder;
    }

    public static IHostBuilder RegisterPluginBuilder<TPluginBuilder, TPluginBuilderImplementation>(this IHostBuilder builder)
        where TPluginBuilder : class, IPluginBuilder
        where TPluginBuilderImplementation : class, TPluginBuilder
    {
        builder.GetSlimHost().RegisterPluginBuilder<TPluginBuilder, TPluginBuilderImplementation>();
        return builder;
    }

    public static IHostBuilder RegisterPluginHostOptions<TOptions>(this IHostBuilder builder)
        where TOptions : class, IVariableSetup
    {
        builder.GetSlimHost().RegisterPluginHostOptions<TOptions>();
        return builder;
    }

    public static void ConfigurePluginHost(this SlimHost slimHost, Action<IPluginHost> factory)
    {
        slimHost.ConfigurePluginHost<IPluginHost>(factory);
    }

    public static void ConfigureWebPluginHost(this SlimHost slimHost, Action<IWebPluginHost> factory)
    {
        slimHost.ConfigureWebPluginHost<IWebPluginHost>(factory);
    }

    public static EnvironmentOptions GetEnvironment(this SlimHost slimHost)
    {
        EnvironmentOptions? options = slimHost.BuildEnvironmentOptions();
        if (options != null)
            return options;

        throw new SdkException("Failed to build environment options.");
    }

    public static IEnumerable<TPluginHost> GetPluginHosts<TPluginHost>(this SlimHost slimHost)
        where TPluginHost : IPluginHost
    {
        return slimHost.GetPluginHosts<TPluginHost>();
    }
}
