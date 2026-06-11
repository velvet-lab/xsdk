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
        name: Commands.DefaultCommandSettings.Definitions.Stage.Name,
        template: Commands.DefaultCommandSettings.Definitions.Stage.Template,
        helpText: Commands.DefaultCommandSettings.Definitions.Stage.HelpText,
        defaultValue: Commands.DefaultCommandSettings.Definitions.Stage.DefaultValue,
        resourceNames: ["{{app.prefix}}.environment.stage", "deployment.environment"]
    )]
    public Stage Stage
    {
        get => ReadValue<Stage>(Commands.DefaultCommandSettings.Definitions.Stage.Name);
        set => SetValue(Commands.DefaultCommandSettings.Definitions.Stage.Name, value);
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
        name: Commands.DefaultCommandSettings.Definitions.Demo.Name,
        template: Commands.DefaultCommandSettings.Definitions.Demo.Template,
        helpText: Commands.DefaultCommandSettings.Definitions.Demo.HelpText,
        resourceNames: ["{{app.prefix}}.environment.demo"]
    )]
    public bool IsDemo
    {
        get => ReadValue<bool>(Commands.DefaultCommandSettings.Definitions.Demo.Name);
        set => SetValue(Commands.DefaultCommandSettings.Definitions.Demo.Name, value);
    }

    [Variable(
        name: Commands.DefaultCommandSettings.Definitions.ContentRoot.Name,
        template: Commands.DefaultCommandSettings.Definitions.ContentRoot.Template,
        helpText: Commands.DefaultCommandSettings.Definitions.ContentRoot.HelpText
    )]
    public string? ContentRoot
    {
        get => ReadValue<string>(Commands.DefaultCommandSettings.Definitions.ContentRoot.Name);
        set => SetValue(Commands.DefaultCommandSettings.Definitions.ContentRoot.Name, value);
    }

    [Variable(
        name: Commands.DefaultCommandSettings.Definitions.LogLevel.Name,
        template: Commands.DefaultCommandSettings.Definitions.LogLevel.Template,
        helpText: Commands.DefaultCommandSettings.Definitions.LogLevel.HelpText,
        defaultValue: Commands.DefaultCommandSettings.Definitions.LogLevel.DefaultValue
    )]
    public string? LogLevelAsString
    {
        get => ReadValue<string>(Commands.DefaultCommandSettings.Definitions.LogLevel.Name);
        set => SetValue(Commands.DefaultCommandSettings.Definitions.LogLevel.Name, value);
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
        string? contentRoot = ReadCommandlineValue<string>(Commands.DefaultCommandSettings.Definitions.ContentRoot.Name);
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

        var parser = Commands.CommandlineParser.Parse();
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
}
