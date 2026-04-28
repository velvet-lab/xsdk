/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using xSdk.Extensions.IO;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public class SlimHost
{
    private bool _isBuilded;
    private IServiceCollection _slimServices = null!;
    private IServiceCollection? _appServices;
    private readonly List<Action> _appServicesDelegates = [];
    private IServiceProvider? _serviceProvider;
    private ApplicationOptions? _applicationOptions;
    private readonly List<Type> _registeredPluginHostTypes = [];

    private IServiceProvider Provider
    {
        get
        {
            _serviceProvider ??= _slimServices.BuildServiceProvider();
            _isBuilded = true;

            return _serviceProvider;
        }
    }

    internal SlimHost() { }

    internal void ConfigurePluginHost<TPluginHost>(Action<IPluginHost> factory)
        where TPluginHost : IPluginHost
        => ConfigurePluginHostInternal(factory, default);

    internal void ConfigureWebPluginHost<TPluginHost>(Action<IWebPluginHost> factory)
        where TPluginHost : IWebPluginHost
        => ConfigurePluginHostInternal(default, factory);

    private void ConfigurePluginHostInternal(Action<IPluginHost>? factory, Action<IWebPluginHost>? webFactory)
    {
        var plugins = Provider.GetServices<IPluginHost>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<IPluginHost>();

        foreach (IPluginHost plugin in plugins)
        {
            factory?.Invoke(plugin);

            var pluginType = plugin.GetType();
            if (pluginType.IsAssignableTo(typeof(IWebPluginHost)))
            {
                IWebPluginHost? webPlugin = plugin as IWebPluginHost;
                if (webPlugin != null)
                {
                    webFactory?.Invoke(webPlugin);
                }
            }
        }
    }

    internal IEnumerable<TPluginHost> GetPluginHosts<TPluginHost>()
        where TPluginHost : IPluginHost
    {
        var plugins = Provider.GetServices<IPluginHost>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<IPluginHost>();

        List<TPluginHost> result = new();
        foreach (IPluginHost plugin in plugins)
        {
            Type pluginType = plugin.GetType();
            if (pluginType.IsAssignableTo(typeof(TPluginHost)))
            {
                result.Add((TPluginHost)plugin);
            }
        }

        return result;
    }

    internal static SlimHost InitializeSlimHost(string[] args, ApplicationOptions appOptions)
    {
        var instance = new SlimHost();
        instance._applicationOptions = appOptions;
        instance._slimServices = new ServiceCollection();
        instance.ConfigureDefaults(instance._slimServices);
        instance._slimServices.AddSingleton<IPluginHostCollection>(_ =>
            new PluginHostCollection(instance._registeredPluginHostTypes.AsReadOnly()));
        return instance;
    }

    internal void PostConfigure(IServiceCollection applicationServices)
    {
        _appServices = applicationServices;
        foreach (var action in _appServicesDelegates)
        {
            action?.Invoke();
        }

        // Expose the same IPluginHostCollection in the application DI container
        // so post-build consumers (e.g. HostInitializer) can inject it directly.
        applicationServices.AddSingleton<IPluginHostCollection>(
            new PluginHostCollection(_registeredPluginHostTypes.AsReadOnly()));
    }

    internal void RegisterPluginHost<TPluginHost, TPluginHostImplementation>()
        where TPluginHost : class, IPluginHost
        where TPluginHostImplementation : class, TPluginHost
    {
        if (_isBuilded)
        {
            throw new SdkException("Cannot register plugin host after the service provider has been built.");
        }

        _registeredPluginHostTypes.Add(typeof(TPluginHostImplementation));
        _slimServices.AddSingleton<TPluginHost>(provider =>
        {
            var pluginHost = ActivatorUtilities.CreateInstance<TPluginHostImplementation>(provider);
            pluginHost.SetServiceProvider(provider);

            return pluginHost;
        });
    }

    internal void RegisterPluginBuilder<TPluginBuilder, TPluginBuilderImplementation>()
        where TPluginBuilder : class, IPluginBuilder
        where TPluginBuilderImplementation : class, TPluginBuilder
    {
        if (_isBuilded)
        {
            throw new SdkException("Cannot register plugin builder after the service provider has been built.");
        }

        _slimServices.AddSingleton<TPluginBuilder, TPluginBuilderImplementation>();
        if (_appServices != null)
        {
            _appServices.AddSingleton<TPluginBuilder, TPluginBuilderImplementation>();
        }
        else
        {
            _appServicesDelegates.Add(new Action(() =>
            {
                _appServices?.AddSingleton<TPluginBuilder, TPluginBuilderImplementation>();
            }));
        }
    }

    internal void RegisterPluginHostOptions<TOptions>()
        where TOptions : class, IVariableSetup
    {
        if (_isBuilded)
        {
            throw new SdkException("Cannot register plugin host options after the service provider has been built.");
        }

        _slimServices.RegisterOptions<TOptions>();
        if (_appServices != null)
        {
            _appServices.RegisterOptions<TOptions>();
        }
        else
        {
            _appServicesDelegates.Add(new Action(() =>
            {
                _appServices?.RegisterOptions<TOptions>();
            }));
        }
    }

    internal EnvironmentOptions? BuildEnvironmentOptions()
    {
        if (_applicationOptions == null)
        {
            throw new SdkException("Application options must be set before building environment options.");
        }

        var services = new ServiceCollection();

        ConfigureDefaults(services);

        var provider = services.BuildServiceProvider();

        return provider.GetService<IOptions<EnvironmentOptions>>()?.Value;
    }

    private void ConfigureDefaults(IServiceCollection services)
    {
        if (_applicationOptions == null)
        {
            throw new SdkException("Application options must be set before building environment options.");
        }

        services
            .AddSingleton<IServiceProvider>(provider => provider)
            .AddSingleton(provider => provider)
            .RegisterApplicationOptions(_applicationOptions)
            .RegisterOptions<EnvironmentOptions>()
            .AddSingleton<IConfiguration>(provider => default!)
            .AddLogging()
            .AddVariableServices()
            .AddFileServices();
    }
}
