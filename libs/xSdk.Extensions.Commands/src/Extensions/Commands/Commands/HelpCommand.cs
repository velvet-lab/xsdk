using System.CommandLine;
using System.ComponentModel;
using Microsoft.Extensions.Options;
using Spectre.Console;
using xSdk.Extensions.Commands.Attributes;
using xSdk.Plugins.Commands;

namespace xSdk.Extensions.Commands;


public sealed class HelpCommand(RootCommand rootCommand, IReplConsolePluginBuilder builder, IOptions<PluginOptions> options) : CommandHandler
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
    public string[] Commands {  get; set; }

    public override int Execute()
    {
        List<Command> commandsToShow = new List<Command>();
        foreach (var command in Commands)
        {
            var foundCommands = SearchCommand(rootCommand, command);
            if (foundCommands.Any())
            {
                commandsToShow.AddRange(foundCommands);
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Command '{command}' not found.[/]");
            }
        }

        var setup = options.Value;
        if (setup.DisableDefaultHelp)
        {
            builder.CreateHelp(commandsToShow);
        }

        return 0;
    
    }

    private IEnumerable<Command> SearchCommand(Command parent, string filter)
    {
        foreach(var command in parent.Subcommands)
        {
            if(string.Compare(command.Name, filter, StringComparison.OrdinalIgnoreCase) == 0)
            {
                yield return command;
            }
            else
            {
                foreach (var result in SearchCommand(command, filter))
                {
                    yield return result;
                }
            }
        }
    }    
}
