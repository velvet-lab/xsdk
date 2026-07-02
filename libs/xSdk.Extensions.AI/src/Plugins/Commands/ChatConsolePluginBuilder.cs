using System.CommandLine;
using xSdk.Extensions.Commands;

namespace xSdk.Plugins.Commands;

public abstract class ChatConsolePluginBuilder : IReplConsolePluginBuilder
{
    public void Configure(IApplicationBuilder builder)
    {
        builder
            .AddDefaultCommands()
            .AddCommand<ChatCommand>(ChatCommand.Definitions.Name);

        // ConfigureChatCommands(builder);
    }

    public virtual void CreateBanner()
    { }

    public virtual void CreateHelp(IList<Command> commands)
    { }

    public virtual void CreateLastWill()
    { }

    public abstract string CreateUserPrompt();
}
