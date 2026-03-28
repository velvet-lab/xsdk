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

using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.IO;

namespace xSdk.Hosting;

public static class HostConfigurationManager
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    internal static void LoadHostConfiguration(IConfigurationBuilder builder)
    {
        _logger.LogInformation("Try to load Machine Configuration");

        _logger.LogTrace("Clear all Configuration Providers and load our own Providers");
        builder.Sources.Clear();

        builder.AddEnvironmentVariables(prefix: "DOTNET_");
        builder.AddEnvironmentVariables(prefix: "ASPNET_");
        builder.AddEnvironmentVariables(prefix: $"{SlimHostInternal.Instance.AppPrefix.ToUpperInvariant()}_");
    }

    internal static void LoadAppConfiguration(IConfigurationBuilder builder)
    {
        LoadAppConfiguration(null, builder);
    }

    internal static void LoadAppConfiguration(HostBuilderContext? context, IConfigurationBuilder builder)
    {
        _logger.LogInformation("Try to load Application Configuration");

        var fileSystemService = new FileSystemService();
        var root = fileSystemService.RequestFileSystemAsync(FileSystemContext.Machine).GetAwaiter().GetResult();

        var configFolder = FileSystemHelper.CreateSpecificDataFolder(root, "/config");

        var configFile = GetConfigFile(configFolder);
        LoadConfigurationFile(builder, configFile, false);

        if (context != null)
        {
            configFile = GetConfigFile(configFolder, context.HostingEnvironment.EnvironmentName);
            LoadConfigurationFile(builder, configFile, true);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _logger.LogTrace("Try load Config from Container");
            if (Directory.Exists("/var/run/configs"))
            {
                builder.AddKeyPerFile("/var/run/configs", true);
            }
        }
    }


    private static void LoadConfigurationFile(IConfigurationBuilder builder, string? file, bool reloadOnChange = false)
    {
        if (!string.IsNullOrEmpty(file) && File.Exists(file))
        {
            _logger.LogInformation("Try to load Configuration from File '{0}'", file);
            _logger.LogTrace("Configuration File exists. Load it!");
            builder.AddJsonFile(file, true, reloadOnChange);
        }
        else
        {
            _logger.LogInformation("Configuration File not exists. Nothing to do.");
        }
    }

    private static string? GetConfigFile(string configFolder, string? envName = null)
    {
        var logPostFix = "";

        var configFileName = $"appsettings.json".ToLower();
        if (!string.IsNullOrEmpty(envName))
        {
            configFileName = $"appsettings.{envName}.json".ToLower();
            logPostFix = $" for Environment '{envName}'";
        }

        _logger.LogInformation("Try to determine configuration file in folder '{0}'{1}", configFolder, logPostFix);
        var configFile = Path.Combine(configFolder, configFileName);

        if (!File.Exists(configFile))
        {
            _logger.LogTrace("Configuration file could not found!");
            configFolder = FileSystemHelper.GetExecutingFolder();

            _logger.LogInformation("Last try! Try to load configuration file from Visual Studio project folder '{0}'{1}", configFolder, logPostFix);
            configFile = Path.Combine(configFolder, configFileName);
        }

        if (!File.Exists(configFile))
        {
            _logger.LogTrace("Give up! Configuration file could not found.");
            return null;
        }
        else
        {
            _logger.LogTrace("Success! Configuration file found.");
            return configFile;
        }
    }

    internal static void LoadTestConfiguration(IConfigurationBuilder configBuilder)
    {
        configBuilder.SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.tests.json", true, true);
    }
}
