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
    private ApplicationOptions? _applicationOptions;
    private readonly List<Type> _registeredPluginHostTypes = [];

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

    public IEnumerable<TPluginHost> GetPluginHosts<TPluginHost>()
        where TPluginHost : IPluginHost
    {
        bool searchForWebPluginHost = typeof(TPluginHost) != typeof(IPluginHost);

        IEnumerable<TPluginHost> plugins = Provider.GetServices<IPluginHost>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<IPluginHost>()
            .Where(x => x.IsWebPluginHost == searchForWebPluginHost)
            .Cast<TPluginHost>();

        return plugins;
    }

    internal static SlimHost InitializeSlimHost(string[] args, ApplicationOptions appOptions)
    {
        var instance = new SlimHost
        {
            _applicationOptions = appOptions,
            _slimServices = new ServiceCollection()
        };
        instance.ConfigureDefaults(instance._slimServices);
        instance._slimServices.AddSingleton<IPluginHostCollection>(_ =>
            new PluginHostCollection(instance._registeredPluginHostTypes.AsReadOnly()));
        return instance;
    }

    internal void PostConfigure(IServiceCollection applicationServices)
    {
        _appServices = applicationServices;
        foreach (Action action in _appServicesDelegates)
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
        if (_appServices != null)
        {
            _appServices.AddSingleton<TPluginBuilder, TPluginBuilderImplementation>();
        }
        else
        {
            _appServicesDelegates.Add(new Action(() => _appServices?.AddSingleton<TPluginBuilder, TPluginBuilderImplementation>()));
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
            if (_appServices != null)
            {
                _appServices.RegisterOptions<TOptions>(configureOptions);
            }
            else
            {
                _appServicesDelegates.Add(new Action(() => _appServices?.RegisterOptions<TOptions>(configureOptions)));
            }
        }
        else
        {
            _slimServices.RegisterOptions<TOptions>();
            if (_appServices != null)
            {
                _appServices.RegisterOptions<TOptions>();
            }
            else
            {
                _appServicesDelegates.Add(new Action(() => _appServices?.RegisterOptions<TOptions>()));
            }
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
            .AddLogging()
            .AddVariableServices()
            .AddFileServices();
    }
}
