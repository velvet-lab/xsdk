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
using xSdk.Extensions.IO;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using xSdk.Hosting.Managers;

namespace xSdk.Hosting;

public static class TestHostFactory
{
    public static IHostBuilder CreateTestHost(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        ApplicationOptions appOptions = new()
        {
            Name = appName ?? ApplicationOptions.Definitions.AppName.DefaultValue,
            Company = appCompany ?? ApplicationOptions.Definitions.AppCompany.DefaultValue,
            Prefix = appPrefix ?? ApplicationOptions.Definitions.AppPrefix.DefaultValue
        };

        var slimHost = SlimHost.InitializeSlimHost(args, appOptions);

        // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden
#pragma warning disable EXTEXP0016
        var builder = Microsoft.Extensions.Hosting.Testing.FakeHost.CreateBuilder(config =>
        {
            config.FakeLogging = true;
            config.FakeRedaction = true;
        });
#pragma warning restore EXTEXP0016

        builder
            .SetSlimHost(slimHost)
            .ConfigureHostConfiguration(ConfigurationManager.LoadTestConfiguration)
            .ConfigureServices(services =>
            {
                slimHost.PostConfigure(services);

                services
                    .RegisterApplicationOptions(appOptions)
                    .RegisterOptions<EnvironmentOptions>()
                    .AddLogging()
                    .AddVariableServices()
                    .AddFileServices()
                    .AddPluginServices();

                // Add initializer for Logger Factory
                services
                    .AddHostedService<HostInitializer>();
            })
            .ConfigureServices((context, services) =>
            {
                slimHost.ConfigurePluginHost(x => x.ConfigureServices(context, services));
            });

        return builder;
    }
}
