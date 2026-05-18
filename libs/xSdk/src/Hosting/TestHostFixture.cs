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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;

namespace xSdk.Hosting;

public class TestHostFixture : IDisposable
{
    private IHost? _host;

    private readonly List<Action<IServiceCollection>> _servicesDelegates = [];
    private readonly List<Action<HostBuilderContext, IServiceCollection>> _servicesWithContextDelegates = [];

    internal List<Action<IHostBuilder>> _builderDelegates = [];

    internal List<Action<string>> _loggingOutputHandlers = [];

    private bool _disposed;

    private bool? _currentDemoMode;
    private bool _demoModeShouldEnabled;

    // Serializes BuildHost calls so that parallel test runners cannot cause
    // concurrent host initialization within the same fixture instance.
    private readonly Lock _buildLock = new();

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

    public TestHostFixture ConfigureBuilder(Action<IHostBuilder> configure)
    {
        _builderDelegates.Add(configure);
        return this;
    }

    public TestHostFixture ConfigureServices(Action<IServiceCollection> configure)
    {
        _servicesDelegates.Add(configure);
        return this;
    }

    public TestHostFixture ConfigureServices(Action<HostBuilderContext, IServiceCollection> configure)
    {
        _servicesWithContextDelegates.Add(configure);
        return this;
    }

    public TestHostFixture RegisterLoggingOutput(Action<string> handler)
    {
        _loggingOutputHandlers.Add(handler);
        return this;
    }

    public IHost BuildHost()
    {
        lock (_buildLock)
        {
            return BuildHostCore();
        }
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

            // Reset LogManager to prevent disposed ILoggerFactory access
            LogManager.Reset();
        }

        // Free unmanaged resources.
        _disposed = true;
    }

    public EnvironmentOptions Environment => (_host ?? BuildHost()).Services.GetRequiredService<IOptions<EnvironmentOptions>>().Value;

    protected static string GetEnvironmentVariable(string key)
    {
        string? imageName = System.Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(imageName))
        {
            throw new SdkException($"The environment variable '{key}' is not defined.");
        }

        return imageName;
    }

    protected virtual void Initialize() { }

    protected virtual IHostBuilder CreateHostBuilder()
        => TestHost.CreateBuilder();

    private IHost BuildHostCore()
    {
        // If the host was already built, return it directly (singleton semantics).
        // The caller holds _buildLock, so no other thread can race here.
        if (_host is not null)
        {
            return _host;
        }

        // Reset LogManager to ensure a clean state before building a new host
        LogManager.Reset();

        IHostBuilder builder = CreateHostBuilder()
            .ConfigureServices((context, services) =>
            {
                foreach (Action<IServiceCollection> configure in _servicesDelegates)
                {
                    configure?.Invoke(services);
                }

                foreach (Action<HostBuilderContext, IServiceCollection> configure in _servicesWithContextDelegates)
                {
                    configure?.Invoke(context, services);
                }
            });

#pragma warning disable EXTEXP0016 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
        builder
            .AddFakeLoggingOutputSink(message =>
            {
                foreach (Action<string> handler in _loggingOutputHandlers)
                {
                    handler?.Invoke(message);
                }
            });
#pragma warning restore EXTEXP0016 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.

        Initialize();

        // Configure the host builder with any additional delegates
        foreach (Action<IHostBuilder> configure in _builderDelegates)
        {
            configure?.Invoke(builder);
        }

        _host = builder.Build();

        HandleDemoMode(_demoModeShouldEnabled);

        // Start the HostInitializer hosted service if it exists, because in Unit tests the host
        // is not automatically started as in a real application.
        IEnumerable<IHostedService> hostedServices = _host.Services.GetServices<IHostedService>();
        if (hostedServices != null)
        {
            foreach (IHostedService hostedService in hostedServices)
            {
                if (hostedService is HostInitializer)
                {
                    hostedService.StartAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    break;
                }
            }
        }

        return _host;
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
        if (!_currentDemoMode.HasValue)
        {
            _currentDemoMode = Environment?.IsDemo;
        }

        Environment?.IsDemo = enable;
    }
}
