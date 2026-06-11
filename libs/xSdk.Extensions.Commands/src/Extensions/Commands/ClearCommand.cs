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
using Spectre.Console;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

[Description(Definitions.HelpText)]
[ExcludeFromCodeCoverage]
internal class ClearCommand : Command<EmptyCommandSettings>
{
    internal static class Definitions
    {
        public const string Name = "clear";
        public const string HelpText = "Clears the last console output";
    }

    protected override int Execute(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        System.Console.Clear();
        AnsiConsole.Clear();
        return 0;
    }
}
