using xSdk.Extensions.Plugin;

namespace xSdk.Extensions.Commands;

public sealed class ChatConsolePluginOptions : ConsolePluginOptions
{
    protected override void OnInitialize()
    {
        ListenForCommands = true;
    }

    public bool ListenForCommands { get; set; }
}
