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

using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using xSdk.Extensions.IO;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

internal static class HostLoggingManager
{
    internal static void ConfigureSlimLogging(ILoggingBuilder builder) => ConfigureLogging(builder, true);

    internal static void ConfigureLogging(ILoggingBuilder builder) => ConfigureLogging(builder, false);

    private static void ConfigureLogging(ILoggingBuilder builder, bool isSlimMode)
    {
        ResetLogger(builder);

        EnvironmentSetup envSetup;
        if (isSlimMode)
        {
            envSetup = new EnvironmentSetup();
        }
        else
        {
            envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
        }

        var logSetup = LogManager.Setup();
        EnableNLogViewerTarget();
        EnableConsoleTarget(envSetup);
        if (!isSlimMode)
        {
            EnableFileTarget(logSetup, envSetup);
        }

        builder.SetMinimumLevel(ConvertLogLevel(envSetup));

        var configuration = LogManager.Configuration ?? new LoggingConfiguration();
        LogManager.Configuration = configuration;
        builder.AddNLog(configuration);

        LogManager.ReconfigExistingLoggers();
    }

    internal static void ResetLogger(ILoggingBuilder? builder = default)
    {
        // Reset Logger
        LogManager.Shutdown();
        LogManager.Configuration = null;
        NLog.Common.InternalLogger.Reset();
        NLog.Common.InternalLogger.LogToConsole = false;
        NLog.Common.InternalLogger.LogToConsoleError = false;
        NLog.Common.InternalLogger.LogLevel = NLog.LogLevel.Off;

        builder?.ClearProviders();

        LogManager.Configuration = new LoggingConfiguration();
    }

    private static (NLog.LogLevel minLogLevel, NLog.LogLevel maxLogLevel) GetLogLevels(EnvironmentSetup envSetup)
    {
        var minLogLevel = NLog.LogLevel.FromString(envSetup.LogLevel);
        var maxLogLevel = NLog.LogLevel.Fatal;
        return (minLogLevel, maxLogLevel);
    }

    private static Microsoft.Extensions.Logging.LogLevel ConvertLogLevel(EnvironmentSetup setup)
    {
        Microsoft.Extensions.Logging.LogLevel result = Microsoft.Extensions.Logging.LogLevel.Warning;
        var level = NLog.LogLevel.FromString(setup.LogLevel);

        if (NLog.LogLevel.Off == level)
            result = Microsoft.Extensions.Logging.LogLevel.None;

        if (NLog.LogLevel.Fatal == level)
            result = Microsoft.Extensions.Logging.LogLevel.Critical;

        if (NLog.LogLevel.Error == level)
            result = Microsoft.Extensions.Logging.LogLevel.Error;

        if (NLog.LogLevel.Warn == level)
            result = Microsoft.Extensions.Logging.LogLevel.Warning;

        if (NLog.LogLevel.Info == level)
            result = Microsoft.Extensions.Logging.LogLevel.Information;

        if (NLog.LogLevel.Debug == level)
            result = Microsoft.Extensions.Logging.LogLevel.Debug;

        if (NLog.LogLevel.Trace == level)
            result = Microsoft.Extensions.Logging.LogLevel.Trace;

        return result;
    }

    private static void EnableNLogViewerTarget()
    {
        // Enable only in Visual Studio
        if (Debugger.IsAttached)
        {
            var target = new NLog.Targets.Log4JXmlTarget("NLogViewerTarget")
            {
                Address = "udp://localhost:7071", // DevSkim: ignore DS162092
            };

            LogManager.Configuration?.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, target);
        }
    }

    private static void EnableConsoleTarget(EnvironmentSetup envSetup)
    {
        // Enable Console in Visual Studio or in Containers
        if (Debugger.IsAttached || envSetup.IsDotNetRunningInContainer)
        {
            var (minLogLevel, maxLogLevel) = GetLogLevels(envSetup);

            if (Debugger.IsAttached)
            {
                NLog.Common.InternalLogger.LogToConsole = true;
                NLog.Common.InternalLogger.LogLevel = minLogLevel;
            }
            var target = new NLog.Targets.ConsoleTarget("ConsoleTarget");
            target.Layout = "${date:format=dd.MM.yyyy HH\\:mm\\:ss} ${level:uppercase=true} ${message:withexception=true}";

            LogManager.Configuration?.AddRule(minLogLevel, maxLogLevel, target);
        }
    }

    private static void EnableFileTarget(ISetupBuilder logSetup, EnvironmentSetup envSetup)
    {
        // Disable File Log for Visual Studio and Containers
        var configFile = LoadDefaultLogConfig(!Debugger.IsAttached && !envSetup.IsDotNetRunningInContainer);
        if (File.Exists(configFile))
        {
            logSetup.LoadConfigurationFromFile(configFile);
        }
    }

    private static string LoadDefaultLogConfig(bool shouldExists)
    {
        var configFolder = FileSystemHelper.CreateSpecificDataFolder(FileSystemContext.Machine, "/config");
        var logFolder = FileSystemHelper.CreateSpecificDataFolder(FileSystemContext.Machine, "/logs");
        var configFile = Path.Combine(configFolder, "nlog.config");

        if (shouldExists && !File.Exists(configFile))
        {
            var config =
                $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<nlog xmlns=""http://www.nlog-project.org/schemas/NLog.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
        autoReload=""true""
        throwConfigExceptions=""true"">

    <variable name=""logDirectory"" value=""{logFolder}""/>
    <include file=""common.config"" />
</nlog>
";
            File.WriteAllText(configFile, config, Encoding.UTF8);

            configFile = Path.Combine(configFolder, "common.config");
            config =
                @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<nlog xmlns=""http://www.nlog-project.org/schemas/NLog.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">

    <targets>
        <target xsi:type=""AsyncWrapper"" name=""fileLog"" overflowAction=""Block"">
            <target type=""File""
                    fileName=""${logDirectory}/${shortdate}.log""
                    archiveFileName=""${logDirectory}/archive/{#}.zip""
                    archiveDateFormat=""yyyy-MM-dd""
                    archiveNumbering=""Date""
                    enableArchiveFileCompression=""True""
                    maxArchiveFiles=""7""
                    archiveEvery=""Day""/>
        </target>
    </targets>

    <rules>
        <logger name=""*"" minlevel=""Warn"" writeTo=""fileLog"" />
    </rules>
</nlog>
";
            File.WriteAllText(configFile, config, Encoding.UTF8);
        }

        return configFile;
    }
}
