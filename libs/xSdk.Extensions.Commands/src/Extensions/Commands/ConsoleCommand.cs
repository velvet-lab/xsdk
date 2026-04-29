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

namespace xSdk.Extensions.Commands;

[Description(Definitions.HelpText)]
internal class ConsoleCommand : Command<EmptyCommandSettings>
{
    internal static class Definitions
    {
        public const string Name = "console";
        public const string HelpText = "Creates a interactive REPL Console";
    }

    protected override int Execute(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        return 0;
    }
}
