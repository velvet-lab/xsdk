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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static partial class WebHost
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, default, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, default, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        var builder = xSdk
            .Hosting.Host.CreateBuilder(args, appName, appCompany, appPrefix)
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                _logger.LogDebug("Configuring WebHostBuilder");

                var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
                var stage = envSetup.Stage;

                var contentRoot = GetContentRoot(envSetup);
                webHostBuilder
                    // Set the Content Root
                    .UseContentRoot(contentRoot)
                    .UseWebRoot(contentRoot)

                    // Set the Environment
                    .UseEnvironment(stage.ToString())
                    // Enabled detailed Errors if in Development Mode
                    .UseSetting(WebHostDefaults.DetailedErrorsKey, (stage == Stage.Development).ToString())
                    // Configure Services
                    .ConfigureServices((services) =>
                    {
                        SlimHost.Instance.PluginSystem.ConfigureHost<WebPluginHost>(x => x.ConfigureServices(services));
                    })

                    // Configure Services
                    .ConfigureServices((context, services) =>
                    {
                        SlimHost.Instance.PluginSystem.ConfigureHost<WebPluginHost>(x => x.ConfigureServices(context, services));
                    })
                    // Load Middlewares
                    .Configure(ConfigureApplicationWithContext)
                    // Configure Kestrel
                    .UseKestrel(ConfigureKestrel);

                if (stage == Stage.Development)
                    webHostBuilder.CaptureStartupErrors(true);
            });

        SlimHost.Instance.VariableSystem.RegisterSetup<WebHostSetup>();
        return builder;
    }

    private static string GetContentRoot(EnvironmentSetup envSetup)
    {
        _logger.LogDebug(envSetup.IsDemo ? "Demo Mode" : "Production Mode");
        _logger.LogDebug("Try to get Content Root");

        var root = envSetup.ContentRoot;
        if (envSetup.IsDemo)
        {
            return FileSystemHelper.GetExecutingFolder();
        }

        if (!Directory.Exists(root))
        {
            try
            {
                _logger.LogTrace("Content root does not exist, creating it");
                Directory.CreateDirectory(root);
            }
            catch
            {
                // Only catch, nothing to tell
            }
        }
        return Path.GetFullPath(root);
    }
}
