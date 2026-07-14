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

using System.CommandLine;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Spectre.Console;
using xSdk.Extensions.Commands.Attributes;

namespace xSdk.Extensions.Commands;

[ExcludeFromCodeCoverage(Justification = "Requires injected RootCommand and ConsoleOptions — tested via integration tests.")]
public sealed class HelpCommand(RootCommand rootCommand, ReplConsoleBuilder builder, IOptions<ConsoleOptions> options) : CommandHandler
{
    public static class Definitions
    {
        public const string Name = "help";
        public const string HelpText = "Display help information";
    }

    [
        CommandArgument("commands"),
        Description("Show help for the specified commands")
    ]
    public string[] Commands { get; set; }

    public override int Execute()
    {
        var commandsToShow = new List<Command>();
        foreach (string command in Commands)
        {
            IEnumerable<Command> foundCommands = SearchCommand(rootCommand, command);
            if (foundCommands.Any())
            {
                commandsToShow.AddRange(foundCommands);
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Command '{command}' not found.[/]");
            }
        }

        ConsoleOptions setup = options.Value;
        if (setup.DisableDefaultHelp)
        {
            builder.CreateHelpAction?.Invoke(commandsToShow);
        }

        return 0;

    }

    private static IEnumerable<Command> SearchCommand(Command parent, string filter)
    {
        foreach (Command command in parent.Subcommands)
        {
            if (string.Compare(command.Name, filter, StringComparison.OrdinalIgnoreCase) == 0)
            {
                yield return command;
            }
            else
            {
                foreach (Command result in SearchCommand(command, filter))
                {
                    yield return result;
                }
            }
        }
    }
}
