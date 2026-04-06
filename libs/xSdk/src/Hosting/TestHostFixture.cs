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
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public class TestHostFixture : IDisposable
{
    private IHost? _host;

    private readonly List<Action<IServiceCollection>> _servicesDelegates = new();
    private readonly List<Action<HostBuilderContext, IServiceCollection>> _hostServicesDelegates = new();
    private readonly List<Action<WebHostBuilderContext, IServiceCollection>> _webhostServicesDelegates = new();

    internal List<Action<IHostBuilder>> builderDelegates = new();

    private bool _disposed;

    private bool? _currentDemoMode;
    private bool _demoModeShouldEnabled;

    public TestHostFixture()
    {        
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

    public IHost BuildHost()
        => BuildHost(false);

    public IHost BuildHost(bool reinitialize)
    {
        if (reinitialize)
        {
            Initialize();
        }

        var builder = TestHost.CreateBuilder()
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
            );

            //.ConfigureWebHost(webhostBuilder =>
            //{
            //    webhostBuilder.ConfigureServices(
            //        (context, services) =>
            //        {
            //            foreach (var configure in _webhostServicesDelegates)
            //            {
            //                configure?.Invoke(context, services);
            //            }
            //        }
            //    );
            //});

        foreach (var configure in builderDelegates)
        {
            configure?.Invoke(builder);
        }

        // Stop and dispose the previous host if it exists
        _host?.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        _host?.Dispose();

        _host = builder.Build();        

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

    public TestHostFixture DisableDemoMode()
    {
        _demoModeShouldEnabled = false;

        return this;
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
        //var setup = _host.Services.GetService<IVariableService>().GetSetup<EnvironmentSetup>();
        //if (!_currentDemoMode.HasValue)
        //{
        //    _currentDemoMode = setup.IsDemo;
        //}
        //setup.IsDemo = enable;
    }
}
