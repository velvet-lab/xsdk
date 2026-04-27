using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static class SlimHostExtensions
{
    public static IHostBuilder RegisterPluginHost<TPluginHost>(this IHostBuilder builder)
        where TPluginHost : class, IPluginHost
    {
        SlimHost.Instance.RegisterPluginHost<IPluginHost, TPluginHost>();

        return builder;
    }

    public static IHostBuilder RegisterPluginBuilder<TPluginBuilder>(this IHostBuilder builder)
        where TPluginBuilder : class, IPluginBuilder
    {
        SlimHost.Instance.RegisterPluginBuilder<IPluginBuilder, TPluginBuilder>();

        return builder;
    }

    public static IHostBuilder RegisterPluginBuilder<TPluginBuilder, TPluginBuilderImplementation>(this IHostBuilder builder)
        where TPluginBuilder : class, IPluginBuilder
        where TPluginBuilderImplementation : class, TPluginBuilder
    {
        SlimHost.Instance.RegisterPluginBuilder<TPluginBuilder, TPluginBuilderImplementation>();

        return builder;
    }

    public static IHostBuilder RegisterPluginHostOptions<TOptions>(this IHostBuilder builder)
        where TOptions : class, IVariableSetup
    {
        SlimHost.Instance.RegisterPluginHostOptions<TOptions>();

        return builder;
    }

    public static void ConfigurePluginHost(this SlimHost slimHost, Action<IPluginHost> factory)
    {
        SlimHost.Instance.ConfigurePluginHost<IPluginHost>(factory);
    }

    public static void ConfigureWebPluginHost(this SlimHost slimHost, Action<IWebPluginHost> factory)        
    {        
        SlimHost.Instance.ConfigureWebPluginHost<IWebPluginHost>(factory);
    }

    public static EnvironmentOptions GetEnvironment(this SlimHost slimHost)
    {
        EnvironmentOptions? options = SlimHost.Instance.BuildEnvironmentOptions();
        if(options != null)
        {
            return options;
        }

        throw new SdkException("Failed to build environment options.");
    }

    public static IEnumerable<TPluginHost> GetPluginHosts<TPluginHost>(this SlimHost slimHost)
        where TPluginHost : IPluginHost
    {
        return slimHost.GetPluginHosts<TPluginHost>();
    }
}
