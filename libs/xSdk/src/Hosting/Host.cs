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
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static partial class Host
{
    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, default, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, default, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        SlimHostInternal.Initialize(args, appName, appCompany, appPrefix);

        var builder = new HostBuilder()
            .ConfigureHostConfiguration(HostConfigurationManager.LoadHostConfiguration)
            .ConfigureAppConfiguration(HostConfigurationManager.LoadAppConfiguration)
            .ConfigureServices(services =>
            {
                services
                    .AddLogging(HostLoggingManager.ConfigureLogging)
                    .AddFileServices()
                    .AddPluginServices()
                    .AddVariableServices();

                // Add initializer for Logger Factory.
                services
                    .AddHostedService<LoggerFactoryInitializer>();

                SlimHostInternal.Instance.PluginSystem
                    .Invoke<PluginBase>(x => x.ConfigureServices(services));
            })
            .ConfigureServices((context, services) =>
            {
                SlimHostInternal.Instance.PluginSystem
                    .Invoke<HostPluginBase>(x => x.ConfigureServices(context, services));
            });

        return builder;
    }
}
