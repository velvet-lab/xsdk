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
using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

internal sealed class SlimHostInternal : SlimHostBase
{
    private static ISlimHost _host;

    internal static ISlimHost Instance => _host ?? throw new InvalidOperationException("SlimHost is not initialized");

    internal static ISlimHost Initialize(string[] args, string? appName, string? appCompany, string? appPrefix) =>
        InitializeSlimHost(appName, appCompany, appPrefix, false);

    internal static ISlimHost InitializeTestHost(string[] args, string? appName, string? appCompany, string? appPrefix) =>
        InitializeSlimHost(appName, appCompany, appPrefix, true);

    private static ISlimHost InitializeSlimHost(string? appName, string? appCompany, string? appPrefix, bool isTestHost)
    {
        if (_host == null)
        {
            HostLoggingManager.ResetLogger();

            var builder = SlimHostBuilder
                .CreateBuilder<SlimHostInternal>()
                .ValidateAppName(appName, EnvironmentSetup.Definitions.AppName.DefaultValue)
                .ValidateAppCompany(appCompany, EnvironmentSetup.Definitions.AppCompany.DefaultValue)
                .ValidateAppPrefix(appPrefix, EnvironmentSetup.Definitions.AppPrefix.DefaultValue);

            // Get soon as possible an instance of the host
            _host = builder.PreBuild();

            // Letz continue with the configuration
            IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddInMemoryCollection();

            if (!isTestHost)
            {
                HostConfigurationManager.LoadHostConfiguration(configBuilder);
                HostConfigurationManager.LoadAppConfiguration(configBuilder);
            }
            else
            {
                HostConfigurationManager.LoadTestConfiguration(configBuilder);
            }
            var config = configBuilder.Build();

            // Load all slim services
            builder.ConfigureServices(services =>
            {
                services
                    .AddLogging(HostLoggingManager.ConfigureSlimLogging)
                    .AddSlimPluginServices()
                    .AddSlimFileServices()
                    .AddSlimVariableServices(config, false);
            });

            // Now get the real instance of the host
            _host = builder.Build();

            // Validate the app version
            var envSetup = _host.VariableSystem.GetSetup<EnvironmentSetup>();
            builder.ValidateAppVersion(envSetup.AppVersion);

            // Set the environment setup
            envSetup.AppName = _host.AppName;
            envSetup.AppCompany = _host.AppCompany;
            envSetup.AppPrefix = _host.AppPrefix;
        }

        return _host;
    }
}
