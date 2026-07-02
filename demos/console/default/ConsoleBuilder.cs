using xSdk.Extensions.Commands;
using xSdk.Extensions.Variable.Commands;
using xSdk.Plugins.Commands;

namespace xSdk.Demos;

internal class ConsoleBuilder : IConsolePluginBuilder
{
    public void Configure(IApplicationBuilder builder)
    {
        var root = builder
            .SetDescription("Custom Command")
            .AddDefaultCommands()
            .AddVariableCommands()
            .AddCommand<MyCommand>("my", "Hello command");
    }
}
