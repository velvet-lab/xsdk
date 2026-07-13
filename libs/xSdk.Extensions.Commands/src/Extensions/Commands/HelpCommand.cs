using System.CommandLine;
using System.ComponentModel;
using Microsoft.Extensions.Options;
using Spectre.Console;
using xSdk.Extensions.Commands.Attributes;

namespace xSdk.Extensions.Commands;

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
