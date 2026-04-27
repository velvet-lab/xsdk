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

public static partial class Host
{
    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, default, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, default, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        ApplicationOptions appOptions = new()
        {
            Name = appName ?? ApplicationOptions.Definitions.AppName.DefaultValue,
            Company = appCompany ?? ApplicationOptions.Definitions.AppCompany.DefaultValue,
            Prefix = appPrefix ?? ApplicationOptions.Definitions.AppPrefix.DefaultValue
        };

        SlimHost.Instance.InitializeSlimHost(args, appOptions);

        return new HostBuilder()
            .ConfigureHostConfiguration(configBuilder =>
            {
                HostConfigurationManager.LoadHostConfiguration(configBuilder, appOptions);
            })
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                HostConfigurationManager.LoadAppConfiguration(context, configBuilder, appOptions);
            })
            .ConfigureServices(services =>
            {
                SlimHost.Instance.PostConfigure(services);

                services
                    .RegisterApplicationOptions(appOptions)                    
                    .RegisterOptions<EnvironmentOptions>(options =>
                    {
                        services
                            .AddLogging(logBuilder => HostLoggingManager.ConfigureLogging(logBuilder, options));
                    })
                    .AddVariableServices()
                    .AddFileServices()
                    .AddPluginServices();

                // Add initializer for the host
                services
                    .AddHostedService<HostInitializer>();

                SlimHost.Instance.ConfigurePluginHost(x => x.ConfigureServices(services));
                
            })
            .ConfigureServices((context, services) =>
            {
                SlimHost.Instance.ConfigurePluginHost(x => x.ConfigureServices(context, services));
            });
    }
}
