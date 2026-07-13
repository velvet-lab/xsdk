using xSdk.Extensions.Commands;
using xSdk.Extensions.Variable.Commands;

namespace xSdk.Demos;

internal class MyConsoleBuilder : ConsoleBuilder
{
    public override void ConfigureBuilder()
    {
        this
            .WithDescription("Custom Command")
            .AddDefaultCommands()
            .AddVariableCommands()
            .AddCommand<MyCommand>("my", "Hello command");
    }
}
