using xSdk.Extensions.Plugin;

namespace xSdk.Extensions.Commands;

public sealed class ChatConsolePluginOptions : PluginOptions
{
    protected override void OnInitialize()
    {
        ListenForCommands = true;
    }

    public bool ListenForCommands { get; set; }
}
