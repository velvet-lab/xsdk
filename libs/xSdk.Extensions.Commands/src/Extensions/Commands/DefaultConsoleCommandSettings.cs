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
using xSdk.Extensions.Options;

namespace xSdk.Extensions.Commands;

[ExcludeFromCodeCoverage(Justification = "Spectre.Console command settings – only used by CLI framework at runtime.")]
public class DefaultConsoleCommandSettings : CommandSettings
{
    [CommandOption(EnvironmentOptions.Definitions.LogLevel.Template)]
    [Description(EnvironmentOptions.Definitions.LogLevel.HelpText)]
    public string LogLevel { get; set; } = EnvironmentOptions.Definitions.LogLevel.DefaultValue;

    [CommandOption(EnvironmentOptions.Definitions.Stage.Template)]
    [Description(EnvironmentOptions.Definitions.Stage.HelpText)]
    [DefaultValue(EnvironmentOptions.Definitions.Stage.DefaultValue)]
    public Stage Stage { get; set; }

    [CommandOption(EnvironmentOptions.Definitions.Demo.Template)]
    [Description(EnvironmentOptions.Definitions.Demo.HelpText)]
    public bool IsDemo { get; set; }

    [CommandOption(EnvironmentOptions.Definitions.ContentRoot.Template)]
    [Description(EnvironmentOptions.Definitions.ContentRoot.HelpText)]
    public string? ContentRoot { get; set; }
}
