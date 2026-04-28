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
using xSdk.Extensions.Options;

namespace xSdk.Hosting;

public static partial class WebHost
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, default, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, default, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        var hostBuilder = xSdk.Hosting.Host.CreateBuilder(args, appName, appCompany, appPrefix);
        var slimHost = hostBuilder.GetSlimHost();

        hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
        {
            _logger.LogDebug("Configuring WebHostBuilder");

            EnvironmentOptions environmentSetup = slimHost.GetEnvironment();
            Stage stage = environmentSetup.Stage;

            string contentRoot = GetContentRoot(environmentSetup);
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
                    services
                        .RegisterOptions<WebHostOptions>();

                    slimHost.ConfigureWebPluginHost(x => x.ConfigureServices(services));
                })
                // Configure Services with Context
                .ConfigureServices((context, services) =>
                {
                    slimHost.ConfigureWebPluginHost(x => x.ConfigureServices(context, services));
                })
                // Load Middlewares
                .Configure((context, app) => ConfigureApplicationWithContext(context, app, slimHost))
                // Configure Kestrel
                .UseKestrel(ConfigureKestrel);

            if (stage == Stage.Development)
                webHostBuilder.CaptureStartupErrors(true);
        });

        return hostBuilder;
    }

    private static string GetContentRoot(EnvironmentOptions environmentOptions)
    {
        _logger.LogDebug(environmentOptions.IsDemo ? "Demo Mode" : "Production Mode");
        _logger.LogDebug("Try to get Content Root");

        var root = environmentOptions.ContentRoot;
        if (environmentOptions.IsDemo)
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
