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
using xSdk.Extensions.Commands;
using xSdk.Extensions.IO;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Tools;

namespace xSdk.Extensions.Options;

public sealed partial class EnvironmentOptions
{
    [Variable(
        name: DefaultCommandSettings.Definitions.Stage.Name,
        template: DefaultCommandSettings.Definitions.Stage.Template,
        helpText: DefaultCommandSettings.Definitions.Stage.HelpText,
        defaultValue: DefaultCommandSettings.Definitions.Stage.DefaultValue,
        resourceNames: ["{{app.prefix}}.environment.stage", "deployment.environment"]
    )]
    public Stage Stage
    {
        get => ReadValue<Stage>(DefaultCommandSettings.Definitions.Stage.Name);
        set => SetValue(DefaultCommandSettings.Definitions.Stage.Name, value);
    }

    [Variable(
        name: DefaultCommandSettings.Definitions.Demo.Name,
        template: DefaultCommandSettings.Definitions.Demo.Template,
        helpText: DefaultCommandSettings.Definitions.Demo.HelpText,
        resourceNames: ["{{app.prefix}}.environment.demo"]
    )]
    public bool IsDemo
    {
        get => ReadValue<bool>(DefaultCommandSettings.Definitions.Demo.Name);
        set => SetValue(DefaultCommandSettings.Definitions.Demo.Name, value);
    }

    [Variable(
        name: DefaultCommandSettings.Definitions.ContentRoot.Name,
        template: DefaultCommandSettings.Definitions.ContentRoot.Template,
        helpText: DefaultCommandSettings.Definitions.ContentRoot.HelpText
    )]
    public string? ContentRoot
    {
        get => ReadValue<string>(DefaultCommandSettings.Definitions.ContentRoot.Name);
        set => SetValue(DefaultCommandSettings.Definitions.ContentRoot.Name, value);
    }

    [Variable(
        name: DefaultCommandSettings.Definitions.LogLevel.Name,
        template: DefaultCommandSettings.Definitions.LogLevel.Template,
        helpText: DefaultCommandSettings.Definitions.LogLevel.HelpText,
        defaultValue: DefaultCommandSettings.Definitions.LogLevel.DefaultValue
    )]
    public string? LogLevel
    {
        get => ReadValue<string>(DefaultCommandSettings.Definitions.LogLevel.Name);
        set => SetValue(DefaultCommandSettings.Definitions.LogLevel.Name, value);
    }

    private static string DetermineContentRoot()
    {
        string? contentRoot = ReadCommandlineValue<string>(DefaultCommandSettings.Definitions.ContentRoot.Name);
        if (string.IsNullOrEmpty(contentRoot))
        {
            contentRoot = Environment.CurrentDirectory;

            if (Debugger.IsAttached)
            {
                contentRoot = FileSystemHelper.SearchGitRoot(contentRoot);
            }
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
}
