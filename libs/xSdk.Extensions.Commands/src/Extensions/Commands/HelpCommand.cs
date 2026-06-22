using System.ComponentModel;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace xSdk.Extensions.Commands;

[Description(Definitions.HelpText)]
public sealed class HelpCommand(ICommandAppSettings appSettings, ICommandModel model, IConsolePluginBuilder builder, IOptions<ConsolePluginOptions> options) : Command<EmptyCommandSettings>
{
    public static class Definitions
    {
        public const string Name = "help";
        public const string HelpText = "Display help information";
    }

    protected override int Execute(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        if (options.Value.DisableDefaultHelp)
        {
            builder.CreateHelp(appSettings, model);
        }
        else
        {
            CreateProgramHelp();
        }

        return 0;
    }

    private void CreateProgramHelp()
    {
        var helpProvider = new HelpProvider(appSettings);

        var helpItems = helpProvider.Write(model, null).ToList();

        AnsiConsole.MarkupLine("[yellow]VERWENDUNG:[/]");
        AnsiConsole.MarkupLine("    [cyan]<KOMMANDO>[/] [gray][[OPTIONEN]][/]");

        foreach (var item in helpItems.Skip(1))
        {
            AnsiConsole.Write(item);
        }
    }
}
