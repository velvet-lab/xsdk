using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Extensions.Commands;

internal class ChatConsoleBuilder : ReplConsoleBuilder
{
    public override void ConfigureBuilder()
    {
        this
            .AddDefaultCommands()
            .AddCommand<ChatCommand>(ChatCommand.Definitions.Name);
    }

    protected override IApplication BuildApplication(IServiceProvider provider)
        => ActivatorUtilities.CreateInstance<ChatApplication>(provider);
}
