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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public class TestHostFixture : IDisposable
{
    private readonly IHostBuilder _builder;
    private IHost? _host;

    private readonly List<Action<IServiceCollection>> _servicesDelegates = new();
    private readonly List<Action<HostBuilderContext, IServiceCollection>> _hostServicesDelegates = new();
    private readonly List<Action<WebHostBuilderContext, IServiceCollection>> _webhostServicesDelegates = new();

    internal List<Action<IHostBuilder>> builderDelegates = new();

    private bool _disposed;

    private bool? _currentDemoMode;
    private bool _demoModeShouldEnabled;

    private readonly bool _initializeShouldCalled;

    public TestHostFixture()
    {
        _builder = TestHost.CreateBuilder();
    }

    protected TestHostFixture(bool callInitialize)
    {
        _builder = TestHost.CreateBuilder();
        _initializeShouldCalled = callInitialize;
    }

    ~TestHostFixture()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IHostBuilder Builder => _builder;

    public IHost Host => Build();

    public string AppName => SlimHostInternal.Instance.AppName;

    public string AppCompany => SlimHostInternal.Instance.AppCompany;

    public string AppPrefix => SlimHostInternal.Instance.AppPrefix;

    public string AppVersion => SlimHostInternal.Instance.AppVersion;

    public TService? GetService<TService>()
        where TService : notnull => Host.Services.GetService<TService>();

    //public TService? GetService<TService>(Action<IServiceCollection> configure)
    //    where TService : notnull
    //    => TestHost.CreateBuilder().ConfigureServices(configure).Build().Services.GetService<TService>();



    public IEnumerable<TService> GetServices<TService>()
        where TService : notnull => Host.Services.GetServices<TService>();

    //public IEnumerable<TService> GetServices<TService>(Action<IServiceCollection> configure)
    //    where TService : notnull => TestHost.CreateBuilder().ConfigureServices(configure).Build().Services.GetServices<TService>();




    public TService GetRequiredService<TService>()
        where TService : notnull => Host.Services.GetRequiredService<TService>();

    //public TService GetRequiredService<TService>(Action<IServiceCollection> configure)
    //    where TService : notnull => TestHost.CreateBuilder().ConfigureServices(configure).Build().Services.GetRequiredService<TService>();



    public TService? GetRequiredKeyedService<TService>(object? serviceKey)
        where TService : notnull => Host.Services.GetRequiredKeyedService<TService>(serviceKey);

    //public TService? GetRequiredKeyedService<TService>(object? serviceKey, Action<IServiceCollection> configure)
    //    where TService : notnull => TestHost.CreateBuilder().ConfigureServices(configure).Build().Services.GetRequiredKeyedService<TService>(serviceKey);




    public TService? GetKeyedService<TService>(object? serviceKey)
        where TService : notnull => Host.Services.GetKeyedService<TService>(serviceKey);

    //public TService? GetKeyedService<TService>(object? serviceKey, Action<IServiceCollection> configure)
    //    where TService : notnull => TestHost.CreateBuilder().ConfigureServices(configure).Build().Services.GetKeyedService<TService>(serviceKey);




    public IEnumerable<TService> GetKeyedServices<TService>(object? serviceKey)
        where TService : notnull => Host.Services.GetKeyedServices<TService>(serviceKey);


    //public IEnumerable<TService> GetKeyedServices<TService>(object? serviceKey, Action<IServiceCollection> configure)
    //    where TService : notnull => TestHost.CreateBuilder().ConfigureServices(configure).Build().Services.GetKeyedServices<TService>(serviceKey);


    public TestHostFixture ConfigureServices(Action<IServiceCollection> configure)
    {
        _servicesDelegates.Add(configure);

        return this;
    }

    public TestHostFixture ConfigureHostServices(Action<HostBuilderContext, IServiceCollection> configure)
    {
        _hostServicesDelegates.Add(configure);

        return this;
    }

    public TestHostFixture ConfigureWebHostServices(Action<WebHostBuilderContext, IServiceCollection> configure)
    {
        _webhostServicesDelegates.Add(configure);

        return this;
    }

    public TestHostFixture EnablePlugin(Action<IHostBuilder> configure)
    {
        builderDelegates.Add(configure);

        return this;
    }

    private IHost Build()
    {
        if (_host == null)
        {
            if (_initializeShouldCalled)
            {
                Initialize();
            }

            _builder
                .ConfigureServices(
                    (context, services) =>
                    {
                        foreach (var configure in _servicesDelegates)
                        {
                            configure?.Invoke(services);
                        }

                        foreach (var configure in _hostServicesDelegates)
                        {
                            configure?.Invoke(context, services);
                        }
                    }
                )
                .ConfigureWebHost(webhostBuilder =>
                {
                    webhostBuilder.ConfigureServices(
                        (context, services) =>
                        {
                            foreach (var configure in _webhostServicesDelegates)
                            {
                                configure?.Invoke(context, services);
                            }
                        }
                    );
                });

            foreach (var configure in builderDelegates)
            {
                configure?.Invoke(_builder);
            }

            _host = _builder.Build();
        }

        HandleDemoMode(_demoModeShouldEnabled);

        return _host;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            RestoreDemoMode();

            // Dispose managed state (managed objects).
            LogManager.Flush();
            LogManager.Shutdown();

            _host?.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            _host?.Dispose();
        }

        // Free unmanaged resources.
        _disposed = true;
    }

    protected string GetEnvironmentVariable(string key)
    {
        var imageName = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(imageName))
        {
            throw new SdkException($"The environment variable '{key}' is not defined.");
        }

        return imageName;
    }

    protected virtual void Initialize()
    {

    }

    public TestHostFixture EnableDemoMode()
    {
        _demoModeShouldEnabled = true;

        return this;
    }

    public void DisableDemoMode()
    {
        _demoModeShouldEnabled = false;
    }

    private void RestoreDemoMode()
    {
        if (_currentDemoMode.HasValue)
        {
            HandleDemoMode(_currentDemoMode.Value);
        }

        _demoModeShouldEnabled = false;
    }

    private void HandleDemoMode(bool enable)
    {
        var setup = _host.Services.GetService<IVariableService>().GetSetup<EnvironmentSetup>();
        if (!_currentDemoMode.HasValue)
        {
            _currentDemoMode = setup.IsDemo;
        }
        setup.IsDemo = enable;
    }
}
