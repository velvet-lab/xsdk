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

using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Hosting;

public sealed class SlimHostBuilder
{
    private Action<IServiceCollection> _configureServicesDelegate;
    private SlimHostBase _host;

    public static SlimHostBuilder CreateBuilder<TSlimHost>()
        where TSlimHost : ISlimHost, new()
    {
        // Create the builder
        var builder = new SlimHostBuilder();
#nullable disable
        // Create the host
        builder._host = new TSlimHost() as SlimHostBase;
#nullable restore

        // return the builder
        return builder;
    }

    public ISlimHost PreBuild()
    {
        // Return a not fully initialized host
        return _host;
    }

    public ISlimHost Build()
    {
        // Prepare and build service collection
        var services = new ServiceCollection();
        _configureServicesDelegate?.Invoke(services);

        // Save the provider to the host
        var provider = services.BuildServiceProvider();
        _host.Configure(provider);

        // Configure only the fake SlimHost in the Abstractions Library
        SlimHost.Configure(_host);

        // Return the fully initialized host
        return _host;
    }

    public SlimHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate)
    {
        this._configureServicesDelegate = configureDelegate;

        return this;
    }

    public SlimHostBuilder ValidateAppPrefix(string? appPrefix, string defaultValue)
    {
        if (string.IsNullOrEmpty(appPrefix))
        {
            appPrefix = defaultValue;
        }

        if (_host != null)
        {
            _host.AppPrefix = appPrefix;
        }

        return this;
    }

    public SlimHostBuilder ValidateAppName(string? appName, string defaultValue)
    {
        if (string.IsNullOrEmpty(appName))
        {
            appName = defaultValue;
        }

        if (_host != null)
        {
            _host.AppName = appName;
        }

        return this;
    }

    public SlimHostBuilder ValidateAppCompany(string? appCompany, string defaultValue)
    {
        if (string.IsNullOrEmpty(appCompany))
        {
            appCompany = defaultValue;
        }

        if (_host != null)
        {
            _host.AppCompany = appCompany;
        }

        return this;
    }

    public SlimHostBuilder ValidateAppVersion(string? appVersion)
    {
        if (!string.IsNullOrEmpty(appVersion) && _host != null)
        {
            _host.AppVersion = appVersion;
        }

        return this;
    }
}
