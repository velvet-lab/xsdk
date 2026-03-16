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
using Spectre.Console.Cli;

namespace xSdk.Extensions.Variable.Commands;

internal class ListCommandSettings : CommandSettings
{
    [CommandOption("-f|--format <FORMAT>")]
    [Description("Formats the Output (default Name, Template, Protected, Prefix, Defined, Value)")]
    [DefaultValue("Name, Template, Protected, Prefix, Defined, Value")]
    public string FormatString { get; set; }

    [CommandOption("--show-help")]
    [Description("Should Help foreach Variable displayed?")]
    public bool ShowHelp { get; set; }
}
