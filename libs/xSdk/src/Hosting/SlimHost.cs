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
using xSdk.Extensions.Logging;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public class SlimHost
{
    private bool _isBuilded;
    private IServiceCollection _slimServices = null!;
    private readonly List<Action<IServiceCollection>> _slimServicesDelegates = [];
    private IServiceCollection? _hostServices;
    private readonly List<Action> _hostServicesDelegates = [];
    private ApplicationOptions? _applicationOptions;
    private readonly List<Type> _registeredPluginHostTypes = [];

    public IEnumerable<TPluginHost> GetPluginHosts<TPluginHost>()
        where TPluginHost : IPluginHost
    {
        bool onlyWebPlugins = typeof(TPluginHost) != typeof(IPluginHost);

        IEnumerable<TPluginHost> plugins = Provider.GetServices<IPluginHost>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<PluginHost>()
            .Where(x => x.IsWebPluginHost == onlyWebPlugins)
            .Cast<TPluginHost>();

        return plugins;
    }

    internal IServiceProvider Provider
    {
        get
        {
            field ??= _slimServices.BuildServiceProvider();
            _isBuilded = true;

            return field;
        }
    }

    internal SlimHost() { }

    internal void ConfigurePluginHost(Action<IPluginHost> factory)
    {
        IEnumerable<IPluginHost> plugins = GetPluginHosts<IPluginHost>();
        foreach (IPluginHost plugin in plugins)
        {
            factory?.Invoke(plugin);
        }
    }

    internal static SlimHost InitializeSlimHost(string[] args, ApplicationOptions appOptions)
    {
        var instance = new SlimHost
        {
            _applicationOptions = appOptions,
            _slimServices = new ServiceCollection()
        };

        instance.ConfigureDefaults(instance._slimServices);
        instance._slimServices
            .AddSingleton<IPluginHostCollection>(_ => new PluginHostCollection(instance._registeredPluginHostTypes.AsReadOnly()))
            .AddHostedService<SlimHostInitializer>();

        return instance;
    }

    internal void PostConfigure(IServiceCollection hostServices)
    {
        _hostServices = hostServices;
        foreach (Action action in _hostServicesDelegates)
        {
            action?.Invoke();
        }

        // Expose the same IPluginHostCollection in the application DI container
        // so post-build consumers (e.g. HostInitializer) can inject it directly.
        hostServices.AddSingleton<IPluginHostCollection>(
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
            TPluginHostImplementation pluginHost = ActivatorUtilities.CreateInstance<TPluginHostImplementation>(provider);
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
        if (_hostServices != null)
        {
            _hostServices.AddSingleton<TPluginBuilder, TPluginBuilderImplementation>();
        }
        else
        {
            _hostServicesDelegates.Add(new Action(() => _hostServices?.AddSingleton<TPluginBuilder, TPluginBuilderImplementation>()));
        }
    }

    internal void RegisterPluginHostOptions<TOptions>(Action<TOptions>? configureOptions)
        where TOptions : class, IVariableSetup
    {
        if (_isBuilded)
        {
            throw new SdkException("Cannot register plugin host options after the service provider has been built.");
        }

        if (configureOptions != null)
        {
            _slimServices.RegisterOptions<TOptions>(configureOptions);
            if (_hostServices != null)
            {
                _hostServices.RegisterOptions<TOptions>(configureOptions);
            }
            else
            {
                _hostServicesDelegates.Add(new Action(() => _hostServices?.RegisterOptions<TOptions>(configureOptions)));
            }
        }
        else
        {
            _slimServices.RegisterOptions<TOptions>();
            if (_hostServices != null)
            {
                _hostServices.RegisterOptions<TOptions>();
            }
            else
            {
                _hostServicesDelegates.Add(new Action(() => _hostServices?.RegisterOptions<TOptions>()));
            }
        }
    }

    internal void RegisterPluginServices(Action<IServiceCollection> configureServices)        
    {
        if (_isBuilded)
        {
            throw new SdkException("Cannot register plugin services after the service provider has been built.");
        }

        configureServices(_slimServices);
    }

    internal void RegisterHostServices(Action<IServiceCollection> configureServices)
    {   
        if (_hostServices != null)
        {
            configureServices(_hostServices);
        }
        else
        {
            _hostServicesDelegates.Add(new Action(() => configureServices(_hostServices!)));
        }
    }

    internal EnvironmentOptions? BuildEnvironmentOptions()
    {
        // Builds temporary Environment Options
        if (_applicationOptions == null)
        {
            throw new SdkException("Application options must be set before building environment options.");
        }

        var services = new ServiceCollection();

        ConfigureDefaults(services);

        ServiceProvider provider = services.BuildServiceProvider();

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
            .RegisterOptions<EnvironmentOptions>(options => options.PostConfigure(_applicationOptions))
            .AddSingleton<IConfiguration>(provider => default!)
            .AddLoggingQueue()
            .AddVariableServices()
            .AddFileServices();
    }
}
