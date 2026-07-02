using System.CommandLine;
using Spectre.Console;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Variable.Commands;
using xSdk.Plugins.Commands;

namespace xSdk.Demos;

internal class ReplConsoleBuilder() : IReplConsolePluginBuilder
{
    public void Configure(IApplicationBuilder builder)
    {
        var root = builder
            .SetDescription("Repl Console")
            .AddDefaultCommands()
            .AddVariableCommands()
            .AddCommand<ReplCommand>("my", "Hello command");
    }

    public void CreateBanner()
    {
        AnsiConsole.Write(
            new FigletText("xSDK REPL Console")
                .Color(Color.Green)
                .Centered());
    }

    public string CreateUserPrompt()
        => AnsiConsole.Ask<string>("REPL> ");

    public void CreateLastWill()
        => AnsiConsole.WriteLine("REPL console is shutting down. Goodbye!");

    public void CreateHelp(IList<Command> commands)
    {
        foreach (var command in commands)
        {
            AnsiConsole.WriteLine($"Command: {command.Name}");
            AnsiConsole.WriteLine($"Description: {command.Description}");
            AnsiConsole.WriteLine();
        }
    }
}
