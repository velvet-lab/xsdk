using Spectre.Console.Cli;
using xSdk.Demos.Commands;
using xSdk.Extensions.Commands;

namespace xSdk.Plugins.Commands;

public abstract class ChatConsolePluginBuilder : ConsolePluginBuilder
{
    public sealed override void Configure(IConfigurator builder)
    {
        builder
            .PropagateExceptions()
            .AddDefaultCommands()            
            .AddCommand<ChatCommand>(ChatCommand.Definitions.Name);

        ConfigureChatCommands(builder);
    }

    protected virtual void ConfigureChatCommands(IConfigurator builder)
    {
        
    }
}
