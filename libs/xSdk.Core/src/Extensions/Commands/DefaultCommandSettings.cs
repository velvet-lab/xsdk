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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

[ExcludeFromCodeCoverage(Justification = "Spectre.Console command settings – only used by CLI framework at runtime.")]
public class DefaultCommandSettings : CommandSettings
{
    [CommandOption(Definitions.LogLevel.Template)]
    [Description(Definitions.LogLevel.HelpText)]
    public string LogLevel { get; set; }

    [CommandOption(Definitions.Stage.Template)]
    [Description(Definitions.Stage.HelpText)]
    [DefaultValue(Definitions.Stage.DefaultValue)]
    public Stage Stage { get; set; }

    [CommandOption(Definitions.Demo.Template)]
    [Description(Definitions.Demo.HelpText)]
    public bool IsDemo { get; set; }

    [CommandOption(Definitions.ContentRoot.Template)]
    [Description(Definitions.ContentRoot.HelpText)]
    public string ContentRoot { get; set; }

    public static class Definitions
    {
        public static class LogLevel
        {
            public const string Name = "log-level";
            public const string Template = "--log-level <LEVEL>";
            public const string HelpText =
                "Set the log level for the application. Default primaryKey is 'Info'. Possible Values: Off, Trace, Debug, Info, Warn, Error or Fatal";
            public const string DefaultValue = "Info";
        }

        public static class Stage
        {
            public const string Name = "stage";
            public const string Template = "--stage <STAGE>";
            public const string HelpText = "Stage where application is running. Default primaryKey is 'Development'.";
            public const xSdk.Stage DefaultValue = xSdk.Stage.Development;
        }

        public static class Demo
        {
            public const string Name = "demo";
            public const string Template = "--demo";
            public const string HelpText = "Enables the demo mode for the application. This will generate fake data for demostration";
        }

        public static class ContentRoot
        {
            public const string Name = "content-root";
            public const string Template = "--content-root <ROOT>";
            public const string HelpText = "Content root folder where application should working. If not given, content root will automatically determined";
        }
    }
}
