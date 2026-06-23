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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.IO;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Tools;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace xSdk.Extensions.Options;

public sealed partial class EnvironmentOptions
{
    [Variable(
        name: Definitions.Stage.Name,
        template: Definitions.Stage.Template,
        helpText: Definitions.Stage.HelpText,
        defaultValue: Definitions.Stage.DefaultValue,
        resourceNames: ["{{app.prefix}}.environment.stage", "deployment.environment"]
    )]
    public Stage Stage
    {
        get => ReadValue<Stage>(Definitions.Stage.Name);
        set => SetValue(Definitions.Stage.Name, value);
    }

    public string StageAsString => Stage switch
    {
        Stage.Development => Environments.Development,
        Stage.Integration => Environments.Staging,
        Stage.PreProduction => Environments.Staging,
        Stage.Production => Environments.Production,
        Stage.All => Environments.Production,
        Stage.None => Environments.Production,
        _ => Environments.Production,
    };

    [Variable(
        name: Definitions.Demo.Name,
        template: Definitions.Demo.Template,
        helpText: Definitions.Demo.HelpText,
        resourceNames: ["{{app.prefix}}.environment.demo"]
    )]
    public bool IsDemo
    {
        get => ReadValue<bool>(Definitions.Demo.Name);
        set => SetValue(Definitions.Demo.Name, value);
    }

    [Variable(
        name: Definitions.ContentRoot.Name,
        template: Definitions.ContentRoot.Template,
        helpText: Definitions.ContentRoot.HelpText
    )]
    public string? ContentRoot
    {
        get => ReadValue<string>(Definitions.ContentRoot.Name);
        set => SetValue(Definitions.ContentRoot.Name, value);
    }

    [Variable(
        name: Definitions.LogLevel.Name,
        template: Definitions.LogLevel.Template,
        helpText: Definitions.LogLevel.HelpText,
        defaultValue: Definitions.LogLevel.DefaultValue
    )]
    public string? LogLevelAsString
    {
        get => ReadValue<string>(Definitions.LogLevel.Name);
        set => SetValue(Definitions.LogLevel.Name, value);
    }

    public LogLevel LogLevel => LogLevelAsString?.ToLowerInvariant() switch
    {
        "off" => LogLevel.None,
        "none" => LogLevel.None,
        "fatal" => LogLevel.Critical,
        "critical" => LogLevel.Critical,
        "error" => LogLevel.Error,
        "warn" => LogLevel.Warning,
        "warning" => LogLevel.Warning,
        "info" => LogLevel.Information,
        "information" => LogLevel.Information,
        "debug" => LogLevel.Debug,
        "trace" => LogLevel.Trace,
        _ => LogLevel.Warning,
    };

    private static string DetermineContentRoot()
    {
        string? contentRoot = ReadCommandlineValue<string>( Definitions.ContentRoot.Name);
        if (string.IsNullOrEmpty(contentRoot))
        {
            contentRoot = Environment.CurrentDirectory;

            if (Debugger.IsAttached)
            {
                contentRoot = FileSystemHelper.SearchGitRoot(contentRoot);
            }
        }

        if (!string.IsNullOrEmpty(contentRoot))
        {
            contentRoot = Path.GetFullPath(contentRoot);
        }

        return contentRoot;
    }

    private static TValue? ReadCommandlineValue<TValue>(string pattern)
    {
        string? result = string.Empty;

        var parser = CommandlineParser.Parse();
        if (parser.ContainsPattern(pattern))
        {
            result = parser.ReadPattern(pattern);
        }

        if (!string.IsNullOrEmpty(result))
        {
            return TypeConverter.ConvertTo<TValue>(result);
        }

        return default;
    }

    public static partial class Definitions
    {
        public static class LogLevel
        {
            public const string Name = nameof(LogLevel);
            public const string Template = "--log-level <LEVEL>";
            public const string HelpText =
                "Set the log level for the application. Default primaryKey is 'Info'. Possible Values: Off, Trace, Debug, Info, Warn, Error or Fatal";
            public const string DefaultValue = "Warning";
        }

        public static class Stage
        {
            public const string Name = nameof(Stage);
            public const string Template = "--stage <STAGE>";
            public const string HelpText = "Stage where application is running. Default primaryKey is 'Development'.";
            public const xSdk.Stage DefaultValue = xSdk.Stage.Development;
        }

        public static class Demo
        {
            public const string Name = nameof(Demo);
            public const string Template = "--demo";
            public const string HelpText = "Enables the demo mode for the application. This will generate fake data for demostration";
        }

        public static class ContentRoot
        {
            public const string Name = nameof(ContentRoot);
            public const string Template = "--content-root <ROOT>";
            public const string HelpText = "Content root folder where application should working. If not given, content root will automatically determined";
        }
    }
}
