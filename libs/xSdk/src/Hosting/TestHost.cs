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

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.IO;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using xSdk.Hosting.Managers;

namespace xSdk.Hosting;

public static partial class TestHost
{
    private const string APP_NAME = "xUnitTestHost";
    private const string APP_COMPANY = "xUnit";
    private const string APP_PREFIX = "UnitTest";

    public static IHostBuilder CreateBuilder() => CreateBuilder(new string[] { }, APP_NAME, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, APP_COMPANY, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        ApplicationOptions appOptions = new()
        {
            Name = appName ?? ApplicationOptions.Definitions.AppName.DefaultValue,
            Company = appCompany ?? ApplicationOptions.Definitions.AppCompany.DefaultValue,
            Prefix = appPrefix ?? ApplicationOptions.Definitions.AppPrefix.DefaultValue
        };

        // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden
#pragma warning disable EXTEXP0016 
        var builder = Microsoft.Extensions.Hosting.Testing.FakeHost.CreateBuilder();        
#pragma warning restore EXTEXP0016

        builder
            .ConfigureHostConfiguration(HostConfigurationManager.LoadTestConfiguration)
            .ConfigureServices(services =>
            {
                services
                    .AddOptions<ApplicationOptions>()
                    .Configure(options =>
                    {
                        options.Name = appName ?? ApplicationOptions.Definitions.AppName.DefaultValue;
                        options.Company = appCompany ?? ApplicationOptions.Definitions.AppCompany.DefaultValue;
                        options.Prefix = appPrefix ?? ApplicationOptions.Definitions.AppPrefix.DefaultValue;

                        var validator = new ApplicationOptionsValidator();
                        validator.ValidateAndThrow(options);
                    });

                services
                    .RegisterOptions<EnvironmentOptions>(options =>
                    {
                        services
                            .AddLogging(logBuilder => HostLoggingManager.ConfigureLogging(logBuilder, options));
                    })
                    .AddVariableServices()
                    .AddFileServices()
                    .AddPluginServices();

                // Add initializer for Logger Factory
                services
                    .AddHostedService<HostInitializer>();

                HostPluginManager.Instance.ConfigureHost<PluginHost>(x => x.ConfigureServices(services));
            })
            .ConfigureServices((context, services) =>
            {
                HostPluginManager.Instance.ConfigureHost<PluginHost>(x => x.ConfigureServices(context, services));
            });
            //.ConfigureWebHost(webhostBuilder =>
            //{
            //    webhostBuilder
            //        .ConfigureServices(services =>
            //        {
            //            SlimHostInternal.Instance.PluginSystem
            //                .ConfigureHost<WebPluginHost>(x => x.ConfigureServices(services));
            //        })

            //        .ConfigureServices((context, services) =>
            //        {
            //            SlimHostInternal.Instance.PluginSystem
            //                .ConfigureHost<WebPluginHost>(x => x.ConfigureServices(context, services));
            //        });
            //});

        return builder;
    }
}
