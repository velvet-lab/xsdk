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
using xSdk.Extensions.Logging;
using xSdk.Extensions.Options;

namespace xSdk.Hosting.Managers;

public static class ConfigurationManager
{
    private static ILogger? _logger;
    private static ILogger Logger => _logger ??= LogManager.CreateLogger(typeof(ConfigurationManager));

    internal static void LoadHostConfiguration(IConfigurationBuilder builder, ApplicationOptions options)
    {
        Logger.LogInformation("Try to load Machine Configuration");

        Logger.LogTrace("Clear all Configuration Providers and load our own Providers");
        builder.Sources.Clear();

        builder.AddEnvironmentVariables(prefix: "DOTNET_");
        builder.AddEnvironmentVariables(prefix: "ASPNET_");
        if (!string.IsNullOrEmpty(options.Prefix))
        {
            builder.AddEnvironmentVariables(prefix: $"{options.Prefix.ToUpperInvariant()}_");
        }
    }

    internal static void LoadAppConfiguration(IConfigurationBuilder builder, ApplicationOptions options) => LoadAppConfiguration(null, builder, options);

    internal static void LoadAppConfiguration(HostBuilderContext? context, IConfigurationBuilder builder, ApplicationOptions options)
    {
        Logger.LogInformation("Try to load Application Configuration");

        FileSystemOptions fileSystemOptions = new()
        {
            Company = options.Company,
            ApplicationName = options.Name,
        };

        FileSystemService fileSystemService = new(fileSystemOptions);
        IFileSystemResult root = fileSystemService.RequestFileSystemAsync(FileSystemContext.Machine).GetAwaiter().GetResult();

        string configFolder = FileSystemHelper.CreateSpecificDataFolder(root, "/config");

        string? configFile = GetConfigFile(configFolder, default);
        LoadConfigurationFile(builder, configFile, false);

        if (context != null)
        {
            configFile = GetConfigFile(configFolder, context.HostingEnvironment.EnvironmentName);
            LoadConfigurationFile(builder, configFile, true);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Logger.LogTrace("Try load Config from Container");
            if (Directory.Exists("/var/run/configs"))
            {
                builder.AddKeyPerFile("/var/run/configs", true);
            }
        }
    }

    private static void LoadConfigurationFile(IConfigurationBuilder builder, string? file, bool reloadOnChange)
    {
        if (!string.IsNullOrEmpty(file) && File.Exists(file))
        {
#pragma warning disable CA1873 // Potenziell kostspielige Protokollierung vermeiden
            Logger.LogInformation("Try to load Configuration from File '{file}'", file);
#pragma warning restore CA1873 // Potenziell kostspielige Protokollierung vermeiden

            Logger.LogTrace("Configuration File exists. Load it!");
            builder.AddJsonFile(file, true, reloadOnChange);
        }
        else
        {
            Logger.LogInformation("Configuration File not exists. Nothing to do.");
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1873:Potenziell kostspielige Protokollierung vermeiden", Justification = "<Ausstehend>")]
    private static string? GetConfigFile(string configFolder, string? envName)
    {
        string logPostFix = "";

        string configFileName = $"appsettings.json".ToLower();
        if (!string.IsNullOrEmpty(envName))
        {
            configFileName = $"appsettings.{envName}.json".ToLower();
            logPostFix = $" for Environment '{envName}'";
        }

        Logger.LogInformation("Try to determine configuration file in folder '{file}'{postfix}", configFolder, logPostFix);
        string configFile = Path.Combine(configFolder, configFileName);

        if (!File.Exists(configFile))
        {
            Logger.LogTrace("Configuration file could not found!");
            configFolder = FileSystemHelper.GetExecutingFolder();

            Logger.LogInformation("Last try! Try to load configuration file from Visual Studio project folder '{folder}'{postfix}", configFolder, logPostFix);
            configFile = Path.Combine(configFolder, configFileName);
        }

        if (!File.Exists(configFile))
        {
            Logger.LogTrace("Give up! Configuration file could not found.");
            return null;
        }
        else
        {
            Logger.LogTrace("Success! Configuration file found.");
            return configFile;
        }
    }

    internal static void LoadTestConfiguration(IConfigurationBuilder configBuilder)
        => configBuilder.SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.tests.json", true, true);
}
