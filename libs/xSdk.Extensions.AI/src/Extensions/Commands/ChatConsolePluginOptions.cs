using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Commands;

public sealed class ChatConsolePluginOptions : ConsolePluginOptions
{
    protected override void OnInitialize()
    {
        EnableCommands = true;
        DisableDefaultHelp = true;
    }

    [Variable(
        name: Definitions.EnableCommands.Name,
        template: Definitions.EnableCommands.Template,
        helpText: Definitions.EnableCommands.HelpText
    )]
    public bool EnableCommands
    {
        get => ReadValue<bool>(Definitions.EnableCommands.Name);
        set => SetValue(Definitions.EnableCommands.Name, value);
    }

    public static class Definitions
    {
        public static class EnableCommands
        {
            public const string Name = nameof(EnableCommands);
            public const string Template = "--enable-commands";
            public const string HelpText = "Whether to enable commands";
        }
    }
}
